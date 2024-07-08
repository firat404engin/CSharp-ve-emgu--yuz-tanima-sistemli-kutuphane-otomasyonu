using MultiFaceRec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultiFaceRec
{
    public partial class dogrula : Form
    {
        string password;
        public dogrula()
        {
            InitializeComponent();
        }
        public string mailadres;
        mailgonderim mailgonderim = new mailgonderim();
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
           
            
            if (textBox1.Text == password)
            {
                MessageBox.Show("GÜVENLİK KODU DOGRU");
                // Yeni formu açmak için yeni bir form örneği oluşturun ve gösterin
                ogrenciguncelle sifredegis = new ogrenciguncelle();
                sifredegis.eposta = mailadres;
                sifredegis.Show(); 
                this.Hide();
            }
            else
            {
                MessageBox.Show("GÜVENLİK KODU HATALI !");
            }

        }

        private void dogrula_Load(object sender, EventArgs e)
        {   randomsifre();
            mailgonderim sm = new mailgonderim();
            string mesaj = "Güvenlik kodu  : " + password.ToString();
            sm.Microsoft("Güvenlik Kodu", "outlook hesabi", "firat1903", mailadres, mesaj);
           
            
        }
    }
}
