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
    public partial class CertificationForm : Form
    {
        ListOfClients list;
        
        internal CertificationForm(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
            //Если сотрудник проходил аттестацию 3 или более лет назад, то выводим информацию о нем на экран
            for (int i = 0; i < list.Count(); i++)
                if (list[i].GetType().Name == "Employee" && (DateTime.Now - ((Employee)list[i]).CertificationDate).TotalDays >= 1095)
                {
                    dataGridView1.Rows.Add(list[i].Print());
                    comboBox1.Items.Add(((Employee)list[i]).Fio);
                }
            if (comboBox1.Items.Count == 0) comboBox1.Text = "Претенденты на прохождение аттестации отсутствуют.";
            if (dataGridView1.Rows.Count == 0) label1.Text = "Претенденты на прохождение аттестации отсутствуют.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                for (int i = 0; i < list.Count(); i++)
                    if (list[i].GetType().Name == "Employee" && (DateTime.Now - ((Employee)list[i]).CertificationDate).TotalDays >= 1095)
                    {
                        if (((Employee)list[i]).Vacancy.Name == "Бухгалтер")
                        {
                            //Если сотрудник - бухгалтер третьей категории, то к нему не предьявляются никакие требования и он успешно проходит аттестацию
                            if (((Accountant)((Employee)list[i]).Vacancy).Category == Category.ThirdCategory)
                            {
                                //Изменяем дату аттестации на текущую
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Если сотрудник - бухгалтер второй категории и имеет опыт работы от 3 до 6 лет или более 6 лет, то он соответствует должности
                            //и успешно проходит аттестацию
                            else if (((Accountant)((Employee)list[i]).Vacancy).Category == Category.SecondCategory && (list[i].WorkExperience == Experience.From3To6
                                || list[i].WorkExperience == Experience.MoreThan6))
                            {
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Если сотрудник - бухгалтер первой категории, который имеет высшее образование и опыт более 6 лет, то он соответствует должности
                            //и успешно проходит аттестацию
                            else if (((Accountant)((Employee)list[i]).Vacancy).Category == Category.FirstCategory && (((Accountant)((Employee)list[i]).Vacancy).Education == Education.HigherEconomic)
                                && list[i].WorkExperience == Experience.MoreThan6)
                            {
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                //Во всех других случаях бухгалтер не соответствует должности и потому выводим предупреждающее сообщение
                                DialogResult result = MessageBox.Show($"\nВнимание! Навыки сотрудника не соответствуют его квалификации. Отправить его на курсы?\n", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                if (result == DialogResult.No) break;
                                else if (result == DialogResult.Yes)
                                {
                                    MessageBox.Show($"\nСотрудник отправлен на курсы повышения квалификации.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                            }
                        }
                        else if (((Employee)list[i]).Vacancy.Name == "Охранник")
                        {
                            //Если сотрудник - охранник первого разряда, то он проходит аттестацию успешно
                            if (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.First)
                            {
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Если сотрудник - охранник второго или третьего разряда и имеет опыт работы, то он соответствует должности
                            //и успешно проходит аттестацию
                            else if ((((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Second || ((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Third) && 
                                (list[i].WorkExperience == Experience.From1To3 || list[i].WorkExperience == Experience.From3To6 || list[i].WorkExperience == Experience.MoreThan6))
                            {
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Если сотрудник - охранник четвертого разряда, имеет опыт работы  от 3 лет и лицензию, то он соответствует должности
                            //и успешно проходит аттестацию
                            else if (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Fourth && (list[i].WorkExperience == Experience.From3To6 || list[i].WorkExperience == Experience.MoreThan6)
                                && ((Guard)((Employee)list[i]).Vacancy).License == true)
                            {
                                ((Employee)list[i]).CertificationDate = DateTime.Now;
                                MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                //Во всех других случаях охранник не соответствует должности  и потому выводим предупреждающее сообщение
                                DialogResult result = MessageBox.Show($"\nВнимание! Навыки сотрудника не соответствуют его квалификации. Отправить его на курсы?\n", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                if (result == DialogResult.No) break;
                                else if (result == DialogResult.Yes)
                                {
                                    MessageBox.Show($"\nСотрудник отправлен на курсы повышения квалификации.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                            }
                        }
                        //К уборщикам особых требований не предьявляется, но на некоторых предприятиях, например, связанных с вредным производством,
                        //им время от времени нужно проходить аттестацию в виде инструктажа или чего-то подобного
                        else if (((Employee)list[i]).Vacancy.Name == "Уборщик")
                        {
                            ((Employee)list[i]).CertificationDate = DateTime.Now;
                            MessageBox.Show($"\nСотрудник успешно прошел аттестацию.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                    }
                Close();
            }           
        }
    }
}
