using MultiFaceRec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class yetkili_giris : Form
    {
        string password;
        public yetkili_giris()
        {
            InitializeComponent();
        }
        void randomsifre()
        {
            Random random = new Random();

            // 6 haneli bir şifre oluşturmak için dize oluşturun
             password = "";
            for (int i = 0; i < 6; i++)
            {
                // 0 ile 9 arasında rastgele bir rakam oluşturun ve dizeye ekleyin
                password += random.Next(10);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            yoneticipanel yoneticipanel = new yoneticipanel();
                if (textBox1.Text == password)
                {
                    MessageBox.Show("GÜVENLİK KODU DOGRU");
                    // Yeni formu açmak için yeni bir form örneği oluşturun ve gösterin
                    this.Hide();
                 FrmPrincipal yuz_tanima = new FrmPrincipal();
                     yuz_tanima.Show();
           this.Hide(); 
            }
            else
                {
                    MessageBox.Show("GÜVENLİK KODU HATALI !");
                }

            }
        

        private void yetkili_giris_Load(object sender, EventArgs e)
        {
            randomsifre();
            mailgonderim sm = new mailgonderim();
            string mesaj = "Güvenlik kodu  : " + password.ToString();
            sm.Microsoft("Güvenlik Kodu", "outlook hesabi", "firat1903", "fengin7373@outlook.com", mesaj);
            
        }
    }
}
