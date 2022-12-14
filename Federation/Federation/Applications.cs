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
    public partial class Applications : Form
    {

        DataBase dataBase = new DataBase();
        public Applications()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("app_num", "Номер заявки");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("sex", "Пол");
            dataGridView1.Columns.Add("name_nomination", "Номинация");
            dataGridView1.Columns.Add("weight_category", "Весовая кат.");
            dataGridView1.Columns.Add("age_category", "Возрастная кат.");

            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[1].Width = 240;
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].Width = 170;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(1), record.GetValue(2), record.GetValue(3), record.GetValue(4), record.GetValue(5));
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = new Add_applications();
            this.Dispose();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }

        private void Applications_Load(object sender, EventArgs e)
        {
            string queryString1 = $"SELECT DISTINCT applications.app_num, FIO, sex, name_nomination, weight_category, age_category FROM applications" +
                $" INNER JOIN (SELECT * FROM participants) participants on applications.participant_id = participants.participant_id" +
                $" INNER JOIN (SELECT * FROM nominations) nominations on applications.id_nomination = nominations.id_nomination" +
                $" INNER JOIN (SELECT * FROM results WHERE id = {DataBank.Text}) results on applications.app_num = results.app_num";
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString1);
        }

        private void Applications_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
