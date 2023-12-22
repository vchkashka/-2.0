using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace курсовая_2._0
{
    public partial class ApplicantForm : Form
    {
        ListOfClients list;
        internal ApplicantForm(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
            label5.Hide();
            //Выводим список соискателей
            for (int i = 0; i < list.PrintApplicants().Count(); i++)
                dataGridView1.Rows.Add(list.PrintApplicants()[i]);
            if (list.PrintApplicants().Count() == 0)
            {
                dataGridView1.Hide();
                label5.Show();
                label5.Text = "Список пуст.";
            }
            if (list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].GetType().Name == "Applicant")
                    {
                        //Заполняем строки comboBox1 ФИО соискателей для возможности выбрать соискателя, которого мы хотим взять на работу
                        for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                        {
                            string str = ((Applicant)list[i]).Fio + ',' + ((Applicant)list[i]).Vacancies[j].Name;
                            comboBox1.Items.Add(str);
                        }
                        //Заполняем строки comboBox2 ФИО соискателей для возможности выбрать, каком соискателе необходимо редактировать информацию
                        comboBox2.Items.Add(((Applicant)list[i]).Fio);
                        //Заполняем строки comboBox3 ФИО соискателей для возможности выбрать, кого мы хотим удалить
                        comboBox3.Items.Add(((Applicant)list[i]).Fio);
                    }

                }
            }
            else
            {
                comboBox1.Items.Add("Список соискателей пуст");
                comboBox2.Items.Add("Список соискателей пуст");
                comboBox3.Items.Add("Список соискателей пуст");
            }
        }

        //Добавление соискателя
        private void Insert(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                label5.Hide();
                label1.Text = "Список соискателей:";
                dataGridView1.Rows.Clear();
                if (list.PrintApplicants().Count() > 0)
                {
                    dataGridView1.Show();
                    for (int i = 0; i < list.PrintApplicants().Count(); i++)
                        dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                }
                else
                {
                    label5.Show();
                    dataGridView1.Hide();
                    label5.Text = "Список пуст.";
                }
                //Вызываем форму для заполнения информации о соикателе
                using (InformationAboutApplicant form = new InformationAboutApplicant())
                {
                    form.ShowDialog(this);
                }
                //Если из формы InformationAboutApplicant получены данные, то
                if (Data.Value != null)
                {
                    //Добавляем соискателя в список
                    list.Insert(Data.Value);
                    //После добавления нового соискателя обновляем строки всех combobox
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].GetType().Name == "Applicant")
                        {
                            for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                            {
                                string str = ((Applicant)list[i]).Fio + ',' + ((Applicant)list[i]).Vacancies[j].Name;
                                comboBox1.Items.Add(str);
                            }
                            comboBox2.Items.Add(((Applicant)list[i]).Fio);
                            comboBox3.Items.Add(((Applicant)list[i]).Fio);
                        }
                    }
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                }
                if (list.PrintApplicants().Count() > 0)
                {
                    label5.Hide();
                    dataGridView1.Show();
                    if (Data.Value != null)
                        //Выводим информацию о добавленном соискателе
                        dataGridView1.Rows.Add(Data.Value.Print());
                }
                Data.Value = null;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Редактирование информации о соискателе
        private void Edit(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedIndex > -1 && comboBox2.SelectedItem.ToString() != "Список соискателей пуст")
                {
                    label5.Hide();
                    label1.Text = "Список соискателей:";
                    dataGridView1.Rows.Clear();
                    if (list.PrintApplicants().Count() > 0)
                    {
                        for (int i = 0; i < list.PrintApplicants().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                    }
                    else
                    {
                        label5.Show();
                        dataGridView1.Hide();
                        label5.Text = "Список пуст.";
                    }
                    //Редактируем информацию о выбранном соискателе
                    list.Edit(comboBox2.Text);
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                    //Очищаем оконо вывода информации
                    dataGridView1.Rows.Clear();
                    //Обновляем строки всех combobox
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].GetType().Name == "Applicant")
                        {
                            for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                            {
                                string str = ((Applicant)list[i]).Fio + ',' + ((Applicant)list[i]).Vacancies[j].Name;
                                comboBox1.Items.Add(str);
                            }
                            comboBox2.Items.Add(((Applicant)list[i]).Fio);
                            comboBox3.Items.Add(((Applicant)list[i]).Fio);
                        }
                    }

                    //Выводим отредактированный список 
                    if (list.PrintApplicants().Count() > 0)
                    {
                        label5.Hide();
                        for (int i = 0; i < list.PrintApplicants().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                    }
                    Data.Value = null;
                    comboBox2.Text = "";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Удаление соискателя
        private void Remove(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedIndex > -1 && comboBox3.SelectedItem.ToString() != "Список соискателей пуст")
                {
                    label5.Hide();
                    label1.Text = "Список соискателей:";
                    dataGridView1.Rows.Clear();
                    if (list.PrintApplicants().Count() > 0)
                    {
                        dataGridView1.Show();
                        for (int i = 0; i < list.PrintApplicants().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                    }
                    else
                    {
                        label5.Show();
                        dataGridView1.Hide();
                        label5.Text = "Список пуст.";
                    }
                    for (int i = 0; i < comboBox3.Items.Count; i++)
                    {
                        if (i == comboBox3.SelectedIndex)
                        {
                            //Удаляем выбранного соискателя из списка
                            list.Remove(comboBox3.Text);
                            //Удаляем выбранного соискателя из combobox
                            comboBox3.Items.RemoveAt(i);
                            comboBox2.Items.RemoveAt(i);
                            comboBox1.Items.RemoveAt(i);
                            break;
                        }
                    }
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                    dataGridView1.Rows.Clear();
                    if (list.PrintApplicants().Count() > 0)
                    {
                        //Выводим отредактированный список
                        for (int i = 0; i < list.PrintApplicants().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                    }
                    else
                    {
                        label5.Show();
                        dataGridView1.Hide();
                        label5.Text = "Список пуст.";
                        comboBox1.Items.Add("Список соискателей пуст");
                        comboBox2.Items.Add("Список соискателей пуст");
                        comboBox3.Items.Add("Список соискателей пуст");
                    }
                    comboBox3.Text = "";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Поиск соискателей
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //Вызываем форму для заполнения критериев и поиска по ним соискателей
                using (SearchApplicant form = new SearchApplicant(list))
                {
                    form.ShowDialog(this);
                }
                dataGridView1.Rows.Clear();
                if (list.Count() > 0)
                {
                    //Выводим отредактированный список соискателей
                    for (int i = 0; i < list.PrintApplicants().Count(); i++)
                        dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    label5.Show();
                    dataGridView1.Hide();
                    label5.Text = "Список пуст.";
                }
                //Сохраняем в файл обновленный список
                list.SaveToFile("AWP.txt");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Найм соискателей
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != null)
                for (int i = 0; i < list.Count(); i++)
                {
                    string[] str = comboBox1.Text.ToString().Split(',');
                    //Прежде чем нанять соискателя, проводим аттестацию - проверяем на соответсвие квалификации его навыков и знаний соискателя 
                    if (list[i].GetType().Name == "Applicant" && ((Applicant)list[i]).Fio == str[0])
                    {
                        Employee employee;
                        for (int j = 0; j < ((Applicant)list[i]).Vacancies.Count; j++)
                            if ((((Applicant)list[i]).Vacancies[j]).Name == "Бухгалтер" && (((Applicant)list[i]).Vacancies[j]).Name == str[1])
                            {
                                if ((((Accountant)((Applicant)list[i]).Vacancies[j]).Category == Category.SecondCategory && (list[i].WorkExperience != Experience.From3To6 || list[i].WorkExperience != Experience.MoreThan6)) ||
                                     (((Accountant)((Applicant)list[i]).Vacancies[j]).Category == Category.FirstCategory && (((Accountant)((Applicant)list[i]).Vacancies[j]).Education != Education.HigherEconomic) && list[i].WorkExperience == Experience.MoreThan6))
                                {
                                    //Если навыки соискателя не соответствуют его квалификации, то выводим предупреждающее сообщение
                                    DialogResult result = MessageBox.Show($"\nВнимание! Навыки соиcкателя не соответствуют его квалификации. Вы все равно хотите его нанять?\n", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                    //Если пользователь нажимает "Нет", то возращаемся к исходному окну
                                    if (result == DialogResult.No) break;
                                    //Если пользователь нажимает "Да", то добавляем соискателя так, как есть. При этом дату аттестации указываем текущую
                                    else if (result == DialogResult.Yes)
                                    {
                                        comboBox1.Items.Remove(((Applicant)list[i]).Fio);
                                        employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                    list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                        //Удаляем бывшего соискателя
                                        list.Remove(((Applicant)list[i]).Fio);
                                        //Добавляем нового сотрудника
                                        list.Insert(employee);
                                        //Выводим отредактированный список соискателей
                                        for (i = 0; i < list.PrintApplicants().Count(); i++)
                                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                        if (list.PrintApplicants().Count() == 0)
                                        {
                                            dataGridView1.Hide();
                                        }
                                        comboBox1.Text = "";
                                        MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        break;
                                    }
                                }
                                //Если навыки соискателя полностью соответствуют его квалификации, то без проблем нанимаем его на работу
                                else
                                {
                                    comboBox1.Items.Remove(((Applicant)list[i]).Fio);
                                    employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                    list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                    //Удаляем бывшего соискателя
                                    list.Remove(((Applicant)list[i]).Fio);
                                    //Добавляем нового сотрудника
                                    list.Insert(employee);
                                    //Выводим отредактированный список соискателей
                                    for (i = 0; i < list.PrintApplicants().Count(); i++)
                                        dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                    if (list.PrintApplicants().Count() == 0)
                                    {
                                        dataGridView1.Hide();
                                    }
                                    comboBox1.Text = "";
                                    MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                        comboBox1.Items.Remove(((Applicant)list[i]).Fio);
                                        employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                    list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                        list.Remove(((Applicant)list[i]).Fio);
                                        list.Insert(employee);
                                        for (i = 0; i < list.PrintApplicants().Count(); i++)
                                            dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                        if (list.PrintApplicants().Count() == 0)
                                        {
                                            dataGridView1.Hide();
                                        }
                                        comboBox1.Text = "";
                                        MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        break;
                                    }
                                }
                                else
                                {
                                    comboBox1.Items.Remove(((Applicant)list[i]).Fio);
                                    employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                    list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                    list.Remove(((Applicant)list[i]).Fio);
                                    list.Insert(employee);
                                    for (i = 0; i < list.PrintApplicants().Count(); i++)
                                        dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                    if (list.PrintApplicants().Count() == 0)
                                    {
                                        dataGridView1.Hide();
                                    }
                                    comboBox1.Text = "";
                                    MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                            }
                            else if ((((Applicant)list[i]).Vacancies[j]).Name == str[1])
                            {
                                comboBox1.Items.Remove(((Applicant)list[i]).Fio);
                                employee = new Employee(((Applicant)list[i]).Fio, ((Applicant)list[i]).Number, list[i].WorkExperience, list[i].Birthdate,
                                    list[i].IncomeLevel, ((Applicant)list[i]).Vacancies[j], DateTime.Now);
                                list.Remove(((Applicant)list[i]).Fio);
                                list.Insert(employee);
                                for (i = 0; i < list.PrintApplicants().Count(); i++)
                                    dataGridView1.Rows.Add(list.PrintApplicants()[i]);
                                if (list.PrintApplicants().Count() == 0)
                                {
                                    dataGridView1.Hide();
                                }
                                comboBox1.Text = "";
                                MessageBox.Show($"\nНовый сотрудник успешно добавлен.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                    }
                }
            list.SaveToFile("AWP.txt");
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
