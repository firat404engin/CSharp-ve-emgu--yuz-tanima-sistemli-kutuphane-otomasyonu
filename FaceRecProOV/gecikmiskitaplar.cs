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
    public partial class gecikmiskitaplar : Form
    {
        public gecikmiskitaplar()
        {
            InitializeComponent();
        }
        veritabani veritabani = new veritabani();
        void gecikmiskitaplarigetir()
        {
            try
            {   
               try
            {
                SqlConnection conn = veritabani.baglanti();
                string query = "SELECT * From odunc WHERE odunciade < GETDATE()";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);   
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Kullanıcıların aldığı kitapları listelerken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void gecikmiskitaplar_Load(object sender, EventArgs e)
        {
            gecikmiskitaplarigetir();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Kullanıcıdan onay al
                DialogResult result = MessageBox.Show("Kitap teslim uyarısı için kullanıcıya Mail Gönderilsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kullanıcı onay verdiyse devam et
                if (result == DialogResult.Yes)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Çift tıklanan satırdan oduncKitap adlı hücrenin değerini alın
                    string odunctc = selectedRow.Cells["odunctc"].Value.ToString();
                    string kitapadi = selectedRow.Cells["odunckitap"].Value.ToString();
                    MessageBox.Show(odunctc + " posta adresinize ilgili kayıt gönderilmiştir.");
                    mailgonderim sm = new mailgonderim();
                    string mesaj = "Merhaba : " + kitapadi +  " Kitabini teslim etmediniz lütfen kitabı teslim ediniz...  ";
                    sm.Microsoft("İADE SÜRESİ AŞIMI", "outlook hesabi", "firat1903", odunctc, mesaj);
                    MessageBox.Show("işlem başarıyla gerçekleşti.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("İşlemi iptal ettiiniz");
                }
            }
            else
            {
                MessageBox.Show("birşey seçmediniz !");
            }

        }
    }
}
