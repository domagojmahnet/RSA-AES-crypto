using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OS___projekt
{
    public class rsa_crypto
    {
        static string tekstPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\tekst.txt";
        static string javnikljucPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\javni_kljuc.txt";
        static string privatnikljucPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\privatni_kljuc.txt";

        static string dekriptiranoPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\rsa_dekriptirano.txt";
        static string kriptiranoPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\rsa_kriptirano.txt";

        byte[] kriptirano = null;

        string dekriptirano = null;


        public void RsaManaged()
        {
            string tekst = System.IO.File.ReadAllText(tekstPath);
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                byte[] dataToEncrypt = ByteConverter.GetBytes(tekst);
                byte[] encryptedData;
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
                    string privatni = RSA.ToXmlString(true);
                    string javni = RSA.ToXmlString(false);

                    using (StreamWriter sw = new StreamWriter((privatnikljucPath).ToString(), false))
                    {

                        if (!File.Exists(privatnikljucPath))
                        {
                            File.Create(privatnikljucPath).Dispose();
                            sw.WriteLine(privatni);
                            sw.Close();
                        }
                        else if (File.Exists(privatnikljucPath))
                        {
                            sw.WriteLine(privatni);
                            sw.Close();
                        }
                    }

                    using (StreamWriter sw = new StreamWriter((javnikljucPath).ToString(), false))
                    {
                        if (!File.Exists(javnikljucPath))
                        {
                            File.Create(javnikljucPath).Dispose();
                            sw.WriteLine(javni);
                            sw.Close();
                        }
                        else if (File.Exists(javnikljucPath))
                        {
                            sw.WriteLine(javni);
                            sw.Close();
                        }
                    }

                    using (StreamWriter sw = new StreamWriter((kriptiranoPath).ToString(), false))
                    {
                        if (!File.Exists(kriptiranoPath))
                        {
                            File.Create(kriptiranoPath).Dispose();
                            sw.WriteLine(Convert.ToBase64String(encryptedData));
                            sw.Close();
                        }
                        else if (File.Exists(kriptiranoPath))
                        {
                            sw.WriteLine(Convert.ToBase64String(encryptedData));
                            sw.Close();
                        }
                        
                    }
                   
                    using (StreamReader sr = new StreamReader(kriptiranoPath))
                    {
                        kriptirano = Convert.FromBase64String(sr.ReadLine());
                        sr.Close();
                    }

                    decryptedData = RSADecrypt(kriptirano, RSA.ExportParameters(true), false);
                    dekriptirano = System.Text.Encoding.Unicode.GetString(decryptedData);
                }
            }
            catch (ArgumentNullException)
            {
            }
        }
        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        public void PrintDecrypted()
        {
            using(StreamWriter sw = new StreamWriter((dekriptiranoPath).ToString(), false))
            {
                sw.WriteLine(dekriptirano);
            }
        }
    }
}
