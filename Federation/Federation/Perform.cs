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
using System.Globalization;

namespace Federation
{
    public partial class Perform : Form
    {
        DataBase dataBase = new DataBase();
        string[] qs = new string[8];


        public Perform(string[] queries)
        {
            InitializeComponent();
            qs = queries;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        int selectedRow;

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("app_num", "Номер заявки");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("weight_category", "Весовая категория");
            dataGridView1.Columns.Add("weight1", "Вес1");
            dataGridView1.Columns.Add("result1", "Результат");
            dataGridView1.Columns.Add("weight2", "Вес2");
            dataGridView1.Columns.Add("result2", "Результат");
            dataGridView1.Columns.Add("weight3", "Вес3");
            dataGridView1.Columns.Add("result3", "Результат");
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 220;
            dataGridView1.ReadOnly = true;
            for (int i = 0; i < 9; i++)
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetValue(2), record.GetValue(3), record.GetValue(4), record.GetValue(5), record.GetValue(6), record.GetValue(7), record.GetValue(8));
        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            SqlDataReader reader;
            for (int i = 0; i < 8; i++)
                if (qs[i].Length != 0) {
                    SqlCommand command = new SqlCommand(qs[i], dataBase.GetConnection());
                    dataBase.openConnection();
                    reader = command.ExecuteReader();
                    while (reader.Read() && reader.GetValue(0) != null)
                    {
                        ReadSingleRow(dwg, reader);
                    }
                    reader.Close();
            dataBase.closeConnection();
                }
            dwg.AllowUserToAddRows = false;
        }

        private void UpdateResults()
        {
            string addQuery = $"UPDATE results set weight1 = {textBox2.Text.Replace(",", ".")}";
            if (checkBox1.Checked)
                addQuery += $", result1 = 1";
            if ((!checkBox1.Checked && dataGridView1.Rows[selectedRow].Cells[4].Value.ToString() != "") || (!checkBox1.Checked && dataGridView1.Rows[selectedRow].Cells[3].Value.ToString() != ""))
                addQuery += $", result1 = 0";
            if (textBox3.Text.Length > 0)
            {
                addQuery += $", weight2 = {textBox3.Text.Replace(",", ".")}";
                if (checkBox2.Checked)
                    addQuery += $", result2 = 1";
                if ((!checkBox2.Checked && dataGridView1.Rows[selectedRow].Cells[6].Value.ToString() != "") || (!checkBox2.Checked && dataGridView1.Rows[selectedRow].Cells[5].Value.ToString() != ""))
                    addQuery += $", result2 = 0";
                if (textBox4.Text.Length > 0)
                {
                    addQuery += $", weight3 = {textBox4.Text.Replace(",", ".")}";
                    if (checkBox3.Checked)
                        addQuery += $", result3 = 1";
                    if ((!checkBox3.Checked && dataGridView1.Rows[selectedRow].Cells[8].Value.ToString() != "") || (!checkBox3.Checked && dataGridView1.Rows[selectedRow].Cells[7].Value.ToString() != ""))
                        addQuery += $", result3 = 0";
                }
            }
            addQuery += $" WHERE app_num = {dataGridView1.Rows[selectedRow].Cells[0].Value.ToString()} AND id = {DataBank.Text} AND id_action = {DataBank.id_action}";

            dataBase.openConnection();
            SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.closeConnection();
        }
        private void Change()
        {
            try
            {
                if (dataGridView1.Rows[selectedRow].Cells[0] == null)
                    MessageBox.Show("Спортсмен не выбран", "Ошибка", MessageBoxButtons.OK);
                else
                {
                    if (textBox2.Text.Length == 0)
                        MessageBox.Show("Не задан вес в 1-ом подходе", "Ошибка", MessageBoxButtons.OK);
                    else
                    {
                        if (textBox3.Text.Length == 0)
                            if (textBox4.Text.Length != 0)
                                MessageBox.Show("Не задан вес во 2-ом подходе", "Ошибка", MessageBoxButtons.OK);
                            else
                                UpdateResults();
                        else
                        {
                            if (textBox4.Text.Length == 0)
                            {
                                if (Convert.ToDouble(textBox2.Text) > Convert.ToDouble(textBox3.Text))
                                    MessageBox.Show("Вес во 2-ом подходе меньше, чем в 1-ом", "Ошибка", MessageBoxButtons.OK);
                                else
                                    UpdateResults();
                            }
                            else
                            {
                                if (Convert.ToDouble(textBox2.Text) > Convert.ToDouble(textBox3.Text))
                                    MessageBox.Show("Вес во 2-ом подходе меньше, чем в 1-ом", "Ошибка", MessageBoxButtons.OK);
                                else
                                {
                                    if (Convert.ToDouble(textBox3.Text) > Convert.ToDouble(textBox4.Text))
                                        MessageBox.Show("Вес во 3-ем подходе меньше, чем в 2-ом", "Ошибка", MessageBoxButtons.OK);
                                    else
                                        UpdateResults();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода данных", "Ошибка", MessageBoxButtons.OK);
            }
        }
        private void Perform_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            textBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Change();
            RefreshDataGrid(dataGridView1);
            selectedRow = 0;
            selectRow();
        }

        private void selectRow()
        {
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            if (row.Cells[0].Value != null)
            {
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[3].Value.ToString();
                textBox3.Text = row.Cells[5].Value.ToString();
                textBox4.Text = row.Cells[7].Value.ToString();

                if (row.Cells[4].Value != null)
                    if (row.Cells[4].Value.ToString() == "True")
                        checkBox1.Checked = true;
                    else
                        checkBox1.Checked = false;
                else
                    checkBox1.Checked = false;

                if (row.Cells[6].Value != null)
                    if (row.Cells[6].Value.ToString() == "True")
                        checkBox2.Checked = true;
                    else
                        checkBox2.Checked = false;
                else
                    checkBox2.Checked = false;

                if (row.Cells[8].Value != null)
                    if (row.Cells[8].Value.ToString() == "True")
                        checkBox3.Checked = true;
                    else
                        checkBox3.Checked = false;
                else
                    checkBox3.Checked = false;
            }
            else
            {
                MessageBox.Show("Ошибка выбора", "Ошибка", MessageBoxButtons.OK);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                selectRow();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions(); ;
            this.Dispose();
            form1.Show();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Perform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
