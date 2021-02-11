using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS___projekt
{
    public partial class Form1 : Form
    {
        private aes_crypto aes = new aes_crypto();
        private rsa_crypto rsa = new rsa_crypto();
        private sazetak_i_potpis sip = new sazetak_i_potpis();
        static string tekstPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\tekst.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aes.EncryptAesManaged();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aes.PrintDecrypt();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rsa.RsaManaged();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rsa.PrintDecrypted();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter((tekstPath).ToString(), false))
            {

                if (!File.Exists(tekstPath))
                {
                    File.Create(tekstPath).Dispose();
                    sw.WriteLine(richTextBox1.Text.ToString());
                    sw.Close();
                }
                else if (File.Exists(tekstPath))
                {
                    sw.WriteLine(richTextBox1.Text.ToString());
                    sw.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sip.ComputeSha256Hash(); 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sip.DigitalSignature();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (sip.CheckSignature() == true)
            {
                MessageBox.Show("Potpis je valjan!");
            }
            else MessageBox.Show("Potpis nije valjan!");
        }
    }
}
