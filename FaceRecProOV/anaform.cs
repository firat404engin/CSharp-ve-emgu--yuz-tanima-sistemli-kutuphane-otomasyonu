using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MultiFaceRec
{
    public partial class anaform : Form
    {
        public anaform()
        {
            InitializeComponent();
            textBox1.TabIndex = 0;
            textBox2.TabIndex = 1;
            textBox3.TabIndex = 2;
            this.AcceptButton = button1;
        }
        string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";

        private void button2_Click(object sender, EventArgs e)
        {
            sifretalebi sifretalebi = new sifretalebi();
            sifretalebi.Show();

        }
        void captcha()
        {
            string[] harf = { "a", "b", "c", "d", "e", "f" };
            char[] sembol = { 'A', 'B', 'C', 'D', 'E', 'F' };
            Random rnd = new Random();
            int s1, s2, s3, s4,s5;
            s1 = rnd.Next(0, 5);
            s2 = rnd.Next(0, harf.Length);
            s3 = rnd.Next(0, 5);
            s4 = rnd.Next(0, sembol.Length);
            s5 = rnd.Next(0, harf.Length);
            label1.Text = s1.ToString() + harf[s2].ToString() + s3.ToString() + sembol[s4].ToString()+harf[s5].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            yetkili_giris yetkili_Giris = new yetkili_giris();
             yetkili_Giris.Show();
            // FrmPrincipal yuz_tanima = new FrmPrincipal();
            // yuz_tanima.Show();
             this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eposta = textBox1.Text;
            string sifre = sifrelecoz.sifrele(textBox2.Text, "kkkk123");
            if (KontrolEt(eposta, sifre))
            {
                ogrenci_menu ogrgiris = new ogrenci_menu();
                ogrgiris.ogrenci_tc = textBox1.Text;
                if (label1.Text == textBox3.Text)
                {
                    MessageBox.Show("Tebrikler,onaylandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    ogrgiris.Show();
                    this.Hide();
                }
                else
                {
                    textBox3.Text = "";
                    MessageBox.Show("Lütfen Güvenlik Kodunu Doğru Giriniz", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    captcha();
                }
            }
            else
            {
                MessageBox.Show("Hatalı e-posta veya şifre");
            }
        }
        
        private bool KontrolEt(string eposta, string sifre)
        {
            string query = "SELECT COUNT(*) FROM Bilgi WHERE eposta = @eposta AND sifre = @sifre";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@eposta", eposta);
                    command.Parameters.AddWithValue("@sifre", sifre);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            uyeekle uyeekle = new uyeekle();   
            uyeekle.Show();
          
        }

        private void anaform_Load(object sender, EventArgs e)
        {
            captcha();
            textBox1.Text = "E-POSTA ADRESİ GİRİNİZ";
            textBox1.Enter += (s, ev) => { if (textBox1.Text == "E-POSTA ADRESİ GİRİNİZ") textBox1.Text = ""; };
            textBox1.Leave += (s, ev) => { if (string.IsNullOrWhiteSpace(textBox1.Text)) textBox1.Text = "E-POSTA ADRESİ GİRİNİZ"; };

            // Parola kutusu için varsayılan metni ve olayları ayarlama
            textBox2.Text = "ŞİFRENİZİ GİRİNİZ";
            textBox2.Enter += (s, ev) => { if (textBox2.Text == "ŞİFRENİZİ GİRİNİZ") { textBox2.Text = ""; textBox2.PasswordChar = '*'; } };
            textBox2.Leave += (s, ev) => { if (string.IsNullOrWhiteSpace(textBox2.Text)) { textBox2.Text = "ŞİFRENİZİ GİRİNİZ"; textBox2.PasswordChar = '\0'; } };
            button6.Image = Image.FromFile(@"C:\Users\fengi\Downloads\göz1.2.png");


        }


        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("TÜM HAKLARI SAKLDIRI®");
            MessageBox.Show("FIRAT ENGİN :) ");
        }
        bool passwordVisible = false;

        private void button6_Click(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
               
                textBox2.PasswordChar = '\0'; // Parola karakterini gizle


                button6.Image = Image.FromFile(@"C:\Users\fengi\Downloads\gizligöz1.2.png");


            }
            else
            {
                textBox2.PasswordChar = '*'; // Parola karakterini görünür yap

                button6.Image = Image.FromFile(@"C:\Users\fengi\Downloads\göz1.2.png");
            }
            passwordVisible = !passwordVisible;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    } 
}
