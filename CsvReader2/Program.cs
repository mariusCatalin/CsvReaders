﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Globalization;
using CsvReader;
using System.Xml.Linq;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//Proiectul CSV Reader2
//BEGIN
//Parse CSV-ul ->
//Datele parsate le pun intr-un DataTable ->
//Inserez DataTable in baza cu ajutorul SqlBulkCopy 
//Mut fisierul
//END

namespace CsvReader2
{
    class Program
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            start();
        }



        public static void start()
        {
            log.Info("Start");
            log.Info("Se verifica continutul folderului.");

            // Daca nu exista fisiere in folder returneaza un mesaj
            if (checkDir(ConfigurationManager.AppSettings["dirPath"]) == null)
            {
                log.Warn("Nu exista fisiere in folder");
            }
            else
            {
                log.Info(String.Format("In folder sunt {0} fisiere", checkDir(ConfigurationManager.AppSettings["dirPath"]).Length.ToString()));
                //Daca exista fisiere in folder apeleaza functiile insertIntoDatabase si readCsv pentru fiecare fisiere
                foreach (string file in checkDir(ConfigurationManager.AppSettings["dirPath"]))
                {
                    if (checkExtension(file) == true)
                    {

                        insertIntoDatabase(readCsv(file));
                    }

                }
            }
        }

        //Verific daca extensia fisierului e .csv si in functie de asta returnez true sau false
        static bool checkExtension(string path)
        {
            bool check = false;
            FileInfo fisier = new FileInfo(path);
            if (fisier.Extension == ".csv")
            {
                check = true;
            }

            return check;
        }

        static string[] checkDir(string path)
        {
            //Verifica daca exista fisiere in folder
            //Daca exista returneaza un array cu path-urile
            //Daca nu exista retuneaza null
            string[] filesInDir = null;
            if (Directory.EnumerateFiles(path).Any() == true)
            {
                filesInDir = Directory.GetFiles(path);
            }

            return filesInDir;

        }


        static DataTable readCsv(string path)
        {
            //Creez un tabel de tip DataTable in care o sa introduc valorile din CSV
            DataTable tabel = new DataTable("Csv");
            tabel.Columns.Add("LogTime", typeof(DateTime));
            tabel.Columns.Add("Action", typeof(string));
            tabel.Columns.Add("FolderPath", typeof(string));
            tabel.Columns.Add("FileName", typeof(string));
            tabel.Columns.Add("Username", typeof(string));
            tabel.Columns.Add("IpAdress", typeof(string));
            tabel.Columns.Add("XferSize", typeof(int));
            tabel.Columns.Add("Duration", typeof(double));
            tabel.Columns.Add("AgentBrand", typeof(string));
            tabel.Columns.Add("AgentVersion", typeof(string));
            tabel.Columns.Add("Error", typeof(int));

            log.Info(String.Format("Incepe parsarea fisierului: {0}", path));
            try
            {
                // Pentru citirea datelor din CSV folosesc StreamReader
                StreamReader reader = new StreamReader(path);
                //Citesc prima linie si nu fac nimic cu ea pentru ca e header-ul CSV-ului
                reader.ReadLine();

                //Un while in care citesc fiecare linie din CSV pana cand se termina stream-ul
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var element = line.Split(',');

                    DateTime logTime;
                    DateTime? logTime2 = null;
                    //Parsez data din CSV
                    bool succes = DateTime.TryParseExact(element[0].Trim('"'), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out logTime);
                    if (succes == true)
                    {
                        logTime2 = logTime;
                    }
                    //Instantiez un obiect de tip CsvClass, care are ca proprietati elementele citite pe randul respectiv
                    CsvClass CsvClass = new CsvClass(logTime2, element[1].Trim('"'), element[2].Trim('"'), element[3].Trim('"'), element[4].Trim('"'),
                                                    element[5].Trim('"'), Convert.ToInt32(element[6].Trim('"')), Convert.ToDouble(element[7].Trim('"')),
                                                    element[8].Trim('"'), element[9].Trim('"'), Convert.ToInt32(element[10].Trim('"')));
                    //Adaug pe un rand datele citite
                    DataRow rand = tabel.NewRow();
                    if (CsvClass.LogTime == null)
                        rand["LogTime"] = DBNull.Value;
                    else
                        rand["LogTime"] = CsvClass.LogTime;
                    rand["Action"] = CsvClass.Action;
                    rand["FolderPath"] = CsvClass.FolderPath;
                    rand["FileName"] = CsvClass.Filename;
                    rand["Username"] = CsvClass.Username;
                    rand["IpAdress"] = CsvClass.IpAdress;
                    rand["XferSize"] = CsvClass.XferSize;
                    rand["Duration"] = CsvClass.Duration;
                    rand["AgentBrand"] = CsvClass.AgentBrand;
                    rand["AgentVersion"] = CsvClass.AgentVersion;
                    rand["Error"] = CsvClass.Error;

                    tabel.Rows.Add(rand);

                }
                reader.Close();
            }
            catch (Exception e)
            {
                log.Error("Au fost probleme la parsarea fisierului fisierului:", e);
            }
            finally
            {
                //Dupa ce am citit csv-ul, il mut intr-o alta locatie(marcare)
                moveFile(path);
            }
            
            

            return tabel;

        }

        static void insertIntoDatabase(DataTable tabel)
        { 
            using (SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["dbConnInfo"]))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConn))
                {
                    //Mapez coloanele din tabela din baza de date cu coloanele din DataTable
                    for (int i = 0; i < tabel.Columns.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(i, i + 1);
                    }

                    //Copiez tabelul de tip DataTable in Baza de date
                    bulkCopy.DestinationTableName = ConfigurationManager.AppSettings["destTable"];
                    dbConn.Open();
                    try
                    {
                        log.Info("Incepe insertul in baza de date");
                        bulkCopy.WriteToServer(tabel);
                        dbConn.Close();
                    }
                    catch(Exception e)
                    {
                        log.Error("Au aparut probleme la insert", e);
                    }
                    
                }
            }



        }

        //Functia care muta fisierul
        static void moveFile(string path)
        {
            log.Info(String.Format("Fisierul {0} se muta in noua destinatie", path));
            try
            {
                FileInfo fisier = new FileInfo(path);
                fisier.MoveTo(ConfigurationManager.AppSettings["destDir"] + fisier.Name);
            }
            catch (Exception e)
            {
                log.Error("Fisierul nu a putut fi mutat", e);
            }
        }


    }
}
