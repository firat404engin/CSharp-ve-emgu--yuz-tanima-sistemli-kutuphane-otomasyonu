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

namespace MultiFaceRec
{
    public partial class destek : Form
    {
        public destek()
        {
            InitializeComponent();
        }
        public string mail;
        private void destek_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string konu = textBox1.Text;
            string mesaj = richTextBox1.Text;
            mailgonderim mailgonderim = new mailgonderim();
            mailgonderim.Microsoft(mail, "outlook hesabi", "firat1903", "outlook hesabi",mail + "  adli üyemizin talebi ;  "+ mesaj);
            MessageBox.Show("Destek Talebi İletildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
