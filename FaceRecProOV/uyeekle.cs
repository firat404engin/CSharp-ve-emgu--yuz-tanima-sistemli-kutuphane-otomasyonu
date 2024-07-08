using MultiFaceRec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MultiFaceRec
{
    public partial class uyeekle : Form
    {
        public uyeekle()
        {
            InitializeComponent();
            textBox5.KeyPress += TextBox5_KeyPress;
            textBox5.TextChanged += TextBox5_TextChanged;
            textBox5.Leave += TextBox5_Leave;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            veritabani veritabani = new veritabani();
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
        string.IsNullOrWhiteSpace(textBox2.Text) ||
        string.IsNullOrWhiteSpace(textBox3.Text) ||
        string.IsNullOrWhiteSpace(textBox4.Text) ||
        string.IsNullOrWhiteSpace(textBox5.Text) ||
        string.IsNullOrWhiteSpace(textBox6.Text))
            {

                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }
            else
            {
                try
                {
                    // E-posta adresini MailAddress nesnesi oluşturarak kontrol et
                    MailAddress mailAddress = new MailAddress(textBox2.Text);

                    // Eğer geçerli bir e-posta adresi ise ve adresin domain kısmı "outlook.com" ise
                    if (mailAddress.Host.ToLower() == "outlook.com" || mailAddress.Host.ToLower() == "hotmail.com")
                    {
                        string checkEmailQuery = "SELECT COUNT(*) FROM Bilgi WHERE eposta = @eposta";

                        // SQL komut nesnesi
                        using (SqlCommand checkEmailCommand = new SqlCommand(checkEmailQuery, veritabani.baglanti()))
                        {
                            // E-posta parametresini ekleyin
                            checkEmailCommand.Parameters.AddWithValue("@eposta", textBox2.Text);

                            try
                            {
                                // Bağlantıyı açma
                                veritabani.baglanti();

                                // E-posta adresinin veritabanında kaç kez bulunduğunu alın
                                int emailCount = (int)checkEmailCommand.ExecuteScalar();

                                // Eğer e-posta adresi zaten mevcutsa, kullanıcıya bir uyarı gösterin
                                if (emailCount > 0)
                                {
                                    MessageBox.Show("Bu e-posta adresi zaten kayıtlıdır.");
                                    return; // İşlemi sonlandırın
                                }
                                else
                                {

                                    string pass = sifrelecoz.sifrele(textBox4.Text, "kkkk123");
                                    veritabani.uyeekle(textBox1.Text, textBox2.Text, textBox3.Text, pass, textBox5.Text);
                                    // anaform anaform = new anaform();
                                    // anaform.Show();
                                    this.Hide();
                                    mailgonderim sm = new mailgonderim();
                                    string mesaj = "Merhaba " + textBox3.Text + "  Kütüphanemize Hoşgeldiniz";
                                    sm.Microsoft("Hoşgeldiniz", "outlook hesabi", "firat1903", textBox2.Text, mesaj);


                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu: " + ex.Message);
                                return; // İşlemi sonlandırın
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Girilen e-posta adresi Outlook adresi değildir.");
                    }

                }
                catch (FormatException)
                {
                    MessageBox.Show("Geçersiz bir e-posta adresi girdiniz.");
                }


            }

        }
        
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
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
                        string hedefDizin = @"C:\Users\fengi\Desktop\PROJE 2 SON VERSİYON\kullanicilar";

                        // TextBox'dan dosya adını al
                        string dosyaAdi = textBox5.Text.Trim(); // TextBox'ın adını ve kontrolünüzün adını kontrol edin
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
                        textBox6.Text = "RESİM YÜKLENDİ";
                        textBox6.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
                    }
                }
            }


        }
        private void TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlara izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            // Maksimum uzunluğu kontrol et
            if (textBox5.Text.Length > 11)
            {
                textBox5.Text = textBox5.Text.Substring(0, 11);
                textBox5.SelectionStart = textBox5.Text.Length; // İmleci sona taşı
            }
        }

        private void TextBox5_Leave(object sender, EventArgs e)
        {
            // Tam 11 karakter olup olmadığını kontrol et
            if (textBox5.Text.Length != 11)
            {
                MessageBox.Show("TC Kimlik Numarası 11 karakter uzunluğunda olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Focus(); // Tekrar TextBox'a odaklan
            }
        }


        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void uyeekle_Load(object sender, EventArgs e)
        {

        }
    }
}
