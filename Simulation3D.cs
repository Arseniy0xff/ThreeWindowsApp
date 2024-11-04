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

        private double[,] rndr;

        private int Y;
        private int X;
        private bool run_sim = true;

        private int frame_rendering = 1;
        private int frame_counter = 0;


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




            DrawWave(atm, X / 2, Y / 2, 4, 30, 50);
            DrawWave(atm, X / 2 - 8, Y / 2, 4, 30, 50);

            //DrawStripedCircle(atm, (int)X / 2, (int)Y / 2, 10, 50, 0);
            //DrawRectangle(atm, X/2, Y/2, 1, 10, true, 50);
            //DrawRectangle(atm, X / 2 - 4, Y / 2, 1, 10, true, 50);

            //DrawRectangle(atm, (int)X / 2, (int)Y / 2, 1, Y / 8, true, 50);

            //DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            //DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);

            //DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            //DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);


            //DrawCircle(weight, (int)X / 2 + X / 8 + Y / 8, (int)Y / 2 + Y / 10 + Y / 10, Y / 8, true, 1.2);

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
                    int colorValue = (int)Math.Clamp(Math.Pow(atm[y, x] + 1, 2), 0, 255);

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
                        // Вычисляем расстояние от центра
                        double distance = Math.Sqrt(x * x + y * y);
                        // Нормализуем расстояние в диапазоне от 0 до 1
                        double normalizedDistance = distance / radius;

                        // Вычисляем затухающее значение
                        double fadeValue = (1 - normalizedDistance);

                        // Используем синус для плавного разбиения на полосы
                        double sineValue = Math.Sin((distance / radius) * Math.PI * 2); // Синус от 0 до 2π
                        sineValue = (sineValue + 1) / 2; // Приводим значение в диапазон от 0 до 1

                        // Интерполяция между value1 и value2
                        double interpolatedValue = value1 * (1 - sineValue) + value2 * sineValue;

                        // Применяем затухание
                        arr[centerY + y, centerX + x] = interpolatedValue * fadeValue;
                    }
                }
            }
        }

        static void DrawWave(double[,] arr, int startX, int startY, int width, int height, double maxValue) {
            int centerX = startX + width / 2;
            int centerY = startY + height / 2;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    // Вычисляем расстояние от центра
                    double distance = Math.Sqrt(Math.Pow(centerX - (startX + x), 2) + Math.Pow(centerY - (startY + y), 2));

                    // Нормализуем расстояние и вычисляем значение
                    double normalizedDistance = distance / (Math.Sqrt((width / 2) * (width / 2) + (height / 2) * (height / 2)));
                    double value = maxValue * (1 - normalizedDistance);

                    // Устанавливаем значение в массив, если оно положительное
                    if (value > 0) {
                        arr[startY + y, startX + x] = value;
                    }
                }
            }
        }


        // Функция для рисования ряби на воде
        static void DrawRipples(double[,] arr, int centerX, int centerY, double amplitude, double frequency, double speed) {
            int rows = arr.GetLength(0);
            int cols = arr.GetLength(1);

            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++) {
                    // Вычисляем расстояние от центра
                    double distance = Math.Sqrt(Math.Pow(centerX - x, 2) + Math.Pow(centerY - y, 2));

                    // Вычисляем значение ряби с использованием синусоидальной функции
                    double value = amplitude * Math.Sin(frequency * distance - speed * y);

                    // Устанавливаем значение в массив
                    arr[y, x] = value;
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
