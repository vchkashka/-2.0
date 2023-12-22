using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсовая_2._0
{
    public partial class SearchApplicant : Form
    {
        ListOfClients list;
        internal SearchApplicant(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
            dataGridView1.Hide();
            label4.Hide();
            if (list.Count() == 0) comboBox4.Text = "Список соискателей пуст.";
        }

        private void SearchEmployerForApplicant_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int k = 0;
            dataGridView1.Rows.Clear();
            comboBox4.Items.Clear();
            comboBox4.Text = "";
            Experience ex = Experience.EMPTY;
            switch (comboBox1.Text)
            {
                case "Нет опыта": ex = Experience.None; break;
                case "От 1 года до 3 лет": ex = Experience.From1To3; break;
                case "От 3 до 6 лет": ex = Experience.From3To6; break;
                case "Более 6 лет": ex = Experience.MoreThan6; break;
                default: ex = Experience.EMPTY; break;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].GetType().Name == "Applicant") 
                for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                    if (((Applicant)list[i]).Vacancies[j].Name == comboBox3.Text)
                        k++;
                if (!(comboBox3.SelectedIndex < 0 && ex == Experience.EMPTY && textBox6.Text == "" && textBox7.Text == ""))
                {
                    //Соискатель подходит по критериям, если 
                    if (list[i].GetType().Name == "Applicant" &&
                        //если название вакансии задано и совпадает с одной из вакансий соискателя, либо не задано
                        (k > 0 || comboBox3.SelectedIndex < 0) &&
                        //если опыт работы задан и совпадает с опытом работы соискателя, либо не задан
                        (ex == Experience.EMPTY || list[i].WorkExperience == ex) &&
                        //если возраст задан и совпадает с возрастом соискателя, либо не задан
                        (textBox6.Text == "" || (DateTime.Now - list[i].Birthdate).TotalDays / 365 >= Convert.ToInt32(textBox6.Text))
                        //если уровень дохода задан и совпадает с уровенем дохода соискателя, либо не задан
                        && (textBox7.Text == "" || list[i].IncomeLevel <= Convert.ToInt32(textBox7.Text)))
                    {
                        label4.Hide();
                        dataGridView1.Show();
                        dataGridView1.Rows.Add(list[i].Print());
                        for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                        {
                            string str = ((Applicant)list[i]).Fio + ',' + ((Applicant)list[i]).Vacancies[j].Name;
                            comboBox4.Items.Add(str);
                        }
                    }
                    if (dataGridView1.Rows.Count == 0) 
                    {
                        dataGridView1.Hide();
                        label4.Show();
                        label4.Text = "По данным критериям не найден ни один соискатель.";
                    }                    
                }
                else
                {
                    dataGridView1.Hide();
                    label4.Show();
                    label4.Text = "Будьте внимательнее. Вы не указали ни один из критериев поиска.";
                }
                k = 0;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (list.Count() > 0)
            {
                if (comboBox4.Text != null && comboBox4.Text != "Список соискателей пуст.")
                    for (int i = 0; i < list.Count(); i++)
                    {
                        string[] str = comboBox4.Text.ToString().Split(',');
                        if (list[i].GetType().Name == "Applicant" && ((Applicant)list[i]).Fio == str[0])
                            for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)                            
                            {
                                Employee employee;
                                if (((Applicant)list[i]).Vacancies[j].Name == "Бухгалтер" && (((Applicant)list[i]).Vacancies[j]).Name == str[1])
                                {
                                    if ((((Accountant)((Applicant)list[i]).Vacancies[j]).Category == Category.SecondCategory && (list[i].WorkExperience != Experience.From3To6 || list[i].WorkExperience != Experience.MoreThan6)) ||
                                         (((Accountant)((Applicant)list[i]).Vacancies[j]).Category == Category.FirstCategory && (((Accountant)((Applicant)list[i]).Vacancies[j]).Education != Education.HigherEconomic)))
                                    {
                                        //Если навыки соискателя не соответствуют его квалификации, то выводим предупреждающее сообщение
                                        DialogResult result = MessageBox.Show($"\nВнимание! Навыки соиcкателя не соответствуют его квалификации. Вы все равно хотите его нанять?\n", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        //Если пользователь нажимает "Нет", то возращаемся к исходному окну
                                        if (result == DialogResult.No) break;
                                        //Если пользователь нажимает "Да", то добавляем соискателя так, как есть. При этом дату аттестации указываем текущую
                                        else if (result == DialogResult.Yes)
                                        {
                                            comboBox4.Items.Remove(((Applicant)list[i]).Fio);
                                            employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                        list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                            //Удаляем бывшего соискателя
                                            list.Remove(((Applicant)list[i]).Fio);
                                            //Добавляем нового сотрудника
                                            list.Insert(employee);
                                            //Выводим отредактированный список соискателей
                                            if (list.PrintApplicants().Count() > 0)
                                            {
                                                label4.Hide();
                                                dataGridView1.Show();
                                                for (i = 0; i < list.PrintApplicants().Count(); i++)
                                                    dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                            }
                                            comboBox4.Text = "";
                                            MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Close();
                                            break;
                                        }
                                    }
                                    //Если навыки соискателя полностью соответствуют его квалификации, то без проблем нанимаем его на работу
                                    else
                                    {
                                        comboBox4.Items.Remove(((Applicant)list[i]).Fio);
                                        employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                        list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                        //Удаляем бывшего соискателя
                                        list.Remove(((Applicant)list[i]).Fio);
                                        //Добавляем нового сотрудника
                                        list.Insert(employee);
                                        //Выводим отредактированный список соискателей
                                        if (list.PrintApplicants().Count() > 0)
                                        {
                                            label4.Hide();
                                            dataGridView1.Show();
                                            for (i = 0; i < list.PrintApplicants().Count(); i++)
                                                dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                        }
                                        MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        comboBox4.Text = "";
                                        Close();
                                        break;
                                    }
                                }
                                else if (((Applicant)list[i]).Vacancies[j].Name == "Охранник" && (((Applicant)list[i]).Vacancies[j]).Name == str[1])
                                {
                                    if ((((Guard)((Applicant)list[i]).Vacancies[j]).Rank == Rank.Second && list[i].WorkExperience != Experience.From1To3) ||
                                (((Guard)((Applicant)list[i]).Vacancies[j]).Rank == Rank.Third && list[i].WorkExperience != Experience.From1To3) ||
                                (((Guard)((Applicant)list[i]).Vacancies[j]).Rank == Rank.Fourth && (list[i].WorkExperience != Experience.From3To6 || ((Guard)((Applicant)list[i]).Vacancies[j]).License == false)))
                                    {
                                        DialogResult result = MessageBox.Show($"\nВнимание! Навыки соиcкателя не соответствуют его квалификации. Вы все равно хотите его нанять?\n", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        if (result == DialogResult.No) break;
                                        else if (result == DialogResult.Yes)
                                        {
                                            comboBox4.Items.Remove(((Applicant)list[i]).Fio);
                                            employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                        list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                            list.Remove(((Applicant)list[i]).Fio);
                                            list.Insert(employee);
                                            if (list.PrintApplicants().Count() > 0)
                                            {
                                                label4.Hide();
                                                dataGridView1.Show();
                                                for (i = 0; i < list.PrintApplicants().Count(); i++)
                                                    dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                            }
                                            MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            comboBox4.Text = "";
                                            Close();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        comboBox4.Items.Remove(((Applicant)list[i]).Fio);
                                        employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                        list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                        list.Remove(((Applicant)list[i]).Fio);
                                        list.Insert(employee);
                                        if (list.PrintApplicants().Count() > 0)
                                        {
                                            label4.Hide();
                                            dataGridView1.Show();
                                            for (i = 0; i < list.PrintApplicants().Count(); i++)
                                                dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                        }
                                        MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        comboBox4.Text = "";
                                        Close();
                                        break;
                                    }
                                }
                                else if ((((Applicant)list[i]).Vacancies[j]).Name == str[1])
                                {
                                    comboBox4.Items.Remove(((Applicant)list[i]).Fio);
                                    employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                        list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                    list.Remove(((Applicant)list[i]).Fio);
                                    list.Insert(employee);
                                    if (list.PrintApplicants().Count() > 0)
                                    {
                                        label4.Hide();
                                        dataGridView1.Show();
                                        for (i = 0; i < list.PrintApplicants().Count(); i++)
                                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                    }
                                    MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    comboBox4.Text = "";
                                    Close();
                                    break;
                                }
                            }
                    }
            }
        }
    }
}
