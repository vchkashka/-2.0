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
    public partial class InformationAboutApplicant : Form
    {
        public InformationAboutApplicant()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
        }

        private void InformationAboutApplicant_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Experience ex = Experience.EMPTY; Applicant applicant;
            switch (comboBox1.Text)
            {
                case "Нет опыта": ex = Experience.None; break;
                case "От 1 года до 3 лет": ex = Experience.From1To3; break;
                case "От 3 до 6 лет": ex = Experience.From3To6; break;
                case "Более 6 лет": ex = Experience.MoreThan6; break;
                default: ex = Experience.EMPTY; break;
            }
            List<Vacancy> vacancies = new List<Vacancy>();
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                vacancies.Add(new Vacancy(checkedListBox1.CheckedItems[i].ToString()));
            //Если одно из полей пустое, то выводим ошибку
            if (ex == Experience.EMPTY || textBox8.Text == null || maskedTextBox1.MaskCompleted == false || textBox7.Text == null || checkedListBox1.CheckedItems.Count == 0) 
                throw new Exception("Не все поля заполнены. Попробуйте заново.");
            else
            {
                applicant = new Applicant(textBox8.Text.ToString(), maskedTextBox1.Text.ToCharArray(), ex, dateTimePicker1.Value, Convert.ToInt32(textBox7.Text), vacancies);
                //В зависимости от выбранной вакансии создаем экземпляр класса-потомка и вызываем форму, в которой заполняем информацию
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                    switch (checkedListBox1.CheckedItems[i].ToString())
                    {
                        case "Бухгалтер":
                            {
                                using (AccountantForm form = new AccountantForm(applicant))
                                {
                                    form.ShowDialog(this);
                                }
                            }
                            break;
                        case "Охранник":
                            {
                                using (GuardForm form = new GuardForm(applicant))
                                {
                                    form.ShowDialog(this);
                                }
                            }
                            break;
                        case "Уборщик":
                            {
                                for (int j = 0; j < applicant.Vacancies.Count; j++)
                                    if (applicant.Vacancies[j].Name == "Уборщик")
                                        applicant.Vacancies[j] = new Cleaner("Уборщик");
                                applicant = new Applicant(applicant.Fio, applicant.Number, applicant.WorkExperience, applicant.Birthdate, applicant.IncomeLevel, applicant.Vacancies);
                                Data.Value = applicant;
                            }
                            break;
                    }
                Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
