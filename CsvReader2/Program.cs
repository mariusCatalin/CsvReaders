using System;
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

namespace CsvReader2
{
    class Program
    {
        static void Main(string[] args)
        {
            start();
        }



        static void start()
        {
            if (checkDir(ConfigurationManager.AppSettings["dirPath"]) == null)
            {
                Console.WriteLine("Nu exista fisiere in folder");
                Console.ReadLine();
            }
            else
            {
                foreach (string file in checkDir(ConfigurationManager.AppSettings["dirPath"]))
                {
                    readAndInsert(file);
                }
            }
        }

        static string[] checkDir(string path)
        {

            string[] filesInDir = null;
            if (Directory.EnumerateFiles(path).Any() == true)
            {
                filesInDir = Directory.GetFiles(path);
            }

            return filesInDir;
        }


        static void readAndInsert(string path)
        {
            DataTable tabel = new DataTable("Csv");
            tabel.Columns.Add("LogTime",typeof(DateTime));
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

            
            StreamReader reader = new StreamReader(path);
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var element = line.Split(',');

                DateTime logTime;
                DateTime? logTime2 = null;
                bool succes = DateTime.TryParseExact(element[0].Trim('"'), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out logTime);
                if (succes == true)
                {
                    logTime2 = logTime;
                }

                CsvClass CsvClass = new CsvClass(logTime2, element[1].Trim('"'), element[2].Trim('"'), element[3].Trim('"'), element[4].Trim('"'),
                                                element[5].Trim('"'), Convert.ToInt32(element[6].Trim('"')), Convert.ToDouble(element[7].Trim('"')),
                                                element[8].Trim('"'), element[9].Trim('"'), Convert.ToInt32(element[10].Trim('"')));
                
                var rand = tabel.NewRow();
                if (logTime2 == null)
                    rand["LogTime"] = DBNull.Value;
                else
                    rand["LogTime"] = logTime2;
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


            using (SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["dbConnInfo"]))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConn)) 
                {
                    
                    for (int i = 0; i < tabel.Columns.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(i, i+1);
                    }
     
                    bulkCopy.DestinationTableName = ConfigurationManager.AppSettings["destTable"];
                    dbConn.Open();
                    bulkCopy.WriteToServer(tabel);
                    dbConn.Close();
                }
                
            }

            FileInfo fisier = new FileInfo(path);
            fisier.MoveTo(ConfigurationManager.AppSettings["destDir"] + fisier.Name);
        }


    }
}
