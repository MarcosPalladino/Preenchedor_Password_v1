using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TPPreenchedor.Forms;

namespace TPPreenchedor
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [STAThread]
        static void Main()
        {
            bool instanciaCriada;

            using (var mutex = new Mutex(true, "Global\\TPPreenchedor", out instanciaCriada))
            {
                if (!instanciaCriada)
                {
                    return;
                }

                FreeConsole();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Preenchedor());
            }
        }
    }
}
