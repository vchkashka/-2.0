using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace курсовая_2._0
{
    internal class Employee : Client
    {
        string fio;//ФИО сотрудника
        public char[] number;//номер телефона
        Vacancy vacancy;
        DateTime certification_date;//дата последней аттестации

        private bool IsNameValid(string fio)
        {

            string pattern = "^[A-Za-zА-Яа-я -]+$";
            return Regex.IsMatch(fio, pattern);
        }
        public string Fio
        {
            set
            {
                if (!IsNameValid(value))
                {
                    throw new Exception("Некорректный ввод ФИО сотрудника (содержит цифру или другой неподходящий символ). Попробуйте заново.");
                }
                else fio = value;
            }
            get
            {
                return fio;
            }
        }

        public char[] Number
        {
            set
            {
                number = value;
            }
            get
            {
                return number;
            }
        }
        public DateTime CertificationDate
        {
            set
            {
                if (value > DateTime.Now) 
                {
                    throw new Exception("Дата аттестации указана неверно. Попробуйте заново.");
                }
                else certification_date = value;
            }
            get
            {
                return certification_date;
            }
        }

        public Vacancy Vacancy
        {
            set
            {
                vacancy = value;
            }
            get
            {
                return vacancy;
            }
        }
        public Employee (string fio, char[] number, Experience work_expirience, DateTime birthdate, decimal income_level, Vacancy vacancy, DateTime certification_date) : base(work_expirience, birthdate, income_level)
        {
            this.number = number;
            if (!IsNameValid(fio))
            {
                throw new Exception("Некорректный ввод ФИО сотрудника (содержит цифру или другой неподходящий символ). Попробуйте заново.");
            }
            else this.fio = fio;
            if (certification_date > DateTime.Now || certification_date <= birthdate)
            {
                throw new Exception("Дата аттестации указана неверно. Попробуйте заново.");
            }
            else this.certification_date = certification_date;
            this.vacancy = vacancy;
        }

        public override string[] Print()
        {
            string[] employee = new string[7];
            employee[0] = fio;
            employee[1] = $"8{String.Join("", number)}";
            employee[2] = base.Print()[0];
            employee[3] = base.Print()[1];
            employee[4] = base.Print()[2];
            employee[5] = vacancy.Print();
            employee[6] = $"{certification_date.ToLongDateString()}";
            return employee;
        }
        public override string PrintToFile()
        {
            return $"{fio}|{String.Join("", number)}|" + base.PrintToFile()+ vacancy.PrintToFile() + $"{certification_date.ToFileTime()}|";
        }
    }
}
