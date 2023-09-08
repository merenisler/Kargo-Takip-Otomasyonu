using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KargoTakip
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KargoGonder kargoGonder = new KargoGonder();
            kargoGonder.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Raporlar raporlar = new Raporlar();
            raporlar.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KayitliKargolar kayitliKargolar = new KayitliKargolar();
            kayitliKargolar.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            KayitliMusteriler kayitliMusteriler = new KayitliMusteriler();
            kayitliMusteriler.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Application.Exit();
        }
    }
}
