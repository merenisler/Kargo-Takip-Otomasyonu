using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using ZXing;
using ZXing.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Security.Cryptography;

namespace KargoTakip
{
    public partial class KargoGonder : Form
    {
        public KargoGonder()
        {
            InitializeComponent();
        }

        private static bool onayBtn;
        private static double desi;
        private static int veiOkumaSayisi = 0;
        private static long BarkodNo = 0;

        private Bitmap barcodeBitmap;
        private int pageCount = 0;

        private string barkodYazdir = "";
        private string adSoyadYazdirG = "";
        private string ilYazdirG = "";
        private string ilceYazdirG = "";
        private string adresYazdirG = "";
        private string telefonYazdirG = "";
        private string ePostaYazdirG = "";

        private string adSoyadYazdirA = "";
        private string ilYazdirA = "";
        private string ilceYazdirA = "";
        private string adresYazdirA = "";
        private string telefonYazdirA = "";
        private string ePostaYazdirA = "";
        
        private string odemeSekliYazdir = "";
        private string kargoTipiYazdir = "";
        
        private string desiYazdir = "";
        private string agirlikYazdir = "";
        private string fiyatYazdir = "";

        private bool korumalimiYazdir = false;
        private bool sigortalimiYazdir = false;

        private void ilEkle(System.Windows.Forms.ComboBox cmbBx)
        {
            cmbBx.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection veri = new AutoCompleteStringCollection();
            SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from Sehirler order by sehirAdi", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                veri.Add((string)oku[1]);
                cmbBx.Items.Add(oku[1]);
            }
            bg.Close();
            cmbBx.AutoCompleteCustomSource = veri;
        }
        private void ilceEkle(System.Windows.Forms.ComboBox cmbBxSecilen, System.Windows.Forms.ComboBox cmbBx)
        {
            cmbBx.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection veri = new AutoCompleteStringCollection();
            SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from Ilceler where SehirAdi=@p1 order by IlceAdi", bg);
            cmd.Parameters.AddWithValue("@p1", cmbBxSecilen.Text);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                veri.Add((string)oku["IlceAdi"]);
                cmbBx.Items.Add(oku["IlceAdi"]);
            }
            bg.Close();
            cmbBx.AutoCompleteCustomSource = veri;
        }


        private void KargoGonder_Load(object sender, EventArgs e)
        {
            label50.Visible = false;
            label51.Visible = false;

            ilEkle(cmbBxIl);
            ilEkle(cmbBxIl2);

            txtBxAgirlik.Enabled = false;
            txtBxEn.Enabled = false;
            txtBxBoy.Enabled = false;
            txtBxYukseklik.Enabled=false;

            txtBxDesi.Enabled = false;
            txtBxFiyat.Enabled = false;

            cmbBxKargoTipi.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection veri = new AutoCompleteStringCollection();
            veri.Add("Zarf");
            veri.Add("Koli");
            veri.Add("Kitap");
            cmbBxKargoTipi.AutoCompleteCustomSource = veri;

            cmbBxOdemeBilgisi.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection veri2 = new AutoCompleteStringCollection();
            veri2.Add("Gönderici Ödemeli");
            veri2.Add("Karşı Ödemeli");
            cmbBxOdemeBilgisi.AutoCompleteCustomSource = veri2;

        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (radioBtnKurumsal.Checked==true)
            {
                if ((cmbBxIl.Text == "" || cmbBxIl2.Text == "" || cmbBxIlce.Text == "" || cmbBxIlce2.Text == "" || richTxtBoxAdres.Text == "" || richTxtBoxAdres2.Text == "" || txtBxAdSoyad.Text == "" || txtBxAdSoyad2.Text == "" || txtBxTelefon.Text == "" || txtBxTelefon2.Text == "" || txtBxDesi.Text == "" || cmbBxOdemeBilgisi.Text == "" || cmbBxKargoTipi.Text == "" || txtBxVg.Text == ""))
                    MessageBox.Show("Boş Yerleri Doldurunuz!");
                else
                {
                    onayBtn = true;
                    string VG = "";
                    string VG2 = "";
                    if (txtBxVg.Text != "")
                        VG = txtBxVg.Text;
                    else
                        VG = "";
                    if (txtBxVg2.Text != "")
                        VG2 = txtBxVg2.Text;
                    else
                        VG2 = "";

                    double desi = Convert.ToDouble(txtBxDesi.Text);
                    string tarih = DateTime.Now.ToShortDateString();
                    char ayrac2 = '.';
                    string[] tarih1 = tarih.Split(ayrac2);
                    string yıl2 = tarih1[2];
                    string ay2 = tarih1[1];
                    string gun2 = tarih1[0];
                    string tarihİlk2 = yıl2 + "-" + ay2 + "-" + gun2;
                    string saat = DateTime.Now.ToLongTimeString();
                    long barkodNo = 0;
                    SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                    bg.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) AS RandomNumber WHERE CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) NOT IN (SELECT barkodNo FROM KargoBilgileri)", bg);
                    SqlDataReader dr = cmd.ExecuteReader();
                    //WITH cte AS(SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber UNION ALL SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber FROM cte WHERE(SELECT COUNT(*) FROM KargoBilgileri WHERE barkodNo = randomNumber) > 0) SELECT TOP 1 randomNumber as UretilenSayi FROM cte OPTION(MAXRECURSION 0)WHILE EXISTS(SELECT* FROM tabloAdi WHERE barkodNo = @randomNumber) BEGIN SET @randomNumber = ABS(CHECKSUM(NewId())) % 9000000000 + 1000000000; END SELECT @randomNumber as UretilenSayi

                    while (dr.Read())
                    {
                        barkodNo = (long)dr["RandomNumber"];
                    }
                    bg.Close();

                    BarkodNo = barkodNo;

                    barkodYazdir = barkodNo.ToString();
                    adSoyadYazdirA = txtBxAdSoyad.Text;
                    adSoyadYazdirG = txtBxAdSoyad2.Text;
                    ilYazdirA = cmbBxIl.Text;
                    ilYazdirG = cmbBxIl2.Text;
                    ilceYazdirA = cmbBxIlce.Text;
                    ilceYazdirG = cmbBxIlce2.Text;
                    adresYazdirA = richTxtBoxAdres.Text;
                    adresYazdirG = richTxtBoxAdres2.Text;
                    telefonYazdirA = txtBxTelefon.Text;
                    telefonYazdirG = txtBxTelefon2.Text;
                    ePostaYazdirA = txtBxEmail.Text;
                    ePostaYazdirG = txtBxEmail2.Text;
                    odemeSekliYazdir = cmbBxOdemeBilgisi.Text;
                    kargoTipiYazdir = cmbBxKargoTipi.Text;
                    agirlikYazdir = txtBxAgirlik.Text;
                    fiyatYazdir = txtBxFiyat.Text;
                    if (checkBxKorumali.Checked == true)
                        korumalimiYazdir = true;
                    if (checkBxSigortali.Checked == true)
                        sigortalimiYazdir = true;


                    bg.Open();
                    string kayit = "insert into KargoBilgileri(barkodNo, alimTarihi, alimSaati, alimYapanPersonelId, aliciAdSoyad, gondericiAdSoyad, kargoTipi, desi, odemeBilgisi, kargoDurumu, fiyat, teslimTarihi, teslimSaati, teslimEdenPersonelId, sonDegisiklikTarihi, sonDegisiklikSaati) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)";
                    SqlCommand komut = new SqlCommand(kayit, bg);
                    komut.Parameters.AddWithValue("@p1", barkodNo);
                    komut.Parameters.AddWithValue("@p2", tarihİlk2);
                    komut.Parameters.AddWithValue("@p3", saat);
                    komut.Parameters.AddWithValue("@p4", Giris.personelId);
                    komut.Parameters.AddWithValue("@p5", txtBxAdSoyad.Text);
                    komut.Parameters.AddWithValue("@p6", txtBxAdSoyad2.Text);
                    komut.Parameters.AddWithValue("@p7", cmbBxKargoTipi.Text);
                    komut.Parameters.AddWithValue("@p8", desi);
                    komut.Parameters.AddWithValue("@p9", cmbBxOdemeBilgisi.Text);
                    komut.Parameters.AddWithValue("@p10", "Teslim Alındı");
                    komut.Parameters.AddWithValue("@p11", txtBxFiyat.Text);
                    komut.Parameters.AddWithValue("@p12", tarihİlk2);
                    komut.Parameters.AddWithValue("@p13", saat);
                    komut.Parameters.AddWithValue("@p14", Giris.personelId);
                    komut.Parameters.AddWithValue("@p15", tarihİlk2);
                    komut.Parameters.AddWithValue("@p16", saat);
                    komut.ExecuteNonQuery();
                    bg.Close();

                    bg.Open();
                    komut = new SqlCommand("insert into kargoHareketleri(barkodNo, taTarihi, taSaati, taIl, taIlce, trTarihi, trSaati, trIl, trIlce, tsTarihi, tsSaati, tsIl, tsIlce, kdTarihi, kdSaati, kdIl, kdIlce, tTarihi, tSaati, tIl, tIlce) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21)", bg);
                    komut.Parameters.AddWithValue("@p1", SqlDbType.BigInt).Value = barkodNo;
                    komut.Parameters.AddWithValue("@p2", SqlDbType.Date).Value = tarihİlk2;
                    komut.Parameters.AddWithValue("@p3", SqlDbType.Time).Value = saat;
                    komut.Parameters.AddWithValue("@p4", SqlDbType.NVarChar).Value = cmbBxIl.Text;
                    komut.Parameters.AddWithValue("@p5", SqlDbType.NVarChar).Value = cmbBxIlce.Text;
                    komut.Parameters.AddWithValue("@p6", SqlDbType.Date).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p7", SqlDbType.Time).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p8", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p9", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p10", SqlDbType.Date).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p11", SqlDbType.Time).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p12", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p13", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p14", SqlDbType.Date).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p15", SqlDbType.Time).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p16", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p17", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p18", SqlDbType.Date).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p19", SqlDbType.Time).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p20", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.Parameters.AddWithValue("@p21", SqlDbType.NVarChar).Value = DBNull.Value;
                    komut.ExecuteNonQuery();
                    bg.Close();

                    string adresTipi = "";
                    string aliciTipi = "";
                    string adresTipi2 = "";
                    string gondericiTipi = "";

                    if (radioBtnEv.Checked == true)
                        adresTipi = "Ev";
                    else if (radioBtnIs.Checked == true)
                        adresTipi = "İş";
                    else if (radioBtnDiger.Checked == true)
                        adresTipi = "Diğer";

                    if (radioBtnEv2.Checked == true)
                        adresTipi2 = "Ev";
                    else if (radioBtnIs2.Checked == true)
                        adresTipi2 = "İş";
                    else if (radioBtnDiger2.Checked == true)
                        adresTipi2 = "Diğer";

                    if (radioBtnBireysel.Checked == true)
                        aliciTipi = "Bireysel";
                    else if (radioBtnKurumsal.Checked == true)
                        aliciTipi = "Kurumsal";

                    if (radioBtnBireysel2.Checked == true)
                        gondericiTipi = "Bireysel";
                    else if (radioBtnKurumsal2.Checked == true)
                        gondericiTipi = "Kurumsal";

                    bg.Open();
                    kayit = "insert into aliciBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                    komut = new SqlCommand(kayit, bg);
                    komut.Parameters.AddWithValue("@p1", barkodNo);
                    komut.Parameters.AddWithValue("@p2", txtBxTc.Text);
                    komut.Parameters.AddWithValue("@p3", txtBxAdSoyad.Text);
                    komut.Parameters.AddWithValue("@p4", txtBxTelefon.Text);
                    komut.Parameters.AddWithValue("@p5", VG);
                    komut.Parameters.AddWithValue("@p6", txtBxEmail.Text);
                    komut.Parameters.AddWithValue("@p7", cmbBxIl.Text);
                    komut.Parameters.AddWithValue("@p8", cmbBxIlce.Text);
                    komut.Parameters.AddWithValue("@p9", richTxtBoxAdres.Text);
                    komut.Parameters.AddWithValue("@p10", adresTipi);
                    komut.Parameters.AddWithValue("@p11", aliciTipi);
                    komut.ExecuteNonQuery();
                    bg.Close();
                    bg.Open();
                    kayit = "insert into gondericiBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                    komut = new SqlCommand(kayit, bg);
                    komut.Parameters.AddWithValue("@p1", barkodNo);
                    komut.Parameters.AddWithValue("@p2", txtBxTc2.Text);
                    komut.Parameters.AddWithValue("@p3", txtBxAdSoyad2.Text);
                    komut.Parameters.AddWithValue("@p4", txtBxTelefon2.Text);
                    komut.Parameters.AddWithValue("@p5", VG2);
                    komut.Parameters.AddWithValue("@p6", txtBxEmail2.Text);
                    komut.Parameters.AddWithValue("@p7", cmbBxIl2.Text);
                    komut.Parameters.AddWithValue("@p8", cmbBxIlce2.Text);
                    komut.Parameters.AddWithValue("@p9", richTxtBoxAdres2.Text);
                    komut.Parameters.AddWithValue("@p10", adresTipi2);
                    komut.Parameters.AddWithValue("@p11", gondericiTipi);
                    komut.ExecuteNonQuery();
                    bg.Close();

                    MessageBox.Show("Kargo Onaylandı");

                    string AdresTipi = "";
                    string MusteriTipi = "";
                    string kayitliMusteri = "";
                    string kayitliMusteri2 = "";

                    bg.Open();
                    cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                    SqlDataReader oku = cmd.ExecuteReader();
                    while (oku.Read())
                    {
                        kayitliMusteri = oku["TcNo"].ToString();
                    }
                    bg.Close();
                    bg.Open();
                    cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                    oku = cmd.ExecuteReader();
                    while (oku.Read())
                    {
                        kayitliMusteri2 = oku["TcNo"].ToString();
                    }
                    bg.Close();

                    if (txtBxTc.Text != "")
                    {
                        if (kayitliMusteri != txtBxTc.Text)
                        {
                            if (radioBtnEv.Checked == true)
                                AdresTipi = "Ev";
                            else if (radioBtnIs.Checked == true)
                                AdresTipi = "İş";
                            else if (radioBtnDiger.Checked == true)
                                AdresTipi = "Diğer";

                            if (radioBtnBireysel.Checked == true)
                                MusteriTipi = "Bireysel";
                            else if (radioBtnKurumsal.Checked == true)
                                MusteriTipi = "Kurumsal";

                            bg.Open();
                            kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                            komut = new SqlCommand(kayit, bg);
                            komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                            komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                            komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                            komut.Parameters.AddWithValue("@p4", VG);
                            komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                            komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                            komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                            komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                            komut.Parameters.AddWithValue("@p9", AdresTipi);
                            komut.Parameters.AddWithValue("@p10", MusteriTipi);
                            komut.ExecuteNonQuery();
                            bg.Close();
                        }
                        if (kayitliMusteri == txtBxTc.Text)
                        {
                            bg.Open();
                            SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                            verisil.ExecuteNonQuery();
                            bg.Close();

                            if (radioBtnEv.Checked == true)
                                AdresTipi = "Ev";
                            else if (radioBtnIs.Checked == true)
                                AdresTipi = "İş";
                            else if (radioBtnDiger.Checked == true)
                                AdresTipi = "Diğer";

                            if (radioBtnBireysel.Checked == true)
                                MusteriTipi = "Bireysel";
                            else if (radioBtnKurumsal.Checked == true)
                                MusteriTipi = "Kurumsal";

                            bg.Open();
                            kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                            komut = new SqlCommand(kayit, bg);
                            komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                            komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                            komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                            komut.Parameters.AddWithValue("@p4", VG);
                            komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                            komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                            komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                            komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                            komut.Parameters.AddWithValue("@p9", AdresTipi);
                            komut.Parameters.AddWithValue("@p10", MusteriTipi);
                            komut.ExecuteNonQuery();
                            bg.Close();
                        }
                    }

                    if (txtBxTc2.Text != "")
                    {
                        if (kayitliMusteri2 != txtBxTc2.Text)
                        {
                            if (radioBtnEv2.Checked == true)
                                AdresTipi = "Ev";
                            else if (radioBtnIs2.Checked == true)
                                AdresTipi = "İş";
                            else if (radioBtnDiger2.Checked == true)
                                AdresTipi = "Diğer";

                            if (radioBtnBireysel2.Checked == true)
                                MusteriTipi = "Bireysel";
                            else if (radioBtnKurumsal2.Checked == true)
                                MusteriTipi = "Kurumsal";

                            bg.Open();
                            kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                            komut = new SqlCommand(kayit, bg);
                            komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                            komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                            komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                            komut.Parameters.AddWithValue("@p4", VG2);
                            komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                            komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                            komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                            komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                            komut.Parameters.AddWithValue("@p9", AdresTipi);
                            komut.Parameters.AddWithValue("@p10", MusteriTipi);
                            komut.ExecuteNonQuery();
                            bg.Close();
                        }
                        if (kayitliMusteri2 == txtBxTc2.Text)
                        {
                            bg.Open();
                            SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                            verisil.ExecuteNonQuery();
                            bg.Close();

                            if (radioBtnEv2.Checked == true)
                                AdresTipi = "Ev";
                            else if (radioBtnIs2.Checked == true)
                                AdresTipi = "İş";
                            else if (radioBtnDiger2.Checked == true)
                                AdresTipi = "Diğer";

                            if (radioBtnBireysel2.Checked == true)
                                MusteriTipi = "Bireysel";
                            else if (radioBtnKurumsal2.Checked == true)
                                MusteriTipi = "Kurumsal";

                            bg.Open();
                            kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                            komut = new SqlCommand(kayit, bg);
                            komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                            komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                            komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                            komut.Parameters.AddWithValue("@p4", VG2);
                            komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                            komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                            komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                            komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                            komut.Parameters.AddWithValue("@p9", AdresTipi);
                            komut.Parameters.AddWithValue("@p10", MusteriTipi);
                            komut.ExecuteNonQuery();
                            bg.Close();

                        }
                    }
                }
            }
            else if (radioBtnKurumsal2.Checked == true)
            {
                if ((cmbBxIl.Text == "" || cmbBxIl2.Text == "" || cmbBxIlce.Text == "" || cmbBxIlce2.Text == "" || richTxtBoxAdres.Text == "" || richTxtBoxAdres2.Text == "" || txtBxAdSoyad.Text == "" || txtBxAdSoyad2.Text == "" || txtBxTelefon.Text == "" || txtBxTelefon2.Text == "" || txtBxDesi.Text == "" || cmbBxOdemeBilgisi.Text == "" || cmbBxKargoTipi.Text == "" || txtBxVg2.Text == ""))
                    MessageBox.Show("Boş Yerleri Doldurunuz!");
                else
                {
                    if (veiOkumaSayisi == 0)
                    {
                        onayBtn = true;

                        veiOkumaSayisi++;
                        string VG = "";
                        string VG2 = "";
                        if (txtBxVg.Text != "")
                            VG = txtBxVg.Text;
                        else
                            VG = "";
                        if (txtBxVg2.Text != "")
                            VG2 = txtBxVg2.Text;
                        else
                            VG2 = "";

                        double desi = Convert.ToDouble(txtBxDesi.Text);
                        string tarih = DateTime.Now.ToShortDateString();
                        char ayrac2 = '.';
                        string[] tarih1 = tarih.Split(ayrac2);
                        string yıl2 = tarih1[2];
                        string ay2 = tarih1[1];
                        string gun2 = tarih1[0];
                        string tarihİlk2 = yıl2 + "-" + ay2 + "-" + gun2;
                        string saat = DateTime.Now.ToLongTimeString();
                        long barkodNo = 0;
                        SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                        bg.Open();
                        SqlCommand cmd = new SqlCommand("SELECT CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) AS RandomNumber WHERE CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) NOT IN (SELECT barkodNo FROM KargoBilgileri)", bg);
                        SqlDataReader dr = cmd.ExecuteReader();
                        //WITH cte AS(SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber UNION ALL SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber FROM cte WHERE(SELECT COUNT(*) FROM KargoBilgileri WHERE barkodNo = randomNumber) > 0) SELECT TOP 1 randomNumber as UretilenSayi FROM cte OPTION(MAXRECURSION 0)WHILE EXISTS(SELECT* FROM tabloAdi WHERE barkodNo = @randomNumber) BEGIN SET @randomNumber = ABS(CHECKSUM(NewId())) % 9000000000 + 1000000000; END SELECT @randomNumber as UretilenSayi

                        while (dr.Read())
                        {
                            barkodNo = (long)dr["RandomNumber"];
                        }
                        bg.Close();

                        BarkodNo = barkodNo;

                        barkodYazdir = barkodNo.ToString();
                        adSoyadYazdirA = txtBxAdSoyad.Text;
                        adSoyadYazdirG = txtBxAdSoyad2.Text;
                        ilYazdirA = cmbBxIl.Text;
                        ilYazdirG = cmbBxIl2.Text;
                        ilceYazdirA = cmbBxIlce.Text;
                        ilceYazdirG = cmbBxIlce2.Text;
                        adresYazdirA = richTxtBoxAdres.Text;
                        adresYazdirG = richTxtBoxAdres2.Text;
                        telefonYazdirA = txtBxTelefon.Text;
                        telefonYazdirG = txtBxTelefon2.Text;
                        ePostaYazdirA = txtBxEmail.Text;
                        ePostaYazdirG = txtBxEmail2.Text;
                        odemeSekliYazdir = cmbBxOdemeBilgisi.Text;
                        kargoTipiYazdir = cmbBxKargoTipi.Text;
                        agirlikYazdir = txtBxAgirlik.Text;
                        fiyatYazdir = txtBxFiyat.Text;
                        if (checkBxKorumali.Checked == true)
                            korumalimiYazdir = true;
                        if (checkBxSigortali.Checked == true)
                            sigortalimiYazdir = true;


                        bg.Open();
                        string kayit = "insert into KargoBilgileri(barkodNo, alimTarihi, alimSaati, alimYapanPersonelId, aliciAdSoyad, gondericiAdSoyad, kargoTipi, desi, odemeBilgisi, kargoDurumu, fiyat, teslimTarihi, teslimSaati, teslimEdenPersonelId, sonDegisiklikTarihi, sonDegisiklikSaati) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)";
                        SqlCommand komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", tarihİlk2);
                        komut.Parameters.AddWithValue("@p3", saat);
                        komut.Parameters.AddWithValue("@p4", Giris.personelId);
                        komut.Parameters.AddWithValue("@p5", txtBxAdSoyad.Text);
                        komut.Parameters.AddWithValue("@p6", txtBxAdSoyad2.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxKargoTipi.Text);
                        komut.Parameters.AddWithValue("@p8", desi);
                        komut.Parameters.AddWithValue("@p9", cmbBxOdemeBilgisi.Text);
                        komut.Parameters.AddWithValue("@p10", "Teslim Alındı");
                        komut.Parameters.AddWithValue("@p11", txtBxFiyat.Text);
                        komut.Parameters.AddWithValue("@p12", tarihİlk2);
                        komut.Parameters.AddWithValue("@p13", saat);
                        komut.Parameters.AddWithValue("@p14", Giris.personelId);
                        komut.Parameters.AddWithValue("@p15", tarihİlk2);
                        komut.Parameters.AddWithValue("@p16", saat);
                        komut.ExecuteNonQuery();
                        bg.Close();

                        bg.Open();
                        komut = new SqlCommand("insert into kargoHareketleri(barkodNo, taTarihi, taSaati, taIl, taIlce, trTarihi, trSaati, trIl, trIlce, tsTarihi, tsSaati, tsIl, tsIlce, kdTarihi, kdSaati, kdIl, kdIlce, tTarihi, tSaati, tIl, tIlce) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21)", bg);
                        komut.Parameters.AddWithValue("@p1", SqlDbType.BigInt).Value = barkodNo;
                        komut.Parameters.AddWithValue("@p2", SqlDbType.Date).Value = tarihİlk2;
                        komut.Parameters.AddWithValue("@p3", SqlDbType.Time).Value = saat;
                        komut.Parameters.AddWithValue("@p4", SqlDbType.NVarChar).Value = cmbBxIl.Text;
                        komut.Parameters.AddWithValue("@p5", SqlDbType.NVarChar).Value = cmbBxIlce.Text;
                        komut.Parameters.AddWithValue("@p6", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p7", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p8", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p9", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p10", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p11", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p12", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p13", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p14", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p15", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p16", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p17", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p18", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p19", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p20", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p21", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.ExecuteNonQuery();
                        bg.Close();

                        string adresTipi = "";
                        string aliciTipi = "";
                        string adresTipi2 = "";
                        string gondericiTipi = "";

                        if (radioBtnEv.Checked == true)
                            adresTipi = "Ev";
                        else if (radioBtnIs.Checked == true)
                            adresTipi = "İş";
                        else if (radioBtnDiger.Checked == true)
                            adresTipi = "Diğer";

                        if (radioBtnEv2.Checked == true)
                            adresTipi2 = "Ev";
                        else if (radioBtnIs2.Checked == true)
                            adresTipi2 = "İş";
                        else if (radioBtnDiger2.Checked == true)
                            adresTipi2 = "Diğer";

                        if (radioBtnBireysel.Checked == true)
                            aliciTipi = "Bireysel";
                        else if (radioBtnKurumsal.Checked == true)
                            aliciTipi = "Kurumsal";

                        if (radioBtnBireysel2.Checked == true)
                            gondericiTipi = "Bireysel";
                        else if (radioBtnKurumsal2.Checked == true)
                            gondericiTipi = "Kurumsal";

                        bg.Open();
                        kayit = "insert into aliciBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                        komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", txtBxTc.Text);
                        komut.Parameters.AddWithValue("@p3", txtBxAdSoyad.Text);
                        komut.Parameters.AddWithValue("@p4", txtBxTelefon.Text);
                        komut.Parameters.AddWithValue("@p5", VG);
                        komut.Parameters.AddWithValue("@p6", txtBxEmail.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxIl.Text);
                        komut.Parameters.AddWithValue("@p8", cmbBxIlce.Text);
                        komut.Parameters.AddWithValue("@p9", richTxtBoxAdres.Text);
                        komut.Parameters.AddWithValue("@p10", adresTipi);
                        komut.Parameters.AddWithValue("@p11", aliciTipi);
                        komut.ExecuteNonQuery();
                        bg.Close();
                        bg.Open();
                        kayit = "insert into gondericiBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                        komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", txtBxTc2.Text);
                        komut.Parameters.AddWithValue("@p3", txtBxAdSoyad2.Text);
                        komut.Parameters.AddWithValue("@p4", txtBxTelefon2.Text);
                        komut.Parameters.AddWithValue("@p5", VG2);
                        komut.Parameters.AddWithValue("@p6", txtBxEmail2.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxIl2.Text);
                        komut.Parameters.AddWithValue("@p8", cmbBxIlce2.Text);
                        komut.Parameters.AddWithValue("@p9", richTxtBoxAdres2.Text);
                        komut.Parameters.AddWithValue("@p10", adresTipi2);
                        komut.Parameters.AddWithValue("@p11", gondericiTipi);
                        komut.ExecuteNonQuery();
                        bg.Close();

                        MessageBox.Show("Kargo Onaylandı");

                        string AdresTipi = "";
                        string MusteriTipi = "";
                        string kayitliMusteri = "";
                        string kayitliMusteri2 = "";

                        bg.Open();
                        cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                        SqlDataReader oku = cmd.ExecuteReader();
                        while (oku.Read())
                        {
                            kayitliMusteri = oku["TcNo"].ToString();
                        }
                        bg.Close();
                        bg.Open();
                        cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                        oku = cmd.ExecuteReader();
                        while (oku.Read())
                        {
                            kayitliMusteri2 = oku["TcNo"].ToString();
                        }
                        bg.Close();

                        if (txtBxTc.Text != "")
                        {
                            if (kayitliMusteri != txtBxTc.Text)
                            {
                                if (radioBtnEv.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                                komut.Parameters.AddWithValue("@p4", VG);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                            if (kayitliMusteri == txtBxTc.Text)
                            {
                                bg.Open();
                                SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                                verisil.ExecuteNonQuery();
                                bg.Close();

                                if (radioBtnEv.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                                komut.Parameters.AddWithValue("@p4", VG);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                        }

                        if (txtBxTc2.Text != "")
                        {
                            if (kayitliMusteri2 != txtBxTc2.Text)
                            {
                                if (radioBtnEv2.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs2.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger2.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel2.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal2.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                                komut.Parameters.AddWithValue("@p4", VG2);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                            if (kayitliMusteri2 == txtBxTc2.Text)
                            {
                                bg.Open();
                                SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                                verisil.ExecuteNonQuery();
                                bg.Close();

                                if (radioBtnEv2.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs2.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger2.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel2.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal2.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                                komut.Parameters.AddWithValue("@p4", VG2);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();

                            }
                        }
                    }
                }
            }
            else
            {
                if ((cmbBxIl.Text == "" || cmbBxIl2.Text == "" || cmbBxIlce.Text == "" || cmbBxIlce2.Text == "" || richTxtBoxAdres.Text == "" || richTxtBoxAdres2.Text == "" || txtBxAdSoyad.Text == "" || txtBxAdSoyad2.Text == "" || txtBxTelefon.Text == "" || txtBxTelefon2.Text == "" || txtBxDesi.Text == "" || cmbBxOdemeBilgisi.Text == "" || cmbBxKargoTipi.Text == ""))
                    MessageBox.Show("Boş Yerleri Doldurunuz!");
                else
                {
                    if (veiOkumaSayisi == 0)
                    {
                        onayBtn = true;

                        veiOkumaSayisi++;
                        string VG = "";
                        string VG2 = "";
                        if (txtBxVg.Text != "")
                            VG = txtBxVg.Text;
                        else
                            VG = "";
                        if (txtBxVg2.Text != "")
                            VG2 = txtBxVg2.Text;
                        else
                            VG2 = "";

                        double desi = Convert.ToDouble(txtBxDesi.Text);
                        string tarih = DateTime.Now.ToShortDateString();
                        char ayrac2 = '.';
                        string[] tarih1 = tarih.Split(ayrac2);
                        string yıl2 = tarih1[2];
                        string ay2 = tarih1[1];
                        string gun2 = tarih1[0];
                        string tarihİlk2 = yıl2 + "-" + ay2 + "-" + gun2;
                        string saat = DateTime.Now.ToLongTimeString();
                        long barkodNo = 0;
                        SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                        bg.Open();
                        SqlCommand cmd = new SqlCommand("SELECT CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) AS RandomNumber WHERE CAST(RAND(CHECKSUM(NEWID()))*(9999999999-1000000000)+1000000000 AS bigint) NOT IN (SELECT barkodNo FROM KargoBilgileri)", bg);
                        SqlDataReader dr = cmd.ExecuteReader();
                        //WITH cte AS(SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber UNION ALL SELECT CAST(RAND(CHECKSUM(NEWID())) * 900000000 + 100000000 AS BIGINT) AS randomNumber FROM cte WHERE(SELECT COUNT(*) FROM KargoBilgileri WHERE barkodNo = randomNumber) > 0) SELECT TOP 1 randomNumber as UretilenSayi FROM cte OPTION(MAXRECURSION 0)WHILE EXISTS(SELECT* FROM tabloAdi WHERE barkodNo = @randomNumber) BEGIN SET @randomNumber = ABS(CHECKSUM(NewId())) % 9000000000 + 1000000000; END SELECT @randomNumber as UretilenSayi

                        while (dr.Read())
                        {
                            barkodNo = (long)dr["RandomNumber"];
                        }
                        bg.Close();

                        BarkodNo = barkodNo;

                        barkodYazdir = barkodNo.ToString();
                        adSoyadYazdirA = txtBxAdSoyad.Text;
                        adSoyadYazdirG = txtBxAdSoyad2.Text;
                        ilYazdirA = cmbBxIl.Text;
                        ilYazdirG = cmbBxIl2.Text;
                        ilceYazdirA = cmbBxIlce.Text;
                        ilceYazdirG = cmbBxIlce2.Text;
                        adresYazdirA = richTxtBoxAdres.Text;
                        adresYazdirG = richTxtBoxAdres2.Text;
                        telefonYazdirA = txtBxTelefon.Text;
                        telefonYazdirG = txtBxTelefon2.Text;
                        ePostaYazdirA = txtBxEmail.Text;
                        ePostaYazdirG = txtBxEmail2.Text;
                        odemeSekliYazdir = cmbBxOdemeBilgisi.Text;
                        kargoTipiYazdir = cmbBxKargoTipi.Text;
                        agirlikYazdir = txtBxAgirlik.Text;
                        fiyatYazdir = txtBxFiyat.Text;
                        if (checkBxKorumali.Checked == true)
                            korumalimiYazdir = true;
                        if (checkBxSigortali.Checked == true)
                            sigortalimiYazdir = true;


                        bg.Open();
                        string kayit = "insert into KargoBilgileri(barkodNo, alimTarihi, alimSaati, alimYapanPersonelId, aliciAdSoyad, gondericiAdSoyad, kargoTipi, desi, odemeBilgisi, kargoDurumu, fiyat, teslimTarihi, teslimSaati, teslimEdenPersonelId, sonDegisiklikTarihi, sonDegisiklikSaati) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)";
                        SqlCommand komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", tarihİlk2);
                        komut.Parameters.AddWithValue("@p3", saat);
                        komut.Parameters.AddWithValue("@p4", Giris.personelId);
                        komut.Parameters.AddWithValue("@p5", txtBxAdSoyad.Text);
                        komut.Parameters.AddWithValue("@p6", txtBxAdSoyad2.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxKargoTipi.Text);
                        komut.Parameters.AddWithValue("@p8", desi);
                        komut.Parameters.AddWithValue("@p9", cmbBxOdemeBilgisi.Text);
                        komut.Parameters.AddWithValue("@p10", "Teslim Alındı");
                        komut.Parameters.AddWithValue("@p11", txtBxFiyat.Text);
                        komut.Parameters.AddWithValue("@p12", tarihİlk2);
                        komut.Parameters.AddWithValue("@p13", saat);
                        komut.Parameters.AddWithValue("@p14", Giris.personelId);
                        komut.Parameters.AddWithValue("@p15", tarihİlk2);
                        komut.Parameters.AddWithValue("@p16", saat);
                        komut.ExecuteNonQuery();
                        bg.Close();

                        bg.Open();
                        komut = new SqlCommand("insert into kargoHareketleri(barkodNo, taTarihi, taSaati, taIl, taIlce, trTarihi, trSaati, trIl, trIlce, tsTarihi, tsSaati, tsIl, tsIlce, kdTarihi, kdSaati, kdIl, kdIlce, tTarihi, tSaati, tIl, tIlce) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21)", bg);
                        komut.Parameters.AddWithValue("@p1", SqlDbType.BigInt).Value = barkodNo;
                        komut.Parameters.AddWithValue("@p2", SqlDbType.Date).Value = tarihİlk2;
                        komut.Parameters.AddWithValue("@p3", SqlDbType.Time).Value = saat;
                        komut.Parameters.AddWithValue("@p4", SqlDbType.NVarChar).Value = cmbBxIl.Text;
                        komut.Parameters.AddWithValue("@p5", SqlDbType.NVarChar).Value = cmbBxIlce.Text;
                        komut.Parameters.AddWithValue("@p6", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p7", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p8", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p9", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p10", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p11", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p12", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p13", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p14", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p15", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p16", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p17", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p18", SqlDbType.Date).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p19", SqlDbType.Time).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p20", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.Parameters.AddWithValue("@p21", SqlDbType.NVarChar).Value = DBNull.Value;
                        komut.ExecuteNonQuery();
                        bg.Close();

                        string adresTipi = "";
                        string aliciTipi = "";
                        string adresTipi2 = "";
                        string gondericiTipi = "";

                        if (radioBtnEv.Checked == true)
                            adresTipi = "Ev";
                        else if (radioBtnIs.Checked == true)
                            adresTipi = "İş";
                        else if (radioBtnDiger.Checked == true)
                            adresTipi = "Diğer";

                        if (radioBtnEv2.Checked == true)
                            adresTipi2 = "Ev";
                        else if (radioBtnIs2.Checked == true)
                            adresTipi2 = "İş";
                        else if (radioBtnDiger2.Checked == true)
                            adresTipi2 = "Diğer";

                        if (radioBtnBireysel.Checked == true)
                            aliciTipi = "Bireysel";
                        else if (radioBtnKurumsal.Checked == true)
                            aliciTipi = "Kurumsal";

                        if (radioBtnBireysel2.Checked == true)
                            gondericiTipi = "Bireysel";
                        else if (radioBtnKurumsal2.Checked == true)
                            gondericiTipi = "Kurumsal";

                        bg.Open();
                        kayit = "insert into aliciBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                        komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", txtBxTc.Text);
                        komut.Parameters.AddWithValue("@p3", txtBxAdSoyad.Text);
                        komut.Parameters.AddWithValue("@p4", txtBxTelefon.Text);
                        komut.Parameters.AddWithValue("@p5", VG);
                        komut.Parameters.AddWithValue("@p6", txtBxEmail.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxIl.Text);
                        komut.Parameters.AddWithValue("@p8", cmbBxIlce.Text);
                        komut.Parameters.AddWithValue("@p9", richTxtBoxAdres.Text);
                        komut.Parameters.AddWithValue("@p10", adresTipi);
                        komut.Parameters.AddWithValue("@p11", aliciTipi);
                        komut.ExecuteNonQuery();
                        bg.Close();
                        bg.Open();
                        kayit = "insert into gondericiBilgileri(barkodNo, TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, aliciTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";
                        komut = new SqlCommand(kayit, bg);
                        komut.Parameters.AddWithValue("@p1", barkodNo);
                        komut.Parameters.AddWithValue("@p2", txtBxTc2.Text);
                        komut.Parameters.AddWithValue("@p3", txtBxAdSoyad2.Text);
                        komut.Parameters.AddWithValue("@p4", txtBxTelefon2.Text);
                        komut.Parameters.AddWithValue("@p5", VG2);
                        komut.Parameters.AddWithValue("@p6", txtBxEmail2.Text);
                        komut.Parameters.AddWithValue("@p7", cmbBxIl2.Text);
                        komut.Parameters.AddWithValue("@p8", cmbBxIlce2.Text);
                        komut.Parameters.AddWithValue("@p9", richTxtBoxAdres2.Text);
                        komut.Parameters.AddWithValue("@p10", adresTipi2);
                        komut.Parameters.AddWithValue("@p11", gondericiTipi);
                        komut.ExecuteNonQuery();
                        bg.Close();

                        MessageBox.Show("Kargo Onaylandı");

                        string AdresTipi = "";
                        string MusteriTipi = "";
                        string kayitliMusteri = "";
                        string kayitliMusteri2 = "";

                        bg.Open();
                        cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                        SqlDataReader oku = cmd.ExecuteReader();
                        while (oku.Read())
                        {
                            kayitliMusteri = oku["TcNo"].ToString();
                        }
                        bg.Close();
                        bg.Open();
                        cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                        oku = cmd.ExecuteReader();
                        while (oku.Read())
                        {
                            kayitliMusteri2 = oku["TcNo"].ToString();
                        }
                        bg.Close();

                        if (txtBxTc.Text != "")
                        {
                            if (kayitliMusteri != txtBxTc.Text)
                            {
                                if (radioBtnEv.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                                komut.Parameters.AddWithValue("@p4", VG);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                            if (kayitliMusteri == txtBxTc.Text)
                            {
                                bg.Open();
                                SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                                verisil.ExecuteNonQuery();
                                bg.Close();

                                if (radioBtnEv.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon.Text);
                                komut.Parameters.AddWithValue("@p4", VG);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                        }

                        if (txtBxTc2.Text != "")
                        {
                            if (kayitliMusteri2 != txtBxTc2.Text)
                            {
                                if (radioBtnEv2.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs2.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger2.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel2.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal2.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                                komut.Parameters.AddWithValue("@p4", VG2);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();
                            }
                            if (kayitliMusteri2 == txtBxTc2.Text)
                            {
                                bg.Open();
                                SqlCommand verisil = new SqlCommand("delete from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                                verisil.ExecuteNonQuery();
                                bg.Close();

                                if (radioBtnEv2.Checked == true)
                                    AdresTipi = "Ev";
                                else if (radioBtnIs2.Checked == true)
                                    AdresTipi = "İş";
                                else if (radioBtnDiger2.Checked == true)
                                    AdresTipi = "Diğer";

                                if (radioBtnBireysel2.Checked == true)
                                    MusteriTipi = "Bireysel";
                                else if (radioBtnKurumsal2.Checked == true)
                                    MusteriTipi = "Kurumsal";

                                bg.Open();
                                kayit = "insert into kayitliMusteri(TcNo, adSoyad, telefon, VgNo, email, il, ilce, adres, adresTipi, musteriTipi) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                                komut = new SqlCommand(kayit, bg);
                                komut.Parameters.AddWithValue("@p1", txtBxTc2.Text);
                                komut.Parameters.AddWithValue("@p2", txtBxAdSoyad2.Text);
                                komut.Parameters.AddWithValue("@p3", txtBxTelefon2.Text);
                                komut.Parameters.AddWithValue("@p4", VG2);
                                komut.Parameters.AddWithValue("@p5", txtBxEmail2.Text);
                                komut.Parameters.AddWithValue("@p6", cmbBxIl2.Text);
                                komut.Parameters.AddWithValue("@p7", cmbBxIlce2.Text);
                                komut.Parameters.AddWithValue("@p8", richTxtBoxAdres2.Text);
                                komut.Parameters.AddWithValue("@p9", AdresTipi);
                                komut.Parameters.AddWithValue("@p10", MusteriTipi);
                                komut.ExecuteNonQuery();
                                bg.Close();

                            }
                        }
                    }
                }
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.Show();
            this.Hide();
        }

        private void txtBxDesi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtBxFiyat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtBxAgirlik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxEn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxYukseklik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxBoy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar);

            if (txtBxTc.Text.Length >= 11)
            {
                e.Handled = true;
                e.Handled = char.IsDigit(e.KeyChar);
            }
            else
                e.Handled = false;

            if (txtBxTc.Text.Length==11)
            {
                string adresTipi = "";
                string musteriTipi = "";
                long vg = 0;
                SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                bg.Open();
                SqlCommand cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc.Text + "", bg);
                SqlDataReader oku = cmd.ExecuteReader();
                while (oku.Read())
                {
                    cmbBxIl.Text = oku["il"].ToString();
                    cmbBxIlce.Text = oku["ilce"].ToString();
                    txtBxAdSoyad.Text = oku["adSoyad"].ToString();
                    txtBxTelefon.Text = oku["telefon"].ToString();
                    txtBxEmail.Text = oku["email"].ToString();
                    richTxtBoxAdres.Text = oku["adres"].ToString();
                    vg = (long)oku["VgNo"];
                    adresTipi = oku["adresTipi"].ToString();
                    musteriTipi = oku["musteriTipi"].ToString();
                }
                bg.Close();
                if (adresTipi == "Ev")
                    radioBtnEv.Checked = true;
                else if (adresTipi == "İş")
                    radioBtnIs.Checked = true;
                else if (adresTipi == "Diğer")
                    radioBtnDiger.Checked = true;

                if (musteriTipi == "Bireysel")
                    radioBtnBireysel.Checked = true;
                else if (musteriTipi == "Kurumsal")
                {
                    radioBtnKurumsal.Checked = true;
                    txtBxVg.Text = vg.ToString();
                }
            }
        }

        private void txtBxTc2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar);

            if (txtBxTc2.Text.Length >= 11)
            {
                e.Handled = true;
                e.Handled = char.IsDigit(e.KeyChar);
            }
            else
            {
                e.Handled = false;
            }
            if (txtBxTc2.Text.Length == 11)
            {
                string adresTipi = "";
                string musteriTipi = "";
                long vg = 0;
                SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                bg.Open();
                SqlCommand cmd = new SqlCommand("select * from kayitliMusteri where TcNo=" + txtBxTc2.Text + "", bg);
                SqlDataReader oku = cmd.ExecuteReader();
                while (oku.Read())
                {
                    cmbBxIl2.Text = oku["il"].ToString();
                    cmbBxIlce2.Text = oku["ilce"].ToString();
                    txtBxAdSoyad2.Text = oku["adSoyad"].ToString();
                    txtBxTelefon2.Text = oku["telefon"].ToString();
                    txtBxEmail2.Text = oku["email"].ToString();
                    richTxtBoxAdres2.Text = oku["adres"].ToString();
                    vg = (long)oku["VgNo"];
                    adresTipi = oku["adresTipi"].ToString();
                    musteriTipi = oku["musteriTipi"].ToString();
                }
                bg.Close();
                if (adresTipi == "Ev")
                    radioBtnEv2.Checked = true;
                else if (adresTipi == "İş")
                    radioBtnIs2.Checked = true;
                else if (adresTipi == "Diğer")
                    radioBtnDiger2.Checked = true;

                if (musteriTipi == "Bireysel")
                    radioBtnBireysel2.Checked = true;
                else if (musteriTipi == "Kurumsal")
                {
                    radioBtnKurumsal2.Checked = true;
                    txtBxVg2.Text = vg.ToString();
                }
            }
        }

        private void txtBxTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar);

            if (txtBxTelefon.Text.Length >= 11)
            {
                e.Handled = true;
                e.Handled = char.IsDigit(e.KeyChar);
            }
            else
                e.Handled = false;
        }

        private void txtBxTelefon2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar);

            if (txtBxTelefon2.Text.Length >= 11)
            {
                e.Handled = true;
                e.Handled = char.IsDigit(e.KeyChar);
            }
            else
                e.Handled = false;
        }


        private void txtBxVg_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxVg2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbBxIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBxIlce.Items.Clear();
            ilceEkle(cmbBxIl, cmbBxIlce);
        }

        private void cmbBxIl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBxIlce2.Items.Clear();
            ilceEkle(cmbBxIl2, cmbBxIlce2);
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            
            if (txtBxYukseklik.Text!= "" && txtBxEn.Text != "" && txtBxBoy.Text != "" && txtBxAgirlik.Text != "" && cmbBxOdemeBilgisi.SelectedIndex != -1)
            {
                bool korumalı = false;
                bool sigortalı = false;

                if (checkBxKorumali.Checked == true)
                    korumalı = true;
                else
                    korumalı = false;

                if (checkBxSigortali.Checked == true)
                    sigortalı = true;
                else
                    sigortalı = false;

                double en = Convert.ToDouble(txtBxEn.Text);
                double boy = Convert.ToDouble(txtBxBoy.Text);
                double yukseklik = Convert.ToDouble(txtBxYukseklik.Text);
                double agirlik = Convert.ToDouble(txtBxAgirlik.Text);
                double sonuc = (en * boy * yukseklik) / 6000;

                if (agirlik > 0 && agirlik < 1000)
                    agirlik = 1;
                else if (agirlik >= 1000 && agirlik < 2000)
                    agirlik = 2;
                else if (agirlik >= 3000 && agirlik < 4000)
                    agirlik = 3;
                else if (agirlik >= 4000 && agirlik < 5000)
                    agirlik = 4;
                else if (agirlik >= 5000 && agirlik < 6000)
                    agirlik = 5;
                else if (agirlik >= 6000 && agirlik < 7000)
                    agirlik = 6;
                else if (agirlik >= 7000 && agirlik < 8000)
                    agirlik = 7;
                else if (agirlik >= 8000 && agirlik < 9000)
                    agirlik = 8;
                else if (agirlik >= 9000 && agirlik < 10000)
                    agirlik = 9;
                else if (agirlik >= 10000 && agirlik < 11000)
                    agirlik = 10;
                else if (agirlik >= 11000 && agirlik < 12000)
                    agirlik = 11;
                else if (agirlik >= 12000 && agirlik < 13000)
                    agirlik = 12;
                else if (agirlik >= 13000 && agirlik < 14000)
                    agirlik = 13;
                else if (agirlik >= 14000 && agirlik < 15000)
                    agirlik = 14;
                else if (agirlik >= 15000 && agirlik < 16000)
                    agirlik = 15;
                else if (agirlik >= 16000 && agirlik < 17000)
                    agirlik = 16;
                else if (agirlik >= 17000 && agirlik < 18000)
                    agirlik = 17;
                else if (agirlik >= 18000 && agirlik < 19000)
                    agirlik = 18;
                else if (agirlik >= 19000 && agirlik < 20000)
                    agirlik = 19;
                else if (agirlik >= 20000 && agirlik < 21000)
                    agirlik = 20;
                else if (agirlik >= 21000 && agirlik < 22000)
                    agirlik = 21;
                else if (agirlik >= 22000 && agirlik < 23000)
                    agirlik = 22;
                else if (agirlik >= 23000 && agirlik < 24000)
                    agirlik = 23;
                else if (agirlik >= 24000 && agirlik < 25000)
                    agirlik = 24;
                else if (agirlik >= 25000 && agirlik < 26000)
                    agirlik = 25;

                if (agirlik > sonuc)
                {
                    desi = agirlik;
                    desi = desi / 1000;
                }
                else
                    desi = sonuc;

                txtBxDesi.Text = desi.ToString();

                int Desi = Convert.ToInt32(desi);

                desiYazdir = Desi.ToString();

                double fiyat = 0;
                string kargoTipi = cmbBxKargoTipi.Text;
                SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                bg.Open();
                SqlCommand cmd = new SqlCommand("select * from Fiyatlar where desi=" + Desi + " and kargoTipi='" + kargoTipi + "' and korumali='" + korumalı + "' and sigortali='" + sigortalı + "'", bg);
                SqlDataReader oku = cmd.ExecuteReader();
                while (oku.Read())
                    fiyat = (double)oku["fiyat"];
                bg.Close();

                if (cmbBxOdemeBilgisi.Text == "Karşı Ödemeli")
                {
                    fiyat = fiyat + 7;
                }

                txtBxFiyat.Text = fiyat.ToString();
            }
            else
                MessageBox.Show("Lütfen Boş Alanları Doldurunuz!");
        
        }

        private void txtBxAdSoyad2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtBxAdSoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void cmbBxKargoTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBxAgirlik.Enabled = true;
            txtBxEn.Enabled = true;
            txtBxBoy.Enabled = true;
            txtBxYukseklik.Enabled = true;
        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            if (onayBtn==true)
            {
                DialogResult dialogResult = MessageBox.Show("Kargo Fişini İptal Etmek İstiyormusnuz?", "Uyarı!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    AnaSayfa anaSayfa = new AnaSayfa();
                    anaSayfa.Show();
                    this.Hide();

                    txtBxAdSoyad.Text = "";
                    txtBxAdSoyad2.Text = "";
                    txtBxTc.Text = "";
                    txtBxTc2.Text = "";
                    txtBxVg.Text = "";
                    txtBxVg2.Text = "";
                    txtBxEmail.Text = "";
                    txtBxEmail2.Text = "";
                    txtBxTelefon.Text = "";
                    txtBxTelefon2.Text = "";
                    richTxtBoxAdres.Text = "";
                    richTxtBoxAdres2.Text = "";
                    cmbBxIl.Text = "";
                    cmbBxIl2.Text = "";
                    cmbBxIlce.Text = "";
                    cmbBxIlce2.Text = "";
                    cmbBxMahalle.Text = "";
                    cmbBxMahalle2.Text = "";
                    cmbBxCadde.Text = "";
                    cmbBxCadde2.Text = "";
                    cmbBxSokak.Text = "";
                    cmbBxSokak2.Text = "";
                    cmbBxKargoTipi.Text = "";
                    cmbBxOdemeBilgisi.Text = "";
                    txtBxAgirlik.Text = "";
                    txtBxEn.Text = "";
                    txtBxBoy.Text = "";
                    txtBxYukseklik.Text = "";
                    txtBxDesi.Text = "";
                    txtBxFiyat.Text = "";
                    checkBxKorumali.Checked = false;
                    checkBxSigortali.Checked = false;
                    radioBtnEv.Checked = true;
                    radioBtnEv2.Checked = true;
                    radioBtnBireysel.Checked = true;
                    radioBtnBireysel2.Checked = true;

                    SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
                    bg.Open();
                    SqlCommand verisil = new SqlCommand("delete from KargoBilgileri where barkodNo=" + BarkodNo + "", bg);
                    verisil.ExecuteNonQuery();
                    bg.Close();
                }
            }
            else
            {

            }
        }
        
        private void btnYazdir_Click(object sender, EventArgs e)
        {
            if (onayBtn == false)
                MessageBox.Show("Kargoyu Onaylamadan Yazdıramazsınız!");
            else
                printPreviewDialog1.ShowDialog();
        }

        private void KargoFisiYazdir_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string kargoDurumu = "";
            SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from KargoBilgileri where barkodNo=" + barkodYazdir + "", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
                kargoDurumu = (string)oku["kargoDurumu"];
            bg.Close();
            // Oluşturmak istediğiniz barkod verisini belirleyin
            string barcodeData = barkodYazdir + "\n" + kargoDurumu; // Örnek URL
            // Barkodun tipini ve özelliklerini ayarlayın
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 65, // Barkod yüksekliği
                    Width = 65,  // Barkod genişliği
                    Margin = 0   // Kenar boşluğu
                }
            };

            // Barkodu bir görüntü olarak oluşturun
            barcodeBitmap = barcodeWriter.Write(barcodeData);

            Font font = new Font("Arial", 14);
            SolidBrush firca = new SolidBrush(Color.Black);
            // Pen kalem = new Pen(Color.Black);
            e.Graphics.DrawString($"{DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}", font, firca, 50, 17);
            e.Graphics.DrawString("Barkod No: " + barkodYazdir, font, firca, 590, 17);

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
            e.Graphics.DrawString(desiYazdir, font2, firca, 363, 478);
            
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

            // Barkodun yazdırılacağı koordinatları belirleyin
            int x = 60;
            int y = 605;
            // Barkodu yazdırın
            e.Graphics.DrawImage(barcodeBitmap, x, y);
            e.Graphics.DrawImage(barcodeBitmap, 730, y);
            e.Graphics.DrawImage(barcodeBitmap, 400, y);

            cizgi = new Font("Arial", 15);
            e.Graphics.DrawString("----------------------------------------------------------------------------------------------------------------------", cizgi, firca, 9, 670);

        }

        private void radioBtnKurumsal_CheckedChanged(object sender, EventArgs e)
        {
            label50.Visible = true;
        }

        private void radioBtnKurumsal2_CheckedChanged(object sender, EventArgs e)
        {
            label51.Visible = true;
        }

        private void radioBtnBireysel_CheckedChanged(object sender, EventArgs e)
        {
            label50.Visible = false;
        }

        private void radioBtnBireysel2_CheckedChanged(object sender, EventArgs e)
        {
            label51.Visible = false;
        }

        private void btnAlanlarıTemizle_Click(object sender, EventArgs e)
        {
            txtBxAdSoyad.Text = "";
            txtBxAdSoyad2.Text = "";
            txtBxTc.Text = "";
            txtBxTc2.Text = "";
            txtBxVg.Text = "";
            txtBxVg2.Text = "";
            txtBxEmail.Text = "";
            txtBxEmail2.Text = "";
            txtBxTelefon.Text = "";
            txtBxTelefon2.Text = "";
            richTxtBoxAdres.Text = "";
            richTxtBoxAdres2.Text = "";
            cmbBxIl.Text = "";
            cmbBxIl2.Text = "";
            cmbBxIlce.Text = "";
            cmbBxIlce2.Text = "";
            cmbBxMahalle.Text = "";
            cmbBxMahalle2.Text = "";
            cmbBxCadde.Text = "";
            cmbBxCadde2.Text = "";
            cmbBxSokak.Text = "";
            cmbBxSokak2.Text = "";
            cmbBxKargoTipi.Text = "";
            cmbBxOdemeBilgisi.Text = "";
            txtBxAgirlik.Text = "";
            txtBxEn.Text = "";
            txtBxBoy.Text = "";
            txtBxYukseklik.Text = "";
            txtBxDesi.Text = "";
            txtBxFiyat.Text = "";
            checkBxKorumali.Checked = false;
            checkBxSigortali.Checked = false;
            radioBtnEv.Checked = true;
            radioBtnEv2.Checked = true;
            radioBtnBireysel.Checked=true;
            radioBtnBireysel2.Checked=true;
        }
    }
}
