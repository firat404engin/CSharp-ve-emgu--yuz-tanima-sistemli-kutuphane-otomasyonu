using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    internal class veritabani
    {
       
        private string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
           public SqlConnection baglanti()
            {
            SqlConnection baglan = new SqlConnection(connectionString); 
            baglan.Open();
            return baglan;
             }

        public void KisileriGetir()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM Bilgi";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MessageBox.Show("Ad: " + reader["kullanici_adi"] + ", eposta: " + reader["eposta"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }
        public bool kontrol(string eposta)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                string sqlQuery = "SELECT COUNT(*) FROM Bilgi WHERE eposta = @eposta";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@eposta", eposta);
                int count = (int)command.ExecuteScalar();

                return count > 0;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }
        public class RandomPasswordGenerator
        {
            private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            public static string GeneratePassword(int length)
            {
                Random random = new Random();
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public void uyeekle(string kullanici_adi, string eposta, string ad_soyad, string sifre,string tc)
        {
            SqlConnection connection = new SqlConnection(connectionString);
    

            string query = "INSERT INTO Bilgi (kullanici_adi, eposta, ad_soyad,sifre,tc) VALUES (@kullanici_adi, @eposta, @ad_soyad,@sifre,@tc)";

            // SQL komut nesnesi
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Parametrelerin eklenmesi
                command.Parameters.AddWithValue("@kullanici_adi", kullanici_adi);
                command.Parameters.AddWithValue("@eposta", eposta);
                command.Parameters.AddWithValue("@ad_soyad", ad_soyad);
                command.Parameters.AddWithValue("@sifre", sifre);
                command.Parameters.AddWithValue("@tc", tc);

                try
                {
                    // Bağlantıyı açma
                    connection.Open();

                    // Komutu çalıştırma
                    int rowsAffected = command.ExecuteNonQuery();

                    // Ekleme işleminin başarılı olup olmadığını kontrol etme
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kayıt başarıyla eklendi.");
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenirken bir hata oluştu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu: " + ex.Message);
                }
            }
            
        }
        public void listele(DataGridView dataGridView1)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                {
                    // SQL sorgusu
                    string query = "SELECT * FROM Bilgi";

                    // SQL komut nesnesi
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Veri adaptörü oluşturma
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // DataSet oluşturma
                        DataSet dataSet = new DataSet();

                        // DataSet'e verileri doldurma
                        adapter.Fill(dataSet);

                        // DataGridView'e verileri bağlama
                        dataGridView1.DataSource = dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanından veriler yüklenirken bir hata oluştu: " + ex.Message);
            }
        }
    }
    }
    

