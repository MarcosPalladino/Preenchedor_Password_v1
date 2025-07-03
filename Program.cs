using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPPreenchedor.Forms;

namespace TPPreenchedor
{
    static class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //VALIDA SE JÁ EXISTE INSTANCIA DA APLICAÇÃO
            if (Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location)).Count() > 1) return;

            FreeConsole(); // Console desaparece COMPLETAMENTE
            //FreeConsole(); // Console desaparece, mas ainda é possível abrir o console com F12 no Visual Studio

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Preenchedor());
        }
    }
}
