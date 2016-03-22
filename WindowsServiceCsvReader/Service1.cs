using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32;
using CsvReaderLibWithDataLayer;
using log4net;

namespace WindowsServiceCsvReader
{
    public partial class Service1 : ServiceBase
    {
        
        public Service1()
        {
            InitializeComponent();
        }

        private Timer myTimer = null;
        private string sourceDirectory;
        private string destDirectory;
        const string regKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CsvReaderService";

        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStart(string[] args)
        {
            log.Info("Serviciul a pornit");
            try
            {
                //Initializez timer-ul la 20 de secunde si dupa cele 20 de secunde ruleaza cele 2 metode;
                myTimer = new Timer();
                myTimer.Elapsed += new ElapsedEventHandler(setParameters);
                myTimer.Elapsed += new ElapsedEventHandler(timeElapsed);
                myTimer.Interval = 1000 * 20;
                myTimer.Enabled = true;
                myTimer.Start();
            }
            catch(Exception ex)
            {
                log.Error("Au fost probleme cu timerul:", ex);
            }
        }
        private void timeElapsed(object sender, ElapsedEventArgs e)
        {
            log.Info("Se citesc fisierele si se scriu in baza");
            try
            {
                ReadAndInsertCsv readAndInsert = new ReadAndInsertCsv(sourceDirectory, destDirectory);
                readAndInsert.Start();
            }
            catch(Exception ex)
            {
                log.Error("Au fost probleme la citirea sau scriere fisierelor.", ex);
            }
            
        }

        private void setParameters(object sender, ElapsedEventArgs e)
        {/*Se seteaza parametrii in functie de valorile din registri
           Parametrii pot fi schimbati in timp ce ruleaza serviciul modificand valoarea key din registry*/
            log.Info("Se preiau parametrii din registri");
            try
            {
                sourceDirectory = Registry.GetValue(regKey, "sourceDirectory", null).ToString();
                destDirectory = Registry.GetValue(regKey, "destinationDirectory", null).ToString();
            }
            catch(Exception ex)
            {
                log.Error("Au fost probleme la preluarea valorilor din registry", ex);
            }
        }

        protected override void OnStop()
        {
            //Daca se opreste serviciul timerul se dezactiveaza
            log.Info("Serviciul a fost oprit");
            myTimer.Enabled = false;
        }
    }
}
