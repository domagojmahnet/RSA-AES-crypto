using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace OS___projekt
{
    public class aes_crypto
    {
        static string tekstPath =AppDomain.CurrentDomain.BaseDirectory + "\\documents\\tekst.txt";
        static string kljucPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\tajni_kljuc.txt";
        static string dekriptiranoPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\aes_dekriptirano.txt";
        static string kriptiranoPath = AppDomain.CurrentDomain.BaseDirectory + "\\documents\\aes_kriptirano.txt";
        static string tekst;
        static byte[] kriptiranTekst=null;

        string dekriptirano=null;

        public void PrintDecrypt()
        {
            using (StreamWriter sw = new StreamWriter((dekriptiranoPath).ToString(), true))
            {
                if (!File.Exists(tekstPath))
                {
                    File.Create(tekstPath).Dispose();
                    sw.WriteLine(dekriptirano);
                    sw.Close();
                }
                else if (File.Exists(tekstPath))
                {
                    sw.WriteLine(dekriptirano);
                    sw.Close();
                }

            }
        }

        public void EncryptAesManaged()
        { 
           try
           {
                using (AesManaged aes = new AesManaged())
                {
                    using (StreamReader sr = new StreamReader(tekstPath))
                    {
                        tekst = sr.ReadLine();
                    }

                    byte[] encrypted = Encrypt(tekst, aes.Key, aes.IV);
                        using (StreamWriter sw = new StreamWriter((kljucPath).ToString(), true))
                        {
                            if (!File.Exists(kljucPath))
                            {
                                File.Create(kljucPath).Dispose();
                                sw.WriteLine(Convert.ToBase64String(aes.Key));
                                sw.Close();
                            }
                            else if (File.Exists(kljucPath))
                            {
                                sw.WriteLine(Convert.ToBase64String(aes.Key));
                                sw.Close();
                            }
                        }
                        using (StreamWriter sw = new StreamWriter((kriptiranoPath).ToString(), true))
                        {
                            if (!File.Exists(kriptiranoPath))
                            {
                                File.Create(kriptiranoPath).Dispose();
                                sw.WriteLine(Convert.ToBase64String(encrypted));
                                sw.Close();

                            }
                            else if (File.Exists(kriptiranoPath))
                            {
                                sw.WriteLine(Convert.ToBase64String(encrypted));
                                sw.Close();
                            }
                        }
                        using (StreamReader sr = new StreamReader(kriptiranoPath))
                        {
                            kriptiranTekst = Convert.FromBase64String(sr.ReadLine());
                            sr.Close();
                        }
                dekriptirano = Decrypt(kriptiranTekst, aes.Key, aes.IV);
                }
            }
            catch (Exception exp)
            {
            }
        }

        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;   
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV); 
                using (MemoryStream ms = new MemoryStream())
                {   
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    { 
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;   
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV); 
                using (MemoryStream ms = new MemoryStream(cipherText))
                { 
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    { 
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

    } 

}
