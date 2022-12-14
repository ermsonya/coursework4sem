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
    public partial class Add_competition : Form
    {
        public Add_competition()
        {
            InitializeComponent();
        }

        DataBase dataBase = new DataBase();

        private void add_competitions()
        {
            var title = textBox1.Text;
            DateTime start = Convert.ToDateTime(maskedTextBox1.Text);
            var date_start = start.ToString("yyyy-MM-dd");
            DateTime end = Convert.ToDateTime(maskedTextBox2.Text);
            var date_end = end.ToString("yyyy-MM-dd");
            var place = textBox4.Text;

            dataBase.openConnection();
            string addQuery = $"INSERT INTO competitions (title, date_start, date_end, place) values ('{title}', '{date_start}', '{date_end}', '{place}')";
            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.closeConnection();
            Competitions form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBox1.Text == null) || (maskedTextBox1.Text == null) || (maskedTextBox2.Text == null) || (textBox4.Text == null))
                    MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButtons.OK);
                else if (Convert.ToDateTime(maskedTextBox1.Text) > Convert.ToDateTime(maskedTextBox2.Text))
                    MessageBox.Show("Ошибка в хронологии", "Ошибка", MessageBoxButtons.OK);
                else if (Convert.ToDateTime(maskedTextBox2.Text) < DateTime.Now)
                    MessageBox.Show("Введите актуальные даты", "Ошибка", MessageBoxButtons.OK);
                else
                    add_competitions();
            }
            catch
            {
                MessageBox.Show("Ошибка при вводе данных", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Add_competition_Load(object sender, EventArgs e)
        {

        }

        private void Add_competition_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
