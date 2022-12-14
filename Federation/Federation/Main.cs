using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Federation
{
    public partial class Main : Form
    {

        DataBase dataBase = new DataBase();
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form2 = new Competitions();
            this.Hide();
            form2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form4 = new Participants();
            this.Hide();
            form4.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form3 = new Archive();
            this.Hide();
            form3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form form5 = new Nominations();
            this.Hide();
            form5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Judges form6 = new Judges();
            this.Hide();
            form6.Show();
        }
    }
}
