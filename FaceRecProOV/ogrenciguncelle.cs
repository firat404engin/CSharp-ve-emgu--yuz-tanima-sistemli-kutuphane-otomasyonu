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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultiFaceRec
{
    public partial class ogrenciguncelle : Form
    {
        public ogrenciguncelle()
        {
            InitializeComponent();
        }
       
        veritabani veritabani = new veritabani();
       public string eposta;

        private const string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            string pass = sifrelecoz.sifrele(textBox1.Text, "kkkk123");
            if (textBox1.Text == textBox2.Text)
            {

        // Veritabanını güncellemek için SQL sorgusu
        string query = "UPDATE Bilgi SET sifre = @sifre WHERE eposta = @eposta";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@sifre", pass);
                    command.Parameters.AddWithValue("@eposta", eposta);
                   

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Şifre başarıyla güncellendi.");
                           Application.Restart();
                        }
                        else
                        {
                            MessageBox.Show("Bir hata oluştu, şifre güncellenemedi.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı hatası: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("şifreler eşleşmiyor.");
            }
        }

        private void ogrenciguncelle_Load(object sender, EventArgs e)
        {
            

        }
    }
    }

