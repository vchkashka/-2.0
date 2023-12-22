using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсовая_2._0
{
    enum Rank
        {
            First, Second, Third, Fourth, EMPTY
        }
    internal class Guard : Vacancy
    {
        bool license;//наличие лицензии(удостоверения) охранника
        Rank rank;//разряд охранника

        public Guard(string name, bool license, Rank rank) : base(name)
        {
            this.license = license;
            if (rank == Rank.EMPTY)
            {
                throw new Exception("Не указан разряд. Попробуйте заново.");
            }
            else this.rank = rank;
        }

        public bool License
        {
            set
            {
                license = value;
            }
            get
            {
                return license;
            }
        }

        public Rank Rank
        {
            set
            {
                if (rank == Rank.EMPTY)
                {
                    throw new Exception("Не указан разряд. Попробуйте заново.");
                }
                else rank = value;
            }
            get
            {
                return rank;
            }
        }

        public override string Print()
        {
            string str = "", str1 = "";
            switch (license)
            {
                case true: str = "Присутствует"; break;
                case false: str = "Отсутствует"; break;
            }
            switch (rank)
            {
                case Rank.First: str1 = "Первый разряд"; break;
                case Rank.Second: str1 = "Второй разряд"; break;
                case Rank.Third: str1 = "Третий разряд"; break;
                case Rank.Fourth: str1 = "Четвертый разряд"; break;
            }
            return base.Print() + $"Лицензия: {str}\nРазряд: {str1}\n";
        }

        public override string PrintToFile()
        {
            string str = "", str1 = "";
            switch (license)
            {
                case true: str = "Присутствует"; break;
                case false: str = "Отсутствует"; break;
            }
            switch (rank)
            {
                case Rank.First: str1 = "Первый разряд"; break;
                case Rank.Second: str1 = "Второй разряд"; break;
                case Rank.Third: str1 = "Третий разряд"; break;
                case Rank.Fourth: str1 = "Четвертый разряд"; break;
            }
            return base.PrintToFile() + $"{str}|{str1}|";
        }
    }
}