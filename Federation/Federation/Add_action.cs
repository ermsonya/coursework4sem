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
    public partial class Add_action : Form
    {
        public Add_action()
        {
            InitializeComponent();
        }

        DataBase dataBase = new DataBase();

        private void add_action()
        {
            dataBase.openConnection();
            string addQuery = $"INSERT INTO actions (name_action) values ('{textBox1.Text}')";
            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            int number = command.ExecuteNonQuery();
            dataBase.closeConnection();

            Nominations form1 = new Nominations();
            this.Dispose();
            form1.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.TextLength > 0)
            {
                string queryString = $"Select name_action from actions";
                SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                bool check = true;
                while (reader.Read())
                {
                    if (textBox1.Text == Convert.ToString(reader[0]))
                        check = false;
                }
                reader.Close();
                dataBase.closeConnection();

                if (check)
                    add_action();
                else
                    MessageBox.Show("Движение с таким названием уже существует", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Nominations();
            this.Dispose();
            form1.Show();
        }

        private void Add_action_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Add_action_Load(object sender, EventArgs e)
        {

        }
    }
}
