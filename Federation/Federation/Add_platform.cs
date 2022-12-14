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
    public partial class Add_platform : Form
    {
        DataBase dataBase = new DataBase();
        public Add_platform()
        {
            InitializeComponent();
        }

        private void RefreshCB(ComboBox comboBox)
        {
            string queryString = $"Select FIO from judges";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox.Items.Add(reader[0]);
            }
            dataBase.closeConnection();
        }

        private bool test()
        {
            string FIO = "";
            bool test = false;
            if ((comboBox1.SelectedItem == null) || (comboBox2.SelectedItem == null) || (comboBox3.SelectedItem == null))
                MessageBox.Show("Не все пункты заполнены", "Ошибка", MessageBoxButtons.OK);
            else
                if ((comboBox1.SelectedItem.ToString() == comboBox2.SelectedItem.ToString()) || (comboBox2.SelectedItem.ToString() == comboBox3.SelectedItem.ToString()) || (comboBox1.SelectedItem.ToString() == comboBox3.SelectedItem.ToString()))
                MessageBox.Show("Необходимо выбрать трех разных судий", "Ошибка", MessageBoxButtons.OK);
            else
            {
                string queryString = $"Select FIO from judges INNER JOIN (SELECT * FROM platforms WHERE id = {DataBank.Text}) platforms on judges.id_judge = platforms.id_judge";
                SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                bool check = true;
                while (reader.Read())
                {
                    if ((comboBox1.Text == Convert.ToString(reader[0])) || (comboBox2.Text == Convert.ToString(reader[0])) || (comboBox3.Text == Convert.ToString(reader[0])))
                    {
                        check = false;
                        FIO = Convert.ToString(reader[0]);
                    }
                }
                reader.Close();
                dataBase.closeConnection();
                if (check)
                {
                    add_platform();
                    test = true;
                }
                else
                    MessageBox.Show($"{FIO} уже занимает место на одном из помостов", "Ошибка", MessageBoxButtons.OK);
            }
            return test;
        }
        private int get_id(string FIO)
        {
            string queryString = $"Select id_judge from judges WHERE FIO = '{FIO}'";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            dataBase.closeConnection();
            return id;
        }
        private void add_platform()
        {
            int id1 = get_id(comboBox1.Text);
            int id2 = get_id(comboBox2.Text);
            int id3 = get_id(comboBox3.Text);

            dataBase.openConnection();
            string addQuery = $"INSERT INTO platforms (id, platform_number, number, id_judge) values ({DataBank.Text}, {DataBank.Count}, 1, {id1})";
            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            addQuery = $"INSERT INTO platforms (id, platform_number, number, id_judge) values ({DataBank.Text}, {DataBank.Count}, 2, {id2})";
            command = new SqlCommand(addQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            addQuery = $"INSERT INTO platforms (id, platform_number, number, id_judge) values ({DataBank.Text}, {DataBank.Count}, 3, {id3})";
            command = new SqlCommand(addQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.closeConnection();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Add_platform_Load(object sender, EventArgs e)
        {
            string queryString = $"Select count(*) from platforms WHERE id = {DataBank.Text}";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            DataBank.Count = Convert.ToInt32(reader.GetValue(0)) / 3 + 1;
            reader.Close();
            dataBase.closeConnection();
            groupBox1.Text = "Помост № " + DataBank.Count;

            if (DataBank.Count == 1)
            {
                button4.Enabled = false;
            }
            RefreshCB(comboBox1);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            RefreshCB(comboBox2);
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            RefreshCB(comboBox3);
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool i = test();
            if (i)
            {
                Form form1 = new Selection();
                this.Dispose();
                form1.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                Form form1 = new Selection();
                this.Dispose();
                form1.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            bool i = test();
            if (i)
            {
                Form form1 = new Add_platform();
                this.Dispose();
                form1.Show();
            }
        }

        private void Add_platform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
