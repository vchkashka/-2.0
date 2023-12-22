using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace курсовая_2._0
{
    internal class ListOfClients
    {
        public List<Client> clients = new List<Client>();

        public Client this[int index]
        {
            get
            {
                if (index >= 0 && index < clients.Count)
                    return clients[index];
                else
                    throw new Exception("Введен некорректный индекс");
            }
            set
            {
                if (index >= 0 && index < clients.Count)
                    clients[index] = value;
            }
        }

        // Подсчет количества элементов в списке
        public int Count()
        {
            return clients.Count();
        }

        //Вставка элемента в список
        public void Insert(Client client)
        {
            clients.Add(client);
        }

        // Редактирование элемента в списке по ФИО
        public void Edit(string fio)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                string s2 = "";
                switch (clients[i].WorkExperience)
                {
                    case Experience.None: s2 = "Нет опыта"; break;
                    case Experience.From1To3: s2 = "От 1 года до 3 лет"; break;
                    case Experience.From3To6: s2 = "От 3 до 6 лет"; break;
                    case Experience.MoreThan6: s2 = "Более 6 лет"; break;
                }
                // Если нужный клиент найден, и это соискатель, то вызываем форму соответствующую для заполнения новых сведений о нем
                if (clients[i].GetType().Name == "Applicant")
                {
                    if (((Applicant)clients[i]).Fio == fio)
                        using (InformationAboutApplicant form = new InformationAboutApplicant())
                        {
                            //Для наглядности заполняем все элементы старыми данными, которые пользователь может отредактировать
                            form.maskedTextBox1.Text = String.Join("", ((Applicant)clients[i]).Number);
                            form.dateTimePicker1.Text = clients[i].Birthdate.ToLongDateString();
                            form.textBox7.Text = clients[i].IncomeLevel.ToString();
                            form.textBox8.Text = ((Applicant)clients[i]).Fio;
                            form.comboBox1.Text = s2;
                            for (int j = 0; j < ((Applicant)clients[i]).Vacancies.Count; j++)
                                for (int h = 0; h < form.checkedListBox1.Items.Count; h++)
                                    if (form.checkedListBox1.Items[h].ToString() == ((Applicant)clients[i]).Vacancies[j].Name)
                                        form.checkedListBox1.SetItemCheckState(h, CheckState.Checked);
                            form.ShowDialog();
                        }
                }
                // Если нужный клиент найден, и это сотрудник, то вызываем соответствующую форму для заполнения новых сведений о нем
                else if (((Employee)clients[i]).Fio == fio)
                    using (InformationAboutEmployee form = new InformationAboutEmployee())
                    {
                        //Для наглядности заполняем все элементы старыми данными, которые пользователь может отредактировать
                        form.maskedTextBox1.Text = String.Join("", ((Employee)clients[i]).Number);
                        form.dateTimePicker2.Text = clients[i].Birthdate.ToLongDateString();
                        form.textBox7.Text = clients[i].IncomeLevel.ToString();
                        form.textBox8.Text = ((Employee)clients[i]).Fio;
                        form.comboBox1.Text = s2;
                        form.comboBox3.Text = ((Employee)clients[i]).Vacancy.Name;
                        form.dateTimePicker1.Text = ((Employee)clients[i]).CertificationDate.ToLongDateString();
                        form.ShowDialog();
                    }
                if (Data.Value != null)
                {
                    clients[i] = Data.Value;
                    break;
                }
            }
        }

        // Удаление элемента из списка по ФИО
        public void Remove(string fio)
        {
            // Проверяем, что указанный клиент существует. Если он сущесвует, то удаляем информацию о нем
            for (int i = 0; i < clients.Count; i++)
                if (clients[i].GetType().Name == "Applicant")
                {
                    if (((Applicant)clients[i]).Fio == fio)
                        clients.RemoveAt(i);
                }
                else if (((Employee)clients[i]).Fio == fio)
                    clients.RemoveAt(i);

        }

        // Печать списка соискателей
        public string[][] PrintApplicants()
        {
            int k = 0;
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].GetType().Name == "Applicant")
                    k++;
            }
            int j = 0;
            string[][] List = new string[k][];
            if (clients.Count > 0)
                for (int i = 0; i < clients.Count; i++)
                    if (clients[i].GetType().Name == "Applicant")
                    {
                        List[j] = clients[i].Print();
                        j++;
                    }

            return List;
        }

        // Печать списка сотрудников
        public string[][] PrintEmployees()
        {
            int k = 0;
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].GetType().Name == "Employee")
                    k++;
            }
            int j = 0;
            string[][] List = new string[k][];
            if (clients.Count > 0)
                for (int i = 0; i < clients.Count; i++)
                    if (clients[i].GetType().Name == "Employee")
                    {
                        List[j] = clients[i].Print();
                        j++;
                    }
            return List;
        }

        //Сохранение данных в файл
        public void SaveToFile(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    foreach (var client in clients)
                    {
                        writer.WriteLine($"{client.GetType().Name}|{client.PrintToFile()}");
                    }
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        //Чтение данных из файла
        public void LoadFromFile(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split('|');
                        string clientType = data[0];
                        switch (clientType)
                        {
                            case "Applicant":
                                string fio = data[1];
                                char[] number = data[2].ToCharArray();
                                DateTime birthdate = DateTime.FromFileTime((long)Convert.ToDouble(data[3]));
                                Experience workExperience = Experience.EMPTY;
                                switch (data[4])
                                {
                                    case "Нет опыта": workExperience = Experience.None; break;
                                    case "От 1 года до 3 лет": workExperience = Experience.From1To3; break;
                                    case "От 3 до 6 лет": workExperience = Experience.From3To6; break;
                                    case "Более 6 лет": workExperience = Experience.MoreThan6; break;
                                }
                                string nameVacancy = null;
                                decimal incomeLevel = Convert.ToInt32(data[5]);
                                List<Vacancy> vacancies = new List<Vacancy>();
                                Insert(new Applicant(fio, number, workExperience, birthdate, incomeLevel, vacancies));
                                for (int i = 6; i < data.Length; i++)
                                    switch (data[i])
                                    {
                                        case "Бухгалтер":
                                            nameVacancy = data[i];
                                            Education education = Education.EMPTY;
                                            switch (data[i + 1])
                                            {
                                                case "Высшее экономическое": education = Education.HigherEconomic; break;
                                                case "Среднее профессиональное": education = Education.SecondaryProfessional; break;
                                            }
                                            Category category = Category.EMPTY;
                                            switch (data[i + 2])
                                            {
                                                case "Первая категория": category = Category.FirstCategory; break;
                                                case "Вторая категория": category = Category.SecondCategory; break;
                                                case "Третья категория": category = Category.ThirdCategory; break;
                                            }
                                            DateTime date = DateTime.FromFileTime((long)Convert.ToDouble(data[i + 3]));
                                            vacancies.Add(new Accountant(nameVacancy, education, category, date));
                                            for (int j = 0; j < clients.Count(); j++)
                                                if (clients[j].GetType().Name == "Applicant")
                                                    if (fio == ((Applicant)clients[j]).Fio)
                                                        ((Applicant)clients[j]).Vacancies = vacancies;
                                            break;
                                        case "Охранник":
                                            nameVacancy = data[i];
                                            bool license = false; Rank rank = Rank.EMPTY;
                                            if (data[i + 1] == "Присутствует") license = true;
                                            else if (data[i + 1] == "Отсутствует") license = false;
                                            switch (data[i + 2])
                                            {
                                                case "Первый разряд": rank = Rank.First; break;
                                                case "Второй разряд": rank = Rank.Second; break;
                                                case "Третий разряд": rank = Rank.Third; break;
                                                case "Четвертый разряд": rank = Rank.Fourth; break;
                                            }
                                            vacancies.Add(new Guard(nameVacancy, license, rank));
                                            for (int j = 0; j < clients.Count(); j++)
                                                if (clients[j].GetType().Name == "Applicant")
                                                    if (fio == ((Applicant)clients[j]).Fio)
                                                        ((Applicant)clients[j]).Vacancies = vacancies;
                                            break;
                                        case "Уборщик":
                                            nameVacancy = data[i];
                                            vacancies.Add(new Cleaner(nameVacancy));
                                            for (int j = 0; j < clients.Count(); j++)
                                                if (clients[j].GetType().Name == "Applicant")
                                                    if (fio == ((Applicant)clients[j]).Fio)
                                                        ((Applicant)clients[j]).Vacancies = vacancies;
                                            break;
                                    }
                                break;

                            case "Employee":
                                fio = data[1];
                                number = data[2].ToCharArray();
                                birthdate = DateTime.FromFileTime((long)Convert.ToDouble(data[3]));
                                workExperience = Experience.EMPTY;
                                switch (data[4])
                                {
                                    case "Нет опыта": workExperience = Experience.None; break;
                                    case "От 1 года до 3 лет": workExperience = Experience.From1To3; break;
                                    case "От 3 до 6 лет": workExperience = Experience.From3To6; break;
                                    case "Более 6 лет": workExperience = Experience.MoreThan6; break;
                                }
                                incomeLevel = Convert.ToInt32(data[5]);
                                nameVacancy = data[6];
                                switch (nameVacancy)
                                {
                                    case "Бухгалтер":
                                        Education education = Education.EMPTY;
                                        switch (data[7])
                                        {
                                            case "Высшее экономическое": education = Education.HigherEconomic; break;
                                            case "Среднее профессиональное": education = Education.SecondaryProfessional; break;
                                        }
                                        Category category = Category.EMPTY;
                                        switch (data[8])
                                        {
                                            case "Первая категория": category = Category.FirstCategory; break;
                                            case "Вторая категория": category = Category.SecondCategory; break;
                                            case "Третья категория": category = Category.ThirdCategory; break;
                                        }
                                        DateTime certification_date = DateTime.FromFileTime((long)Convert.ToDouble(data[10]));
                                        Insert(new Employee(fio, number, workExperience, birthdate, incomeLevel, new Accountant(nameVacancy, education, category, DateTime.FromFileTime((long)Convert.ToDouble(data[9]))), certification_date));
                                        break;
                                    case "Охранник":
                                        bool license = false; Rank rank = Rank.EMPTY;
                                        if (data[7] == "Присутствует") license = true;
                                        else if (data[7] == "Отсутствует") license = false;
                                        switch (data[8])
                                        {
                                            case "Первый разряд": rank = Rank.First; break;
                                            case "Второй разряд": rank = Rank.Second; break;
                                            case "Третий разряд": rank = Rank.Third; break;
                                            case "Четвертый разряд": rank = Rank.Fourth; break;
                                        }
                                        certification_date = DateTime.FromFileTime((long)Convert.ToDouble(data[9]));
                                        Insert(new Employee(fio, number, workExperience, birthdate, incomeLevel, new Guard(nameVacancy, license, rank), certification_date));
                                        break;
                                    case "Уборщик":
                                        certification_date = DateTime.FromFileTime((long)Convert.ToDouble(data[7]));
                                        Insert(new Employee(fio, number, workExperience, birthdate, incomeLevel, new Cleaner(nameVacancy), certification_date));
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }
    }
}
