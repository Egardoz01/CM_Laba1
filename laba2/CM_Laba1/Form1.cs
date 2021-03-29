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
        private decimal a;
        private decimal b;
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
        private List<Point> points;
        private bool new_data;
        public Form1()
        {
            InitializeComponent();
            comboBoxDelta.SelectedIndex = 0;
            comboBoxParam.SelectedIndex = 0;
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

                a = decimal.Parse(textBoxSmallA.Text);
                b = decimal.Parse(textBoxSmallB.Text);
                alpha = decimal.Parse(textBoxAlpha.Text);
                bettah = decimal.Parse(textBoxBetta.Text);
                gamma = decimal.Parse(textBoxGamma.Text);
                epsilon = decimal.Parse(textBoxEpsilon.Text);

                delta = decimal.Parse(comboBoxDelta.Text);
                points = new List<Point>();
                new_data = true;
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
          //  return F_Gleb(x);
            if (Math.Abs(x - gamma) < (decimal)0.00000001)
                return 0;
            // return x*x;
            //return (decimal)Math.Sin((double)x);
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

        private decimal F_Gleb(decimal x)
        {

            return (decimal)(Math.Sin((double)x * (double)alpha));
            try
            {
                return (decimal)((double)alpha * Math.Sin(Math.Pow(Math.Abs((double)x), (double)bettah)) + (double)gamma * (double)Math.Cos(Math.Tan((double)(1 * x))));
            }
            catch (OverflowException ex)
            {
                return 0;
            }
        }

        private void DrawIntegral(Graphics g)
        {
           
            Pen pen = new Pen(Color.Red, 3);

            decimal interval= (B - A) / 10000;
            if (!new_data)
            {
                g.DrawLines(pen, points.ToArray());
                return;
            }
            new_data = false;

            int prev = 100;
            int maxN = 0;
            string selected = comboBoxParam.SelectedItem.ToString();
            for (decimal peremennaya = A; peremennaya <= B; peremennaya += interval)
            {
                int X = GetXPixel(peremennaya);
                if (X == prev)
                    continue;
                if (selected == "α")
                    alpha = peremennaya;

                if (selected == "β")
                    bettah = peremennaya;

                if (selected == "γ")
                    gamma = peremennaya;

                if (selected == "ε")
                    epsilon = peremennaya;

                prev = X;

                int l=N, r = 10000;
                
                while (l < r - 1)
                {
                   
                    int m = (r+l)/2;

                    decimal k1 = Tochno(m);
                    decimal k2 = Tochno(m/2);

                    if ((k2 - k1) < delta)
                        r = m;
                    else
                        l = m;
                }

                //int Y = GetYPixel(Tochno(l));
                maxN = Math.Max(maxN, l);
               // points.Add(new Point(X, Y));
            }


            label23.Text = "-Nmax="+maxN;

            for (decimal peremennaya = A; peremennaya <= B; peremennaya += interval)
            {
                int X = GetXPixel(peremennaya);
                if (X == prev)
                    continue;
                if (selected == "α")
                    alpha = peremennaya;

                if (selected == "β")
                    bettah = peremennaya;

                if (selected == "γ")
                    gamma = peremennaya;

                if (selected == "ε")
                    epsilon = peremennaya;

                prev = X;

              
                int Y = GetYPixel(Tochno(maxN));
                points.Add(new Point(X, Y));
            }

            g.DrawLines(pen, points.ToArray());
        }

        private decimal Tochno(int N)
        {
            decimal summ = 0;
            decimal h = (b - a) / N;
            for (decimal x = a; x <= b; x += h)
            {
                summ += F(x) * h;
            }
            return summ;
        }

  
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.White;
            DrawAxises(e.Graphics);

            DrawIntegral(e.Graphics);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ParseData())
                return;
            panel1.Refresh();
        }

        private void comboBoxParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAlpha.Enabled = true;
            textBoxBetta.Enabled = true;
            textBoxGamma.Enabled = true;
            textBoxEpsilon.Enabled = true;

            if (comboBoxParam.SelectedItem.ToString() == "α")
                textBoxAlpha.Enabled = false;

            if (comboBoxParam.SelectedItem.ToString() == "β")
                textBoxBetta.Enabled = false;

            if (comboBoxParam.SelectedItem.ToString() == "γ")
                textBoxGamma.Enabled = false;

            if (comboBoxParam.SelectedItem.ToString() == "ε")
                textBoxEpsilon.Enabled = false;
        }
    }
}
