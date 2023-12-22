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
    public partial class GuardForm : Form
    {
        Client client;
        internal GuardForm(Client client)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.client = client;
        }

        private void GuardForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool license = false; Rank rank = Rank.EMPTY;
            switch (comboBox1.Text)
            {
                case "Первый разряд": rank = Rank.First; break;
                case "Второй разряд": rank = Rank.Second; break;
                case "Третий разряд": rank = Rank.Third; break;
                case "Четвертый разряд": rank = Rank.Fourth; break;
                default: rank = Rank.EMPTY; break;
            }
            if (checkBox4.Checked == true) license = true;
            else if (checkBox3.Checked == true) license = false;
            if (client.GetType().Name == "Applicant")
                for (int i = 0; i < ((Applicant)client).Vacancies.Count; i++)
                    if (((Applicant)client).Vacancies[i].Name == "Охранник")
                        ((Applicant)client).Vacancies[i] = new Guard("Охранник", license, rank);
            //создаем экземпляр класса в зависимости от типа клиента
            if (client.GetType().Name == "Applicant") client = new Applicant(((Applicant)client).Fio, ((Applicant)client).Number, client.WorkExperience,
                client.Birthdate, client.IncomeLevel, ((Applicant)client).Vacancies);
            if (client.GetType().Name == "Employee") client = new Employee(((Employee)client).Fio, ((Employee)client).Number, client.WorkExperience, 
                client.Birthdate, client.IncomeLevel, new Guard("Охранник", license, rank), ((Employee)client).CertificationDate);
            Data.Value = client;
            Close();
        }
    }
}
