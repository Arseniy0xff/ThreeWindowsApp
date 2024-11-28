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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThreeWindowsApp {
    public partial class Simulation3Dc : Form {


        private Menu MainForm;

        private double rsl = 0.6; // resolution
        private int user_width = 0;//3840;
        private int user_height = 0;//2160;

        private double[,,] buff;
        private double[,,] atm;
        private double[,,] weight;
        private double[,,] a;

        private double[,,] rndr;

        private int Y;
        private int X;
        private bool run_sim = true;

        private int frame_rendering = 0;
        private int frame_counter = 0;


        public Simulation3Dc(Menu Form) {

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Simulation_FormClosing);


            timer1.Interval = 1;
            MainForm = Form;

            this.DoubleBuffered = true;

            if (user_height > 0 && user_width > 0) {
                rsl = (double)user_width / this.ClientSize.Width;

            }


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

            DrawRectangle(atm, 1, (int)Y / 2, 1, Y / 8, true, 50);

            DrawRectangle(weight, 1, (int)Y / 2, X / 8, 1, true, 0);
            DrawRectangle(weight, 1, (int)Y / 2 + Y / 8, X / 8, 1, true, 0);

            DrawRectangle(atm, 1, (int)Y / 2, X / 8, 1, true, 0);
            DrawRectangle(atm, 1, (int)Y / 2 + Y / 8, X / 8, 1, true, 0);


            DrawCircle(weight, X / 4, Y / 2, Y / 8, true, 1.2);
            DrawCircle(weight, X / 2, Y / 2 - Y / 8, Y / 8, true, 1.2);
            DrawCircle(weight, X / 2 + X / 8, Y / 2 + Y / 8, Y / 8, true, 1.2);
            DrawCircle(weight, X / 2 + (X / 8) * 2, Y / 2 - Y / 10, Y / 8, true, 1.2);



            //DrawRectangle(atm, (int)X / 2, (int)Y / 2, 1, Y / 8, true, 50);

            //DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            //DrawRectangle(weight, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);

            //DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2, X / 4, 1, true, 0);
            //DrawRectangle(atm, (int)X / 2 - X / 8, (int)Y / 2 + Y / 8, X / 4, 1, true, 0);


            //DrawCircle(weight, (int)X / 2 + X / 8 + Y / 8, (int)Y / 2 + Y / 10 + Y / 10, Y / 8, true, 1.2);
            //DrawTriangle(weight, (int)X / 2 - X / 4, (int)Y / 2 - Y / 12, Y / 4, true, 1.2);



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
                                rndr[y, x, c] += Math.Pow(atm[y, x, c], 2);//(atm[y, x, c] / 255) * ((255 - rndr[y, x, c]) * 0.8) / 8;
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


            double[,,] normalizedArray = NormalizeArray(a, 0, 255);
            


            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {

                    // Нормализуем значение atm для отображения
                    //int colorValueR = (int)Math.Clamp(Math.Pow(rndr[y, x, 0] + 1, 2), 0, 255);
                    //int colorValueG = (int)Math.Clamp(Math.Pow(rndr[y, x, 1] + 1, 2), 0, 255);
                    //int colorValueB = (int)Math.Clamp(Math.Pow(rndr[y, x, 2] + 1, 2), 0, 255);

                    int colorValueR = Math.Clamp((int)normalizedArray[y, x, 0] * 2, 0, 255);
                    int colorValueG = Math.Clamp((int)normalizedArray[y, x, 1] * 2, 0, 255);
                    int colorValueB = Math.Clamp((int)normalizedArray[y, x, 2] * 2, 0, 255);

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


        private double[,,] NormalizeArray(double[,,] array, double minRange, double maxRange) {
            double minValue = double.MaxValue;
            double maxValue = double.MinValue;

            double[,,] normalizedArray = new double[array.GetLength(0), array.GetLength(1), 3];

            // Находим минимальное и максимальное значения в массиве
            for (int c = 0; c < 3; c++) {
                for (int i = 0; i < array.GetLength(0); i++) {
                    for (int j = 0; j < array.GetLength(1); j++) {
                        if (array[i, j, c] < minValue)
                            minValue = array[i, j, c];
                        if (array[i, j, c] > maxValue)
                            maxValue = array[i, j, c];
                    }
                }


                // Нормализуем значения
                for (int i = 0; i < array.GetLength(0); i++) {
                    for (int j = 0; j < array.GetLength(1); j++) {
                        normalizedArray[i, j, c] = (array[i, j, c] - minValue) / (maxValue - minValue) * (maxRange - minRange) + minRange;
                    }
                }
            }


            return normalizedArray;
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
        private void DrawStripedCircle(double[,] arr, int centerX, int centerY, int radius, double value1, double value2) {
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

        private void SaveArrayAsImage(double[,,] colorArray, string fileName) {
            int width = colorArray.GetLength(1);
            int height = colorArray.GetLength(0);

            double[,,] normalizedArray = NormalizeArray(rndr, 0, 255);

            using (Bitmap bitmap = new Bitmap(width, height)) {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {

                        // Получаем цвет из массива
                        int colorValueR = Math.Clamp((int)normalizedArray[y, x, 0] * 2, 0, 255);
                        int colorValueG = Math.Clamp((int)normalizedArray[y, x, 1] * 2, 0, 255);
                        int colorValueB = Math.Clamp((int)normalizedArray[y, x, 2] * 2, 0, 255);

                        // Получаем цвет из массива
                        Color color = Color.FromArgb(colorValueR, colorValueG, colorValueB);
                        bitmap.SetPixel(x, y, color);
                    }
                }

                // Сохраняем изображение
                bitmap.Save(fileName);
            }

            Debug.WriteLine($"Изображение сохранено как {fileName}");
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

        private void button3_Click(object sender, EventArgs e) {
            SaveArrayAsImage(rndr, "render.png");
        }
    }
}
