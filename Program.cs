using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Velopack;

namespace Skydat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            VelopackApp.Build().Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new forms2065.frmbase());
        }
    }
}
