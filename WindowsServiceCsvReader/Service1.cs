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
            base.OnStart(args);
            sourceDir = args[0];
            destDir = args[1];
            connString = args[2];
            destTable = args[3];


            myTimer = new Timer();
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

        protected override void OnStop()
        {
            myTimer.Enabled = false;
        }
    }
}
