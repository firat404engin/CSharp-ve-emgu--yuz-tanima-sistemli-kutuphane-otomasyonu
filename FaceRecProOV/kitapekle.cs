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
    public partial class kitapekle : Form
    {
        public kitapekle()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        public void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.DataSource = null;
            textBox5.Clear();
            richTextBox1.Clear();
                }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısını oluştur
                using (veritabani.baglanti())
                {
                    // SQL sorgusu
                    string query = "INSERT INTO kitap (kitapid, kitapadi, kitapyazar, kitaptur, durum,kitapadet,ozet) VALUES (@kitapid, @kitapadi, @kitapyazar, @kitaptur, 1,@kitapadet,@ozet)";

                    // SqlCommand ve parametreleri oluştur
                    SqlCommand cmd = new SqlCommand(query, veritabani.baglanti());
                    cmd.Parameters.AddWithValue("@kitapid", textBox1.Text);
                    cmd.Parameters.AddWithValue("@kitapadi", textBox2.Text);
                    cmd.Parameters.AddWithValue("@kitapyazar", textBox3.Text);
                    cmd.Parameters.AddWithValue("@kitaptur", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@kitapadet", textBox4.Text);
                    cmd.Parameters.AddWithValue("@ozet", richTextBox1.Text);

                    // Bağlantıyı açma
                    veritabani.baglanti();

                    // Sorguyu çalıştır
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kitap başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        kitaplar();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Kitap eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } // Bağlantı burada otomatik olarak kapanacaktır
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap eklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void kitaplar()
        {
            try
            {
                // Veritabanı bağlantısını oluştur
                using (SqlConnection connection = veritabani.baglanti())
                {
                    // SQL sorgusu
                    string query = "SELECT kitapid,kitapadi,kitapyazar,kitaptur,kitapadet FROM kitap";

                    // SqlCommand ve parametreleri oluştur
                    SqlCommand cmd = new SqlCommand(query, connection);

                    // SqlDataAdapter ve DataTable oluştur
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    // Verileri doldur
                    da.Fill(dt);

                    // DataGridView'e verileri yükle
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void kitapekle_Load(object sender, EventArgs e)
        {
            kitaplar();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // DataGridView'den değerleri alıp ilgili kontrol alanlarına atama
                textBox1.Text = row.Cells["kitapid"].Value.ToString();
                textBox2.Text = row.Cells["kitapadi"].Value.ToString();
                textBox3.Text = row.Cells["kitapyazar"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["kitaptur"].Value.ToString();
                textBox4.Text = row.Cells["kitapadet"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kullanıcıya silme işlemi için onay iletişim kutusu göster
            DialogResult result = MessageBox.Show("Kitabı silmek istediğinizden emin misiniz?", "Kitap Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kullanıcının seçimine göre işlem yap
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Veritabanı bağlantısını oluştur
                    using (veritabani.baglanti())
                    {
                        // SQL sorgusu
                        string query = "DELETE FROM kitap WHERE kitapid = @kitapid";

                        // SqlCommand ve parametreleri oluştur
                        SqlCommand cmd = new SqlCommand(query, veritabani.baglanti());
                        cmd.Parameters.AddWithValue("@kitapid", textBox1.Text);

                        // Bağlantıyı aç
                        veritabani.baglanti();

                        // Sorguyu çalıştır
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kitap başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            kitaplar();
                            clear();
                        }
                        else
                        {
                            MessageBox.Show("Silinecek kitap bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
                try
                {
                    // Veritabanı bağlantısını oluştur
                    using (veritabani.baglanti())
                    {
                        // SQL sorgusu
                        string query = "UPDATE kitap SET kitapadi = @kitapadi, kitapyazar = @kitapyazar, kitaptur = @kitaptur, kitapadet=@kitapadet WHERE kitapid = @kitapid";

                        // SqlCommand ve parametreleri oluştur
                        SqlCommand cmd = new SqlCommand(query, veritabani.baglanti());
                        cmd.Parameters.AddWithValue("@kitapid", textBox1.Text);
                        cmd.Parameters.AddWithValue("@kitapadi", textBox2.Text);
                        cmd.Parameters.AddWithValue("@kitapyazar", textBox3.Text);
                        cmd.Parameters.AddWithValue("@kitaptur", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@kitapadet",textBox4.Text);

                    // Bağlantıyı aç
                    veritabani.baglanti();

                        // Sorguyu çalıştır
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kitap başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        kitaplar();
                        clear();
                        }
                        else
                        {
                            MessageBox.Show("Güncellenecek kitap bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap güncellenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Sadece PNG dosyalarını göster
                openFileDialog.Filter = "PNG Dosyaları|*.png";
                openFileDialog.Title = "PNG Resim Seç";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Seçilen dosyanın tam yolunu al
                        string dosyaYolu = openFileDialog.FileName;

                        // Dizine resmi kopyala
                        string hedefDizin = @"C:\Users\fengi\Desktop\PROJE 2 SON VERSİYON\Kitaplar";

                        // TextBox'dan dosya adını al
                        string dosyaAdi = textBox2.Text.Trim(); // TextBox'ın adını ve kontrolünüzün adını kontrol edin
                        if (string.IsNullOrEmpty(dosyaAdi))
                        {
                            MessageBox.Show("Lütfen bir dosya adı girin.");
                            return;
                        }

                        // Dosya adında geçersiz karakterleri kontrol etmek için kullanabilirsiniz
                        dosyaAdi = Path.GetInvalidFileNameChars().Aggregate(dosyaAdi, (current, c) => current.Replace(c.ToString(), string.Empty));

                        string hedefYol = Path.Combine(hedefDizin, dosyaAdi + ".png"); // Dosya adına .png ekle
                        File.Copy(dosyaYolu, hedefYol, true);

                        MessageBox.Show("Resim başarıyla yüklendi.");
                        textBox5.Text = "RESİM YÜKLENDİ";
                        textBox5.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
    }

    

