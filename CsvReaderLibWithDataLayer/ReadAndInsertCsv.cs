using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlClient;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System.IO;
using DataLayer;

namespace CsvReaderLibWithDataLayer
{
    public class ReadAndInsertCsv
    {
        //Proprietatile clasei ReadAndWriteCsv
        private string SourceDir { get; set; }
        private string DestDir { get; set; }
        public ILog Log { get; set; }

        //Constructorul clasei
        public ReadAndInsertCsv(string sourceDir, string destDir)
        {
            SourceDir = sourceDir;
            DestDir = destDir;

            //Configurez log4net programatic(library-ul nu are config)
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();
            FileAppender fileAppender = new FileAppender();
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File = @"D:\Log\MyLogg.txt";
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = "%date %timestamp [%thread] %level %logger - %message %exception %newline";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(fileAppender);
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        //Metoda pe care apelez dupa instantierea obiectului. E singura publica. Restul sunt private
        public void Start()
        {
            Log.Info("Start");
            Log.Info(String.Format("Se verifica continutul folderului: {0}", SourceDir));
            DB db = new DB();
            // Daca nu exista fisiere in folder returneaza un mesaj
            if (checkDir(SourceDir) == null)
            {
                Log.Warn("Nu exista fisiere in folder");
            }
            else
            {
                Log.Info(String.Format("In folder sunt {0} fisiere", checkDir(SourceDir).Length.ToString()));
                //Daca exista fisiere in folder apeleaza functiile insertIntoDatabase si readCsv pentru fiecare fisier
                foreach (string file in checkDir(SourceDir))
                {
                    if (checkExtension(file) == true)
                    {
                        foreach(CsvClass csvObject in readCsv(file))
                        {
                            try
                            {
                                db.InsertIntoDatabase(csvObject);
                            }
                            catch (SqlException e)
                            {
                                Log.Error(String.Format("Fisierul {0} contine date care au mai fost adaugate in baza", file), e);
                            }
                            catch (Exception e)
                            {
                                Log.Error(String.Format("Nu a reusit insertul in baza pentru fisierul:{0}", file), e);
                            }
                        }
                    }
                }
            }
        }

        //Metoda care verifica daca extensia fisierului e .csv si in functie de asta returneaza true sau false
        private bool checkExtension(string path)
        {
            bool check = false;
            try
            {
                FileInfo fisier = new FileInfo(path);
                if (fisier.Extension == ".csv")
                {
                    check = true;
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Au fost probleme la verificarea extensiei fisierului: {0}", path), e);
            }
            return check;
        }

        //Metoda care verifica daca exista fisiere in folder
        private string[] checkDir(string path)
        {
            //Verifica daca exista fisiere in folder
            //Daca exista returneaza un array cu path-urile
            //Daca nu exista retuneaza null

            string[] filesInDir = null;

            try
            {
                if (Directory.EnumerateFiles(path).Any() == true)
                {
                    filesInDir = Directory.GetFiles(path);
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Au fost probleme la verificarea folderului:{0}", path), e);
            }

            return filesInDir;

        }

        //Metoda care citeste un csv si returneaza o lista de obiecte
        private List<CsvClass> readCsv(string path)
        {
            //Creez o lista in care o sa adaug obiectele citite in CSV 
            List<CsvClass> lista = new List<CsvClass>();

            Log.Info(String.Format("Incepe parsarea fisierului: {0}", path));
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

                    int element6;
                    int? element6Null = null;
                    double element7;
                    double? element7Null = null;
                    int element10;
                    int? element10Null = null;
                    DateTime logTime;
                    DateTime? logTime2 = null;

                    //Parsez data din CSV, daca parsarea nu reuseste datetime-ul o sa fie null
                    bool succes = DateTime.TryParseExact(element[0].Trim('"'), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out logTime);
                    if (succes == true)
                        logTime2 = logTime;

                    //Parsez un element de tip Int,daca parsarea nu reuseste int-ul o sa fie null   
                    bool tryParseElement6 = Int32.TryParse(element[6].Trim('"'), out element6);
                    if (tryParseElement6 == true)
                        element6Null = element6;

                    //Parsez un element de tip Double,daca parsarea nu reuseste double-ul o sa fie null   
                    bool tryParseElement7 = Double.TryParse(element[7].Trim('"'), out element7);
                    if (tryParseElement7 == true)
                        element7Null = element7;

                    //Parsez un element de tip Int,daca parsarea nu reuseste int-ul o sa fie null
                    bool tryParseElement10 = Int32.TryParse(element[10].Trim('"'), out element10);
                    if (tryParseElement10 == true)
                        element10Null = element10;


                    //Instantiez un obiect de tip CsvClass, care are ca proprietati elementele citite pe randul respectiv
                    CsvClass CsvClass = new CsvClass(logTime2, element[1].Trim('"'), element[2].Trim('"'), element[3].Trim('"'), element[4].Trim('"'),
                                                    element[5].Trim('"'), element6Null, element7Null,
                                                    element[8].Trim('"'), element[9].Trim('"'), element10Null);
                    //Adaug obiectul citit in lista
                    lista.Add(CsvClass);
                }
                //Dupa ce stream-ul s-a terminat inchid reader-ul
                reader.Close();
            }
            catch (Exception e)
            {
                Log.Error("Au fost probleme la parsarea fisierului fisierului:", e);

            }
            finally
            {
                //Dupa ce am citit csv-ul, il mut intr-o alta locatie(marcare)
                moveFile(path);
            }

            //Returnez lista de obiecte
            return lista;

        }

        //Metoda care muta fisierul
        private void moveFile(string path)
        {
            Log.Info(String.Format("Fisierul {0} se muta in noua destinatie", path));
            try
            {
                FileInfo fisier = new FileInfo(path);
                fisier.MoveTo(DestDir + "\\" + fisier.Name);
            }
            catch (Exception e)
            {
                Log.Error("Fisierul nu a putut fi mutat", e);
            }
        }

    }
}

