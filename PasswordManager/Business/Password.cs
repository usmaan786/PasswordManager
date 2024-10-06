using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace PasswordManager
{
    internal class Password
    {
        private KeyVault _keyVault;

        public Password(KeyVault keyVault)
        {
            _keyVault = keyVault;
        }

        public class PasswordEntry
        {
            public string Username { get; set; }
            public string EncryptedPW { get; set; }

            public string DecryptedPassword { get; set; }
        }

        public void SavePassword(string username, string password)
        {

            var entry = new PasswordEntry
            {
                Username = username,
                EncryptedPW = Convert.ToBase64String(AES_Encrypt_Decrypt.EncryptStringToBytes_Aes(password, _keyVault.Key, _keyVault.IV))
            };

            List<PasswordEntry> entries = LoadPassword();
            entries.Add(entry);
            string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText("passwords.json", json);
        
        }

        public List<PasswordEntry> LoadPassword()
        {
            if(!File.Exists("passwords.json"))
            {
                return new List<PasswordEntry>();
            }

            string json = File.ReadAllText("passwords.json");
            var entries = JsonConvert.DeserializeObject<List<PasswordEntry>>(json);

            foreach (var entry in entries)
            {
                byte[] encryptedPW = Convert.FromBase64String(entry.EncryptedPW);
                string decryptedPW = AES_Encrypt_Decrypt.DecryptStringFromBytes_Aes(encryptedPW, _keyVault.Key, _keyVault.IV);
                entry.DecryptedPassword = decryptedPW;
            }

            return entries;
        }
    }
}

        
