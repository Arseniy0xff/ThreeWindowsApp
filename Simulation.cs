using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ThreeWindowsApp
{
    public partial class Simulation : Form
    {
        private Menu MainForm;
        private System.Windows.Forms.Timer timer; // Явное указание на Timer из Windows.Forms
        private float ballX, ballY; // Позиция шара
        private float ballSize = 30; // Размер шара
        private float ballSpeedX = 5; // Скорость по оси X
        private float ballSpeedY = 3; // Скорость по оси Y

        public Simulation(Menu Form)
        {
            MainForm = Form;
            InitializeComponent();

            // Включение двойной буферизации
            this.DoubleBuffered = true;

            // Инициализация позиции шара
            ballX = 50;
            ballY = 50;

            // Настройка таймера
            timer = new System.Windows.Forms.Timer(); // Явное указание на Timer из Windows.Forms
            timer.Interval = 20; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.Show();
            this.Close();
        }

        private void Simulation_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Siml");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновление позиции шара
            ballX += ballSpeedX;
            ballY += ballSpeedY;

            // Проверка на столкновение со стенками
            if (ballX < 0 || ballX + ballSize > this.ClientSize.Width)
            {
                ballSpeedX = -ballSpeedX; // Изменение направления по оси X
            }
            if (ballY < 0 || ballY + ballSize > this.ClientSize.Height)
            {
                ballSpeedY = -ballSpeedY; // Изменение направления по оси Y
            }

            // Перерисовка формы
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // Вызов базового метода

            Graphics g = e.Graphics;

            // Рисование шара
            g.FillEllipse(Brushes.Blue, ballX, ballY, ballSize, ballSize);
        }
    }
}
