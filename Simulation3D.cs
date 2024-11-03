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

namespace ThreeWindowsApp {
    public partial class Simulation3D : Form {

        private Menu MainForm;

        private double rsl = 0.2; // resolution
        private double[,] buff;
        private double[,] atm;
        private double[,] weight;
        private double[,] a;

        private int Y;
        private int X;


        public Simulation3D(Menu Form) {

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Simulation_FormClosing);


            timer1.Interval = 100;
            MainForm = Form;

            this.DoubleBuffered = true;

            X = (int)(this.ClientSize.Width * rsl);
            Y = (int)(this.ClientSize.Height * rsl);
            buff = new double[Y, X];
            atm = new double[Y, X];
            weight = new double[Y, X];
            a = new double[Y, X];


            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {

                    buff[y, x] = 0;
                    atm[y, x] = 0;
                    a[y, x] = 0;
                    weight[y, x] = 2;

                }
            }


            for (int y = 1; y < Y - 1; y++) {
                if (y < Y / 2 - 5 || y > Y / 2 + 5) {
                    weight[y, (int)X / 2 + X / 8] = 0;
                }
                

            }


            atm[(int)Y / 2, (int)X / 2] = 250;
            weight[(int)Y / 2, (int)X / 2] = 0.4;


        }


        private void timer1_Tick(object sender, EventArgs e) {
            for (int y = 1; y < Y - 1; y++) {
                for (int x = 1; x < X - 1; x++) {
                    //double max = Math.Max(Math.Max(buff[y - 1, x], buff[y + 1, x]), Math.Max(buff[y, x - 1], buff[y, x + 1]));
                    //double min = Math.Min(Math.Min(buff[y - 1, x], buff[y + 1, x]), Math.Min(buff[y, x - 1], buff[y, x + 1]));
                    //buff[y, x] = (max - min) / 2;
                    buff[y, x] = (atm[y - 1, x] + atm[y + 1, x] + atm[y, x + 1] + atm[y, x - 1]) / 4;
                    // buff[y, x] = (atm[y - 1, x - 1] + atm[y - 1, x] + atm[y - 1, x + 1] + atm[y, x - 1] + atm[y, x + 1] + atm[y + 1, x - 1] + atm[y + 1, x] + atm[y + 1, x + 1]) / 8;


                }
            }

            for (int y = 1; y < Y - 1; y++) {
                for (int x = 1; x < X - 1; x++) {
                    a[y, x] += (buff[y, x] - atm[y, x]) * weight[y, x];
                    atm[y, x] += a[y, x];

                }
            }
            // Debug.WriteLine(buff[(int)Y / 2 - 2, (int)X / 2 - 2]);
            
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {
                    // Нормализуем значение atm для отображения
                    int colorValue = (int)Math.Clamp(Math.Pow(atm[y, x]+1, 2), 0, 255);

                    Color color = Color.FromArgb(colorValue, colorValue, colorValue);
                    using (Brush brush = new SolidBrush(color)) {
                        g.FillRectangle(brush, (float)(x / rsl), (float)(y / rsl), 5, 5); // this.ClientSize.Width / X, this.ClientSize.Height / Y);
                    }
                }
            }
        }



        private void button1_Click(object sender, EventArgs e) {
            MainForm.Show();
            this.Close();
        }

        private void Simulation_FormClosing(object sender, FormClosingEventArgs e) {
            MainForm.Show();
        }

        private void button2_Click(object sender, EventArgs e) {
            for (int y = 1; y < Y -1; y++) {
                
                    atm[y, (int)X / 2] = 100;

            }
        }
    }
}
