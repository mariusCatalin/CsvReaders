using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvReaderLibWithDataLayer;

namespace TestReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDirectory = @"D:\Testt";
            string destinationDirectory = @"D:\Testt2";
            ReadAndInsertCsv readAndInsert = new ReadAndInsertCsv(sourceDirectory, destinationDirectory);
            readAndInsert.start();
        }
    }
}
