
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MMWD_CS
{
    public partial class Form3 : Form
    {
        public double hight;
        public double weight;
        public double BMI;
        public double BMR=1450;
        public double age;
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BMI = weight / (hight * hight);
            label4.Text = BMI.ToString();
            if (BMI < 18.5)
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = "UNDERWEIGHT";
            }
            else if (BMI < 25 && BMI >= 18.5)
            {
                label5.ForeColor = System.Drawing.Color.Green;
                label5.Text = "CORRECT WEIGHT";
            }
            else if (BMI < 30 && BMI >= 25)
            {
                label5.ForeColor = System.Drawing.Color.Orange;
                label5.Text = "OVERWEIGHT";
            }
            else
            {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = "OBESITY";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) hight = 0;
            else hight = double.Parse(textBox1.Text);
            hight = hight / 100;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text)) hight = 0;
            else weight = double.Parse(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                BMR = 66.5 + (13.7 * weight) + (5 * hight * 100) - (6.8 * age);
            }
            else
            {
                BMR = 665 + (9.6 * weight) + (1.85 * hight * 100) - (4.7 * age);
            }
            label8.Text = Convert.ToString(BMR);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text)) age = 0;
            else age = double.Parse(textBox3.Text);
        }
    }
}
