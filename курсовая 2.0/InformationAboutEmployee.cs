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
    public partial class InformationAboutEmployee : Form
    {
        public InformationAboutEmployee()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Experience ex = Experience.EMPTY; Employee employee;
            switch (comboBox1.Text)
            {
                case "Нет опыта": ex = Experience.None; break;
                case "От 1 года до 3 лет": ex = Experience.From1To3; break;
                case "От 3 до 6 лет": ex = Experience.From3To6; break;
                case "Более 6 лет": ex = Experience.MoreThan6; break;
                default: ex = Experience.EMPTY; break;
            }
            //Если одно из полей пустое, то выводим ошибку
            if (ex == Experience.EMPTY || textBox8.Text == null || maskedTextBox1.MaskCompleted == false || textBox7.Text == null || comboBox3.Text == null)
                throw new Exception("Не все поля заполнены. Попробуйте заново");
            else
                employee = new Employee(textBox8.Text.ToString(), maskedTextBox1.Text.ToCharArray(), ex, dateTimePicker2.Value, Convert.ToInt32(textBox7.Text), new Vacancy(comboBox3.Text.ToString()), dateTimePicker1.Value);
            //В зависимости от выбранной вакансии создаем экземпляр класса-потомка и вызываем форму, в которой заполняем информацию
            switch (comboBox3.Text)
            {
                case "Бухгалтер":
                    {
                        using (AccountantForm form = new AccountantForm(employee))
                        {
                            form.ShowDialog(this);
                        }
                    }
                    break;
                case "Охранник":
                    {
                        using (GuardForm form = new GuardForm(employee))
                        {
                            form.ShowDialog(this);
                        }
                    }
                    break;
                case "Уборщик":
                    {
                        Data.Value = employee;
                    }
                    break;
            }
            Close();
        }
    }
}
