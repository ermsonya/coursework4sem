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
    public partial class Judges : Form
    {
        DataBase dataBase = new DataBase();
        public Judges()
        {
            InitializeComponent();
        }
        private void CreateColumns()//создаем колонку
        {
            dataGridView1.Columns.Add("FIO", "ФИО судьей");
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }
        private void ReadSingleRow(DataGridView dwg, IDataRecord record)//тип данных
        {
            dwg.Rows.Add(record.GetString(0));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            this.Dispose();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e) // возвр на старт
        {
            Form form1 = new Add_judges();
            this.Dispose();
            form1.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void RefreshDataGrid(DataGridView dwg, string queryString)
        {
            dwg.Rows.Clear();//чистим строки
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());//запрос
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
            dataBase.closeConnection();
        }
        private void Judges_Load(object sender, EventArgs e)
        {
            string queryString1 = $"Select FIO from judges";
          
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString1);
        }

        private void Judges_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
