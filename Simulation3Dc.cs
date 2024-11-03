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
    public partial class Simulation3Dc : Form {


        private Menu MainForm;

        private double rsl = 0.8; // resolution
        private double[,,] buff;
        private double[,,] atm;
        private double[,,] weight;
        private double[,,] a;

        private double[,,] rndr;

        private int Y;
        private int X;
        private bool run_sim = true;

        private int frame_rendering = 20;
        private int frame_counter = 0;


        public Simulation3Dc(Menu Form) {

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Simulation_FormClosing);


            timer1.Interval = 1;
            MainForm = Form;

            this.DoubleBuffered = true;

            X = (int)(this.ClientSize.Width * rsl);
            Y = (int)(this.ClientSize.Height * rsl);
            buff = new double[Y, X, 3];
            atm = new double[Y, X, 3];
            weight = new double[Y, X, 3];
            a = new double[Y, X, 3];
            rndr = new double[Y, X, 3];


            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {
                    for (int c = 0; c < 3; c++) {
                        buff[y, x, c] = 0;
                        atm[y, x, c] = 0;
                        a[y, x, c] = 0;
                        weight[y, x, c] = 1.7 + c * 0.1; // Почему то после 2 происходит что то необъяснимое
                        rndr[y, x, c] = 0;
                    }


                }
            }


            //DrawRectangle(atm, (int)X / 2, (int)Y / 2, 4, 4,true, 100);
            //DrawStripedCircle(atm, (int)X / 2, (int)Y / 2, 3, 100, 0);
            DrawRectangle(atm, (int)X / 2, (int)Y / 2, 1, Y / 8, true, 50);

            DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);

            DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);

            //DrawRectangle(atm, (int)X / 2-10, (int)Y / 2, 20, 1, true, 0);
            //DrawRectangle(atm, (int)X / 2-10, (int)Y / 2 + 4, 20, 1, true, 0);

            DrawCircle(weight, (int)X / 2 + X / 8 + Y / 8, (int)Y / 2 + Y / 10 + Y / 10, Y / 8, true, 1.2);
            DrawTriangle(weight, (int)X / 2 - X / 4, (int)Y / 2 - Y / 12, Y / 4, true, 1.2);

            //for (int y = 1; y < Y - 1; y++) {
            //    if (y < Y / 2 - 5 || y > Y / 2 + 5) {
            //        weight[y, (int)X / 2 + X / 8] = 0;
            //    }


            //}


            //atm[(int)Y / 2, (int)X / 2] = 250;
            //weight[(int)Y / 2, (int)X / 2] = 0.4;


        }


        private void timer1_Tick_1(object sender, EventArgs e) {
            if (run_sim) {
                for (int c = 0; c < 3; c++) {
                    for (int y = 1; y < Y - 1; y++) {
                        for (int x = 1; x < X - 1; x++) {
                            buff[y, x, c] = (atm[y - 1, x, c] + atm[y + 1, x, c] + atm[y, x + 1, c] + atm[y, x - 1, c]) / 4;
                            
                        }
                    }
                }

                for (int c = 0; c < 3; c++) {
                    for (int y = 1; y < Y - 1; y++) {
                        for (int x = 1; x < X - 1; x++) {
                            a[y, x, c] += (buff[y, x, c] - atm[y, x, c]) * weight[y, x, c];
                            atm[y, x, c] += a[y, x, c];
                            if (atm[y, x, c] > 0) {
                                rndr[y, x, c] += atm[y, x, c] / 8;
                            }

                        }
                    }
                }

                // Debug.WriteLine(buff[(int)Y / 2 - 2, (int)X / 2 - 2]);
                frame_counter++;
                if (frame_counter >= frame_rendering) {
                    frame_counter = 0;
                    this.Invalidate();
                }

            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;


            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {

                    // Нормализуем значение atm для отображения
                    int colorValueR = (int)Math.Clamp(Math.Pow(rndr[y, x, 0] + 1, 2), 0, 255);
                    int colorValueG = (int)Math.Clamp(Math.Pow(rndr[y, x, 1] + 1, 2), 0, 255);
                    int colorValueB = (int)Math.Clamp(Math.Pow(rndr[y, x, 2] + 1, 2), 0, 255);

                    //int colorValueR = (int)Math.Clamp(Math.Pow(weight[y, x, 0] + 1, 2), 0, 255);
                    //int colorValueG = (int)Math.Clamp(Math.Pow(weight[y, x, 1] + 1, 2), 0, 255);
                    //int colorValueB = (int)Math.Clamp(Math.Pow(weight[y, x, 2] + 1, 2), 0, 255);

                    //int colorValueR = (int)Math.Clamp(Math.Pow(atm[y, x, 0] + 1, 2), 0, 255);
                    //int colorValueG = (int)Math.Clamp(Math.Pow(atm[y, x, 1] + 1, 2), 0, 255);
                    //int colorValueB = (int)Math.Clamp(Math.Pow(atm[y, x, 2] + 1, 2), 0, 255);


                    Color color = Color.FromArgb(colorValueR, colorValueG, colorValueB);
                    using (Brush brush = new SolidBrush(color)) {
                        g.FillRectangle(brush, (float)(x / rsl), (float)(y / rsl), this.ClientSize.Width / X + 1, this.ClientSize.Height / Y + 1); // this.ClientSize.Width / X, this.ClientSize.Height / Y);
                    }
                }
            }
        }

        // Функция для рисования прямоугольника
        private void DrawRectangle(double[,,] arr, int x, int y, int width, int height, bool filled, double value) {
            for (int c = 0; c < 3; c++) {
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++) {
                        if (filled || (i == 0 || i == height - 1 || j == 0 || j == width - 1)) {
                            arr[y + i, x + j, c] = value;
                        }
                    }
                }
            }

        }

        // Функция для рисования круга
        private void DrawCircle(double[,,] arr, int centerX, int centerY, int radius, bool filled, double value) {
            for (int c = 0; c < 3; c++) {
                for (int y = -radius; y <= radius; y++) {
                    for (int x = -radius; x <= radius; x++) {
                        if (x * x + y * y <= radius * radius) {
                            if (filled || (x * x + y * y >= (radius - 1) * (radius - 1))) {
                                arr[centerY + y, centerX + x, c] = value;
                            }
                        }
                    }
                }
            }

        }

        // Функция для рисования треугольника
        private void DrawTriangle(double[,,] arr, int x, int y, int size, bool filled, double value) {
            for (int c = 0; c < 3; c++) {
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j <= i; j++) {
                        if (filled || (i == size - 1 || j == 0 || j == i)) {
                            arr[y + i, x + j, c] = value;
                        }
                    }
                }
            }

        }

        // Функция для рисования полосатого круга с вертикальными полосками
        static void DrawStripedCircle(double[,] arr, int centerX, int centerY, int radius, double value1, double value2) {
            for (int y = -radius; y <= radius; y++) {
                for (int x = -radius; x <= radius; x++) {
                    // Проверяем, находится ли точка внутри круга
                    if (x * x + y * y <= radius * radius) {
                        // Определяем, какой цвет использовать в зависимости от x-координаты
                        if (x % 2 == 0) {
                            arr[centerY + y, centerX + x] = value1; // Используем первое значение для четных x
                        } else {
                            arr[centerY + y, centerX + x] = value2; // Используем второе значение для нечетных x
                        }
                    }
                }
            }
        }



        private void Simulation_FormClosing(object sender, FormClosingEventArgs e) {
            MainForm.Show();
        }

        private void button2_Click(object sender, EventArgs e) {
            run_sim = !run_sim;


            //for (int y = 1; y < Y - 1; y++) { 
            //    atm[y, (int)X / 2] = 100;
            //}
        }

        private void button1_Click_1(object sender, EventArgs e) {
            MainForm.Show();
            this.Close();
        }
    }
}
