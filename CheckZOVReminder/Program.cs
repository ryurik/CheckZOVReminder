using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CheckZOVReminder
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
            frmMain _frmMain = new frmMain();
            if (!_frmMain.AutoClose)
            {
                Application.Run(_frmMain);
            }
            else
            {
                _frmMain.Close();
                _frmMain.Dispose();
                Application.Exit();
            }
        }
    }
}
