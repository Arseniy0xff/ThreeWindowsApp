using System.Diagnostics;

namespace ThreeWindowsApp
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Создаем экземпляр новой формы
            Settings formSettings = new Settings(this);
            // Открываем новую форму
            formSettings.Show();
            // Закрываем текущую форму
            //this.Hide(); // или this.Close(); если вы хотите полностью закрыть
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Simulation formSim = new Simulation(this);
            formSim.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Hello, World!");
        }
    }
}
