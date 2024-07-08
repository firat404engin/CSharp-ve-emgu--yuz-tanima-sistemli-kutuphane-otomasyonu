using MultiFaceRec.Models;
using System;
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
    public partial class uyeislemleri : Form
    {
        public uyeislemleri()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        private void button1_Click(object sender, EventArgs e)
        {
            uyeekle uyeekle = new uyeekle();
            uyeekle.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
              string resimisim = dataGridView1.Rows[e.RowIndex].Cells["tc"].Value.ToString();

            // Aranan resmin dosya adını belirle
              string dosyaAdi = resimisim + ".png"; // veya dosya adının başka bir yapıda olduğunu varsayalım

            // Resmin bulunduğu dizini belirle
                string dizin = @"C:\Users\fengi\Desktop\PROJE 2 SON VERSİYON\kullanicilar"; // uygun dizini belirleyin

            // Resmin tam yolunu oluştur
             string resimYolu = Path.Combine(dizin, dosyaAdi);

            // Resmi PictureBox'ta göster
            if (File.Exists(resimYolu))
            {
                // Resim dosyası mevcut, PictureBox'ta göster
                pictureBox1.Image = Image.FromFile(resimYolu);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Resmi boyutlandır
            }
            else
            {
                MessageBox.Show("Resim bulunamadı.");
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;
                string kullaniciAdi = dataGridView1.Rows[rowIndex].Cells["kullanici_adi"].Value.ToString();

                DialogResult result = MessageBox.Show("Seçili satırı silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = veritabani.baglanti())
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM Bilgi WHERE kullanici_adi = @kullaniciAdi", con);
                        cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Seçili satır başarıyla silindi.");

                    // DataGridView'i yeniden yükleme
                    dataGridView1.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir satır seçin.");
            }
        }

        private void uyuislemler_Load(object sender, EventArgs e)
        {
            veritabani.listele(dataGridView1);
            dataGridView1.Columns["kullanici_adi"].HeaderText = "KULLANICI ADI";
            dataGridView1.Columns["eposta"].HeaderText = "E-POSTA ADRESİ";
            dataGridView1.Columns["ad_soyad"].HeaderText = "AD SOYAD";
            dataGridView1.Columns["sifre"].HeaderText = "ŞİFRE";
            dataGridView1.Columns["tc"].HeaderText = "TC KİMLİK";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            veritabani.listele(dataGridView1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kitaplar();
               
            }
    void kitaplar()
        {
            try
            {
                // Veritabanı bağlantısını oluştur
                using (veritabani.baglanti())
                {
                    // SQL sorgusu
                    string query = "SELECT oduncid,odunctc,odunckitap,odunciade FROM odunc WHERE odunciade >= GETDATE()";

                    // SqlCommand ve parametreleri oluştur
                    SqlCommand cmd = new SqlCommand(query, veritabani.baglanti());

                    // SqlDataAdapter ve DataTable oluştur
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    // Verileri doldur
                    da.Fill(dt);

                    // DataGridView'e verileri yükle
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.Columns["oduncid"].HeaderText = "Ödünç ID";
                    dataGridView1.Columns["odunctc"].HeaderText = "Ödünç Alan Kişi";
                    dataGridView1.Columns["odunckitap"].HeaderText = "Ödünç Alınan Kitap";
                    dataGridView1.Columns["odunciade"].HeaderText = "Kitap iade Tarihi";



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // En az bir satır seçildiğinden emin olun
            {
                int oduncid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["oduncid"].Value); // Seçilen satırdaki "oduncid" değerini alın

                DialogResult result = MessageBox.Show("Seçilen Kitabın İadesini Kabul Ediyor Musunuz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Veritabanı bağlantısını oluşturun
                        using (SqlConnection connection = veritabani.baglanti())
                        {
                            // DELETE sorgusunu oluşturun
                            string deleteQuery = "DELETE FROM odunc WHERE oduncid = @oduncid";

                            // DELETE sorgusunu ve parametreleri kullanarak SqlCommand nesnesini oluşturun
                            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@oduncid", oduncid);

                                // DELETE sorgusunu çalıştırın
                                int rowsAffected = deleteCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Seçilen ödünç kaydı başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Ödünç kaydı silinirken bir hata oluştu veya seçilen kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ödünç kaydı silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz bir ödünç kaydı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}