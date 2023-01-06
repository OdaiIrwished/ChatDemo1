using System.Security.Cryptography;
using System;

namespace ChatDemo1.Services
{
    public static class _3DEC
    {


  

        public static byte[] Encrypt(byte[] plaintext, byte[] key, byte[] IV)
        {
            // Create a new TripleDES object.
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                // Set the key and IV.
                tdes.Key = key;
                tdes.IV = IV;

                // Encrypt the plaintext and return the encrypted bytes.
                using (ICryptoTransform encryptor = tdes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
                }
            }
        }

        public static byte[] Decrypt(byte[] ciphertext, byte[] key, byte[] IV)
        {
            // Create a new TripleDES object.
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                // Set the key and IV.
                tdes.Key = key;
                tdes.IV = IV;

                // Decrypt the ciphertext and return the decrypted bytes.
                using (ICryptoTransform decryptor = tdes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                }
            }
        }
    }
}

