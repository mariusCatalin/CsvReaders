using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using System.Timers;
using Microsoft.Win32;

namespace WindowsServiceCsvReader
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private Timer myTimer = null;
        private string sourceDir;
        private string destDir;
        private string connString;
        private string destTable;

       

        protected override void OnStart(string[] args)
        {
            //Initializez timer-ul la 20 de secunde
            myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(setParameters);
            myTimer.Elapsed += new ElapsedEventHandler(timeElapsed);
            myTimer.Interval = 1000 * 20;
            myTimer.Enabled = true;
            myTimer.Start();

        }

        private void timeElapsed(object sender, ElapsedEventArgs e)
        {
            ReadAndWriteCsv test = new ReadAndWriteCsv(sourceDir, destDir, connString, destTable);
            test.start();

        }

        private void setParameters(object sender, ElapsedEventArgs e)
        {
            const string regKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CsvReaderService";
            sourceDir = Registry.GetValue(regKey, "sourceDirectory", null).ToString();
            destDir = Registry.GetValue(regKey, "destinationDirectory", null).ToString();
            connString = Registry.GetValue(regKey, "connString", null).ToString();
            destTable = Registry.GetValue(regKey, "destTable", null).ToString();
            
        }

        protected override void OnStop()
        {
            //Daca se opreste serviciul timerul se dezactiveaza
            myTimer.Enabled = false;
        }
    }
}
