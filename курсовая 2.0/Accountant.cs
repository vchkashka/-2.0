using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace курсовая_2._0
{
    enum Education
    {
        HigherEconomic,
        SecondaryProfessional,
        EMPTY
    }
    enum Category
    {
        FirstCategory,
        SecondCategory,
        ThirdCategory,
        EMPTY
    }
    internal class Accountant : Vacancy
    {
        Education education;//уровень образование бухгалтера
        Category category;//категория 
        DateTime date;//дата последнего повышения квалификации (получения категории)

        public Education Education
        {
            set
            {
                if (value== Education.EMPTY)
                {
                    education = Education.EMPTY;
                    throw new Exception("Не указан уровень образования. Попробуйте заново.");
                }
                else education = value;
            }
            get
            {
                return education;
            }
        }

        public Category Category
        {
            set
            {
                if (value == Category.EMPTY)
                {
                    category = Category.EMPTY;
                    throw new Exception("Не указана категория. Попробуйте заново.");
                }
                else category = value;
            }
            get
            {
                return category;
            }
        }

        public DateTime Date
        {
            set
            {
                if (value > DateTime.Now)
                {
                    throw new Exception("Дата получения категории указана неверно. Попробуйте заново.");
                }
                else date = value;
            }
            get
            {
                return date;
            }
        }

        public Accountant(string name, Education education, Category category, DateTime date) : base(name)
        {
            if (education == Education.EMPTY)
            {
                this.education = Education.EMPTY;
                throw new Exception("Не указан уровень образования. Попробуйте заново.");
            }
            else this.education = education;
            if (category == Category.EMPTY)
            {
                this.category = Category.EMPTY;
                throw new Exception("Не указана категория. Попробуйте заново.");
            }
            else this.category = category;
            if (date > DateTime.Now)
            {
                throw new Exception("Дата получения категории указана неверно. Попробуйте заново.");
            }
            else this.date = date;
        }

        public override string Print()
        {
            string str = "", str1 = "";
            switch (education)
            {
                case Education.HigherEconomic: str = "Высшее экономическое";break;
                case Education.SecondaryProfessional: str = "Среднее профессиональное"; break;
            }
            switch (category)
            {
                case Category.FirstCategory: str1 = "Первая категория"; break;
                case Category.SecondCategory: str1 = "Вторая категория"; break;
                case Category.ThirdCategory: str1 = "Третья категория"; break;
            }
            return base.Print() + $"Образование: {str}\nКатегория: {str1}\nДата присвоения категории: {date.ToLongDateString()}\n";
        }

        public override string PrintToFile()
        {
            string str = "", str1 = "";
            switch (education)
            {
                case Education.HigherEconomic: str = "Высшее экономическое"; break;
                case Education.SecondaryProfessional: str = "Среднее профессиональное"; break;
            }
            switch (category)
            {
                case Category.FirstCategory: str1 = "Первая категория"; break;
                case Category.SecondCategory: str1 = "Вторая категория"; break;
                case Category.ThirdCategory: str1 = "Третья категория"; break;
            }
            return base.PrintToFile() + $"{str}|{str1}|{date.ToFileTime()}|"; 
        }
    }
}
