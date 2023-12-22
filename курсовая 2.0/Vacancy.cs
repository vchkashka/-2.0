using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace курсовая_2._0
{
    internal class Vacancy
    {
        string name;//название вакансии

        private bool IsNameValid(string name)
        {

            string pattern = "^[A-Za-zА-Яа-я -]+$";
            return Regex.IsMatch(name, pattern);
        }
        public string Name
        {
            set
            {
                if (!IsNameValid(value))
                {
                    throw new Exception("Некорректный ввод названия вакансии (содержит цифру или другой неподходящий символ). Попробуйте заново");
                }
                name = value;
            }
            get
            {
                return name;
            }
        }
        public Vacancy(string name)
        {
            if (!IsNameValid(name))
            {
                throw new Exception("Некорректный ввод названия вакансии (содержит цифру или другой неподходящий символ). Попробуйте заново");
            }
            else this.name = name;
        }
        public virtual string Print()
        {
            return $"{name}\n";
        }
        public virtual string PrintToFile()
        {
            return $"{name}|";
        }
    }
}
