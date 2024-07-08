using MultiFaceRec.Models;
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
    public partial class oduncalinanlar : Form
    {
        public oduncalinanlar()
        {
            InitializeComponent();
        }
        private string connectionString = "Server=FIRATENGIN\\SQLEXPRESS;Database=kisiler;Integrated Security=True;";
        veritabani veritabani = new veritabani();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DialogResult result = MessageBox.Show("Seçilen Kitabı iade etmek istiyor musunuz ?", "İade Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                                    MessageBox.Show(" BASARİYLA ALINDI");
                                    ListOverdueRecords();
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

        private void ListOverdueRecords()
        {
            string query = "SELECT * FROM odunc";
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = veritabani.baglanti())
                {
                    connection.Close();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else
                {
                    MessageBox.Show("Süre aşımı olan kitap bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı sorgusu sırasında bir hata oluştu: " + ex.Message);
            }
        }

        private void oduncalinanlar_Load(object sender, EventArgs e)
        {
            ListOverdueRecords();
        }

    }
}
