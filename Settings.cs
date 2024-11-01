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
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ThreeWindowsApp {
    public partial class Settings : Form {
        private Menu MainForm;
        public Settings(Menu Form) {
            this.FormClosing += new FormClosingEventHandler(Settings_FormClosing);
            MainForm = Form;
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e) {
            Debug.WriteLine("Setts");
        }

        private void button1_Click(object sender, EventArgs e) {
            //Menu formMenu = new Menu();
            MainForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e) {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // Здесь вы можете использовать выбранный цвет
                    Color selectedColor = colorDialog.Color;
                    this.BackColor = selectedColor; // Например, изменим цвет фона формы
                }
            }
        }


        private void Settings_FormClosing(object sender, FormClosingEventArgs e) {
            // Здесь вы можете выполнить проверку или показать диалог
            //DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть форму?", "Подтверждение", MessageBoxButtons.YesNo);

            //if (result == DialogResult.No)
            //{
            //    // Отменяем закрытие формы
            //    e.Cancel = true;
            //}
            //base.OnFormClosing(e);
            MainForm.Show();
        }






        private void trackBar1_Scroll(object sender, EventArgs e) {
            //UInt128 phone = trackBar1.Value * 305185094;
            ////string formattedPhoneNumber = string.Format("{0:(###) ###-####}", phone);
            label1.Text = "Номер: " + trackBar1.Value.ToString();
        }
    }
}
