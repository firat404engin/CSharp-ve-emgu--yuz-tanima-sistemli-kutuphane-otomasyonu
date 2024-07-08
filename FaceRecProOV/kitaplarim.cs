using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class kitaplarim : Form
    {
        public kitaplarim()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        public string eposta;
       public void kitaplarim1()
        {
            try
            {
                try
                {
                    // Veritabanı bağlantısını oluştur
                    SqlConnection connection = veritabani.baglanti();

                    // SQL sorgusu
                    string query = "SELECT odunckitap,odunctarih,odunciade FROM odunc WHERE odunctc = @odunctc";

                    // SqlCommand ve parametreleri oluştur
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@odunctc", eposta);

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
        }
        private void kitaplarım_Load(object sender, EventArgs e)
        {
            kitaplarim1();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                
                DialogResult result = MessageBox.Show("Seçilen Kitabı iade etmek istiyor musunuz ?","İade Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                 {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Çift tıklanan satırdan oduncKitap adlı hücrenin değerini alın
                    string odunckitap = selectedRow.Cells["odunckitap"].Value.ToString();

                    // Veritabanındaki kitapadet değerini arttırmak için güncelleme sorgusu oluşturun
                    string updateQuery = "UPDATE kitap SET kitapadet = kitapadet + 1 WHERE kitapadi = @odunckitap";
                    string deleteQuery = "DELETE FROM odunc WHERE odunckitap = @odunckitap";

                    using (veritabani.baglanti())
                    {
                        // UPDATE sorgusu için SqlCommand nesnesi oluşturun
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, veritabani.baglanti()))
                        {
                            updateCommand.Parameters.AddWithValue("@odunckitap", odunckitap);

                            try
                            {
                                veritabani.baglanti();
                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Kitap adeti başarıyla arttırıldı
                                    MessageBox.Show("Ödünç alınan kitap başarıyla iade edildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // DataGridView'i güncelleyin
                                    kitaplarim1();
                                }
                                else
                                {
                                    MessageBox.Show("Kitap adeti güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        // DELETE sorgusu için SqlCommand nesnesi oluşturun
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, veritabani.baglanti()))
                        {
                            deleteCommand.Parameters.AddWithValue("@odunckitap", odunckitap);

                            try
                            {
                                int rowsAffected = deleteCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Odunc tablosundan kayıt başarıyla silindi
                                    // MessageBox.Show("Ödünç alınan kitap başarıyla iade edildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // DataGridView'i güncelleyin
                                    kitaplarim1();
                                }
                                else
                                {
                                    MessageBox.Show("Kitap silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
               
                }
                else
                {
                    MessageBox.Show("Hiç bir işlem gerçekleşmedi");
                }
            }
        }

        }
    }
