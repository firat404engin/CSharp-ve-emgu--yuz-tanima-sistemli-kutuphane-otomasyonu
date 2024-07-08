using Emgu.CV.Geodetic;
using MultiFaceRec.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class odunc : Form
    {
        public odunc()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        public string eposta;
        public string kitapAdi;

        int maxOduncId = 0;


        void maxid()
        {
            string query = "SELECT MAX(CAST(oduncid AS INT)) FROM odunc";


            using (veritabani.baglanti())
            {
                using (SqlCommand command = new SqlCommand(query, veritabani.baglanti()))
                {
                    try
                    {
                        veritabani.baglanti();
                        object result = command.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            maxOduncId = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
        }
        void KitaplarıListele()
        {
            SqlConnection conn = veritabani.baglanti();
            string query = "SELECT kitapid,kitapadi,kitapyazar,kitaptur,ozet FROM kitap WHERE kitapadet>0";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            // Tabloyu DataGridView'e yükle
            dataGridView1.DataSource = dt;

            // Tüm sütunları otomatik olarak genişlet
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            conn.Close();
        }
        private void odunc_Load(object sender, EventArgs e)
        {
            KitaplarıListele();
            label1.Text = eposta;
            richTextBox1.Visible = false;
            maxid();

        }

        private void arama_Click(object sender, EventArgs e)
        {

            string aramaTerimi = kitapara.Text;
            SqlConnection conn = veritabani.baglanti();
            string query = "SELECT * FROM kitap WHERE kitapadi LIKE @aramaTerimi AND kitapadet > 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@aramaTerimi", "%" + aramaTerimi + "%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }
  
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            maxid();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Satır ve sütun seçildi mi kontrolü
            {
                // Seçilen satırı al
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Özet kısmındaki yazıyı RichTextBox'a yazdır
                richTextBox1.Text = row.Cells["ozet"].Value.ToString();

                // Kitabın adını al
                 kitapAdi = row.Cells["kitapadi"].Value.ToString();

                // Resmi dizinde ara
                string resimDizini = @"C:\Users\fengi\Desktop\proje2görüntü\Kitaplar"; // Resimlerin bulunduğu dizin
                string resimYolu = Path.Combine(resimDizini, kitapAdi + ".png"); // Resmin tam yolu

                // Resmi PictureBox'da göster
                if (File.Exists(resimYolu))
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Resmi boyutlandırma modunu ayarla
                    pictureBox1.Image = Image.FromFile(resimYolu);
                    richTextBox1.Visible= true;
                }
                else
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal; // PictureBox boyutlandırma modunu normal yap
                    pictureBox1.Image = null; // Resim bulunamazsa PictureBox'ı boşalt
                    richTextBox1.Visible = false;
                }
            }


        }


        private bool KitapOduncAl(int kitapid,int durum)
        {
            string kitapoduncdurum = label1.Text;
            try
            {
                SqlConnection connection = veritabani.baglanti();
                string query = "UPDATE kitap SET kitapoduncdurum = @kitapoduncdurum , durum=@durum,kitapadet = kitapadet - 1 WHERE kitapid = @kitapid";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@kitapid", kitapid);
                cmd.Parameters.AddWithValue("@kitapoduncdurum", kitapoduncdurum);
                cmd.Parameters.AddWithValue("@durum", durum);
                int affectedRows = cmd.ExecuteNonQuery();
                connection.Close();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap ödünç alınırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void KitapOduncAl2()
        {
            // Ödünç alınan kitabın bilgilerini eklemek için SQL sorgusu
            string query = "INSERT INTO odunc (oduncid,odunctc, odunckitap, odunctarih, odunciade) VALUES (@oduncid,@odunctc, @odunckitap, @odunctarih, DATEADD(day, 7, @odunctarih))";
            
            try
            {
                // Veritabanı bağlantısını oluştur
                SqlConnection connection = veritabani.baglanti();

                // Ödünç alınan kitabın tarih bilgilerini ayarla
                DateTime oduncTarihi = DateTime.Now;

                // SqlCommand ve parametreleri oluştur
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("oduncid", maxOduncId+1);
                cmd.Parameters.AddWithValue("@odunctc", eposta); // Kullanıcının TC'si
                cmd.Parameters.AddWithValue("@odunckitap", kitapAdi); // Ödünç alınan kitabın ID'si
                cmd.Parameters.AddWithValue("@odunctarih", oduncTarihi); // Ödünç alınan tarih
                maxOduncId = maxOduncId + 1;
                // Sorguyu çalıştır
                int rowsAffected = cmd.ExecuteNonQuery();

                // Bağlantıyı kapat
                connection.Close();

                // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kitap başarıyla ödünç alındı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kitap ödünç alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap ödünç alınırken bir hata oluştu: ! " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void oduncal_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
            veritabani veritabani = new veritabani();
            string query = "SELECT odunctc FROM odunc WHERE odunctc = @eposta";

            ogrenci_menu ogrenci_Menu = new ogrenci_menu();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int kitapid = Convert.ToInt32(row.Cells["kitapid"].Value);
                 kitapAdi = row.Cells["kitapadi"].Value.ToString();
                DateTime now = DateTime.Now;
                DateTime nextWeek = now.AddDays(7);

                DialogResult result = MessageBox.Show($"\"{kitapAdi}\" adlı kitabı ödünç almak istediğinize emin misiniz?", "Kitap Ödünç Alma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // E-posta adresini al
                    string eposta = label1.Text;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@eposta", eposta);

                            try
                            {
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();

                                // E-postaya sahip kayıt varsa
                                if (reader.Read())
                                {
                                    MessageBox.Show(eposta + " posta adresinize ilgili kayıt gönderilmiştir.");
                                    mailgonderim sm = new mailgonderim();
                                    string mesaj = "Merhaba : " + ogrenci_Menu.ad + " " + kitapAdi + " Kitabini aldınız  " + nextWeek + " tarihinden önce lütfen kitabi teslim ediniz ";
                                    sm.Microsoft("ÖDÜNÇ KİTAP", "outlook hesabi", "firat1903", eposta, mesaj);
                                    MessageBox.Show("Kitap başarıyla ödünç alındı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    KitapOduncAl(kitapid, 0);
                                    KitapOduncAl2();
                                    KitaplarıListele();
                                }
                                else
                                {
                                    MessageBox.Show("E-posta adresi bulunamadı.");
                                    KitapOduncAl(kitapid, 0);
                                    KitapOduncAl2();
                                    KitaplarıListele();
                                }

                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Hata: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Kitap ödünç alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen ödünç almak istediğiniz kitabı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
