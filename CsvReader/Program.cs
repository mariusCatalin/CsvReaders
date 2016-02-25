using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Data;

namespace CsvReader
{
    class Program
    {
        static void Main(string[] args)
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

            List<CsvClass> elementList = new List<CsvClass>();
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
                elementList.Add(CsvClass);
                

                
            }
            reader.Close();


            using (SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["dbConnInfo"]))
            { 
                

                dbConn.Open();
                SqlCommand insert = new SqlCommand("instert",dbConn);
                insert.CommandType = CommandType.StoredProcedure;
                foreach (CsvClass CsvClass in elementList)
                {
                    insert.Parameters.Clear();

                    if (CsvClass.LogTime == null)
                        insert.Parameters.AddWithValue("@LOGTIME", DBNull.Value);
                    else
                        insert.Parameters.AddWithValue("@LOGTIME", CsvClass.LogTime);

                    insert.Parameters.AddWithValue("@ACTION", CsvClass.Action);
                    insert.Parameters.AddWithValue("@FOLDERPATH", CsvClass.FolderPath);
                    insert.Parameters.AddWithValue("@FILENAME", CsvClass.Filename);
                    insert.Parameters.AddWithValue("@USERNAME", CsvClass.Username);
                    insert.Parameters.AddWithValue("@IPADRESS", CsvClass.IpAdress);
                    insert.Parameters.AddWithValue("@XFERSIZE", CsvClass.XferSize);
                    insert.Parameters.AddWithValue("@DURATION", CsvClass.Duration);
                    insert.Parameters.AddWithValue("@AGENTBRAND", CsvClass.AgentBrand);
                    insert.Parameters.AddWithValue("@AGENTVERSION", CsvClass.AgentVersion);
                    insert.Parameters.AddWithValue("@ERROR", CsvClass.Error);

                    insert.ExecuteNonQuery();
                }
                dbConn.Close();
            }

            FileInfo fisier = new FileInfo(path);
            fisier.MoveTo(ConfigurationManager.AppSettings["destDir"] + fisier.Name);
        }



        /*
        static void test()
        {
            DateTime azi = DateTime.Now;
            string connString = "Data Source=MARIUSCST-MBL;Initial Catalog=CsvDatabase;Persist Security Info=True;User ID=sa;Password=1234%asd";
            using (SqlConnection dbConn = new SqlConnection(connString))
            {

                dbConn.Open();
                var transaction = dbConn.BeginTransaction();

                string insertQuery = "INSERT INTO dbo.CSvTable(LogTime, Username, Duration) Values(@0,@1,@2)";
                SqlCommand insert = new SqlCommand(insertQuery, dbConn);

                insert.Parameters.Add(new SqlParameter("0", azi));
                insert.Parameters.Add(new SqlParameter("1", "marius.catalin"));
                insert.Parameters.Add(new SqlParameter("2", 20.44));
                transaction.Commit();
                insert.ExecuteNonQuery();

                //string selectQuery = "SELECT * from dbo.CsvTable";
                //SqlCommand select = new SqlCommand(selectQuery, dbConn);
            }

        }*/
    }
}
