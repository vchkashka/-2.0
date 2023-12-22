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
    public partial class AWP : Form
    {
        ListOfClients list;
        internal AWP(ListOfClients list)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.MaximizeBox = false;
            this.list = list;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            //Вызываем форму для работы с соикателями, куда передаем список сотрудников и соискателей, загруженный из файла
            using (ApplicantForm form = new ApplicantForm(list))
            {
                form.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Вызываем форму для работы с работодателями, куда передаем список сотрудников и соискателей, загруженный из файла
            using (EmployeeForm form = new EmployeeForm(list))
            {
                form.ShowDialog(this);
            }
        }

        private void AWP_Load(object sender, EventArgs e)
        {

        }
    }
}
