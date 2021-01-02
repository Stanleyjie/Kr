using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows.Service
{
    public partial class TestForm : Form
    {
        TimeService test;
        public TestForm()
        {
            InitializeComponent();
            test = new TimeService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            test.testStart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            test.testStop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath.ToString();
            System.Diagnostics.Process.Start(path);
        }
    }
}
