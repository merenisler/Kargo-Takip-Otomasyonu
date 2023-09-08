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
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }

        public static  string personel;
        public static long personelId = 0;
        public static string personelSube;
        public static string SqlConnection = "Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True";


        private void btnGiris_Click(object sender, EventArgs e)
        {
            string Saat = DateTime.Now.ToLongTimeString();
            string Tarih = DateTime.Now.ToShortDateString();
            char ayrac = '.';
            string[] Tarih1 = Tarih.Split(ayrac);
            string yıl = Tarih1[2];
            string ay = Tarih1[1];
            string gun = Tarih1[0];
            string tarihİlk = yıl + "-" + ay + "-" + gun;

            SqlConnection bg = new SqlConnection(SqlConnection);
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from personel where personelAdSoyad=@p1 and personelSifre=@p2", bg);
            cmd.Parameters.AddWithValue("@p1", txtKullaniciAdi.Text);
            cmd.Parameters.AddWithValue("@p2", txtSifre.Text);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            bg.Close();
            bool sifreKontrol = false;
            if (dt.Rows.Count > 0)
            {
                sifreKontrol = true;
                personel = txtKullaniciAdi.Text;
            }
            else
                sifreKontrol = false;

            bg.Open();
            SqlCommand cmd2 = new SqlCommand("select * from personel where personelAdSoyad='" + txtKullaniciAdi.Text + "'", bg);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                personelId = (long)dr2["personelId"];
                personelSube = (string)dr2["subeIl"];
            }
            bg.Close();
            if (sifreKontrol==true)
            {
                try
                {
                    bg.Open();
                    string kayit = "update personel set sonGirisTarih=@p1, sonGirisSaat=@p2 where personelId=" + personelId  + "";
                    SqlCommand komut = new SqlCommand(kayit, bg);
                    komut.Parameters.AddWithValue("@p1", tarihİlk);
                    komut.Parameters.AddWithValue("@p2", Saat);
                    komut.ExecuteNonQuery();
                    bg.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }
                AnaSayfa anaSayfa = new AnaSayfa();
                anaSayfa.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış!");
                txtKullaniciAdi.Text = "";
                txtSifre.Text = "";
            }
        }
    }
}
