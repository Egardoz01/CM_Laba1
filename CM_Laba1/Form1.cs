using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM_Laba1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private decimal deltaX;
        private decimal deltaY;
        private decimal A;
        private decimal B;
        private decimal C;
        private decimal D;
        private bool ParseData()
        {
            try
            {
                A = decimal.Parse(textBoxA.Text);
                B = decimal.Parse(textBoxB.Text);
                C = decimal.Parse(textBoxC.Text);
                D = decimal.Parse(textBoxD.Text);
                deltaX = decimal.Parse(textBoxDeltaX.Text);
                deltaY = decimal.Parse(textBoxDeltaY.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неправильный формат входных данных");
                return false;
            }

            return true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ParseData();
            panel1.Refresh();
        }
    }
}
