using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class ogrenci_bilgiguncelle : Form
    {
        public ogrenci_bilgiguncelle()
        {
            InitializeComponent();
        }

        private void clear()
        {
            kad.Clear();
            eposta2.Clear();
            adsoyad.Clear();
            sifre.Clear();
            tc2.Clear();
        }
        public string eposta;
        veritabani veritabani = new veritabani();
        private void ogrenci_bilgiguncelle_Load(object sender, EventArgs e)
        {
            eposta2.Text = eposta;
            //bilgileri çekme
            using (SqlConnection baglanti = veritabani.baglanti())
            {
                SqlCommand komut = new SqlCommand("Select kullanici_adi,eposta,ad_soyad,sifre,tc From Bilgi where eposta=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", eposta2.Text);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    kad.Text = dr["kullanici_adi"].ToString();
                    eposta2.Text = dr["eposta"].ToString();
                    adsoyad.Text = dr["ad_soyad"].ToString();
                    sifre.Text = sifrelecoz.coz( dr["sifre"].ToString(),"kkkk123");
                    tc2.Text = dr["tc"].ToString();
                }
            }
            // TC2 text kutusunda girilen ismi al
            string isim = tc2.Text.Trim(); // TC2'nin adını kontrolünüzün adıyla değiştirin

            // Fotoğrafın bulunduğu dizini ve dosya adını oluştur
            string dizin = @"C:\Users\fengi\Desktop\PROJE 2 SON VERSİYON\kullanicilar\" + isim; // Dizini uygun şekilde değiştirin
            string dosyaYolu = dizin + ".png"; // Dosya uzantısını uygun şekilde değiştirin

            // Eğer belirtilen dosya varsa picturebox'a yükle
            if (File.Exists(dosyaYolu))
            {
                pictureBox1.Image = Image.FromFile(dosyaYolu);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Resmi boyutlandır
            }
            else
            {
                MessageBox.Show("Belirtilen dosya bulunamadı.");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult secenek = MessageBox.Show("Merhaba" + " " + adsoyad.Text + " " + "bilgileriniz güncellensin mi ?", "Bilgi Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (secenek == DialogResult.Yes)
            {
                using (SqlConnection baglanti = veritabani.baglanti())
                {
                    SqlCommand komut1 = new SqlCommand("Update Bilgi set kullanici_adi=@p1, ad_soyad=@p3, sifre=@p4 where eposta=@p5", baglanti);
                    komut1.Parameters.AddWithValue("@p1", kad.Text);
                    komut1.Parameters.AddWithValue("@p3", adsoyad.Text);
                    komut1.Parameters.AddWithValue("@p4", sifrelecoz.sifrele( sifre.Text, "kkkk123"));
                    komut1.Parameters.AddWithValue("@p5", eposta2.Text); // E-postayı güncellemeniz gerekiyorsa, burada eposta2.Text'i kullanmalısınız.

                    komut1.ExecuteNonQuery();
                }

                MessageBox.Show("veri güncellendi");
                clear();
                this.Hide();
              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
        public void UyeGuncelleForm(string kullaniciadi, string eposta, string asoyad,string sifree , string tc22)
        {
            kad.Text = kullaniciadi;
            eposta2.Text = eposta;
            adsoyad.Text = asoyad;
            sifre.Text = sifree;
            tc2.Text = tc22;
        }
    }
}