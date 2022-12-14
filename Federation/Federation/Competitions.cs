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
    public partial class Competitions : Form
    {

        DataBase dataBase = new DataBase();
        public Competitions()
        {
            InitializeComponent();
        }
        int selectedRow;
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("title", "Название");
            dataGridView1.Columns.Add("date_start", "Дата начала");
            dataGridView1.Columns.Add("date_end", "Дата окончания");
            dataGridView1.Columns.Add("place", "Место проведения");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[2].Width = 110;
            dataGridView1.Columns[3].Width = 110;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDateTime(2).ToString("dd.MM.yyyy"), record.GetDateTime(3).ToString("dd.MM.yyyy"), record.GetString(4));
        }

        private void RefreshDataGrid(DataGridView dwg, string queryString)
        {
            dwg.Rows.Clear();
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void Change()
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
                {
                    dataBase.openConnection();
                    string addQuery = $"UPDATE competitions set title = '{textBox1.Text}', date_start = '{Convert.ToDateTime(maskedTextBox1.Text)}', date_end = '{Convert.ToDateTime(maskedTextBox2.Text)}', place = '{textBox4.Text}' WHERE id = {dataGridView1.Rows[selectedRow].Cells[0].Value.ToString()}";
                    SqlCommand command = new SqlCommand(addQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                    dataBase.closeConnection();
                }
            }
            catch
            {
                MessageBox.Show("Невозможно выполнить действие", "Ошибка", MessageBoxButtons.OK);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            string queryString1 = $"Select id, title, date_start, date_end, place from competitions where date_end > '{DateTime.Now.ToString("yyyy-MM-dd")}' ORDER BY date_end";
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Add_competition form1 = new Add_competition();
            this.Dispose();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataBank.Text = dataGridView1.Rows[selectedRow].Cells[0].Value.ToString();
                Form form1 = new Applications();
                this.Dispose();
                form1.Show();
            }
            catch
            {
                MessageBox.Show("Невозможно выполнить действие", "Ошибка", MessageBoxButtons.OK);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DataBank.Text = dataGridView1.Rows[selectedRow].Cells[0].Value.ToString();
                Form form1 = new Add_platform();
                this.Dispose();
                form1.Show();
            }
            catch
            {
                MessageBox.Show("Невозможно выполнить действие", "Ошибка", MessageBoxButtons.OK);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            this.Dispose();
            form1.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox1.Text = row.Cells[1].Value.ToString();
                maskedTextBox1.Text = row.Cells[2].Value.ToString();
                maskedTextBox2.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Change();
            string queryString1 = $"Select id, title, date_start, date_end, place from competitions where date_end > '{DateTime.Now.ToString("yyyy-MM-dd")}'";
            RefreshDataGrid(dataGridView1, queryString1);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                DataBank.Text = dataGridView1.Rows[selectedRow].Cells[0].Value.ToString();
                Form form1 = new Results();
                this.Dispose();
                form1.Show();
            }
            catch
            {
                MessageBox.Show("Невозможно выполнить действие", "Ошибка", MessageBoxButtons.OK);

            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Competitions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
