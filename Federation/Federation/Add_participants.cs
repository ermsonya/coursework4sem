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
    public partial class Add_participants : Form
    {

        DataBase dataBase = new DataBase();
        public Add_participants()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBox1.Text == null) || (textBox2.Text == null) || (textBox4.Text == null) || (maskedTextBox1.Text == null))
                    MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButtons.OK);
                else
                {
                    if (Convert.ToDateTime(maskedTextBox1.Text) > DateTime.Now)
                        MessageBox.Show("Введите корректную дату рождения", "Ошибка", MessageBoxButtons.OK);
                    else
                    {
                        if (!radioButton1.Checked && !radioButton2.Checked)
                            MessageBox.Show("Выберите пол", "Ошибка", MessageBoxButtons.OK);
                        else
                        {
                            DateTime date = Convert.ToDateTime(maskedTextBox1.Text);
                            var date_of_birth = date.ToString("yyyy-MM-dd");
                            dataBase.openConnection();
                            string addQuery = $"INSERT INTO participants (passport_data, FIO, date_of_birth, city, sex) values ('{textBox1.Text}', '{textBox2.Text}', '{date_of_birth}', '{textBox4.Text}', ";
                            if(radioButton1.Checked)
                                addQuery += "'ж')";
                            else 
                                addQuery += "'м')";
                            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
                            command.ExecuteNonQuery();
                            dataBase.closeConnection();

                            Form form1 = new Participants();
                            this.Dispose();
                            form1.Show();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно выполнить действие", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }

        private void Add_participants_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Participants();
            this.Dispose();
            form1.Show();
        }
    }
}
