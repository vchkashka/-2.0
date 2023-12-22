using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсовая_2._0
{
    public partial class AccountantForm : Form
    {
        Client client;
        internal AccountantForm(Client client)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.client = client;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Education education = Education.EMPTY; Category category = Category.EMPTY;
            switch (comboBox1.Text)
            {
                case "Высшее экономическое": education = Education.HigherEconomic; break;
                case "Среднее профессиональное": education = Education.SecondaryProfessional; break;
                default: education = Education.EMPTY; break;
            }
            switch (comboBox2.Text)
            {
                case "Первая категория": category = Category.FirstCategory; break;
                case "Вторая категория": category = Category.SecondCategory; break;
                case "Третья категория": category = Category.ThirdCategory; break;
                default: category = Category.EMPTY; break;
            }
            DateTime date;
            date = monthCalendar1.SelectionStart;
            if (client.GetType().Name == "Applicant")
                for (int i = 0; i < ((Applicant)client).Vacancies.Count; i++)
                    if (((Applicant)client).Vacancies[i].Name == "Бухгалтер")
                        ((Applicant)client).Vacancies[i] = new Accountant("Бухгалтер", education, category, date);
            //Если пользователь по ошибке ввел дату получения категории до своего рождения, то выводим сообщение об ошибке, иначе создаем 
            //экземпляр класса в зависимости от типа клиента
            if (date >= client.Birthdate)
            {
                if (client.GetType().Name == "Applicant") client = new Applicant(((Applicant)client).Fio, ((Applicant)client).Number, client.WorkExperience,
                    client.Birthdate, client.IncomeLevel, ((Applicant)client).Vacancies);
                if (client.GetType().Name == "Employee") client = new Employee(((Employee)client).Fio, ((Employee)client).Number, client.WorkExperience,
                    client.Birthdate, client.IncomeLevel, new Accountant("Бухгалтер", education, category, date), ((Employee)client).CertificationDate);
                Data.Value = client;
            }
            else throw new Exception("Дата получения категории указана неверно. Попробуйте заново.");

            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
