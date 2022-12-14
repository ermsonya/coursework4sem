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
    public partial class Results : Form
    {
        DataBase dataBase = new DataBase();
        public Results()
        {
            InitializeComponent();
        }
        private void RefreshCB(ComboBox comboBox)
        {
            string queryString = $"SELECT DISTINCT name_nomination FROM nominations INNER JOIN(SELECT* FROM applications) applications on nominations.id_nomination = applications.id_nomination INNER JOIN (SELECT * FROM results WHERE id = {DataBank.Text}) results on applications.app_num = results.app_num";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox.Items.Add(reader[0]);
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private double[,] calcResults(double[,] results, string queryString)
        {
            SqlCommand command2 = new SqlCommand(queryString, dataBase.GetConnection());
            SqlDataReader reader2 = command2.ExecuteReader();
            int num = 0;
            while (reader2.Read())
            {
                results[num, 0] = Convert.ToDouble(reader2.GetValue(0));
                if (Convert.ToString(reader2.GetBoolean(6)) == "True")
                    results[num, 1] += Convert.ToDouble(reader2.GetValue(5));
                else
                {
                    if (Convert.ToString(reader2.GetBoolean(4)) == "True")
                        results[num, 1] += Convert.ToDouble(reader2.GetValue(3));
                    else
                    {
                        if (Convert.ToString(reader2.GetBoolean(2)) == "True")
                            results[num, 1] += Convert.ToDouble(reader2.GetValue(1));
                        else
                            results[num, 1] += 0;
                    }
                }
                num++;
            }
            reader2.Close();
            return results;
        }

        private double[,] sortResults(double[,] results)
        {
            for (int k = 1; results[k, 0] != 0; k++)
                for (int i = 0; results[i + 1, 0] != 0; i++)
                    if (results[i, 1] < results[i + 1, 1])
                    {
                        double x = results[i, 1];
                        results[i, 1] = results[i + 1, 1];
                        results[i + 1, 1] = x;

                        x = results[i, 0];
                        results[i, 0] = results[i + 1, 0];
                        results[i + 1, 0] = x;
                    }
            return results;
        }

        private void updateApp(string[] sex, int s, string[] age, int ag, int[] action, int id_nomination, int act)
        {
            double[] weight = new double[12];
            int k = 0;
            string queryString = $"SELECT DISTINCT weight_category FROM applications INNER JOIN(SELECT * FROM participants WHERE sex = '{sex[s]}') participants on participants.participant_id = applications.participant_id INNER JOIN(SELECT * FROM results WHERE id = {DataBank.Text}) results on results.app_num = applications.app_num WHERE id_nomination = {id_nomination} and age_category = '{age[ag]}'";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                weight[k] = Convert.ToDouble(reader.GetValue(0));
                k++;
            }
            reader.Close();
            for (int w = 0; w <= k; w++)
            {
                double[,] results = new double[100, 2];
                queryString = $"SELECT DISTINCT results.app_num, weight1, result1, weight2, result2, weight3, result3 FROM results INNER JOIN (SELECT * FROM applications WHERE weight_category = {weight[w]} and age_category = '{age[ag]}') applications on results.app_num = applications.app_num INNER JOIN (SELECT * FROM participants WHERE sex = '{sex[s]}') participants on applications.participant_id = participants.participant_id WHERE id = {DataBank.Text} and id_action = {action[0]} and result1 is not null and result2 is not null and result3 is not null ORDER BY app_num";
                results = calcResults(results, queryString);
                if (act > 1)
                {
                    queryString = $"SELECT DISTINCT results.app_num, weight1, result1, weight2, result2, weight3, result3 FROM results INNER JOIN (SELECT * FROM applications WHERE weight_category = {weight[w]} and age_category = '{age[ag]}') applications on results.app_num = applications.app_num INNER JOIN (SELECT * FROM participants WHERE sex = '{sex[s]}') participants on applications.participant_id = participants.participant_id WHERE id = {DataBank.Text} and id_action = {action[1]} and result1 is not null and result2 is not null and result3 is not null ORDER BY app_num";
                    results = calcResults(results, queryString);
                    if (act > 2)
                    {
                        queryString = $"SELECT DISTINCT results.app_num, weight1, result1, weight2, result2, weight3, result3 FROM results INNER JOIN (SELECT * FROM applications WHERE weight_category = {weight[w]} and age_category = '{age[ag]}') applications on results.app_num = applications.app_num INNER JOIN (SELECT * FROM participants WHERE sex = '{sex[s]}') participants on applications.participant_id = participants.participant_id WHERE id = {DataBank.Text} and id_action = {action[2]} and result1 is not null and result2 is not null and result3 is not null ORDER BY app_num";
                        results = calcResults(results, queryString);
                    }
                }
                results = sortResults(results);

                for (int i = 0; results[i, 0] != 0; i++)
                {
                    string updateQuery = $"UPDATE applications SET result = {Convert.ToString(results[i, 1]).Replace(",", ".")}, position = {Convert.ToInt32(i + 1)} where app_num = {Convert.ToInt32(results[i, 0])}";
                    command = new SqlCommand(updateQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("name_nomination", "Номер заявки");
            dataGridView1.Columns.Add("name_action", "ФИО");
            dataGridView1.Columns.Add("name_action", "Пол");
            dataGridView1.Columns.Add("name_action", "Весовая категория");
            dataGridView1.Columns.Add("name_action", "Возрастная категория");
            dataGridView1.Columns.Add("name_action", "Результат");
            dataGridView1.Columns.Add("name_action", "Место");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetValue(3), record.GetString(4), record.GetValue(5), record.GetInt32(6));
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

        private void Select_nomination_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshCB(comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length == 0)
                MessageBox.Show("Выберите номинацию", "Ошибка", MessageBoxButtons.OK);
            else
            {
                string[] sex = new string[2];
                string[] age = new string[3];
                int[] action = new int[3];

                string queryString = $"SELECT id_nomination FROM nominations WHERE name_nomination = '{comboBox1.Text}'";
                SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int id_nomination = reader.GetInt32(0);
                reader.Close();

                queryString = $"SELECT id_action FROM structure WHERE id_nomination = {id_nomination}";
                command = new SqlCommand(queryString, dataBase.GetConnection());
                reader = command.ExecuteReader();
                int act = 0;
                for (int i = 0; i < 3; i++)
                    if (reader.Read())
                    {
                        action[act] = reader.GetInt32(0);
                        act++;
                    }
                reader.Close();

                bool check = true;
                if (act > 0)
                {
                    queryString = $"SELECT results.app_num FROM results INNER JOIN (SELECT * FROM applications WHERE id_nomination = {id_nomination}) applications on results.app_num = applications.app_num WHERE id = {DataBank.Text} and result3 is not null ORDER BY app_num";
                    command = new SqlCommand(queryString, dataBase.GetConnection());
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int a = reader.GetInt32(0);
                        int ch = 1;
                        if (act > 1)
                        {
                            if (act == 2)
                            {
                                while (reader.Read())
                                {
                                    if (ch % 2 != 0)
                                        if (a != reader.GetInt32(0))
                                            check = false;
                                    a = reader.GetInt32(0);
                                    ch++;
                                }
                                if (ch == 1)
                                    check = false;
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    if (ch % 3 != 0)
                                        if (a != reader.GetInt32(0))
                                            check = false;
                                    a = reader.GetInt32(0);
                                    ch++;
                                }
                                if (ch < 3)
                                    check = false;
                            }
                        }
                    }
                    else
                    {
                        check = false;
                    }
                    reader.Close();
                }

                if (check)
                {
                    queryString = $"SELECT DISTINCT sex FROM participants INNER JOIN (SELECT * FROM applications WHERE id_nomination = {id_nomination}) applications on participants.participant_id = applications.participant_id INNER JOIN (SELECT * FROM results WHERE id = {DataBank.Text}) results on results.app_num = applications.app_num";
                    command = new SqlCommand(queryString, dataBase.GetConnection());
                    reader = command.ExecuteReader();
                    int s = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        if (reader.Read())
                        {
                            sex[s] = reader.GetString(0);
                            s++;
                        }
                    }
                    reader.Close();
                    s--;
                    int ag;
                    while (s >= 0)
                    {
                        ag = 0;
                        queryString = $"SELECT DISTINCT age_category FROM applications INNER JOIN (SELECT * FROM participants WHERE sex = '{sex[s]}') participants on participants.participant_id = applications.participant_id INNER JOIN (SELECT * FROM results WHERE id = {DataBank.Text}) results on results.app_num = applications.app_num WHERE id_nomination = {id_nomination}";
                        command = new SqlCommand(queryString, dataBase.GetConnection());
                        reader = command.ExecuteReader();
                        for (int i = 0; i < 3; i++)
                        {
                            if (reader.Read())
                            {
                                age[ag] = reader.GetString(0);
                                ag++;
                            }
                        }
                        reader.Close();
                        ag--;
                        while (ag >= 0)
                        {
                            updateApp(sex, s, age, ag, action, id_nomination, act);
                            ag--;
                        }
                        s--;
                    }
                    queryString = $"SELECT DISTINCT applications.app_num, participants.FIO, participants.sex, weight_category, age_category, result, position FROM applications INNER JOIN (SELECT * FROM participants) participants on applications.participant_id = participants.participant_id INNER JOIN (SELECT* FROM results WHERE id = {DataBank.Text}) results on applications.app_num = results.app_num WHERE id_nomination = {id_nomination} and result is not null ORDER BY sex ASC, age_category ASC, weight_category ASC, position ASC";
                    RefreshDataGrid(dataGridView1, queryString);

                    dataBase.closeConnection();
                }
                else
                {
                    dataBase.closeConnection();
                    MessageBox.Show("Поток ещё не закончен", "Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Competitions();
            this.Dispose();
            form1.Show();
        }

        private void Results_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
