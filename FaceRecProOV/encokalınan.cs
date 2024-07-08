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

namespace MultiFaceRec
{
    public partial class encokalınan : Form
    {
        public encokalınan()
        {
            InitializeComponent();
        }

        private void encokalınan_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
            string enCokAlinanKitap = "";
            string ozet = "";
            string dizinyolu = "C:\\Users\\fengi\\Desktop\\PROJE 2 SON VERSİYON\\Kitaplar\\";

            try
            {
                // Veritabanı bağlantısını oluştur
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SQL sorgusu
                    string query = "SELECT TOP 1 odunckitap FROM odunc GROUP BY odunckitap ORDER BY COUNT(*) DESC";

                    // SqlCommand oluştur
                    SqlCommand command = new SqlCommand(query, connection);

                    // Bağlantıyı aç
                    connection.Open();

                    // Sorguyu çalıştır ve sonucu al
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        enCokAlinanKitap = reader["odunckitap"].ToString();
                    }

                    // Bağlantıyı kapat
                    connection.Close();
                }

                // İkinci sorgu için yeni bir bağlantı oluştur
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    // SQL sorgusu
                    string query2 = "SELECT ozet FROM kitap WHERE kitapadi=@kitapadi";

                    // SqlCommand oluştur
                    SqlCommand command2 = new SqlCommand(query2, connection2);
                    command2.Parameters.AddWithValue("@kitapadi", enCokAlinanKitap); // kitapadi parametresine değer atama

                    // Bağlantıyı aç
                    connection2.Open();

                    // Sorguyu çalıştır ve sonucu al
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.Read())
                    {
                        ozet = reader2["ozet"].ToString();
                    }

                    // Bağlantıyı kapat
                    connection2.Close();
                }

                if (!string.IsNullOrEmpty(enCokAlinanKitap))
                {
                    richTextBox1.Text = ozet;
                    pictureBox1.Image = Image.FromFile(dizinyolu + enCokAlinanKitap + ".png");
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    label2.Text = enCokAlinanKitap;
                }
                else
                {
                    MessageBox.Show("En çok ödünç alınan kitap bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
