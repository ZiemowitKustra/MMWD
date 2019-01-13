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
        public List<Food> Temp2 = new List<Food>();
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
            Temp.Clear();
            Temp.AddRange(Program.RandSolution(n));
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

        private void button4_Click(object sender, EventArgs e)
        {
            
            Temp2.AddRange(Program.FindNextSolution(Temp, F3.BMR));

            //label3.Text = Convert.ToString(temp2);
            listBox3.Items.Add("Lokalne\n");
            foreach (Food food in Temp2)
                listBox3.Items.Add(food.ToString());
            listBox3.Items.Add("\n");
            listBox3.Items.Add(Program.Function(Temp2, F3.BMR));
            listBox3.Items.Add("\n");
            listBox3.Items.Add("Globalne\n");
            foreach (Food food in Program.Global_Solution)
                listBox3.Items.Add(food.ToString());
            listBox3.Items.Add(Program.Function(Program.Global_Solution, F3.BMR));
            listBox3.Items.Add("\n");
            foreach (int food in Program.TabooList)
                listBox3.Items.Add(food.ToString());
            listBox3.Items.Add("\n");
            foreach (int food in Program.LifeTime)
                listBox3.Items.Add(food.ToString());
            listBox3.Items.Add("\n");
            Program.FromListToFile(Temp2, "wyniki.txt");
            Temp.Clear();
            Temp.AddRange(Temp2);
            Temp2.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            //Program.FromListToFile(Temp, "wyniki.txt");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

