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
    public partial class Add_structure1 : Form
    {

        DataBase dataBase = new DataBase();
        public Add_structure1()
        {
            InitializeComponent();
        }

        private void RefreshCB(ComboBox comboBox)
        {
            string queryString = $"Select name_action from actions";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox.Items.Add(reader[0]);
            }
            dataBase.closeConnection();
        }
        private void add_structure()
        {
            dataBase.openConnection();
            string addQuery = $"INSERT INTO nominations (name_nomination) values ('{DataBank.Text}')";
            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            int number = command.ExecuteNonQuery();

            string Query = $"SELECT id_nomination FROM nominations WHERE name_nomination = '{DataBank.Text.ToString()}'";
            command = new SqlCommand(Query, dataBase.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            var id_nomination = reader.GetValue(0);
            reader.Close();

            Query = $"SELECT id_action FROM actions WHERE name_action = ('{comboBox1.SelectedItem.ToString()}')";
            command = new SqlCommand(Query, dataBase.GetConnection());
            reader = command.ExecuteReader();
            reader.Read();
            var id_action = reader.GetValue(0);
            reader.Close();

            addQuery = $"INSERT INTO structure (id_nomination, number, id_action) values ('{id_nomination}', 1, '{id_action}')";
            command = new SqlCommand(addQuery, dataBase.GetConnection());
            number = command.ExecuteNonQuery();
            dataBase.closeConnection();

            Form form1 = new Nominations();
            this.Dispose();
            form1.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Add_structure_Load(object sender, EventArgs e)
        {
            RefreshCB(comboBox1);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                MessageBox.Show("Не все пункты заполнены", "Ошибка", MessageBoxButtons.OK);
            else
                add_structure();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Nominations();
            this.Dispose();
            form1.Show();
        }

        private void Add_structure1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
