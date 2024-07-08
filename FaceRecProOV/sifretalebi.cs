using MultiFaceRec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class sifretalebi : Form
    {
        public sifretalebi()
        {
            InitializeComponent();
        }
       //  string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
       public  string password;
        public string eposta;

      
        private void button1_Click(object sender, EventArgs e)
        {

            veritabani veritabani = new veritabani();
            veritabani.kontrol(textBox1.Text.Trim());
             eposta = textBox1.Text;

            if (veritabani.kontrol(textBox1.Text.Trim()))

            {
                
                MessageBox.Show("E posta Adresinize Güvenlik kodu gönderilmiştir.");
                dogrula dogrula = new dogrula();
                dogrula.mailadres = eposta;
                dogrula.Show();
                /*    string query = "SELECT sifre FROM Bilgi WHERE eposta = @sifre";
                  using (SqlConnection connection = new SqlConnection(connectionString))
                   {
                       using (SqlCommand command = new SqlCommand(query, connection))
                       {
                           command.Parameters.AddWithValue("@sifre", textBox1.Text);

                           try
                           {
                               connection.Open();
                               SqlDataReader reader = command.ExecuteReader();

                               // E-postaya sahip kayıt varsa
                               if (reader.Read())
                               {
                                   MessageBox.Show(textBox1.Text + " adresine şifreniz gönderilmiştir");
                                   sifre = reader["sifre"].ToString();
                                   mailgonderim sm = new mailgonderim();    
                                   string mesaj = "Merhaba" + textBox1.Text + "şifreniz ektedir ";
                                   sm.Microsoft("ŞİFRE YENİLEME", "outlook hesabi", "firat1903", textBox1.Text, mesaj + sifre);
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
                   } */
            }
            else
            {
                MessageBox.Show("E-posta adresi kayıtlı değildir. ");
            }
        }


        private void sifretalebi_Load(object sender, EventArgs e)
        {
         

        }
    }
}