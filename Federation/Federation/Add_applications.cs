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
    public partial class Add_applications : Form
    {

        DataBase dataBase = new DataBase();

        public Add_applications()
        {
            InitializeComponent();
        }

        private void RefreshCB(ComboBox comboBox, string queryString)
        {
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            if (comboBox == comboBox1)
            {
                AutoCompleteStringCollection autoCompleteSource = new AutoCompleteStringCollection();
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                while (reader.Read())
                {
                    autoCompleteSource.Add(reader[0].ToString());
                }
                comboBox1.AutoCompleteCustomSource = autoCompleteSource;
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            else
            {
                while (reader.Read())
                {
                    comboBox.Items.Add(reader[0]);
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void Add_participants_Load(object sender, EventArgs e)
        {
            string queryString = $"Select passport_data from participants";
            RefreshCB(comboBox1, queryString);
            queryString = $"Select name_nomination from nominations";
            RefreshCB(comboBox2, queryString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length == 0 || textBox1.TextLength == 0 || textBox2.TextLength == 0 || comboBox1.Text.Length == 0)
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка", MessageBoxButtons.OK);
            else
            {
                if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                    MessageBox.Show("Необходимо выбрать возрасную категорию", "Ошибка", MessageBoxButtons.OK);
                else {
                    string queryString = $"SELECT id_nomination FROM nominations WHERE name_nomination = '{comboBox2.Text}'";
                    SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                    dataBase.openConnection();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int id_nomination = reader.GetInt32(0);
                        reader.Close();

                        queryString = $"SELECT id_action FROM structure WHERE id_nomination = '{id_nomination}'";
                        command = new SqlCommand(queryString, dataBase.GetConnection());
                        reader = command.ExecuteReader();
                        int i = 0;
                        int[] id_action = new int[3];
                        while (reader.Read())
                        {
                            id_action[i] = reader.GetInt32(0);
                            i++;
                        }
                        reader.Close();

                        queryString = $"SELECT participant_id, FIO, sex FROM participants WHERE passport_data = '{comboBox1.Text}'";
                        command = new SqlCommand(queryString, dataBase.GetConnection());
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (textBox1.Text == reader.GetString(1))
                            {
                                for (int j = 0; j < 8; j++)
                                    DataBank.Query[j] = "";

                                DataBank.Query[0] = $"INSERT INTO applications (participant_id, id_nomination, own_weight, weight_category, age_category, payment) " +
                                    $"VALUES({reader.GetValue(0)}, {id_nomination}, {textBox2.Text.Replace(",", ".")}, ";
                                double weight = Convert.ToDouble(textBox2.Text);
                                if (reader.GetString(2) == "ж")
                                {
                                    if (weight <= 44)
                                        DataBank.Query[0] += "44, ";
                                    else if (weight <= 48)
                                        DataBank.Query[0] += "48, ";
                                    else if (weight <= 52)
                                        DataBank.Query[0] += "52, ";
                                    else if (weight <= 56)
                                        DataBank.Query[0] += "56, ";
                                    else if (weight <= 60)
                                        DataBank.Query[0] += "60, ";
                                    else if (weight <= 67.5)
                                        DataBank.Query[0] += "67.5, ";
                                    else if (weight <= 75)
                                        DataBank.Query[0] += "75, ";
                                    else if (weight <= 82.5)
                                        DataBank.Query[0] += "82.5, ";
                                    else if (weight <= 90)
                                        DataBank.Query[0] += "90, ";
                                    else
                                        DataBank.Query[0] += "1000, ";
                                }
                                else
                                {
                                    if (weight <= 52)
                                        DataBank.Query[0] += "52, ";
                                    else if (weight <= 56)
                                        DataBank.Query[0] += "56, ";
                                    else if (weight <= 60)
                                        DataBank.Query[0] += "60, ";
                                    else if (weight <= 67.5)
                                        DataBank.Query[0] += "67.5, ";
                                    else if (weight <= 75)
                                        DataBank.Query[0] += "75, ";
                                    else if (weight <= 82.5)
                                        DataBank.Query[0] += "82.5, ";
                                    else if (weight <= 90)
                                        DataBank.Query[0] += "90, ";
                                    else if (weight <= 100)
                                        DataBank.Query[0] += "100, ";
                                    else if (weight <= 110)
                                        DataBank.Query[0] += "110, ";
                                    else if (weight <= 125)
                                        DataBank.Query[0] += "125, ";
                                    else if (weight <= 140)
                                        DataBank.Query[0] += "140, ";
                                    else
                                        DataBank.Query[0] += "1000, ";
                                }
                                if (radioButton1.Checked)
                                    DataBank.Query[0] += "'J', ";
                                else if (radioButton2.Checked)
                                    DataBank.Query[0] += "'O', ";
                                else
                                    DataBank.Query[0] += "'M', ";
                                DataBank.Query[0] += "'True')";

                                reader.Close();
                                if (i == 1)
                                {
                                    DataBank.Query[1] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[0]}, ";
                                    Starting_weights1 form1 = new Starting_weights1();
                                    this.Dispose();
                                    form1.Show();
                                }
                                if (i == 2)
                                {
                                    DataBank.Query[1] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[0]}, ";
                                    DataBank.Query[2] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[1]}, ";
                                    
                                    Starting_weights2 form1 = new Starting_weights2();
                                    this.Dispose();
                                    form1.Show();
                                }
                                if (i == 3)
                                {
                                    DataBank.Query[1] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[0]}, ";
                                    DataBank.Query[2] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[1]}, ";
                                    DataBank.Query[3] = $"INSERT INTO results (id, id_action, app_num, weight1) VALUES ({DataBank.Text}, {id_action[2]}, ";

                                    Starting_weights3 form1 = new Starting_weights3();
                                    this.Dispose();
                                    form1.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Данные не совпадают", "Ошибка", MessageBoxButtons.OK);
                                reader.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Поле паспортных данных заполненно не верно", "Ошибка", MessageBoxButtons.OK);
                            reader.Close();
                        }
                    }
                    else 
                        reader.Close();
                }
                dataBase.closeConnection();
            }


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = new Applications();
            this.Dispose();
            form1.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != null)
            {
                string queryString = $"Select FIO from participants WHERE passport_data = '{comboBox1.Text}'";
                SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                    textBox1.Text = reader.GetString(0);
                reader.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }

        private void Add_applications_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
