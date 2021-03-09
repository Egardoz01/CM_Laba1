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
            ParseData();
        }
        private decimal deltaX;
        private decimal deltaY;
        private decimal A;
        private decimal B;
        private decimal C;
        private decimal D;
        private decimal width;
        private decimal height;
        private bool ParseData()
        {
            try
            {
                A = decimal.Parse(textBoxA.Text);
                B = decimal.Parse(textBoxB.Text);
                C = decimal.Parse(textBoxC.Text);
                D = decimal.Parse(textBoxD.Text);
                deltaX = decimal.Parse(textBoxDeltaY.Text);
                deltaY = decimal.Parse(textBoxDeltaX.Text);
                height = panel1.Height-50;
                width = panel1.Width-50;

                if (A >= B)
                {
                    MessageBox.Show("А должно быть < B");
                    return false;
                }

                if (C >= D)
                {
                    MessageBox.Show("C должно быть < D");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неправильный формат входных данных");
                return false;
            }

            return true;
        }

        private int GetXPixel(decimal x)
        {
            x -= A;
            return (int)Math.Round(x * width / (B-A))+25;

        }

        private int GetYPixel(decimal y)
        {
            y -= C;
            return (int)height-(int)Math.Round(y * height / (D - C))+25;

        }

        private void DrawAxises(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Font txtFont = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Brush brush = new SolidBrush(Color.Black);
            for (decimal i = A; i < B; i+=deltaX)//Оси Y
            {
                g.DrawLine(pen, GetXPixel(i), GetYPixel(C)+25, GetXPixel(i), GetYPixel(D)-25);
               
                g.DrawString(i.ToString(), txtFont, brush, GetXPixel(i), GetYPixel(C));
            }

            g.DrawLine(pen, GetXPixel(B), GetYPixel(C)+25, GetXPixel(B), GetYPixel(D) -25);

            g.DrawString(B.ToString(), txtFont, brush, GetXPixel(B), GetYPixel(C));

            for (decimal i = C; i < D; i += deltaY)//Оси X
            {
                g.DrawLine(pen, GetXPixel(A)-25, GetYPixel(i), GetXPixel(B)+25, GetYPixel(i));

                g.DrawString(i.ToString(), txtFont, brush, GetXPixel(A)-25, GetYPixel(i)-12);
            }

            g.DrawLine(pen, GetXPixel(A) - 25, GetYPixel(D), GetXPixel(B) + 25, GetYPixel(D));

            g.DrawString(D.ToString(), txtFont, brush, GetXPixel(A) - 25, GetYPixel(D)-12);

        }

        private decimal F(decimal x)
        {
        
            return (decimal)Math.Sin((double)x);
        }

        private void DrawF(Graphics g)
        {
            Pen pen = new Pen(panelFColor.BackColor, 3);

            decimal delta = (B - A) / 1000;
            List<Point> points = new List<Point>();
            for (decimal x = A; x <= B; x+=delta)
            {
                int x2, y2;
                x2 = GetXPixel(x);
                y2 = GetYPixel(F(x));

                points.Add(new Point(x2, y2));
            }

            g.DrawLines(pen, points.ToArray());
        }

        private void DrawDF(Graphics g)
        {
            Pen pen = new Pen(panelDFColor.BackColor,3);

            decimal delta = (B - A) / 1000;
            decimal h = (B - A) / 1000;
            List<Point> points = new List<Point>();
            for (decimal x = A; x <= B; x += delta)
            {
                int x2, y2;
                x2 = GetXPixel(x);
                decimal y = (F(x + h) - F(x - h))/(2*h);
                y2 = GetYPixel(y);

                points.Add(new Point(x2, y2));
            }

            g.DrawLines(pen, points.ToArray());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.White;
            DrawAxises(e.Graphics);

            if(checkBoxF.Checked)
                DrawF(e.Graphics);

            if (checkBoxDF.Checked)
                DrawDF(e.Graphics);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ParseData();
            panel1.Refresh();
        }
    }
}
