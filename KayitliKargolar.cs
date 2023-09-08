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
using ZXing.Common;
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.Mail;
using System.Net;

namespace KargoTakip
{
    public partial class KayitliKargolar : Form
    {
        public KayitliKargolar()
        {
            InitializeComponent();
        }

        public static string SqlConnection = "Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True;";

        string sonDegisiklikTarihi = "";
        string sonDegisiklikSaati = "";

        private Bitmap barcodeBitmap;
        private int pageCount = 0;

        private string barkodYazdir = "";
        private string adSoyadYazdirG = "";
        private string ilYazdirG = "";
        private string ilceYazdirG = "";
        private string adresYazdirG = "";
        private long telefonYazdirG = 0;
        private string ePostaYazdirG = "";

        private string adSoyadYazdirA = "";
        private string ilYazdirA = "";
        private string ilceYazdirA = "";
        private string adresYazdirA = "";
        private long telefonYazdirA = 0;
        private string ePostaYazdirA = "";

        private string odemeSekliYazdir = "";
        private string kargoTipiYazdir = "";

        private double desiYazdir = 0;
        private string agirlikYazdir = "";
        private int fiyatYazdir = 0;

        private bool korumalimiYazdir = false;
        private bool sigortalimiYazdir = false;

        private void KayitliKargolar_Load(object sender, EventArgs e)
        {
            SqlConnection bg = new SqlConnection(SqlConnection);
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from KargoBilgileri where barkodNo in (select barkodNo from aliciBilgileri where il='" + Giris.personelSube + "') order by barkodno", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele = new ListViewItem();
                listele.Text = (oku["barkodNo"].ToString());
                listele.SubItems.Add(oku["gondericiAdSoyad"].ToString());
                listele.SubItems.Add(oku["aliciAdSoyad"].ToString());
                listele.SubItems.Add(oku["kargoDurumu"].ToString());
                listele.SubItems.Add(oku["fiyat"].ToString());
                lstViewRapor.Items.Add(listele);
            }
            bg.Close();

            bg.Open();
            cmd = new SqlCommand("select * from KargoBilgileri where barkodNo in (select barkodNo from gondericiBilgileri where il='" + Giris.personelSube + "') order by barkodno", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele2 = new ListViewItem();
                listele2.Text = (oku["barkodNo"].ToString());
                listele2.SubItems.Add(oku["gondericiAdSoyad"].ToString());
                listele2.SubItems.Add(oku["aliciAdSoyad"].ToString());
                listele2.SubItems.Add(oku["kargoDurumu"].ToString());
                listele2.SubItems.Add(oku["fiyat"].ToString());
                lstViewRapor.Items.Add(listele2);
            }
            bg.Close();
        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            SqlConnection bg = new SqlConnection(SqlConnection);
            string kargoDurumu = "";
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from KargoBilgileri where barkodNo=" + Convert.ToInt64(txtBxBarkodNo.Text) + "", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            { kargoDurumu = oku["kargoDurumu"].ToString(); }
            bg.Close();

            string tarih = DateTime.Now.ToShortDateString();
            char ayrac2 = '.';
            string[] tarih1 = tarih.Split(ayrac2);
            string yıl2 = tarih1[2];
            string ay2 = tarih1[1];
            string gun2 = tarih1[0];
            string tarihİlk2 = yıl2 + "-" + ay2 + "-" + gun2;
            string saat = DateTime.Now.ToLongTimeString();
            long barkodNo = Convert.ToInt64(txtBxBarkodNo.Text);

            if (cmbBxKargoDurumu.Text== "Transfer Sürecinde")
            {
                if (kargoDurumu=="Teslim Alındı")
                {
                    bg.Open();
                    cmd = new SqlCommand("update KargoBilgileri set kargoDurumu='Transfer Sürecinde', sonDegisiklikTarihi='" + tarihİlk2 + "', sonDegisiklikSaati='" + saat + "' where barkodNo=" + txtBxBarkodNo.Text + "", bg);
                    try
                    { cmd.ExecuteNonQuery(); }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    bg.Open();
                    cmd = new SqlCommand("update kargoHareketleri set trTarihi=@p1, trSaati=@p2, trIl='" + Giris.personelSube + "', trIlce='" + Giris.personelSube + "' where barkodNo=" + barkodNo + "", bg);
                    try
                    {
                        cmd.Parameters.AddWithValue("@p1", tarihİlk2);
                        cmd.Parameters.AddWithValue("@p2", saat);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();
                }
            }
            if (cmbBxKargoDurumu.Text== "Teslimat Şubesinde")
            {
                if (kargoDurumu=="Teslim Alındı" || kargoDurumu=="Transfer Sürecinde")
                {
                    bg.Open();
                    cmd = new SqlCommand("update KargoBilgileri set kargoDurumu='Teslimat Şubesinde', sonDegisiklikTarihi='" + tarihİlk2 + "', sonDegisiklikSaati='" + saat + "' where barkodNo=" + txtBxBarkodNo.Text + "", bg);
                    try
                    { cmd.ExecuteNonQuery(); }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    bg.Open();
                    cmd = new SqlCommand("update kargoHareketleri set tsTarihi=@p1, tsSaati=@p2, tsIl='" + Giris.personelSube + "', tsIlce='" + Giris.personelSube + "' where barkodNo=" + barkodNo + "", bg);
                    try
                    {
                        cmd.Parameters.AddWithValue("@p1", tarihİlk2);
                        cmd.Parameters.AddWithValue("@p2", saat);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();
                }
            }
            if (cmbBxKargoDurumu.Text== "Kurye Dağıtımda")
            {
                if (kargoDurumu == "Teslim Alındı" || kargoDurumu == "Transfer Sürecinde" || kargoDurumu == "Teslimat Şubesinde")
                {
                    bg.Open();
                    cmd = new SqlCommand("update KargoBilgileri set kargoDurumu='Kurye Dağıtımda', sonDegisiklikTarihi='" + tarihİlk2 + "', sonDegisiklikSaati='" + saat + "' where barkodNo=" + txtBxBarkodNo.Text + "", bg);
                    try
                    { cmd.ExecuteNonQuery(); }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    bg.Open();
                    cmd = new SqlCommand("update kargoHareketleri set kdTarihi=@p1, kdSaati=@p2, kdIl='" + Giris.personelSube + "', kdIlce='" + Giris.personelSube + "' where barkodNo=" + barkodNo + "", bg);
                    try
                    {
                        cmd.Parameters.AddWithValue("@p1", tarihİlk2);
                        cmd.Parameters.AddWithValue("@p2", saat);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();
                }
            }
            if (cmbBxKargoDurumu.Text=="Tamamlandı")
            {
                if (kargoDurumu == "Teslim Alındı" || kargoDurumu == "Transfer Sürecinde" || kargoDurumu == "Teslimat Şubesinde" || kargoDurumu == "Kurye Dağıtımda")
                {
                    bg.Open();
                    cmd = new SqlCommand("update KargoBilgileri set sonDegisiklikTarihi='" + tarihİlk2 + "', sonDegisiklikSaati='" + saat + "' where barkodNo=" + txtBxBarkodNo.Text + "", bg);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    bg.Open();
                    cmd = new SqlCommand("update KargoBilgileri set kargoDurumu=@p1, teslimTarihi=@p2, teslimSaati=@p3, teslimEdenPersonelId=@p4 where barkodNo=" + txtBxBarkodNo.Text + "", bg);
                    try
                    {
                        cmd.Parameters.AddWithValue("@p1", cmbBxKargoDurumu.Text);
                        cmd.Parameters.AddWithValue("@p2", tarihİlk2);
                        cmd.Parameters.AddWithValue("@p3", saat);
                        cmd.Parameters.AddWithValue("@p4", Giris.personelId);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    bg.Open();
                    cmd = new SqlCommand("update kargoHareketleri set tTarihi=@p1, tSaati=@p2, tIl='" + Giris.personelSube + "', tIlce='" + Giris.personelSube + "' where barkodNo=" + barkodNo + "", bg);
                    try
                    {
                        cmd.Parameters.AddWithValue("@p1", tarihİlk2);
                        cmd.Parameters.AddWithValue("@p2", saat);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException hata)
                    { MessageBox.Show(hata.Message.ToString()); }
                    bg.Close();

                    string ad = "";
                    string email = "";
                    bg.Open();
                    cmd = new SqlCommand("select * from aliciBilgileri where barkodNo=" + barkodNo + "", bg);
                    SqlDataReader oku2 = cmd.ExecuteReader();
                    while (oku2.Read())
                    {
                        ad = (oku2["adSoyad"].ToString());
                        email = (oku2["email"].ToString());
                    }
                    bg.Close();
                    if (email != "")
                    {
                        try
                        {
                            // E-posta bilgilerini doldurun
                            string subject = "Kargonuz Teslim Edildi";
                            string fromEmail = "galatasarayeren-2003@hotmail.com";
                            string password = "sezai528";
                            string SMTPServer = "smtp.office365.com";
                            int port = 587;

                            string body = "Sayın " + ad + ", " + barkodNo + " barkod numaralı kargonuz tarafınıza teslim edilmiştir. İyi Günler:)";

                            // SMTP sunucu ve kimlik bilgilerini ayarlayın
                            SmtpClient smtpClient = new SmtpClient(SMTPServer);
                            smtpClient.Port = port; // SMTP port numarası
                            smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                            smtpClient.EnableSsl = true; // SSL kullanılsın mı?

                            // E-posta oluştur
                            MailMessage mail = new MailMessage(fromEmail, email, subject, body);

                            // E-postayı gönder
                            smtpClient.Send(mail);

                            MessageBox.Show("E-posta Başarıyla Gönderildi.");
                        }
                        catch (Exception)
                        { MessageBox.Show("E-posta Gönderilemedi."); }
                    }
                }
            }
            txtBxBarkodNo.Text = "";
            cmbBxKargoDurumu.Text = "";

            lstViewRapor.Items.Clear();
            bg.Open();
            cmd = new SqlCommand("select * from KargoBilgileri where barkodNo in (select barkodNo from aliciBilgileri where il='" + Giris.personelSube + "') order by barkodno", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele = new ListViewItem();
                listele.Text = (oku["barkodNo"].ToString());
                listele.SubItems.Add(oku["gondericiAdSoyad"].ToString());
                listele.SubItems.Add(oku["aliciAdSoyad"].ToString());
                listele.SubItems.Add(oku["kargoDurumu"].ToString());
                listele.SubItems.Add(oku["fiyat"].ToString());
                lstViewRapor.Items.Add(listele);
            }
            bg.Close();

            bg.Open();
            cmd = new SqlCommand("select * from KargoBilgileri where barkodNo in (select barkodNo from gondericiBilgileri where il='" + Giris.personelSube + "') order by barkodno", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele2 = new ListViewItem();
                listele2.Text = (oku["barkodNo"].ToString());
                listele2.SubItems.Add(oku["gondericiAdSoyad"].ToString());
                listele2.SubItems.Add(oku["aliciAdSoyad"].ToString());
                listele2.SubItems.Add(oku["kargoDurumu"].ToString());
                listele2.SubItems.Add(oku["fiyat"].ToString());
                lstViewRapor.Items.Add(listele2);
            }
            bg.Close();
        }

        private void lstViewRapor_SelectedIndexChanged(object sender, EventArgs e)
        {
            long barkodNo = 0;
            lstViewRapor.FullRowSelect = true;
            if (lstViewRapor.SelectedItems.Count == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    string items = lstViewRapor.SelectedItems[0].SubItems[i].Text.ToString();
                    if (i == 0)
                    {
                        txtBxBarkodNo.Text = Convert.ToInt64(items).ToString();
                        barkodNo = Convert.ToInt64(items);
                    }
                }
            }

            SqlConnection bg = new SqlConnection(SqlConnection);
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from KargoBilgileri where barkodNo=" + barkodNo + "", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                sonDegisiklikTarihi = oku["sonDegisiklikTarihi"].ToString();
                sonDegisiklikSaati = oku["sonDegisiklikSaati"].ToString();
            }
            bg.Close();
            lblTarih.Text = "Son Değişiklik Tarihi: " + sonDegisiklikTarihi;
            lblSaat.Text = "Son Değişiklik Saati: " + sonDegisiklikSaati;
            sonDegisiklikTarihi = "";
            sonDegisiklikSaati="";
            barkodNo = 0;
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
        private void KargoFisiYazdir_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            long barkodNo = Convert.ToInt64(txtBxBarkodNo.Text);
            string kargoDurumu = "";
            SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");

            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from aliciBilgileri where barkodNo=" + barkodNo + "", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                adSoyadYazdirA = (string)oku["adSoyad"];
                ilceYazdirA = (string)oku["ilce"];
                ilYazdirA = (string)oku["il"];
                adresYazdirA = (string)oku["adres"];
                telefonYazdirA = (long)oku["telefon"];
                ePostaYazdirA = (string)oku["email"];
            }
            bg.Close();

            bg.Open();
            cmd = new SqlCommand("select * from gondericiBilgileri where barkodNo=" + barkodNo + "", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                adSoyadYazdirG = (string)oku["adSoyad"];
                ilceYazdirG = (string)oku["ilce"];
                ilYazdirG = (string)oku["il"];
                adresYazdirG = (string)oku["adres"];
                telefonYazdirG = (long)oku["telefon"];
                ePostaYazdirG = (string)oku["email"];
            }
            bg.Close();

            bg.Open();
            cmd = new SqlCommand("select * from KargoBilgileri where barkodNo=" + barkodNo + "", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                kargoDurumu = (string)oku["kargoDurumu"];
                odemeSekliYazdir = (string)oku["odemeBilgisi"];
                kargoTipiYazdir = (string)oku["kargoTipi"];
                desiYazdir = (double)oku["desi"];
                fiyatYazdir = (int)oku["fiyat"];
            }
            bg.Close();


            // Oluşturmak istediğiniz barkod verisini belirleyin
            string barcodeData = barkodNo + "\n" + kargoDurumu + "\nAlıcı Adı Soyadı: " + adSoyadYazdirA + "\nGönderici Adı Soyadı: " + adSoyadYazdirG +
                "\nFiyat: " + fiyatYazdir + "\nÖdeme Şekli: " + odemeSekliYazdir; // Örnek URL
            // Barkodun tipini ve özelliklerini ayarlayın
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 100, // Barkod yüksekliği
                    Width = 100,  // Barkod genişliği
                    Margin = 0   // Kenar boşluğu
                }
            };
            // Barkodu bir görüntü olarak oluşturun
            barcodeBitmap = barcodeWriter.Write(barcodeData);

            Font font = new Font("Arial", 14);
            SolidBrush firca = new SolidBrush(Color.Black);
            // Pen kalem = new Pen(Color.Black);
            e.Graphics.DrawString($"{DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}", font, firca, 50, 17);
            e.Graphics.DrawString("Barkod No: " + barkodNo, font, firca, 590, 17);

            // Dikdörtgenin boyutları ve özellikleri
            float rectWidth = 750; // Dikdörtgen genişliği
            float rectHeight = 200; // Dikdörtgen yüksekliği
            float rectX = 50; // Dikdörtgenin sol üst köşesinin x koordinatı
            float rectY = 50; // Dikdörtgenin sol üst köşesinin y koordinatı

            // Dikdörtgeni çizmek için bir Pen oluşturun
            Pen pen = new Pen(Color.Black, 1); // Siyah renkte, 1 kalınlığında bir kalem
            // Dikdörtgeni çiz
            e.Graphics.DrawRectangle(pen, rectX, rectY, rectWidth, rectHeight);
            e.Graphics.DrawRectangle(pen, 50, 50, rectWidth, 30);
            e.Graphics.DrawRectangle(pen, 50, 250, rectWidth, rectHeight);
            e.Graphics.DrawRectangle(pen, 50, 250, rectWidth, 30);

            font = new Font("Arial", 15);
            e.Graphics.DrawString("Gönderici Bilgileri", font, firca, 340, 55);
            e.Graphics.DrawString("Alıcı Bilgileri", font, firca, 365, 255);

            font = new Font("Arial", 13);
            e.Graphics.DrawString(adSoyadYazdirG, font, firca, 70, 95);
            font = new Font("Arial", 12);
            e.Graphics.DrawString(adresYazdirG, font, firca, 70, 125);
            font = new Font("Arial", 13);
            e.Graphics.DrawString(ilceYazdirG + "/" + ilYazdirG, font, firca, 70, 180);
            e.Graphics.DrawString("Telefon: " + telefonYazdirG, font, firca, 70, 215);
            e.Graphics.DrawString("E-Posta: " + ePostaYazdirG, font, firca, 350, 215);

            font = new Font("Arial", 13);
            e.Graphics.DrawString(adSoyadYazdirA, font, firca, 70, 295);
            font = new Font("Arial", 12);
            e.Graphics.DrawString(adresYazdirA, font, firca, 70, 325);
            font = new Font("Arial", 13);
            e.Graphics.DrawString(ilceYazdirA + "/" + ilYazdirA, font, firca, 70, 380);
            e.Graphics.DrawString("Telefon: " + telefonYazdirA, font, firca, 70, 415);
            e.Graphics.DrawString("E-Posta: " + ePostaYazdirA, font, firca, 350, 415);

            e.Graphics.DrawRectangle(pen, 50, 450, 250, 150);
            e.Graphics.DrawRectangle(pen, 300, 450, 250, 150);
            e.Graphics.DrawRectangle(pen, 550, 450, 250, 150);

            font = new Font("Arial", 13);
            e.Graphics.DrawString("Ek Hizmetler", font, firca, 70, 465);
            Font cizgi = new Font("Arial", 14, FontStyle.Bold);
            Font font2 = new Font("Arial", 12);
            e.Graphics.DrawString("-", cizgi, firca, 55, 490);
            if (korumalimiYazdir == true)
                e.Graphics.DrawString("Korumalı", font2, firca, 70, 492);
            e.Graphics.DrawString("-", cizgi, firca, 55, 516);
            if (sigortalimiYazdir == true)
                e.Graphics.DrawString("Değerli/Sigortalı", font2, firca, 70, 518);
            e.Graphics.DrawString("-", cizgi, firca, 55, 542);
            e.Graphics.DrawString("", font2, firca, 70, 544);
            e.Graphics.DrawString("-", cizgi, firca, 55, 568);
            e.Graphics.DrawString("", font2, firca, 70, 570);

            e.Graphics.DrawRectangle(pen, 300, 450, 250, 50);
            e.Graphics.DrawRectangle(pen, 300, 500, 250, 50);

            e.Graphics.DrawString("Ağırlık/Desi:", font2, firca, 310, 455);
            e.Graphics.DrawString(agirlikYazdir, font2, firca, 310, 478);
            e.Graphics.DrawString(desiYazdir.ToString(), font2, firca, 363, 478);

            e.Graphics.DrawRectangle(pen, 300, 450, 122, 50);

            e.Graphics.DrawString("Fiyat: " + fiyatYazdir, font, firca, 435, 465);

            e.Graphics.DrawString("Kargo Tipi: " + kargoTipiYazdir, font, firca, 345, 515);

            font2 = new Font("Arial", 11);
            e.Graphics.DrawString("Ödeme Şekli: " + odemeSekliYazdir, font2, firca, 310, 565);

            e.Graphics.DrawString("Güzergah Bilgileri", font, firca, 565, 465);
            font2 = new Font("Arial", 12);
            e.Graphics.DrawString("-", cizgi, firca, 555, 490);
            e.Graphics.DrawString(ilYazdirA, font2, firca, 570, 492);
            e.Graphics.DrawString("-", cizgi, firca, 555, 516);
            e.Graphics.DrawString(ilceYazdirA, font2, firca, 570, 518);
            e.Graphics.DrawString("-", cizgi, firca, 555, 542);
            e.Graphics.DrawString("-", font2, firca, 570, 544);
            e.Graphics.DrawString("-", cizgi, firca, 555, 568);
            e.Graphics.DrawString("-", font2, firca, 570, 570);

            e.Graphics.DrawRectangle(pen, 50, 600, 750, 250);
            font = new Font("Arial", 15);
            e.Graphics.DrawString("Kargo Hareketleri:", font, firca, 65, 615);
            font = new Font("Arial", 13);
            e.Graphics.DrawString("Teslim Alındı:", font, firca, 60, 655);
            e.Graphics.DrawString("Transfer Sürecinde:", font, firca, 60, 695);
            e.Graphics.DrawString("Teslimat Şubesinde:", font, firca, 60, 735);
            e.Graphics.DrawString("Kurye Dağıtımda:", font, firca, 60, 775);
            e.Graphics.DrawString("Tamamlandı:", font, firca, 60, 815);

            string taTarihi = "", taSaati = "", taIl = "", taIlce = "",
            trTarihi = "", trSaati = "", trIl = "", trIlce = "",
            tsTarihi = "", tsSaati = "", tsIl = "", tsIlce = "",
            kdTarihi = "", kdSaati = "", kdIl = "", kdIlce = "",
            tTarihi = "", tSaati = "", tIl = "", tIlce = "";
            //ta = teslim alındı
            //tr = transfer sürecinde
            //ts = teslimat şubesinde
            //kd = kargo dağıtımda
            //t = tamamlandı

            bg.Open();
            cmd = new SqlCommand("select * from kargoHareketleri where barkodNo=" + barkodNo + "", bg);
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                taTarihi = (string)oku["taTarihi"].ToString();
                taSaati = (string)oku["taSaati"].ToString();
                taIl = (string)oku["taIl"].ToString();
                taIlce = (string)oku["taIlce"].ToString();
                trTarihi = (string)oku["trTarihi"].ToString();
                trSaati = (string)oku["trSaati"].ToString();
                trIl = (string)oku["trIl"].ToString();
                trIlce = (string)oku["trIlce"].ToString();
                tsTarihi = (string)oku["tsTarihi"].ToString();
                tsSaati = (string)oku["tsSaati"].ToString();
                tsIl = (string)oku["tsIl"].ToString();
                tsIlce = (string)oku["tsIlce"].ToString();
                kdTarihi = (string)oku["kdTarihi"].ToString();
                kdSaati = (string)oku["kdSaati"].ToString();
                kdIl = (string)oku["kdIl"].ToString();
                kdIlce = (string)oku["kdIlce"].ToString();
                tTarihi = (string)oku["tTarihi"].ToString();
                tSaati = (string)oku["tSaati"].ToString();
                tIl = (string)oku["tIl"].ToString();
                tIlce = (string)oku["tIlce"].ToString();
            }
            bg.Close();

            if (taTarihi!="")
            {
                if (taTarihi.Length == 19)
                    taTarihi = taTarihi.Remove(10, 9);
                else if (taTarihi.Length == 18)
                    taTarihi = taTarihi.Remove(10, 8);
            }
            if (trTarihi!="")
            {
                if (trTarihi.Length == 19)
                    trTarihi = trTarihi.Remove(10, 9);
                else if (trTarihi.Length == 18)
                    trTarihi = trTarihi.Remove(10, 8);
            }
            if (tsTarihi!="")
            {
                if (tsTarihi.Length == 19)
                    tsTarihi = tsTarihi.Remove(10, 9);
                else if (tsTarihi.Length == 18)
                    tsTarihi = tsTarihi.Remove(10, 8);
            }
            if (kdTarihi!="")
            {
                if (kdTarihi.Length == 19)
                    kdTarihi = kdTarihi.Remove(10, 9);
                else if (kdTarihi.Length == 18)
                    kdTarihi = kdTarihi.Remove(10, 8);
            }
            if (tTarihi!="")
            {
                if (tTarihi.Length == 19)
                    tTarihi = tTarihi.Remove(10, 9);
                else if (tTarihi.Length == 18)
                    tTarihi = tTarihi.Remove(10, 8);
            }

            e.Graphics.DrawString(taTarihi + " - " + taSaati + " - " + taIl, font, firca, 250, 655);
            e.Graphics.DrawString(trTarihi + " - " + trSaati + " - " + trIl, font, firca, 250, 695);
            e.Graphics.DrawString(tsTarihi + " - " + tsSaati + " - " + tsIl, font, firca, 250, 735);
            e.Graphics.DrawString(kdTarihi + " - " + kdSaati + " - " + kdIl, font, firca, 250, 775);
            e.Graphics.DrawString(tTarihi + " - " + tSaati + " - " + tIl, font, firca, 250, 815);

            //e.Graphics.DrawString("30/07/2023 - 11.10 - ", font, firca, 250, 695);
            //e.Graphics.DrawString("01/08/2023 - 10.15 - ADIYAMAN", font, firca, 250, 735);
            //e.Graphics.DrawString("02/08/2023 - 09.20 - ADIYAMAN", font, firca, 250, 775);
            //e.Graphics.DrawString("02/08/2023 - 15.30 - ADIYAMAN", font, firca, 250, 815);

            // Barkodun yazdırılacağı koordinatları belirleyin
            int x = 50;
            int y = 860;
            // Barkodu yazdırın
            e.Graphics.DrawImage(barcodeBitmap, x, y);
            e.Graphics.DrawImage(barcodeBitmap, 700, y);
            e.Graphics.DrawImage(barcodeBitmap, 375, y);

            cizgi = new Font("Arial", 15);
            e.Graphics.DrawString("----------------------------------------------------------------------------------------------------------------------", cizgi, firca, 9, 965);

        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.Show();
            this.Hide();
        }
        private void txtBxBarkodNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void cmbBxKargoDurumu_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
