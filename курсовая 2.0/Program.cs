using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсовая_2._0
{
    static class Data
    {
        public static Client Value { get; set; }
    }
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ListOfClients list = new ListOfClients();
            list.LoadFromFile("AWP.txt");
            Application.Run(new AWP(list));
            
        }
    }
}
