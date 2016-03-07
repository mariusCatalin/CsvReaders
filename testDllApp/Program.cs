using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace testDllApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDir = @"D:\Testt";
            string destDir = @"D:\Testt2";
            string connString = @"Data Source=MARIUSCST-MBL;Initial Catalog=CsvDatabase;Persist Security Info=True;User ID=sa;Password=1234%asd";
            string destTable = "dbo.CsvTable";  

            ReadAndWriteCsv test = new ReadAndWriteCsv(sourceDir, destDir, connString, destTable);
            test.start();
            

        }
    }
}
