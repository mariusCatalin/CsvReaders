using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace CsvReader3
{
    //Proiectul CsvReader3
    //BEGIN
    //Ma uit in folder daca exista fisere
    //Daca exista fisiere, pentru fiecare fisier trimit calea si apelez o procedura stocata
    //Procedura stocata preia calea si executa ea si parsarea fisierului si insertul in baza
    //Vezi procedura stocata in folder Procedura stocata(folosesc Insert bulk(functie din SQL server))
    //Mut fisierul
    //END
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
                    sqlBulkCopy(file);
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

        static void sqlBulkCopy(string path)
        {
            using (SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["dbConnInfo"]))
            {
                using (SqlCommand command = new SqlCommand("sqlBulkInsert", dbConn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@path", path);

                    dbConn.Open();
                    command.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
            FileInfo fisier = new FileInfo(path);
            fisier.MoveTo(ConfigurationManager.AppSettings["destDir"] + fisier.Name);
            
        }
    }
}
