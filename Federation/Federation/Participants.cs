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
    public partial class Participants : Form
    {

        DataBase dataBase = new DataBase();

        public Participants()
        {
            InitializeComponent();
        }

        int selectedRow;

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("passport_data", "Паспорт");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("date_of_birth", "Дата рождения");
            dataGridView1.Columns.Add("city", "Город");
            dataGridView1.Columns.Add("sex", "Пол");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 210;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[5].Width = 50;

            dataGridView2.Columns.Add("title", "Название");
            dataGridView2.Columns.Add("app_num", "Номер заявки");
            dataGridView2.Columns.Add("name_nomination", "Номинация");
            dataGridView2.Columns.Add("own_weight", "Собств. вес");
            dataGridView2.Columns.Add("weight_category", "Весовая кат.");
            dataGridView2.Columns.Add("age_category", "Возрастная кат.");
            dataGridView2.Columns.Add("result", "Результат");
            dataGridView2.Columns.Add("position", "Место");
            dataGridView2.Columns[0].Width = 200;
            dataGridView2.Columns[1].Width = 65;
            dataGridView2.Columns[2].Width = 150;
            dataGridView2.Columns[3].Width = 70;


            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(1), record.GetValue(2),
                 record.GetDateTime(3).ToString("dd.MM.yyyy"), record.GetValue(4), record.GetValue(5));
        }
        private void ReadSingleRowRes(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(1), record.GetValue(2), record.GetValue(3),
                record.GetValue(4), record.GetValue(5), record.GetValue(6), record.GetValue(7));
        }

        private void RefreshDataGrid(DataGridView dwg, string queryString)
        {
            dwg.Rows.Clear();
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (dwg == dataGridView1)
                {
                    ReadSingleRow(dwg, reader);
                }
                else
                    ReadSingleRowRes(dwg, reader);

            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void Change()
        {
            try
            {
                if ((textBox1.Text == null) || (textBox2.Text == null) || (textBox3.Text == null))
                    MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButtons.OK);
                else
                {
                    dataBase.openConnection();
                    string addQuery = $"UPDATE participants SET passport_data = '{textBox1.Text}', FIO = '{textBox2.Text}', city = '{textBox3.Text}' WHERE participant_id = {dataGridView1.Rows[selectedRow].Cells[0].Value}";
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

        public void Clear(DataGridView dwg)
        {
                for (int i = dwg.Rows.Count; i > 0; i--)
                    dwg.Rows.Remove(dwg.Rows[i-1]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            this.Dispose();
            form1.Show();
        }

        private void Participants_Load(object sender, EventArgs e)
        {
            string queryString = $"SELECT * FROM participants ORDER BY FIO";
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                string queryString = $"SELECT DISTINCT title, applications.app_num, name_nomination, own_weight, weight_category, age_category, result, position FROM applications" +
                    $" INNER JOIN(SELECT* FROM results) results on applications.app_num = results.app_num" +
                    $" INNER JOIN(SELECT* FROM competitions) competitions on results.id = competitions.id" +
                    $" INNER JOIN(SELECT* FROM nominations) nominations on applications.id_nomination = nominations.id_nomination" +
                    $" WHERE participant_id = {row.Cells[0].Value}";
                RefreshDataGrid(dataGridView2, queryString);

                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[4].Value.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Change();
            string queryString = $"SELECT * FROM participants ORDER BY FIO";
            RefreshDataGrid(dataGridView1, queryString);
            Clear(dataGridView2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form1 = new Add_participants();
            this.Dispose();
            form1.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Participants_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
