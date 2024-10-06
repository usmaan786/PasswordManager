using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    internal class Generator
    {
        public static string GeneratePassword(int length)
        {
            //string of characters that can be used to generate password
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";

            //using cryptography to generate strong random values
            //"using" ensures the object is disposed after it is used
            using (var rng = new RNGCryptoServiceProvider())
            {
                //creating byte array of desired password length
                //filling array with cryptographically random values in each byte
                var bytes = new byte[length];
                rng.GetBytes(bytes);

                //returning type string - each element in byte array mapped to an index within the bounds of validChars
                return new string(bytes.Select(b => validChars[b % validChars.Length]).ToArray());
            }
        }
    }
}
