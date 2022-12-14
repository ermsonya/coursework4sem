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
    public partial class Archive : Form
    {

        DataBase dataBase = new DataBase();

        public Archive()
        {
            InitializeComponent();
        }

        int selectedRow;

        private void CreateColumns()
        {
            dataGridView2.Columns.Add("app_num", "Номер заявки");
            dataGridView2.Columns.Add("name_nomimation", "Номинация");
            dataGridView2.Columns.Add("name_action", "Движение");
            dataGridView2.Columns.Add("FIO", "ФИО спортсмена");
            dataGridView2.Columns.Add("sex", "Пол");
            dataGridView2.Columns.Add("own_weight", "Собств. вес");
            dataGridView2.Columns.Add("weight_category", "Весовая кат.");
            dataGridView2.Columns.Add("age_category", "Возрастн. кат.");
            dataGridView2.Columns.Add("weight1", "Вес1");
            dataGridView2.Columns.Add("result1", "Результат");
            dataGridView2.Columns.Add("weight2", "Вес2");
            dataGridView2.Columns.Add("result2", "Результат");
            dataGridView2.Columns.Add("weight3", "Вес3");
            dataGridView2.Columns.Add("result3", "Результат");
            dataGridView2.Columns.Add("result", "Сумма");
            dataGridView2.Columns.Add("position", "Место");
            dataGridView2.Columns[0].Width = 65;
            dataGridView2.Columns[1].Width = 170;
            dataGridView2.Columns[2].Width = 170;
            dataGridView2.Columns[3].Width = 200;
            dataGridView2.Columns[4].Width = 50;
            for (int i = 5; i < 16; i++)
                dataGridView2.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < 16; i++)
                dataGridView2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("title", "Название");
            dataGridView1.Columns.Add("date_start", "Дата начала");
            dataGridView1.Columns.Add("date_end", "Дата окончания");
            dataGridView1.Columns.Add("place", "Место проведения");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[3].Width = 90;
            for (int i = 0; i < 5; i++)
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView3.Columns.Add("platform", "Номер помоста");
            dataGridView3.Columns.Add("number", "Номер судьи");
            dataGridView3.Columns.Add("FIO", "ФИО судьи");
            dataGridView3.Columns[0].Width = 70;
            dataGridView3.Columns[1].Width = 70;
            for (int i = 0; i < 3; i++)
                dataGridView3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            this.Dispose();
            form1.Show();
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(1), record.GetDateTime(2).ToString("dd.MM.yyyy"),
                 record.GetDateTime(3).ToString("dd.MM.yyyy"), record.GetValue(4));
        }

        private void ReadSingleRowRes(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(2), record.GetValue(3), record.GetValue(4),
                record.GetValue(5), record.GetValue(6), record.GetValue(7), record.GetValue(8), record.GetValue(9),
                record.GetValue(10), record.GetValue(11), record.GetValue(12), record.GetValue(13), record.GetValue(14),
                record.GetValue(15), record.GetValue(16));
        }
        private void ReadSingleRowJud(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetValue(0), record.GetValue(1), record.GetValue(2));
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
                {
                    if (dwg == dataGridView2)
                        ReadSingleRowRes(dwg, reader);
                    else
                        ReadSingleRowJud(dwg, reader);
                }
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Archive_Load(object sender, EventArgs e)
        {
            string queryString = $"Select id, title, date_start, date_end, place from competitions" +
                $" where date_start <= '{DateTime.Now.ToString("yyyy-MM-dd")}' ORDER BY date_end DESC";
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                string queryString = $"SELECT DISTINCT applications.app_num, nominations.id_nomination, name_nomination, name_action, FIO, sex," +
                    $" own_weight, weight_category, age_category, weight1, result1, weight2, result2, weight3, result3, result, position FROM results" +
                    $" INNER JOIN (SELECT * FROM applications) applications on results.app_num = applications.app_num" +
                    $" INNER JOIN (SELECT * FROM participants) participants on applications.participant_id = participants.participant_id" +
                    $" INNER JOIN (SELECT * FROM nominations) nominations on applications.id_nomination = nominations.id_nomination" +
                    $" INNER JOIN (SELECT * FROM actions) actions on results.id_action = actions.id_action" +
                    $" WHERE id = {row.Cells[0].Value} ORDER BY id_nomination, sex, age_category, weight_category, position";
                RefreshDataGrid(dataGridView2, queryString);

                queryString = $"SELECT platform_number, number, FIO FROM platforms INNER JOIN(SELECT* FROM judges) judges" +
                    $" on platforms.id_judge = judges.id_judge WHERE id = {row.Cells[0].Value} ORDER BY platform_number";
                RefreshDataGrid(dataGridView3, queryString);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Archive_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
