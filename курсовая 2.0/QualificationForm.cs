using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace курсовая_2._0
{
    public partial class QualificationForm : Form
    {
        ListOfClients list;
        internal QualificationForm(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
            if (list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                    if (list[i].GetType().Name == "Employee")
                    {
                        //Если сотрудник является бухгалтером, и он повышал квалификацию пять или более лет назад (1825 дней), то
                        if (((Employee)list[i]).Vacancy.Name == "Бухгалтер" && (DateTime.Now - ((Accountant)((Employee)list[i]).Vacancy).Date).TotalDays >= 1825)
                        {
                            //Если у бухгалтера категория первая и опыт от 3 лет 
                            if ((((Accountant)((Employee)list[i]).Vacancy).Category == Category.ThirdCategory && (list[i].WorkExperience == Experience.From3To6
                                || list[i].WorkExperience == Experience.MoreThan6)) ||
                                //или категория вторая, есть высшее образование и опыт работы более 6 лет
                                //то сотруднику пора повышать квалификацию, значит выводим информацию о нем
                                (((Accountant)((Employee)list[i]).Vacancy).Category == Category.SecondCategory && ((Accountant)((Employee)list[i]).Vacancy).Education == Education.HigherEconomic)
                                && list[i].WorkExperience == Experience.MoreThan6)
                            {
                                dataGridView1.Rows.Add(list[i].Print());
                                comboBox1.Items.Add(((Employee)list[i]).Fio);
                            }                            
                        }
                        if (((Employee)list[i]).Vacancy.Name == "Охранник")
                        {
                            //Если у охранника разряд второй или третий и 
                            if (((((Guard)((Employee)list[i]).Vacancy).Rank == Rank.First || ((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Second) && 
                                //опыт работы имеется
                               (list[i].WorkExperience == Experience.From1To3 || list[i].WorkExperience == Experience.From3To6 || 
                               list[i].WorkExperience == Experience.MoreThan6))
                               //или разряд третий, опыт от 3 лет и есть лицензия
                               //то сотруднику пора повышать квалификацию, значит выводим информацию о нем
                               || (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Third && list[i].WorkExperience == Experience.From3To6 &&
                                ((Guard)((Employee)list[i]).Vacancy).License == true))
                            {
                                dataGridView1.Rows.Add(list[i].Print());
                                comboBox1.Items.Add(((Employee)list[i]).Fio);
                            }
                        }
                    }
            }
            if (comboBox1.Items.Count == 0) comboBox1.Text = "Претенденты на повышение квалификации отсутствуют.";
            if (dataGridView1.Rows.Count == 0) label1.Text = "Претенденты на повышение квалификации отсутствуют.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                for (int i = 0; i < list.Count(); i++)
                    if (list[i].GetType().Name == "Employee")
                    {
                        if (((Employee)list[i]).Vacancy.Name == "Бухгалтер" && (DateTime.Now - ((Accountant)((Employee)list[i]).Vacancy).Date).TotalDays >= 1825)
                        {
                            //Чтобы бухгалтеру третьей категории получить вторую категорию, нужно иметь стаж не менее 3 лет. Значит, проверяем по опыту работы.
                            //Если опыт работы от 3 до 6 лет или более 6 лет, то повышаем категорию до второй
                            if (((Accountant)((Employee)list[i]).Vacancy).Category == Category.ThirdCategory && (list[i].WorkExperience == Experience.From3To6
                                || list[i].WorkExperience == Experience.MoreThan6))
                            {
                                //Изменяем категорию сотрудника, уровень заработной платы и дату повышения квалификации
                                ((Accountant)((Employee)list[i]).Vacancy).Category = Category.SecondCategory;
                                ((Accountant)((Employee)list[i]).Vacancy).Date = DateTime.Now;
                                list[i].IncomeLevel = list[i].IncomeLevel + 0.15M * list[i].IncomeLevel;
                                MessageBox.Show($"\nКвалификация успешно повышена.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                //Чтобы бухгалтеру второй категории получить первую категорию, нужно отработать на должности второй категории не менее 3 лет
                                //и иметь высшее образование. Значит, проверяем по уровню образования и опыту работы
                                //Если есть высшее образование и опыт более 6 лет, то повышаем категорию до первой.
                                if (((Accountant)((Employee)list[i]).Vacancy).Category == Category.SecondCategory && ((Accountant)((Employee)list[i]).Vacancy).Education == Education.HigherEconomic 
                                    && list[i].WorkExperience == Experience.MoreThan6)
                                {
                                    ((Accountant)((Employee)list[i]).Vacancy).Category = Category.FirstCategory;
                                    ((Accountant)((Employee)list[i]).Vacancy).Date = DateTime.Now;
                                    list[i].IncomeLevel = list[i].IncomeLevel + 0.2M * list[i].IncomeLevel;
                                    MessageBox.Show($"\nКвалификация успешно повышена.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        if (((Employee)list[i]).Vacancy.Name == "Охранник")
                        {
                            //Чтобы охраннику первого разряда получить второй, необходим стаж работы не менее года. Сверяем по опыту работы
                            if (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.First && !(list[i].WorkExperience == Experience.None))
                            {
                                ((Guard)((Employee)list[i]).Vacancy).Rank = Rank.Second;
                                MessageBox.Show($"\nКвалификация успешно повышена.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Чтобы охраннику второго разряда получить третий, необходим стаж работы не менее 2 лет. Сверяем по опыту работы
                            else if (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Second && !(list[i].WorkExperience == Experience.From1To3))
                            {
                                ((Guard)((Employee)list[i]).Vacancy).Rank = Rank.Third;
                                MessageBox.Show($"\nКвалификация успешно повышена.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            //Чтобы охраннику третьего разряда получить четвертый, необходима лицензия(удостоверение) и опыт от 3 лет. Сверяем по наличию лицензии и опыту
                            else if (((Guard)((Employee)list[i]).Vacancy).Rank == Rank.Third && (list[i].WorkExperience == Experience.From3To6
                                || list[i].WorkExperience == Experience.MoreThan6) && ((Guard)((Employee)list[i]).Vacancy).License == true)
                            {
                                ((Guard)((Employee)list[i]).Vacancy).Rank = Rank.Fourth;
                                list[i].IncomeLevel = list[i].IncomeLevel + 0.1M * list[i].IncomeLevel;
                                MessageBox.Show($"\nКвалификация успешно повышена.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                Close();
            }
        }
    }
}
