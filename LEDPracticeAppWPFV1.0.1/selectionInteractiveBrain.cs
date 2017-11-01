﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEDPracticeAppWPFV1._0._1
{
    public partial class selectionInteractiveBrain : UserControl
    {
        string selectedBrainPart;
        public selectionInteractiveBrain()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBrainPart = listBox1.GetItemText(listBox1.SelectedValue);
        }
        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = selectedBrainPart + "was chosen. Illuminate " + selectedBrainPart + " on Brain. ";
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
