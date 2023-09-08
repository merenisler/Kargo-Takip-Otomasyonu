namespace KargoTakip
{
    partial class KayitliKargolar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KayitliKargolar));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.lstViewRapor = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtBxBarkodNo = new System.Windows.Forms.TextBox();
            this.cmbBxKargoDurumu = new System.Windows.Forms.ComboBox();
            this.btnOnayla = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCikis = new System.Windows.Forms.Button();
            this.lblTarih = new System.Windows.Forms.Label();
            this.lblSaat = new System.Windows.Forms.Label();
            this.btnYazdir = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // lstViewRapor
            // 
            this.lstViewRapor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lstViewRapor.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstViewRapor.GridLines = true;
            this.lstViewRapor.HideSelection = false;
            this.lstViewRapor.Location = new System.Drawing.Point(12, 12);
            this.lstViewRapor.Name = "lstViewRapor";
            this.lstViewRapor.Size = new System.Drawing.Size(845, 607);
            this.lstViewRapor.TabIndex = 0;
            this.lstViewRapor.UseCompatibleStateImageBehavior = false;
            this.lstViewRapor.View = System.Windows.Forms.View.Details;
            this.lstViewRapor.SelectedIndexChanged += new System.EventHandler(this.lstViewRapor_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Barkod No";
            this.columnHeader1.Width = 175;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Gönderen Adı/Soyadı";
            this.columnHeader2.Width = 242;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Alıcı Adı/Soyadı";
            this.columnHeader3.Width = 179;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Kargo Durumu";
            this.columnHeader4.Width = 168;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Fiyat";
            this.columnHeader5.Width = 75;
            // 
            // txtBxBarkodNo
            // 
            this.txtBxBarkodNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtBxBarkodNo.Location = new System.Drawing.Point(876, 308);
            this.txtBxBarkodNo.Name = "txtBxBarkodNo";
            this.txtBxBarkodNo.Size = new System.Drawing.Size(224, 38);
            this.txtBxBarkodNo.TabIndex = 123;
            this.txtBxBarkodNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBxBarkodNo_KeyPress);
            // 
            // cmbBxKargoDurumu
            // 
            this.cmbBxKargoDurumu.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbBxKargoDurumu.FormattingEnabled = true;
            this.cmbBxKargoDurumu.Items.AddRange(new object[] {
            "Teslim Alındı",
            "Transfer Sürecinde",
            "Teslimat Şubesinde",
            "Kurye Dağıtımda",
            "Tamamlandı"});
            this.cmbBxKargoDurumu.Location = new System.Drawing.Point(1117, 308);
            this.cmbBxKargoDurumu.Name = "cmbBxKargoDurumu";
            this.cmbBxKargoDurumu.Size = new System.Drawing.Size(224, 39);
            this.cmbBxKargoDurumu.TabIndex = 124;
            this.cmbBxKargoDurumu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBxKargoDurumu_KeyPress);
            // 
            // btnOnayla
            // 
            this.btnOnayla.BackColor = System.Drawing.Color.YellowGreen;
            this.btnOnayla.FlatAppearance.BorderSize = 0;
            this.btnOnayla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnayla.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnOnayla.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOnayla.Location = new System.Drawing.Point(1001, 362);
            this.btnOnayla.Name = "btnOnayla";
            this.btnOnayla.Size = new System.Drawing.Size(224, 48);
            this.btnOnayla.TabIndex = 125;
            this.btnOnayla.Text = "Onayla";
            this.btnOnayla.UseVisualStyleBackColor = false;
            this.btnOnayla.Click += new System.EventHandler(this.btnOnayla_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(914, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 32);
            this.label2.TabIndex = 126;
            this.label2.Text = "Barkod No: ";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(1130, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 32);
            this.label1.TabIndex = 127;
            this.label1.Text = "Kargo Durumu: ";
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.Red;
            this.btnCikis.FlatAppearance.BorderSize = 0;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnCikis.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCikis.Location = new System.Drawing.Point(1210, 12);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(137, 49);
            this.btnCikis.TabIndex = 128;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // lblTarih
            // 
            this.lblTarih.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarih.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblTarih.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTarih.Location = new System.Drawing.Point(861, 564);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(353, 31);
            this.lblTarih.TabIndex = 129;
            this.lblTarih.Text = "Son Değişiklik Tarihi: ";
            // 
            // lblSaat
            // 
            this.lblSaat.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSaat.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblSaat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSaat.Location = new System.Drawing.Point(861, 594);
            this.lblSaat.Name = "lblSaat";
            this.lblSaat.Size = new System.Drawing.Size(451, 31);
            this.lblSaat.TabIndex = 130;
            this.lblSaat.Text = "Son Değişiklik Saati: ";
            // 
            // btnYazdir
            // 
            this.btnYazdir.BackColor = System.Drawing.Color.MediumAquamarine;
            this.btnYazdir.FlatAppearance.BorderSize = 0;
            this.btnYazdir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYazdir.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnYazdir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYazdir.Location = new System.Drawing.Point(866, 503);
            this.btnYazdir.Name = "btnYazdir";
            this.btnYazdir.Size = new System.Drawing.Size(152, 42);
            this.btnYazdir.TabIndex = 131;
            this.btnYazdir.Text = "Yazdır";
            this.btnYazdir.UseVisualStyleBackColor = false;
            this.btnYazdir.Click += new System.EventHandler(this.btnYazdir_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Text = "Baskı önizleme";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.KargoFisiYazdir_PrintPage);
            // 
            // KayitliKargolar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1359, 630);
            this.Controls.Add(this.btnYazdir);
            this.Controls.Add(this.lblSaat);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOnayla);
            this.Controls.Add(this.cmbBxKargoDurumu);
            this.Controls.Add(this.txtBxBarkodNo);
            this.Controls.Add(this.lstViewRapor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "KayitliKargolar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KayitliKargolar";
            this.Load += new System.EventHandler(this.KayitliKargolar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.ListView lstViewRapor;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnOnayla;
        private System.Windows.Forms.ComboBox cmbBxKargoDurumu;
        private System.Windows.Forms.TextBox txtBxBarkodNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.Label lblSaat;
        private System.Windows.Forms.Button btnYazdir;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
    }
}