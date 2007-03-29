using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace WallpaperChanger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int rodando = 0;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "WallpaperChanger")
                    rodando++;
            }
            if (rodando > 1)
            {
                MessageBox.Show("O programa já está sendo executando...", "Saindoooo....", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmPrincipal());
            }
        }
    }
}