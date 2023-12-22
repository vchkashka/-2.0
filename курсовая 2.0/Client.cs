using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace курсовая_2._0
{
    enum Experience
    {
        None,
        From1To3,
        From3To6,
        MoreThan6,
        EMPTY
    }
    abstract class Client
    {
        Experience work_expirience;//опыт работы
        DateTime birthdate;//дата рождения
        decimal income_level;//уровень дохода        

        public Experience WorkExperience
        {
            set
            {
                if (value == Experience.EMPTY)
                {
                    work_expirience = Experience.EMPTY;
                    throw new Exception("Не указан опыт работы. Попробуйте заново.");
                }
                else work_expirience = value;                
            }
            get
            {
                return work_expirience;
            }
        }

        public DateTime Birthdate
        {
            set
            {
                if (value > DateTime.Now)
                {
                    throw new Exception("Введена некорректная дата рождения. Попробуйте заново.");
                }
                else if ((DateTime.Now - value).TotalDays / 365 < 16) 
                {
                    throw new Exception("Введена некорректная дата рождения (возраст не может быть меньше 16 лет). Попробуйте заново.");
                }
                else if ((DateTime.Now - value).TotalDays / 365 > 100)
                {
                    throw new Exception("Введена некорректная дата рождения (согласно книге рекордов Гиннеса самому старому работающему человеку 100 лет, вы хотите установить новый рекорд?). Попробуйте заново.");
                }
                else birthdate = value;
            }
            get
            {
                return birthdate;
            }
        }

        public decimal IncomeLevel
        {
            set
            {
                //Максимальная заработная плата среди сотрудников может быть у бухгалтера - не более 250 000
                if (value < 0 || value > 250000)
                {
                    throw new Exception("Введен некорректный уровень дохода. Попробуйте заново.");
                }
                else income_level = value;
            }
            get { return income_level; }
        }
        

        public virtual string[] Print()
        {
            string[] client = new string[3];
            string s2 = "";
            switch (work_expirience)
            {
                case Experience.None: s2 = "Нет опыта"; break;
                case Experience.From1To3: s2 = "От 1 года до 3 лет"; break;
                case Experience.From3To6: s2 = "От 3 до 6 лет"; break;
                case Experience.MoreThan6: s2 = "Более 6 лет"; break;
            }
            client[0] = birthdate.ToLongDateString();
            client[1] = s2;
            client[2] = $"{Math.Truncate(income_level)}";
            return client;
        }

        public virtual string PrintToFile()
        {
            string s2 = "";
            switch (work_expirience)
            {
                case Experience.None: s2 = "Нет опыта"; break;
                case Experience.From1To3: s2 = "От 1 года до 3 лет"; break;
                case Experience.From3To6: s2 = "От 3 до 6 лет"; break;
                case Experience.MoreThan6: s2 = "Более 6 лет"; break;
            }
            return $"{birthdate.ToFileTime()}|{s2}|{income_level}|";
        }

        public Client(Experience work_expirience, DateTime birthdate, decimal income_level)
        {            
            if (work_expirience == Experience.EMPTY)
            {
                throw new Exception("Не указан опыт работы. Попробуйте заново.");
            }
            else this.work_expirience = work_expirience;

            if (birthdate > DateTime.Now)
            {
                throw new Exception("Введена некорректная дата рождения. Попробуйте заново.");
            }
            else if ((DateTime.Now - birthdate).TotalDays / 365 < 16)
            {
                throw new Exception("Введена некорректная дата рождения (возраст не может быть меньше 16). Попробуйте заново.");
            }
            else if ((DateTime.Now - birthdate).TotalDays / 365 > 100)
            {
                throw new Exception("Введена некорректная дата рождения (согласно книге рекордов Гиннеса самому старому работающему человеку 100 лет, вы хотите установить новый рекорд?). Попробуйте заново.");
            }
            else this.birthdate = birthdate;

            if (income_level < 0 || income_level > 250000)
            {
                throw new Exception("Некорректный уровень дохода. Попробуйте заново.");
            }
            else this.income_level = income_level;            
            
        }
    }
}
