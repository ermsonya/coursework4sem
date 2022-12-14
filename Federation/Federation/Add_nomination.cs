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
    public partial class Add_nomination : Form
    {
        public Add_nomination()
        {
            InitializeComponent();
        }

        DataBase dataBase = new DataBase();

        private void Load_Add_structure()
        {
            DataBank.Text = textBox1.Text;

            if (radioButton1.Checked)
            {
                Form form1 = new Add_structure1();
                this.Dispose();
                form1.Show();
            }
            else if (radioButton2.Checked)
            {
                Form form1 = new Add_structure2();
                this.Dispose();
                form1.Show();
            }
            else
            {
                Form form1 = new Add_structure3();
                this.Dispose();
                form1.Show();
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Nominations();
            this.Dispose();
            form1.Show();
        }

        private void Add_nomination_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0) 
            {
                string queryString = $"Select name_nomination from nominations";
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
                    Load_Add_structure();
                else
                    MessageBox.Show("Номинация с таким названием уже существует", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Add_nomination_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
