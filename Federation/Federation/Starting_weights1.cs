using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Federation
{
    public partial class Starting_weights1 : Form
    {

        DataBase dataBase = new DataBase();

        public Starting_weights1()
        {
            InitializeComponent();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0)
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK);
            else
            {
                dataBase.openConnection();
                SqlCommand command = new SqlCommand(DataBank.Query[0], dataBase.GetConnection());
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                string Query = $"SELECT MAX(app_num) FROM applications";
                command = new SqlCommand(Query, dataBase.GetConnection());
                reader = command.ExecuteReader();
                reader.Read();
                var app_num = reader.GetValue(0);
                reader.Close();

                DataBank.Query[1] += $"{app_num}, {textBox1.Text.Replace(",", ".")})";
                command = new SqlCommand(DataBank.Query[1], dataBase.GetConnection());
                reader = command.ExecuteReader();
                reader.Close();
                dataBase.closeConnection();

                Form form1 = new Applications();
                this.Dispose();
                form1.Show();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Applications();
            this.Dispose();
            form1.Show();
        }

        private void Starting_weights1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
