using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace курсовая_2._0
{

    public partial class EmployeeForm : Form
    {
        ListOfClients list = new ListOfClients();
        internal EmployeeForm(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
            label4.Hide();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < list.PrintEmployees().Count(); i++)
                dataGridView1.Rows.Add(list.PrintEmployees()[i]);
            if (list.PrintEmployees().Count() == 0)
            {
                dataGridView1.Hide();
                label4.Show();
                label4.Text = "Список пуст.";
            }
            if (list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].GetType().Name == "Employee")
                    {
                        //Заполняем строки comboBox1 ФИО сотрудников для возможности выбрать, о каком сотруднике мы хотим редактировать информацию
                        comboBox1.Items.Add(((Employee)list[i]).Fio);
                        //Заполняем строки comboBox2 ФИО сотрудников для возможности выбрать, кого мы хотим удалить
                        comboBox2.Items.Add(((Employee)list[i]).Fio);
                    }
                }
            }
            else
            {
                comboBox1.Items.Add("Список сотрудников пуст");
                comboBox2.Items.Add("Список сотрудников пуст");
            }
        }

        //Добавление сотрудника
        private void Insert(object sender, EventArgs e)
        {
            try
            {
                label4.Hide();
                comboBox1.Text = "";
                comboBox2.Text = "";
                label1.Text = "Список сотрудников:";
                dataGridView1.Rows.Clear();
                if (list.PrintEmployees().Count() > 0)
                {
                    dataGridView1.Show();
                    for (int i = 0; i < list.PrintEmployees().Count(); i++)
                        dataGridView1.Rows.Add(list.PrintEmployees()[i]);
                }
                else
                {
                    dataGridView1.Hide();
                    label4.Show();
                    label4.Text = "Список пуст.";
                }
                //Вызываем форму для заполнения информации о сотруднике
                using (InformationAboutEmployee form = new InformationAboutEmployee())
                {
                    form.ShowDialog(this);
                }
                if (Data.Value != null)
                {
                    //Добавляем сотрудника в список
                    list.Insert(Data.Value);
                    //Обновляем строки всех combobox
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].GetType().Name == "Employee")
                        {
                            comboBox1.Items.Add(((Employee)list[i]).Fio);
                            comboBox2.Items.Add(((Employee)list[i]).Fio);
                        }
                    }
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                }
                if (Data.Value != null)
                {
                    label4.Hide();
                    dataGridView1.Show();
                    //Выводим информацию о добавленном сотруднике
                    dataGridView1.Rows.Add(Data.Value.Print());
                }

                Data.Value = null;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Редактирование информации о сотруднике
        private void Edit(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex > -1 && comboBox1.SelectedItem.ToString() != "Список сотрудников пуст")
                {
                    label4.Hide();
                    label1.Text = "Список сотрудников:";
                    dataGridView1.Rows.Clear();
                    if (list.PrintEmployees().Count() > 0)
                        for (int i = 0; i < list.PrintEmployees().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintEmployees()[i]);
                    else
                    {
                        dataGridView1.Hide();
                        label4.Show();
                        label4.Text = "Список пуст.";
                    }
                    //Редактируем информацию о выбранном сотруднике
                    list.Edit(comboBox1.Text);
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                    dataGridView1.Rows.Clear();
                    //Обновляем строки всех combobox
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].GetType().Name == "Employee")
                        {
                            comboBox1.Items.Add(((Employee)list[i]).Fio);
                            comboBox2.Items.Add(((Employee)list[i]).Fio);
                        }
                    }
                    //Выводим отредактированный список сотрудников
                    if (list.PrintEmployees().Count() > 0)
                        for (int i = 0; i < list.PrintEmployees().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintEmployees()[i]);
                    else
                    {
                        dataGridView1.Hide();
                        label4.Show();
                        label4.Text = "Список пуст.";
                    }
                    Data.Value = null;
                    comboBox1.Text = "";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Удаление сотрудника
        private void Remove(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedIndex > -1 && comboBox2.SelectedItem.ToString() != "Список сотрудников пуст")
                {
                    label4.Hide();
                    label1.Text = "Список сотрудников:";
                    dataGridView1.Rows.Clear();
                    if (list.PrintEmployees().Count() > 0)
                    {
                        dataGridView1.Show();
                        for (int i = 0; i < list.PrintEmployees().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintEmployees()[i]);
                    }
                    else
                    {
                        dataGridView1.Hide();
                        label4.Show();
                        label4.Text = "Список пуст.";
                    }
                    //Удаляем выбранного из списка сотрудника
                    for (int i = 0; i < comboBox2.Items.Count; i++)
                    {
                        if (i == comboBox2.SelectedIndex)
                        {
                            list.Remove(comboBox2.Text);
                            comboBox2.Items.RemoveAt(i);
                            comboBox1.Items.RemoveAt(i);
                            break;
                        }
                    }
                    //Сохраняем в файл обновленный список
                    list.SaveToFile("AWP.txt");
                    dataGridView1.Rows.Clear();
                    if (list.PrintEmployees().Count() > 0)
                    {
                        dataGridView1.Show();
                        for (int i = 0; i < list.PrintEmployees().Count(); i++)
                            dataGridView1.Rows.Add(list.PrintEmployees()[i]);
                    }
                    else
                    {
                        dataGridView1.Hide();
                        label4.Show();
                        label4.Text = "Список пуст.";
                    }
                    //Выводим отредактированный список работодателей
                    if (list.Count() == 0)
                    {
                        comboBox1.Items.Add("Список сотрудников пуст");
                        comboBox2.Items.Add("Список сотрудников пуст");
                    }
                    comboBox2.Text = "";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\n{exception.Message} \n", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Вывод информации о том, каких работников организации не хватает
        private void button4_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Hide();
            label4.Show();
            label4.Text = "";
            int k = 0;
            string[] vacancies = new string[] { "Бухгалтер", "Охранник", "Уборщик" };
            //Сравниваем по названиям должностей: если в списке клиентов нет информации о сотруднике на данной должности,
            //то выводим название этой должности в richTextBox1
            for (int j = 0; j < vacancies.Count(); j++)
                if (list.Count() > 0)
                {
                    for (int i = 0; i < list.Count(); i++)
                        if (list[i].GetType().Name == "Employee")
                            if (((Employee)list[i]).Vacancy.Name == vacancies[j])
                                k++;
                    if (k == 0)
                    {
                        label1.Text = "Список открытых должностей:";
                        label4.Text += vacancies[j] + '\n';
                    }
                    k = 0;
                }
                else label4.Text += vacancies[j] + '\n';
            if (label4.Text == "")
            {
                label1.Text = "Список открытых должностей:";
                label4.Text += "В данный момент на предприятии все должности заняты.";
            }
        }

        //Повышение квалификации сотрудников
        private void button5_Click(object sender, EventArgs e)
        {
            label4.Hide();
            //Вызываем форму для отображения того, каким сотрудникам необходимо повысить квалификацию
            using (QualificationForm form = new QualificationForm(list))
            {
                form.ShowDialog(this);
            }
            dataGridView1.Rows.Clear();
            //Выводим отредактированный список
            dataGridView1.Rows.Clear();
            if (list.PrintEmployees().Count() > 0)
            {
                dataGridView1.Show();
                for (int i = 0; i < list.PrintEmployees().Count(); i++)
                    dataGridView1.Rows.Add(list.PrintEmployees()[i]);
            }
            else
            {
                dataGridView1.Hide();
                label4.Show();
                label4.Text = "Список пуст.";
            }
            //Сохраняем в файл обновленный список
            list.SaveToFile("AWP.txt");
        }

        //Аттестация сотрудников
        private void button6_Click(object sender, EventArgs e)
        {
            label4.Hide();
            //Вызываем форму для отображения того, каким сотрудникам необходимо пройти аттестацию
            using (CertificationForm form = new CertificationForm(list))
            {
                form.ShowDialog(this);
            }
            //Выводим отредактированный список
            dataGridView1.Rows.Clear();
            if (list.PrintEmployees().Count() > 0)
            {
                dataGridView1.Show();
                for (int i = 0; i < list.PrintEmployees().Count(); i++)
                    dataGridView1.Rows.Add(list.PrintEmployees()[i]);
            }
            else
            {
                dataGridView1.Hide();
                label4.Show();
                label4.Text = "Список пуст.";
            }
            //Сохраняем в файл обновленный список
            list.SaveToFile("AWP.txt");
        }
    }
}

