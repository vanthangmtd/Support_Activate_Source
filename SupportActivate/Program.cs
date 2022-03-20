using FormWindows.SupportActivate;
using System;
using System.Windows.Forms;

namespace SupportActivate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormSupportActivate());
        }
    }
}
