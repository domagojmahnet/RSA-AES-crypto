using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OS___projekt
{
    public class sazetak_i_potpis
    {
        static string tekstPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\tekst.txt";
        static string sazetakPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\sazetak.txt";
        static string potpisPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\digitalni_potpis.txt";
        static string privatniPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\privatni_kljuc.txt";


        byte[] signature;
        RSACryptoServiceProvider rsa= new RSACryptoServiceProvider(1024);


        public void ComputeSha256Hash()
        {
            string tekst = System.IO.File.ReadAllText(tekstPath);
           
            var sha = new SHA256Managed();
            byte[] sha256 = sha.ComputeHash(Encoding.UTF8.GetBytes(tekst));
            string builder = Convert.ToBase64String(sha256);

                
            using (StreamWriter sw = new StreamWriter((sazetakPath).ToString(), true))
            {
                if (!File.Exists(sazetakPath))
                {
                    File.Create(sazetakPath).Dispose();
                    sw.WriteLine(builder);
                    sw.Close();
                }
                else if (File.Exists(sazetakPath))
                {
                    sw.WriteLine(builder);
                    sw.Close();
                }
            }
        }

        public void DigitalSignature()
        {
            byte[] hash;
            var privatni = File.ReadAllText(privatniPath);

            using (StreamReader sr = new StreamReader(sazetakPath))
            {
                hash = Convert.FromBase64String(sr.ReadLine());
                sr.Close();
            }

            rsa.FromXmlString(privatni);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetKey(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");
            signature = rsaFormatter.CreateSignature(hash);

            using(StreamWriter sw = new StreamWriter((potpisPath).ToString(), true))
            {
                if (!File.Exists(potpisPath))
                {
                    File.Create(potpisPath).Dispose();
                    sw.WriteLine(Convert.ToBase64String(signature));
                    sw.Close();

                }
                else if (File.Exists(potpisPath))
                {
                    sw.WriteLine(Convert.ToBase64String(signature));
                    sw.Close();
                }
                
            }
        }

        public bool CheckSignature()
        {
            byte[] hash;
            byte[] signedHash;

            using (StreamReader sr = new StreamReader(sazetakPath))
            {
                hash = Convert.FromBase64String(sr.ReadLine());
                sr.Close();
            }

            using (StreamReader sr = new StreamReader(potpisPath))
            {
                signedHash = Convert.FromBase64String(sr.ReadLine());
                sr.Close();
            }

            RSAPKCS1SignatureDeformatter signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            signatureDeformatter.SetHashAlgorithm("SHA256");
            bool output = rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signedHash);
            return output;
        }
    }
}
