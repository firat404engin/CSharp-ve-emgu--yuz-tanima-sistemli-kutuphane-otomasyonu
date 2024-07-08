using MultiFaceRec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultiFaceRec
{
    public partial class ogrenci_menu : Form
    {
        public ogrenci_menu()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        public string ogrenci_tc;
        public string eposta;
        public string kitapAdi;
        public string ad;

        private void ogrenci_menu_Load(object sender, EventArgs e)
        {
            kitaplarim kitaplarim = new kitaplarim();
            kitaplarim.eposta = ogrenci_tc;
            destek destek = new destek();
            destek.mail = ogrenci_tc;
            
            SqlConnection conn = veritabani.baglanti();
            bool exists = CheckOduncTc(ogrenci_tc);
            SetButtonStates(exists);
       
       
            LblTc.Text = ogrenci_tc;
            SqlCommand komut = new SqlCommand("Select ad_soyad,tc From Bilgi where eposta=@d1", veritabani.baglanti());
            komut.Parameters.AddWithValue("@d1", ogrenci_tc);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdsoyad.Text = dr["ad_soyad"].ToString(); // Değişiklik burada: sütun adını belirtiyoruz.
                LblTc.Text = dr["tc"].ToString();
                ad = LblTc.Text;

            }
            string klasorYolu = "C:\\Users\\fengi\\Desktop\\PROJE 2 SON VERSİYON\\kullanicilar";
            string dosyaAdi = ad + ".png";
            string dosyaYolu = Path.Combine(klasorYolu, dosyaAdi);

            if (File.Exists(dosyaYolu))
            {
                // MessageBox.Show(dosyaAdi);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Resmi PictureBox boyutlarına otomatik olarak ayarlamak için

                // Resmi yuvarlak yapma işlemi
                Bitmap originalImage = new Bitmap(dosyaYolu);
                Bitmap roundedImage = new Bitmap(originalImage.Width, originalImage.Height);
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, originalImage.Width, originalImage.Height);
                using (Graphics graphics = Graphics.FromImage(roundedImage))
                {
                    graphics.Clear(Color.Transparent);
                    graphics.SetClip(gp);
                    graphics.DrawImage(originalImage, Point.Empty);
                }
                pictureBox1.Image = roundedImage;
            }
            else
            {
                MessageBox.Show("Belirtilen dosya bulunamadı.");
            }
            conn.Close();
        }
        void kitaplar()
        {
            SqlConnection conn = veritabani.baglanti();

            // SQL sorgusu
            string query = "SELECT * FROM kitap  WHERE durum = 1";

            // SqlCommand ve SqlDataAdapter oluştur
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            // DataTable oluştur ve verileri yükle
            DataTable dt = new DataTable();
            da.Fill(dt);

            // DataGridView'e verileri yükle
            dataGridView2.DataSource = dt;

            // Bağlantıyı kapat
            conn.Close();

        }
        void KullaniciKitaplariniListele()
        {
            try
            {
                SqlConnection connection = veritabani.baglanti();
                string query = "SELECT * FROM odunc WHERE odunctc = @ogrenci_tc";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ogrenci_tc", ogrenci_tc);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcının aldığı kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
            veritabani veritabani = new veritabani();
            string query = "SELECT odunctc FROM odunc WHERE odunctc = @ogrenci_tc";



            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                int kitapid = Convert.ToInt32(row.Cells["kitapid"].Value);
                kitapAdi = row.Cells["kitapadi"].Value.ToString();
                DateTime now = DateTime.Now;
                DateTime nextWeek = now.AddDays(7);

                DialogResult result = MessageBox.Show($"\"{kitapAdi}\" adlı kitabı ödünç almak istediğinize emin misiniz?", "Kitap Ödünç Alma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ogrenci_tc", ogrenci_tc);

                            try
                            {
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();

                                // E-postaya sahip kayıt varsa
                                if (reader.Read())
                                {
                                    MessageBox.Show(ogrenci_tc + " posta adresinize ilgili kayıt gönderilmiştir.");
                                    mailgonderim sm = new mailgonderim();
                                    string mesaj = "Merhaba : " + LblAdsoyad.Text + " " + kitapAdi + " Kitabini aldınız  " + nextWeek + " tarihinden önce lütfen kitabi teslim ediniz ";
                                    sm.Microsoft("ÖDÜNÇ KİTAP", "outlook hesabi", "firat1903", ogrenci_tc, mesaj);
                                    MessageBox.Show("Kitap başarıyla ödünç alındı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    KitapOduncAl(kitapid, 0);
                                    KitapOduncAl2();
                                }
                                else
                                {
                                    MessageBox.Show("E-posta adresi bulunamadı.");
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
        }



        private void button1_Click(object sender, EventArgs e)
        {
            encokalınan encokalinan = new encokalınan();
        
            panel2.Controls.Clear();
            encokalinan.TopLevel = false; // Formu üst düzeyden çıkar
            encokalinan.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(encokalinan); // Formu panel içine ekleyin
            encokalinan.Dock = DockStyle.Fill; // Form panelin tamamını kaplasın.
            DataGridView dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            encokalinan.Show();


        }
    


    //// TextBox'tan arama terimini al
    //string aramaTerimi = kitapara.Text;

    //// Veritabanı bağlantısını oluştur
    //SqlConnection conn = veritabani.baglanti();

    //// SQL sorgusu
    //string query = "SELECT * FROM kitap WHERE kitapadi LIKE @aramaTerimi";

    //// SqlCommand ve SqlDataAdapter oluştur
    //SqlCommand cmd = new SqlCommand(query, conn);
    //cmd.Parameters.AddWithValue("@aramaTerimi", "%" + aramaTerimi + "%"); // Parçalı eşleşme için % işareti ekleyin
    //SqlDataAdapter da = new SqlDataAdapter(cmd);

    //// DataTable oluştur ve verileri yükle
    //DataTable dt = new DataTable();
    //da.Fill(dt);

    //// DataGridView'e verileri yükle
    //dataGridView2.DataSource = dt;

    //// Bağlantıyı kapat
    //conn.Close(); 


        private void oduncal_Click(object sender, EventArgs e)
        {
            odunc odunc = new odunc();
            odunc.eposta = ogrenci_tc; ;
            // Panel içeriğini temizle

            panel2.Controls.Clear();
            odunc.TopLevel = false; // Formu üst düzeyden çıkar
            odunc.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(odunc); // Formu panel içine ekleyin
            odunc.Dock = DockStyle.Fill; // Form panelin tamamını kaplasın.
            DataGridView dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            odunc.Show();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MessageBox.Show("tikladin");
                try
                {
                    // Seçilen satırı al
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Odunc tablosundaki odunckitap alanı
                    string oduncKitapAdi = row.Cells["odunckitap"].Value.ToString();

                    // Veritabanı bağlantısını oluştur
                    using (SqlConnection connection = veritabani.baglanti())
                    {
                        // SQL sorgusu
                        string query = "UPDATE odunc SET durumu = 1 WHERE odunckitap = @oduncKitapAdi AND EXISTS (SELECT 1 FROM kitap WHERE kitapadi = @oduncKitapAdi)";

                        // SqlCommand ve parametreleri oluştur
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@oduncKitapAdi", oduncKitapAdi);

                        // Sorguyu çalıştır
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kitap durumu başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Kitap durumu güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap durumu güncellenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private bool KitapOduncAl(int kitapid, int durum)
        {
            string kitapoduncdurum = LblTc.Text;
            try
            {
                SqlConnection connection = veritabani.baglanti();
                string query = "UPDATE kitap SET kitapoduncdurum = @kitapoduncdurum , durum=@durum WHERE kitapid = @kitapid";
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
        private bool CheckOduncTc(string oduncmail2)
        {
            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM odunc WHERE odunctc = @odunctc AND odunciade < @zaman";

            try
            {
                using (veritabani.baglanti())
                {
                    using (SqlCommand cmd = new SqlCommand(query, veritabani.baglanti()))
                    {
                        cmd.Parameters.AddWithValue("@odunctc", oduncmail2);
                        cmd.Parameters.AddWithValue("@zaman", DateTime.Now);

                        veritabani.baglanti();
                        int count = (int)cmd.ExecuteScalar();
                        

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı sorgusu sırasında bir hata oluştu: " + ex.Message);
                return false;
            }
        }



        private void KitapOduncAl2()
        {
            // Ödünç alınan kitabın bilgilerini eklemek için SQL sorgusu
            string query = "INSERT INTO odunc (odunctc, odunckitap, odunctarih, odunciade) VALUES (@eposta, @kitapAdi, @odunctarih, DATEADD(day, 7, @odunctarih))";

            try
            {
                // Veritabanı bağlantısını oluştur
                SqlConnection connection = veritabani.baglanti();

                // Ödünç alınan kitabın tarih bilgilerini ayarla
                DateTime oduncTarihi = DateTime.Now;

                // SqlCommand ve parametreleri oluştur
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@eposta", ogrenci_tc); // Kullanıcının TC'si
                cmd.Parameters.AddWithValue("@kitapAdi", kitapAdi); // Ödünç alınan kitabın ID'si
                cmd.Parameters.AddWithValue("@odunctarih", oduncTarihi); // Ödünç alınan tarih

                // Sorguyu çalıştır
                int rowsAffected = cmd.ExecuteNonQuery();

                // Bağlantıyı kapat
                connection.Close();

                // Etkilenen satır sayısına göre işlemin başarılı olup olmadığını kontrol et
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Veri tabanına başarılı şekilde eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kitap ödünç alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap ödünç alınırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            destek destek = new destek();
            destek.mail = ogrenci_tc;
            panel2.Controls.Clear();
            destek.TopLevel = false;
            destek.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(destek);
            destek.Dock = DockStyle.Fill;
            destek.Show();
        }

        private void LnkGuncelle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // Panel içeriğini temizle
            panel2.Controls.Clear();

            // Yeni formu oluştur ve panel içine ekle
            ogrenci_bilgiguncelle ogrencigunlcel = new ogrenci_bilgiguncelle();
            ogrencigunlcel.TopLevel = false; // Formu üst düzeyden çıkar
            ogrencigunlcel.FormBorderStyle = FormBorderStyle.None;
            ogrencigunlcel.eposta = ogrenci_tc;
            panel2.Controls.Add(ogrencigunlcel); // Formu panel içine ekleyin
            ogrencigunlcel.Dock = DockStyle.Fill; // Form panelin tamamını kaplasın.
            ogrencigunlcel.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Çıkış yapmaktasınız  emin misiniz?", "Güvenli Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                anaform anaform = new anaform();
                anaform.Show();
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            kitaplarim kitaplar = new kitaplarim();
            kitaplar.eposta = ogrenci_tc;
            panel2.Controls.Clear();
            kitaplar.TopLevel = false;
            kitaplar.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(kitaplar);
            kitaplar.Dock = DockStyle.Fill;
            kitaplar.Show();
            /*
            // Panel içeriğini temizle
            panel2.Controls.Clear();

            // DataGridView oluştur
            DataGridView dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill; // DataGridView panelin tamamını kaplasın

            try
            {
                try
                {
                    // Veritabanı bağlantısını oluştur
                    SqlConnection connection = veritabani.baglanti();

                    // SQL sorgusu
                    string query = "SELECT odunckitap,odunctarih,odunciade FROM odunc WHERE odunctc = @ogrenci_tc AND odunciade >= GETDATE()";

                    // SqlCommand ve parametreleri oluştur
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ogrenci_tc", ogrenci_tc);

                    // SqlDataAdapter ve DataTable oluştur
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    // Verileri doldur
                    da.Fill(dt);

                    // DataGridView'e verileri yükle
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Bağlantıyı kapat
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcının aldığı kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // DataGridView'i panele ekle
            panel2.Controls.Add(dataGridView1); */



        }
        private void SetButtonStates(bool isInactive)
        {
            // Süre aşımı durumu
            if (isInactive)
            {
                label1.Text = "SÜRE AŞIMI KİTAP ALAMAZSINIZ!";
                oduncal.Enabled = false;
            }
            // Süre aşımı yoksa
            else
            {
                label1.Text = "";  // Uyarı mesajını temizle
                oduncal.Enabled = true;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void kitapara_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

