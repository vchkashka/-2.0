using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace курсовая_2._0
{
    internal class Cleaner : Vacancy
    {
        public Cleaner(string name) : base(name)
        {
        }

        public override string Print()
        {
            return base.Print();
        }

        public override string PrintToFile()
        {
            return base.PrintToFile();
        }
    }
}
