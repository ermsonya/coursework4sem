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
    public partial class Selection : Form
    {
        DataBase dataBase = new DataBase();
        public Selection()
        {
            InitializeComponent();
        }

        private string[] AddQuery()
        {
            dataBase.openConnection();
            string Query = $"SELECT id_action FROM actions WHERE name_action = '{comboBox1.Text}'";
            SqlCommand command = new SqlCommand(Query, dataBase.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int id_action = reader.GetInt32(0);
            reader.Close();
            dataBase.closeConnection();

            string[] queries = { "", "", "", "", "", "" , "", "" };

            int ind, num;
            if (checkBox4.Checked)
            {
                num = 0;
                bool[] check = {checkBox6.Checked,checkBox7.Checked,checkBox8.Checked,
                    checkBox9.Checked,checkBox10.Checked,checkBox11.Checked,checkBox12.Checked,
                    checkBox13.Checked,checkBox14.Checked,checkBox15.Checked};

                string[] weight = { "44", "48", "52", "56", "60", "67.5", "75", "82.5", "90", "1000" };
                while (num < 10)
                {
                    if (check[num] == false)
                        weight[num] = null;
                    num++;
                }

                num = 0;

                while (weight[num] == null)
                    num++;
                ind = 0;
                queries[ind] = $"Select applications.app_num, FIO, weight_category, weight1, result1, weight2, result2, weight3, result3 from results INNER JOIN(SELECT* FROM applications where";
                queries[ind] = queries[ind] + $" weight_category = {weight[num]}";
                while (num < 10)
                {
                    if (weight[num] != null)
                        queries[ind] += $" or weight_category = {weight[num]}";
                    num++;
                }
                if (checkBox1.Checked)
                        queries[ind] += $" and age_category = 'J'";
                if (checkBox2.Checked)
                        queries[ind] += $" and age_category = 'O'";
                if (checkBox3.Checked)
                        queries[ind] += $" and age_category = 'M'";
                queries[ind] += $") applications on applications.app_num = results.app_num INNER JOIN(SELECT * FROM participants where sex = 'ж') participants on participants.participant_id = applications.participant_id where id_action = {id_action} and id = {DataBank.Text} and";
            }

            if (checkBox5.Checked)
            {
                num = 0;
                bool[] check = {checkBox16.Checked,checkBox17.Checked,checkBox18.Checked,
                    checkBox19.Checked,checkBox20.Checked,checkBox21.Checked,checkBox22.Checked,
                    checkBox23.Checked,checkBox24.Checked,checkBox25.Checked,checkBox26.Checked,checkBox27.Checked};

                string[] weight = {"52", "56", "60", "67.5", "75", "82.5", "90", "100", "110", "125", "140", "1000" };
                while (num < 12)
                {
                    if (check[num] == false)
                        weight[num] = null;
                    num++;
                }

                num = 0;

                while (weight[num] == null)
                    num++;
                if (checkBox4.Checked)
                    ind = 1;
                else
                    ind = 0;
                queries[ind] = $"Select applications.app_num, FIO, weight_category, weight1, result1, weight2, result2, weight3, result3 from results INNER JOIN(SELECT* FROM applications where";
                queries[ind] += $" weight_category = {weight[num]}";
                while (num < 12)
                {
                    if (weight[num] != null)
                        queries[ind] += $" or weight_category = {weight[num]}";
                    num++;
                }
                if (checkBox1.Checked)
                    queries[ind] += $" and age_category = 'J'";
                if (checkBox2.Checked)
                    queries[ind] += $" and age_category = 'O'";
                if (checkBox3.Checked)
                    queries[ind] += $" and age_category = 'M'";
                queries[ind] += $") applications on applications.app_num = results.app_num INNER JOIN(SELECT * FROM participants where sex = 'м') participants on participants.participant_id = applications.participant_id where id_action = {id_action} and id = {DataBank.Text} and";
            }
            if (checkBox4.Checked && checkBox5.Checked)
            {
                queries[2] = queries[0] + $" result2 is null ORDER BY weight_category ASC, weight2 ASC";
                queries[3] = queries[1] + $" result2 is null ORDER BY weight_category ASC, weight2 ASC";
                queries[4] = queries[0] + $" result3 is null ORDER BY weight_category ASC, weight3 ASC";
                queries[5] = queries[1] + $" result3 is null ORDER BY weight_category ASC, weight3 ASC";
                queries[6] = queries[0] + $" result3 is not null ORDER BY weight_category ASC, weight3 ASC";
                queries[7] = queries[1] + $" result3 is not null ORDER BY weight_category ASC, weight3 ASC";
                queries[0] += $" result1 is null ORDER BY weight_category ASC, weight1 ASC";
                queries[1] += $" result1 is null ORDER BY weight_category ASC, weight1 ASC";
            }
            else
            {
                queries[1] = queries[0] + $" result2 is null ORDER BY weight_category ASC, weight2 ASC";
                queries[2] = queries[0] + $" result3 is null ORDER BY weight_category ASC, weight3 ASC";
                queries[3] = queries[0] + $" result3 is not null ORDER BY weight_category ASC, weight3 ASC";
                queries[0] += $" result1 is null ORDER BY weight_category ASC, weight1 ASC";
            }
            DataBank.id_action = id_action;
            return queries;
        }

        private bool Check()
        {
            bool test = false;
            if (comboBox1.Text.Length == 0)
                MessageBox.Show("Выберите движение", "Ошибка", MessageBoxButtons.OK);
            else if (!(checkBox4.Checked) && !(checkBox5.Checked))
                MessageBox.Show("Выберите пол", "Ошибка", MessageBoxButtons.OK);
            else if ((checkBox4.Checked && !checkBox6.Checked && !checkBox7.Checked && !checkBox8.Checked && 
                !checkBox9.Checked && !checkBox10.Checked && !checkBox11.Checked && !checkBox12.Checked && 
                !checkBox13.Checked && !checkBox14.Checked && !checkBox15.Checked) || (checkBox5.Checked && 
                !checkBox16.Checked && !checkBox17.Checked && !checkBox18.Checked && !checkBox18.Checked && 
                !checkBox19.Checked && !checkBox20.Checked && !checkBox21.Checked && !checkBox22.Checked && 
                !checkBox23.Checked && !checkBox24.Checked && !checkBox25.Checked && !checkBox26.Checked && 
                !checkBox27.Checked))
                MessageBox.Show("Выберите весовую категорию", "Ошибка", MessageBoxButtons.OK);
            else if ((checkBox1.Checked) || (checkBox2.Checked) || (checkBox3.Checked))
                test = true;
            else
                MessageBox.Show("Выберите возрастную категорию", "Ошибка", MessageBoxButtons.OK);
            return test;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Execution_Load(object sender, EventArgs e)
        {
            string queryString = $"Select DISTINCT name_action from actions INNER JOIN(SELECT * FROM results WHERE id = {DataBank.Text}) results on actions.id_action = results.id_action";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
            }
            dataBase.closeConnection();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                Perform form1 = new Perform(AddQuery());
                this.Dispose();
                form1.Show();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
                checkBox10.Enabled = true;
                checkBox11.Enabled = true;
                checkBox12.Enabled = true;
                checkBox13.Enabled = true;
                checkBox14.Enabled = true;
                checkBox15.Enabled = true;
            }
            else
            {
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                checkBox10.Enabled = false;
                checkBox11.Enabled = false;
                checkBox12.Enabled = false;
                checkBox13.Enabled = false;
                checkBox14.Enabled = false;
                checkBox15.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox16.Enabled = true;
                checkBox17.Enabled = true;
                checkBox18.Enabled = true;
                checkBox19.Enabled = true;
                checkBox20.Enabled = true;
                checkBox21.Enabled = true;
                checkBox22.Enabled = true;
                checkBox23.Enabled = true;
                checkBox24.Enabled = true;
                checkBox25.Enabled = true;
                checkBox26.Enabled = true;
                checkBox27.Enabled = true;
            }
            else
            {
                checkBox16.Enabled = false;
                checkBox17.Enabled = false;
                checkBox18.Enabled = false;
                checkBox19.Enabled = false;
                checkBox20.Enabled = false;
                checkBox21.Enabled = false;
                checkBox22.Enabled = false;
                checkBox23.Enabled = false;
                checkBox24.Enabled = false;
                checkBox25.Enabled = false;
                checkBox26.Enabled = false;
                checkBox27.Enabled = false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Selection_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
