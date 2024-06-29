using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mp5
{
    public partial class opcje : Form
    {
        private double opa;

        public double opacit
        {
            get { return this.opa; }
        }

        public opcje()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.opa = 1;
            }
            if (radioButton2.Checked)
            {
                this.opa = 0.8;
            }
            if (radioButton3.Checked)
            {
                this.opa = 0.6;
            }
            if (radioButton4.Checked)
            {
                this.opa = 0.4;
            }
            if (radioButton5.Checked)
            {
                this.opa = 0.2;
            }
        }
    }
}
