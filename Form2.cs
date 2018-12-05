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
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string restriction_choise = comboBox1.SelectedItem.ToString();
            if (restriction_choise == "Allergy")
            {
                if(!File.Exists("Used.txt"))
                {
                    Program.FromListToFile(Program.Used, "Used.txt");
                }
                else
                {
                    Program.Used = Program.FromFileToList("Used.txt");
                }
                if(!File.Exists("Taboo.txt"))
                {
                    File.Create("Taboo.txt");
                }
                else
                {
                    Program.Taboo = Program.FromFileToList("Taboo.txt");
                }
                foreach (Food F in Program.Taboo)
                    listBox2.Items.Add(F.ToString());
                foreach (Food F in Program.Used)
                     listBox1.Items.Add(F.ToString());
                groupBox1.Visible = true;
            }else
            {
                groupBox1.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var temp = listBox1.SelectedItem;
            Food food;
            for(int i = 0; i < Program.Used.Count; i++)
            {
                if (Convert.ToString(temp) == Program.Used[i].Name)
                {
                    food = Program.Used[i];
                    Program.Taboo.Add(food);
                    Program.Used.Remove(Program.Used[i]);
                }
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach (Food F in Program.Taboo)
                listBox2.Items.Add(F.ToString());
            foreach (Food F in Program.Used)
                listBox1.Items.Add(F.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var temp = listBox2.SelectedItem;
            Food food;
            for (int i = 0; i <= Program.Taboo.Count; i++)
            {
                if (Convert.ToString(temp) == Program.Taboo[i].Name)
                {
                    food = Program.Taboo[i];
                    Program.Used.Add(food);
                    Program.Taboo.Remove(Program.Taboo[i]);
                }
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach (Food F in Program.Taboo)
                listBox2.Items.Add(F.ToString());
            foreach (Food F in Program.Used)
                listBox1.Items.Add(F.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.FromListToFile(Program.Taboo, "Taboo.txt");
            Program.FromListToFile(Program.Used, "Used.txt");
        }
    }
}
