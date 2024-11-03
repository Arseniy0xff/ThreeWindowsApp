using System.Diagnostics;

namespace ThreeWindowsApp {
    public partial class Menu : Form {
        public Menu() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            // ������� ��������� ����� �����
            Settings formSettings = new Settings(this);
            // ��������� ����� �����
            formSettings.Show();
            // ��������� ������� �����
            //this.Hide(); // ��� this.Close(); ���� �� ������ ��������� �������
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) {
            Simulation formSim = new Simulation(this);
            formSim.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e) {
            Debug.WriteLine("Hello, World!");
        }

        private void button4_Click(object sender, EventArgs e) {
            Simulation3D formSim = new Simulation3D(this);
            formSim.Show();
            this.Hide();
        }
    }
}
