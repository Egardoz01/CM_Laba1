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
        private List<Dictionary<int, decimal>> simDifs;
        private decimal deltaX;
        private decimal deltaY;
        private decimal A;
        private decimal B;
        private decimal C;
        private decimal D;
        private int N;
        private decimal width;
        private decimal height;
        private decimal alpha;
        private decimal bettah;
        public Form1()
        {
            InitializeComponent();
            ParseData();

           
        }
       
        private bool ParseData()
        {
            try
            {
                A = decimal.Parse(textBoxA.Text);
                B = decimal.Parse(textBoxB.Text);
                C = decimal.Parse(textBoxC.Text);
                D = decimal.Parse(textBoxD.Text);
                N = int.Parse(textBoxN.Text);
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

                if (N <= 0)
                {
                    MessageBox.Show("N должно быть > 1");
                    return false;
                }

                simDifs = new List<Dictionary<int, decimal>>();

                for (int i = 0; i <= 2 * N + 1; i++)
                {
                    simDifs.Add(new Dictionary<int, decimal>());
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

           // return x*x;
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



        private decimal simmetrialDifference(int poryadok, int tochka)
        {
            if (simDifs[poryadok].ContainsKey(tochka))
                return simDifs[poryadok][tochka];
            if (poryadok == 1)
            {
                decimal h = (B - A) / (2 * N);
                decimal x1 = A + (tochka+1) * h / 2;
                decimal x2 = A + (tochka -1) * h / 2 ;
                simDifs[poryadok].Add(tochka, F(x1) - F(x2));
                return simDifs[poryadok][tochka];
            }

            decimal f1 = simmetrialDifference(poryadok - 1, tochka + 1);
            decimal f2 = simmetrialDifference(poryadok - 1, tochka - 1);

            try
            {
                decimal f = f1 - f2;
                simDifs[poryadok].Add(tochka, f);
            }
            catch (OverflowException ex)
            {
                 simDifs[poryadok].Add(tochka, 1000000000);
            }
           
            return simDifs[poryadok][tochka];
        }

        private void DrawP(Graphics g)
        {
            Pen pen = new Pen(panelP.BackColor, 3);

            simmetrialDifference(2,0);

            decimal h = (B - A) / (2*N);
            List<Point> points = new List<Point>();
            for (int T = 0; T<= (2 * N) * 100; T++)
            {
                decimal t =  T / 100.0m;
                int x2, y2;
                decimal x = A + h * t;
                if (B==x)
                {
                    int a = 0;
                }
                decimal y = F(A);
                decimal kek = t;
                for (int i = 1; i <= 2 * N; i++)
                {
                    kek /= i;
                    decimal d = simmetrialDifference(i, 0);
                    if (i % 2 == 1)
                    {
                        if(i>=3)
                            kek *= (t * t - ((i-1) / 2)* ((i-1) / 2));
                        y += kek * d;
                    }
                    else
                    {
                        y += t * kek * d;
                    }

                   // if (y > D + 100000)
                  //      break;
                  //  if (y < C - 100000)
                      //  break;
                }
                x2 = GetXPixel(x);
              
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

            if(checkBoxP.Checked)
                DrawP(e.Graphics);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ParseData())
                return;
            panel1.Refresh();
        }
    }
}
