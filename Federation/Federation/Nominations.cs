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
    public partial class Nominations : Form
    {
        DataBase dataBase = new DataBase();
        public Nominations()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("name_nomination", "Название");
            dataGridView2.Columns.Add("name_action", "Название");
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));
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
            dwg.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            this.Dispose();
            form1.Show();
        }

        private void Nominations_Load(object sender, EventArgs e)
        {
            string queryString1 = $"Select name_nomination from nominations";
            string queryString2 = $"Select name_action from actions";
            CreateColumns();
            RefreshDataGrid(dataGridView1, queryString1);
            RefreshDataGrid(dataGridView2, queryString2);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Add_nomination();
            this.Dispose();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form1 = new Add_action();
            this.Dispose();
            form1.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Nominations_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
