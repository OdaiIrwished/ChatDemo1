using System.Text;
using System;
using System.IO;
using System.Security.Cryptography;

namespace ChatDemo1.Services
{


    public static class DECAlgorithm
    {

        public static string Encrypt(string message, string password)
        {
            // Convert the message and password to bytes
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Generate a random salt to use for the encryption
            byte[] salt = GenerateRandomBytes(8);

            // Use the password and salt to generate the key and initialization vector (IV)
            DeriveKeyAndIV(passwordBytes, salt, out byte[] key, out byte[] iv);

            // Create a new DES encryptor to perform the encryption
            using (var encryptor = DES.Create())
            {
                encryptor.Key = key;
                encryptor.IV = iv;

                // Create a new memory stream to store the encrypted data
                using (var ms = new System.IO.MemoryStream())
                {
                    // Write the salt to the beginning of the stream
                    ms.Write(salt, 0, salt.Length);

                    // Create a new crypto stream to perform the encryption
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write the message to the crypto stream
                        cs.Write(messageBytes, 0, messageBytes.Length);
                    }

                    // Return the encrypted message as a base64 string
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedMessage, string password)
        {
            // Convert the encrypted message and password to bytes
            byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Read the salt from the beginning of the encrypted message
            byte[] salt = new byte[8];
            Array.Copy(encryptedMessageBytes, 0, salt, 0, salt.Length);

            // Use the password and salt to generate the key and initialization vector (IV)
            DeriveKeyAndIV(passwordBytes, salt, out byte[] key, out byte[] iv);

            // Create a new DES decryptor to perform the decryption
            using (var decryptor = DES.Create())
            {
                decryptor.Key = key;
                decryptor.IV = iv;

                // Create a new memory stream to store the decrypted data
                using (var ms = new System.IO.MemoryStream())


                {
                    // Create a new crypto stream to perform the decryption
                    using (var cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // Write the encrypted message to the crypto stream, skipping the salt at the beginning
                        cs.Write(encryptedMessageBytes, salt.Length, encryptedMessageBytes.Length - salt.Length);
                    }

                    // Return the decrypted message as a string
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        static byte[] GenerateRandomBytes(int length)
        {
            // Generate a random number generator
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Allocate a buffer to store the random bytes
                byte[] bytes = new byte[length];

                // Fill the buffer with random bytes
                rng.GetBytes(bytes);

                return bytes;
            }
        }

        static void DeriveKeyAndIV(byte[] password, byte[] salt, out byte[] key, out byte[] iv)
        {
            // Create a new password-based key derivation function (PBKDF2) to derive the key and IV
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                // Derive the key and IV
                key = pbkdf2.GetBytes(8);
                iv = pbkdf2.GetBytes(8);
            }
        }
    }
}
