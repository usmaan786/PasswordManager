using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    internal class KeyVault
    {
        private const string KeyFile = "key.bin";
        private const string IVFile = "iv.bin";
        private const string MasterKeyFile = "master_key.bin";

        public byte[] Key { get; private set; }
        public byte[] IV { get; private set; }
        private byte[] masterKey;

        public KeyVault()
        {
            LoadOrGenerateKey();
        }

        private void LoadOrGenerateKey()
        {
            if (File.Exists(MasterKeyFile))
            {
                masterKey = File.ReadAllBytes(MasterKeyFile);
            }
            else
            {
                masterKey = new byte[32];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(masterKey); 
                }
                
                File.WriteAllBytes(MasterKeyFile, masterKey);
            }

            if(File.Exists(KeyFile) && File.Exists(IVFile))
            {
                byte[] encryptedKey = File.ReadAllBytes(KeyFile);
                byte[] encryptedIV = File.ReadAllBytes(IVFile);

                Key = Decrypt(encryptedKey, masterKey);
                IV = Decrypt(encryptedIV, masterKey);
            }

            else
            {
                Key = new byte[32];
                IV = new byte[16];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(Key);
                    rng.GetBytes(IV);
                }

                File.WriteAllBytes(KeyFile, Encrypt(Key, masterKey));
                File.WriteAllBytes(IVFile, Encrypt(IV, masterKey));
            }
        }

        private byte[] Encrypt(byte[] data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        using(var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length); 
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        private byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[16];
                Array.Copy(encryptedData, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    using (var ms = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var resultStream = new MemoryStream())
                            {
                                cs.CopyTo(resultStream);
                                return resultStream.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}
