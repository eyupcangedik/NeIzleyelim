using System;
using System.Windows.Forms;

namespace NeIzleyelim
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
            this.FormClosing += Anasayfa_FormClosing;
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            Ekle ekle = new Ekle();
            ekle.Show();
        }

        private void buttonSec_Click(object sender, EventArgs e)
        {
            YerimeSec dizi_Film_Sec = new YerimeSec();
            dizi_Film_Sec.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Liste liste = new Liste();
            liste.Show();
        }

        private void Anasayfa_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
