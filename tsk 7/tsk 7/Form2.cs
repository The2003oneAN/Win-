using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tsk_7
{
    public partial class Run_process : Form
    {
        public Run_process()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                try
                {
                    Process pro = new Process();
                    pro.StartInfo.FileName = textBox1.Text;
                    pro.Start();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
