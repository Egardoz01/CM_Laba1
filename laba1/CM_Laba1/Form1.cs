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
        private decimal gamma;
        private decimal epsilon;
        private decimal delta;
        public Form1()
        {
            InitializeComponent();
            comboBoxDelta.SelectedIndex = 0;
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
                deltaX = decimal.Parse(textBoxDeltaX.Text);
                deltaY = decimal.Parse(textBoxDeltaY.Text);
                height = panel1.Height-50;
                width = panel1.Width-50;

                alpha = decimal.Parse(textBoxAlpha.Text);
                bettah = decimal.Parse(textBoxBetta.Text);
                gamma = decimal.Parse(textBoxGamma.Text);
                epsilon = decimal.Parse(textBoxEpsilon.Text);

                delta = decimal.Parse(comboBoxDelta.Text);

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
            if (y > D)
                y =  D + 100;
            if(y<C)
                y =  C - 100;

            y -= C;
            int pixel = (int)height - (int)Math.Round(y * height / (D - C)) + 25;
            return pixel;

        }

        private void DrawAxises(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Pen pen2 = new Pen(Color.Red,2);

            g.DrawLine(pen2, GetXPixel(0), GetYPixel(C) + 25, GetXPixel(0), GetYPixel(D) - 25);
            g.DrawLine(pen2, GetXPixel(A)-25, GetYPixel(0), GetXPixel(B)+25, GetYPixel(0));


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
           // return (decimal)Math.Sin((double)x);
            try
            {
                return alpha * (decimal)Math.Sin((double)(epsilon * x)) * (decimal)Math.Cos(Math.Tan((double)(bettah / (x - gamma))));
            }
            catch (DivideByZeroException ex)
            {
                return 0;
            }
            catch (OverflowException ex)
            {
                return 0;
            }
        }

        private void DrawF(Graphics g)
        {
            Pen pen = new Pen(panelFColor.BackColor, 1);

            decimal delta = (B - A) / 10000;
            List<Point> points = new List<Point>();

            int prevx=0, prevy=0;
            for (decimal x = A; x <= B; x+=delta)
            {
                int x2, y2;
                x2 = GetXPixel(x);
                y2 = GetYPixel(F(x));
                if(prevx!=x2 || prevy!=y2)
                    points.Add(new Point(x2, y2));

                prevy = y2;
                prevx = x2;
            }

            g.DrawLines(pen, points.ToArray());
        }

        private void DrawDF(Graphics g)
        {
            Pen pen = new Pen(panelDFColor.BackColor,3);

            decimal delta = (B - A) / 1000;
            decimal h = this.delta;
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


        private void DrawDP(Graphics g)
        {
            Pen pen = new Pen(panelDP.BackColor, 3);

            decimal delta = (B - A) / 1000;
            decimal h = this.delta;
            List<Point> points = new List<Point>();
            for (decimal x = A; x <= B; x += delta)
            {
                int x2, y2;
                x2 = GetXPixel(x);
                decimal y = (P(x + h) - P(x - h)) / (2 * h);
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
                decimal a = (B + A) / 2;
                decimal h = (B - A) / (2 * N);
                decimal x1 = a + (tochka +1) * h / 2;
                decimal x2 = a + (tochka - 1) * h / 2;
                decimal y1 = F(x1);
                decimal y2 = F(x2);
                decimal dif = y1 - y2;
                simDifs[poryadok].Add(tochka,dif);
                return simDifs[poryadok][tochka];
            }

            decimal f1 = simmetrialDifference(poryadok - 1, tochka+1);
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

        private decimal P(decimal x)
        {

            decimal h = (B - A) / (2 * N);
            decimal a = (B + A) / 2;

            decimal t = (x - a) / h;

            if (B == x)
            {
                int c = 0;
            }
            if (A == x)
            {
                int c = 0;
            }
            decimal y = F(a);
            decimal kek = t;
            for (int i = 1; i <= 2 * N; i++)
            {
                kek /= i;
                try
                {
                    if (i % 2 == 1)
                    {
                        if (i >= 3)
                            kek *= (t * t - ((i - 1) / 2) * ((i - 1) / 2));
                        y += kek * (simmetrialDifference(i, 1) + simmetrialDifference(i, -1)) / 2;
                    }
                    else
                    {
                        decimal d = simmetrialDifference(i, 0);
                        y += t * kek * d;
                    }
                }
                catch (Exception ex)
                {
                    return y;
                }
            }

            return y;
        }

        private void DrawP(Graphics g)
        {
            Pen pen = new Pen(panelP.BackColor, 3);

            List<Point> points = new List<Point>();
            for (decimal x = A; x<=B; x+=(B-A)/1000)
            {
                int x2, y2;

                x2 = GetXPixel(x);
                decimal y = P(x);
                y2 = GetYPixel(y);
                points.Add(new Point(x2, y2));
            }

            g.DrawLines(pen, points.ToArray());
        }

        private void DrawR(Graphics g)
        {
            Pen pen = new Pen(panelRn.BackColor, 3);

            List<Point> points = new List<Point>();
            for (decimal x = A; x <= B; x += (B - A) / 1000)
            {
                int x2, y2;

                x2 = GetXPixel(x);
                decimal y = Math.Abs(P(x) - F(x));
                y2 = GetYPixel(y);
                if (y2 > 1000)
                    y2 = 1000;
                if (y2 < -1000)
                    y2 = -1000;
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

            if (checkBoxR.Checked)
                DrawR(e.Graphics);

            if (checkBoxDP.Checked)
                DrawDP(e.Graphics);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ParseData())
                return;
            panel1.Refresh();
        }
    }
}
