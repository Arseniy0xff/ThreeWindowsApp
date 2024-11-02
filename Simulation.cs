using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ThreeWindowsApp {
    public partial class Simulation : Form {

        private Menu MainForm;

        DoublyLinkedList list;
        private double[] buff;
        private double[] atm;
        private double[] weight;
        private double[] a;

        private double k = 0.1;
        private int sample_width = 100; // Ширина выбоки 

        //private double time = 0.0;
        //private double speed = 0.1; // Скорость волны
        //private double frequency = 0.05; // Частота волны
        //private double amplitude = 100.0; // Амплитуда волны


        public Simulation(Menu Form) {

            InitializeComponent();
            timer1.Interval = 10;
            MainForm = Form;
            this.FormClosing += new FormClosingEventHandler(Simulation_FormClosing);
            // this.ClientSize = new Size(width, 200);
            this.DoubleBuffered = true; // Уменьшение мерцания

            buff = new double[sample_width];
            atm = new double[sample_width];
            weight = new double[sample_width];
            a = new double[sample_width];
            


            // list = new DoublyLinkedList();

            for (int i = 0; i < sample_width; i++) {

                atm[i] = 0;
                weight[i] = (double)0.4;
                buff[i] = 0;
                a[i] = 0;


                //list.AddToEnd(i);
            }
            atm[0] = 0;
            atm[sample_width-1] = 0;
            atm[1] = 100;
            //atm[(int)sample_width / 2] = 50;
            //list.SetValueAt(0, 0);
            //list.SetValueAt(sample_width-1, 0);
            //list.SetValueAt((int)sample_width / 2, (double)200);

            //Debug.WriteLine("Siml");

            //list.AddToEnd(2);
            //list.AddToEnd(3);
            //list.PrintList(); // Вывод: 1 2 3

            //list.AddToStart(0);
            //list.PrintList(); // Вывод: 0 1 2 3
        }


        private void timer1_Tick(object sender, EventArgs e) {


            //for (int i = 1; i < sample_width - 1; i++) {

            //    double Va = weight[i] * ((buff[i] - atm[i]) / Math.Abs(buff[i] - atm[i]));
            //    double Va0 = weight[i] * ((0 - atm[i]) / Math.Abs(0 - atm[i]));
            //    if (!double.IsNaN(Va)) {
            //        //double Av = weight[i] * ((0 - atm[i]) / Math.Abs(0 - atm[i]));

            //        a[i] += Va;


            //    }

            //}



            for (int i = 1; i < sample_width - 1; i++) {
                buff[i] = (atm[i - 1] + atm[i + 1]) / 2;
                //buff[i] = Math.Max(atm[i - 1], atm[i + 1]);
                //list.SetValueAt(i, buff[i]);

            }

            for (int i = 1; i < sample_width - 1; i++) {
                a[i] += (buff[i] - atm[i]) * weight[i];
                atm[i] += a[i];
                //list.SetValueAt(i, buff[i]);

            }
            

            this.Invalidate(); // Перерисовка формы
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int i = 0; i < sample_width; i++) {
                g.FillEllipse(Brushes.Blue, i * (this.ClientSize.Width / sample_width), this.ClientSize.Height / 2 + (float)atm[i], (float)4, (float)4);

            }

            // Рисование волны
            //for (int i = 0; i < width - 1; i++) {
            //    g.DrawLine(Pens.Blue, i, 100 - (float)wave[i], i + 1, 100 - (float)wave[i + 1]);
            //}
        }


        private void button1_Click(object sender, EventArgs e) {
            MainForm.Show();
            this.Close();
        }

        private void Simulation_Load(object sender, EventArgs e) {
            Debug.WriteLine("Siml");
        }

        private void Simulation_FormClosing(object sender, FormClosingEventArgs e) {
            // Здесь вы можете выполнить проверку или показать диалог
            //DialogResult result = MessageBox.Show("Stop Simulation?", "Realy?", MessageBoxButtons.YesNo);

            //if (result == DialogResult.No) {
            //    e.Cancel = true;
            //} else {
            //    MainForm.Show();
            //}
            MainForm.Show();
        }

    }
}
