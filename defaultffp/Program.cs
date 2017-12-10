using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace defaultffp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string runprofile=null;
            if (args.Length == 2)
            {
                if (args[0] == "/p")
                {
                    runprofile = args[1];
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormMain form = new FormMain(runprofile);
            Application.Run(form);
        }
    }
}
