using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KargoTakip
{
    public partial class Raporlar : Form
    {
        public Raporlar()
        {
            InitializeComponent();
        }

        public static string SqlConnection = "Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True";


        private void btnCikis_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.Show();
            this.Hide();
        }

        private void btnGoster_Click(object sender, EventArgs e)
        {
            string Tarih = dateTimePicker1.Text.ToString();

            char ayrac = '.';
            string[] Tarih1 = Tarih.Split(ayrac);
            string yıl = Tarih1[2];
            string ay = Tarih1[1];
            string gun = Tarih1[0];
            string tarihİlk = yıl + "-" + ay + "-" + gun;


            listView.Items.Clear();
            SqlConnection bg = new SqlConnection(SqlConnection);
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from KargoBilgileri where alimTarihi='" + tarihİlk + "'", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele = new ListViewItem();
                listele.Text = (oku["barkodNo"].ToString());
                listele.SubItems.Add(oku["alimSaati"].ToString());
                listele.SubItems.Add(oku["kargoDurumu"].ToString());
                listele.SubItems.Add(oku["fiyat"].ToString());
                listView.Items.Add(listele);
            }
            bg.Close();

            int kabulEdilen = 0;
            int yolaCikan = 0;
            int teslimEdilen = 0;
            int subeyeGelen = 0;
            string kargoDurumu = "";
                
            listView2.Items.Clear();
            bg.Open();
            cmd = new SqlCommand("select * from KargoBilgileri where alimTarihi='" + tarihİlk + "'", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                kargoDurumu = oku["kargoDurumu"].ToString();
                if (kargoDurumu=="Teslim Alındı")
                    kabulEdilen++;
                if (kargoDurumu=="Transfer Sürecinde")
                    yolaCikan++;
                if (kargoDurumu=="Tamamlandı")
                {
                    teslimEdilen++;
                    subeyeGelen++;
                }
            }
            bg.Close();
            ListViewItem listele2 = new ListViewItem();
            listele2.Text = kabulEdilen.ToString();
            listele2.SubItems.Add(yolaCikan.ToString());
            listele2.SubItems.Add(teslimEdilen.ToString());
            listele2.SubItems.Add(subeyeGelen.ToString());
            listView2.Items.Add(listele2);

            Tarih = "";
        }
    }
}
