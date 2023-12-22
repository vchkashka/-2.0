using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace курсовая_2._0
{
    internal class Applicant : Client
    {
        string fio;//ФИО соискателя
        public char[] number;//номер телефона
        List<Vacancy> vacancies;//вакансии, на которые претендует соискатель
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
                    throw new Exception("Некорректный ввод ФИО соискателя (содержит цифру или другой неподходящий символ). Попробуйте заново.");
                }
                fio = value;
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

        public List<Vacancy> Vacancies
        {
            set
            {
                vacancies = value;
            }
            get
            {
                return vacancies;
            }
        }
        public Applicant(string fio, char[] number, Experience work_expirience, DateTime birthdate, decimal income_level, List<Vacancy> vacancies) : base( work_expirience, birthdate, income_level)
        {
            this.number = number;
            if (!IsNameValid(fio))
            {
                throw new Exception("Некорректный ввод ФИО соискателя (содержит цифру или другой неподходящий символ). Попробуйте заново.");
            }
            else this.fio = fio;
            this.vacancies = vacancies;
        }

        public override string[] Print()
        {
            string[] applicant = new string[6];
            string vacancy = null;
            for (int i = 0; i < vacancies.Count; i++)
                vacancy += vacancies[i].Print() + "\n";
            applicant[0] = fio;
            applicant[1] = $"8{String.Join("", number)}";
            applicant[2] = base.Print()[0];
            applicant[3] = base.Print()[1];
            applicant[4] = base.Print()[2];
            applicant[5] = vacancy;
            return applicant;
        }
        public override string PrintToFile()
        {
            string vacancy = null;
            for (int i = 0; i < vacancies.Count; i++)
                vacancy += vacancies[i].PrintToFile();
            return $"{fio}|{String.Join("", number)}|" + base.PrintToFile() + vacancy;
        }
    }
}
