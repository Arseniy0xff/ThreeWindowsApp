﻿using System;
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

        private double rsl = 0.8; // resolution
        private double[,] buff;
        private double[,] atm;
        private double[,] weight;
        private double[,] a;

        private double[,] rndr;

        private int Y;
        private int X;
        private bool run_sim = true;

        private int frame_rendering = 20;
        private int frame_counter = 0;


        public Simulation3D(Menu Form) {

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Simulation_FormClosing);


            timer1.Interval = 1;
            MainForm = Form;

            this.DoubleBuffered = true;

            X = (int)(this.ClientSize.Width * rsl);
            Y = (int)(this.ClientSize.Height * rsl);
            buff = new double[Y, X];
            atm = new double[Y, X];
            weight = new double[Y, X];
            a = new double[Y, X];
            rndr = new double[Y, X];


            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {

                    buff[y, x] = 0;
                    atm[y, x] = 0;
                    a[y, x] = 0;
                    weight[y, x] = 2;
                    rndr[y, x] = 0;

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

            //for (int y = 1; y < Y - 1; y++) {
            //    if (y < Y / 2 - 5 || y > Y / 2 + 5) {
            //        weight[y, (int)X / 2 + X / 8] = 0;
            //    }


            //}


            //atm[(int)Y / 2, (int)X / 2] = 250;
            //weight[(int)Y / 2, (int)X / 2] = 0.4;


        }


        private void timer1_Tick(object sender, EventArgs e) {
            if (run_sim) {
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
                        if (atm[y, x] > 0) {
                            rndr[y, x] += atm[y, x] / 8;
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
                    int colorValue = (int)Math.Clamp(Math.Pow(rndr[y, x] + 1, 2), 0, 255);

                    Color color = Color.FromArgb(colorValue, colorValue, colorValue);
                    using (Brush brush = new SolidBrush(color)) {
                        g.FillRectangle(brush, (float)(x / rsl), (float)(y / rsl), 5, 5); // this.ClientSize.Width / X, this.ClientSize.Height / Y);
                    }
                }
            }
        }

        // Функция для рисования прямоугольника
        private void DrawRectangle(double[,] arr, int x, int y, int width, int height, bool filled, double value) {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (filled || (i == 0 || i == height - 1 || j == 0 || j == width - 1)) {
                        arr[y + i, x + j] = value;
                    }
                }
            }
        }

        // Функция для рисования круга
        private void DrawCircle(double[,] arr, int centerX, int centerY, int radius, bool filled, double value) {
            for (int y = -radius; y <= radius; y++) {
                for (int x = -radius; x <= radius; x++) {
                    if (x * x + y * y <= radius * radius) {
                        if (filled || (x * x + y * y >= (radius - 1) * (radius - 1))) {
                            arr[centerY + y, centerX + x] = value;
                        }
                    }
                }
            }
        }

        // Функция для рисования треугольника
        private void DrawTriangle(double[,] arr, int x, int y, int size, bool filled, double value) {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j <= i; j++) {
                    if (filled || (i == size - 1 || j == 0 || j == i)) {
                        arr[y + i, x + j] = value;
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

        private void button1_Click(object sender, EventArgs e) {
            MainForm.Show();
            this.Close();
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
    }
}
