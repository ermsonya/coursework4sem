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
    public partial class Add_judges : Form
    {
        DataBase dataBase = new DataBase();
        public Add_judges(){
            InitializeComponent();
        }
        private void add_judge() {
            dataBase.openConnection();
            string addQuery = $"INSERT INTO judges (FIO) values ('{textBox1.Text}')";

            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            int number= command.ExecuteNonQuery();
            MessageBox.Show("Судья добавлен", "успех", MessageBoxButtons.OK);

            dataBase.closeConnection();
            Judges form1 = new Judges();
            this.Dispose();
            form1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
            {
                string queryString = $"Select FIO from judges";
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
                    add_judge();
                else
                    MessageBox.Show("Судья уже добавлен", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Judges();
            this.Dispose();
            form1.Show();
        }

        private void Add_judges_Load(object sender, EventArgs e)
        {

        }

        private void Add_judges_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
