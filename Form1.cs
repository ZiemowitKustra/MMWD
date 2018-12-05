using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MMWD_CS
{
    public partial class Form1 : Form
    {
        Form3 F3 = new Form3();
        public List<Food> Temp = new List<Food>();
        int n = 4;

        public Form1()
        {
            InitializeComponent();
        }

        private void restrictionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2();
            F2.ShowDialog();
        }

        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F3.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Temp = Program.RandSolution(n);
            foreach (Food food in Temp)
                listBox1.Items.Add(food.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double temp2 = Program.Function(Temp, F3.BMR);
            label1.Text = Convert.ToString(temp2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

