using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ServiceProcess;
//using B2BAISERA.Service;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace B2BAISERA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Process.GetProcessesByName("B2BAISERA_S02003").Length > 1)
                Application.Exit();
            else
                Application.Run(new UploadS02003());

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new B2BAISERAService() 
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
