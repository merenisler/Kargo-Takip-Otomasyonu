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
    public partial class KayitliMusteriler : Form
    {
        public KayitliMusteriler()
        {
            InitializeComponent();
        }

        public static string SqlConnection = "Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True";

        private void txtBxTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBxVg_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtBxTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar);

            if (txtBxTc.Text.Length > 11)
            {
                e.Handled = true;
                e.Handled = char.IsDigit(e.KeyChar);
            }
            else
            {
                e.Handled = false;
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.Show();
            this.Hide();
        }

        private void btnGoster_Click(object sender, EventArgs e)
        {

        }

        private void lstViewMusteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstViewMusteri.FullRowSelect = true;
            if (lstViewMusteri.SelectedItems.Count == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    string items = lstViewMusteri.SelectedItems[0].SubItems[i].Text.ToString();
                    if (i == 0)
                    {
                        txtBxTc.Text = items.ToString();
                    }
                    if (i == 1)
                    {
                        txtBxAdSoyad.Text = items.ToString();
                    }
                    if (i == 2)
                    {
                        txtBxAdres.Text = items.ToString();
                    }
                    if (i == 3)
                    {
                        txtBxTelefon.Text = items.ToString();
                    }
                    if (i == 4)
                    {
                        txtBxEmail.Text = items.ToString();
                    }
                    if (i == 5)
                    {
                        txtBxVg.Text = items.ToString();
                    }
                }
            }
        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            SqlConnection bg = new SqlConnection("Data Source=.;Initial Catalog=KargoTakip1;Integrated Security=True");
            bg.Open();
            SqlCommand cmd = new SqlCommand("update kayitliMusteri set TcNo=@p1, adSoyad=@p2", bg);
            try
            {
                //cmd.Parameters.AddWithValue("@p1", cmbBxKargoDurumu.Text);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException hata)
            {
                MessageBox.Show(hata.Message.ToString());
            }
            bg.Close();

        }

        private void KayitliMusteriler_Load(object sender, EventArgs e)
        {
            lstViewMusteri.Items.Clear();
            SqlConnection bg = new SqlConnection(SqlConnection);
            bg.Open();
            SqlCommand cmd = new SqlCommand("select * from kayitliMusteri", bg);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem listele = new ListViewItem();

                listele.Text = (oku["TcNo"].ToString());
                listele.SubItems.Add(oku["adSoyad"].ToString());
                listele.SubItems.Add(oku["il"].ToString());
                listele.SubItems.Add(oku["ilce"].ToString());
                listele.SubItems.Add(oku["adres"].ToString());
                listele.SubItems.Add(oku["telefon"].ToString());
                listele.SubItems.Add(oku["email"].ToString());
                listele.SubItems.Add(oku["VgNo"].ToString());
                listele.SubItems.Add(oku["adresTipi"].ToString());
                listele.SubItems.Add(oku["musteriTipi"].ToString());
                lstViewMusteri.Items.Add(listele);
            }
            bg.Close();
        }
    }
}
