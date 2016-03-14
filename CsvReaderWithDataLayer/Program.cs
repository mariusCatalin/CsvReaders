using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data.SqlClient;

namespace CsvReaderWithDataLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            DB db = new DB();
            Console.WriteLine(db.ConnectionString);
            List<CsvClass> lista = db.readDatabase(null, null, null);
            foreach(CsvClass obiect in lista)
            {
                Console.WriteLine(obiect.LogTime.ToString());
            }
            Console.ReadLine();
        }
    }
}
