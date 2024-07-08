using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class yoneticipanel : Form
    {
        public yoneticipanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            kitapekle kitapekle = new kitapekle();
            kitapekle.TopLevel=false;
            kitapekle.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(kitapekle);
            kitapekle.Dock= DockStyle.Fill;
            kitapekle.Show();

        }

        private void yoneticipanel_Load(object sender, EventArgs e)
        {
            veritabani veritabani = new veritabani();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            // UyeIslemleri formunu uyeislemleri
            uyeislemleri uyeIslemleriForm = new uyeislemleri();

            // UyeIslemleri formunun Parent'ını panel olarak belirle
            uyeIslemleriForm.TopLevel = false;
            uyeIslemleriForm.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(uyeIslemleriForm);
            uyeIslemleriForm.Dock = DockStyle.Fill;

            // Formu göster
            uyeIslemleriForm.Show();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            gecikmiskitaplar gecikmiskitaplar = new gecikmiskitaplar();
            panel1.Controls.Clear();
            gecikmiskitaplar.TopLevel = false;
            gecikmiskitaplar.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(gecikmiskitaplar);
            gecikmiskitaplar.Dock = DockStyle.Fill;
            gecikmiskitaplar.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            oduncalinanlar oduncalinanlar = new oduncalinanlar();
            panel1.Controls.Clear();
            oduncalinanlar.TopLevel = false;
            oduncalinanlar.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(oduncalinanlar);
            oduncalinanlar.Dock = DockStyle.Fill;
            oduncalinanlar.Show();
        }
    }
}
