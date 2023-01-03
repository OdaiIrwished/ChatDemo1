using System.Text;
using System;
using System.IO;

namespace ChatDemo1.Services
{


   


        public class HillCipher
    {
        public static string Encrypt(string plainText, string key)
        {
            // Get key matrix from the key string
            int[,] keyMatrix = GetKeyMatrix(key);

            // Make sure the key matrix can be inverted
            if (!CheckKeyMatrix(keyMatrix))
            {
                throw new Exception("The key matrix is not invertible.");
            }

            // Make sure the plain text length is a multiple of the key matrix size
            if (plainText.Length % keyMatrix.GetLength(0) != 0)
            {
                plainText = plainText.PadRight(plainText.Length + keyMatrix.GetLength(0) - (plainText.Length % keyMatrix.GetLength(0)));
            }

            // Split the plain text into blocks the size of the key matrix
            string[] plainTextBlocks = Split(plainText, keyMatrix.GetLength(0));

            // Encrypt each block
            string cipherText = "";
            foreach (string block in plainTextBlocks)
            {
                cipherText += EncryptBlock(block, keyMatrix);
            }

            return cipherText;
        }

        public static string Decrypt(string cipherText, string key)
        {
            // Get key matrix from the key string
            int[,] keyMatrix = GetKeyMatrix(key);

            // Make sure the key matrix can be inverted
            if (!CheckKeyMatrix(keyMatrix))
            {
                throw new Exception("The key matrix is not invertible.");
            }

            // Make sure the cipher text length is a multiple of the key matrix size
            if (cipherText.Length % keyMatrix.GetLength(0) != 0)
            {
                cipherText = cipherText.PadRight(cipherText.Length + keyMatrix.GetLength(0) - (cipherText.Length % keyMatrix.GetLength(0)));
            }

            // Split the cipher text into blocks the size of the key matrix
            string[] cipherTextBlocks = Split(cipherText, keyMatrix.GetLength(0));

            // Decrypt each block
            string plainText = "";
            foreach (string block in cipherTextBlocks)
            {
                plainText += DecryptBlock(block, keyMatrix);
            }

            return plainText;
        }
        //EncryptBlock
        private static string EncryptBlock(string plainTextBlock, int[,] keyMatrix)
        {
            // Convert the plain text block to a vector
            int[] plainTextVector = GetVector(plainTextBlock);

            // Multiply the key matrix by the plain text vector
            int[] cipherTextVector = Multiply(keyMatrix, plainTextVector);

            // Convert the cipher text vector to a string
            string cipherTextBlock = GetString(cipherTextVector);

            return cipherTextBlock;
        }
        //GetString
        private static string GetString(int[] vector)
        {
            string s = "";

            foreach (int i in vector)
            {
                s += (char)(i + 65);
            }

            return s;
        }
        //DecryptBlock
        private static string DecryptBlock(string cipherTextBlock, int[,] keyMatrix)
        {
            // Convert the cipher text block to a vector
            int[] cipherTextVector = GetVector(cipherTextBlock);

            // Get the inverse of the key matrix
            int[,] inverseKeyMatrix = GetInverse(keyMatrix);

            // Multiply the inverse key matrix by the cipher text vector
            int[] plainTextVector = Multiply(inverseKeyMatrix, cipherTextVector);

            // Convert the plain text vector to a string
            string plainTextBlock = GetString(plainTextVector);

            return plainTextBlock;
        }
        //Multiply
        private static int[] Multiply(int[,] matrix, int[] vector)
        {
            int[] result = new int[vector.Length];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < vector.Length; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }

                result[i] %= 26;
            }

            return result;
        }
        //GetInverse
        private static int[,] GetInverse(int[,] keyMatrix)
        {
            // Get the determinant of the key matrix
            int determinant = GetDeterminant(keyMatrix);

            // Get the adjugate of the key matrix
            int[,] adjugate = GetAdjugate(keyMatrix);

            // Get the multiplicative inverse of the determinant
            int inverseDeterminant = GetMultiplicativeInverse(determinant);

            // Multiply the adjugate by the multiplicative inverse of the determinant
            int[,] inverseKeyMatrix = Multiply(adjugate, inverseDeterminant);

            return inverseKeyMatrix;
        }

        //Multiply
        private static int[,] Multiply(int[,] matrix, int scalar)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, j] * scalar;
                }
            }

            return result;
        }

        //GetMultiplicativeInverse
        private static int GetMultiplicativeInverse(int number)
        {
            // Make sure the number is positive
            number = number % 26;
            if (number < 0)
            {
                number += 26;
            }

            // Find the multiplicative inverse of the number
            for (int i = 1; i < 26; i++)
            {
                if ((number * i) % 26 == 1)
                {
                    return i;
                }
            }

            throw new Exception("The number has no multiplicative inverse.");
        }
        //GetAdjugate
        private static int[,] GetAdjugate(int[,] keyMatrix)
        {
            // Create a new matrix to store the adjugate
            int[,] adjugate = new int[keyMatrix.GetLength(0), keyMatrix.GetLength(1)];

            // Get the cofactors of the key matrix
            int[,] cofactors = GetCofactors(keyMatrix);

            // Transpose the cofactors
            for (int i = 0; i < cofactors.GetLength(0); i++)
            {
                for (int j = 0; j < cofactors.GetLength(1); j++)
                {
                    adjugate[j, i] = cofactors[i, j];
                }
            }

            return adjugate;
        }
        //GetCofactors
        private static int[,] GetCofactors(int[,] keyMatrix)
        {
            // Create a new matrix to store the cofactors
            int[,] cofactors = new int[keyMatrix.GetLength(0), keyMatrix.GetLength(1)];

            // Get the cofactors of the key matrix
            for (int i = 0; i < keyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < keyMatrix.GetLength(1); j++)
                {
                    cofactors[i, j] = GetCofactor(keyMatrix, i, j);
                }
            }

            return cofactors;
        }
        //GetCofactor
        private static int GetCofactor(int[,] keyMatrix, int row, int column)
        {
            // Get the minor of the key matrix
            int minor = GetMinor(keyMatrix, row, column);

            // Get the cofactor of the key matrix
            int cofactor = (int)Math.Pow(-1, row + column) * minor;

            return cofactor;
        }
        //GetMinor
        private static int GetMinor(int[,] keyMatrix, int row, int column)
        {
            // Create a new matrix to store the minor
            int[,] minor = new int[keyMatrix.GetLength(0) - 1, keyMatrix.GetLength(1) - 1];

            // Get the minor of the key matrix
            for (int i = 0; i < keyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < keyMatrix.GetLength(1); j++)
                {
                    if (i != row && j != column)
                    {
                        minor[i < row ? i : i - 1, j < column ? j : j - 1] = keyMatrix[i, j];
                    }
                }
            }

            // Get the determinant of the minor
            int determinant = GetDeterminant(minor);

            return determinant;
        }
        //MultiplyMatrices
        private static int[,] MultiplyMatrices(int[,] matrix1, int[,] matrix2)
        {
            // Make sure the matrices can be multiplied
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                throw new Exception("The matrices cannot be multiplied.");
            }

            // Create the result matrix
            int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

            // Multiply the matrices
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return result;
        }
        //GetVector
        private static int[] GetVector(string text)
        {
            // Create a vector to store the result
            int[] vector = new int[text.Length];

            // Convert the string to a vector
            for (int i = 0; i < text.Length; i++)
            {
                vector[i] = GetNumber(text[i]);
            }

            return vector;
        }
        //GetNumber
        private static int GetNumber(char character)
        {
            // Convert the character to a number
            int number = (int)character - 65;

            return number;
        }

        //GetKeyMatrix
        private static int[,] GetKeyMatrix(string key)
        {
            // Make sure the key length is a perfect square
            //example key


            double keyLength = Math.Sqrt(key.Length);
            if (keyLength != Math.Floor(keyLength))
            {
                throw new Exception("The key length is not a perfect square.");
            }

            // Create the key matrix
            int[,] keyMatrix = new int[(int)keyLength, (int)keyLength];

            // Fill the key matrix with the key characters
            for (int i = 0; i < key.Length; i++)
            {
                keyMatrix[i % (int)keyLength, i / (int)keyLength] = (int)key[i];
            }

            return keyMatrix;
        }
        //CheckKeyMatrix
        private static bool CheckKeyMatrix(int[,] keyMatrix)
        {
            // Make sure the key matrix is square
            if (keyMatrix.GetLength(0) != keyMatrix.GetLength(1))
            {
                return false;
            }

            // Make sure the key matrix is invertible
            if (GetDeterminant(keyMatrix) == 0)
            {
                return false;
            }

            return true;
        }
        //GetDeterminant
        private static int GetDeterminant(int[,] matrix)
        {
            // Make sure the matrix is square
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new Exception("The matrix is not square.");
            }

            // Make sure the matrix is 2x2 or 3x3
            if (matrix.GetLength(0) < 2 || matrix.GetLength(0) > 3)
            {
                throw new Exception("The matrix size is not supported.");
            }

            // Calculate the determinant
            int determinant = 0;
            if (matrix.GetLength(0) == 2)
            {
                determinant = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else if (matrix.GetLength(0) == 3)
            {
                determinant = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
                              matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                              matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
            }

            return determinant;
        }
        //Split
        private static string[] Split(string text, int chunkSize)
        {
            // Make sure the chunk size is greater than 0
            if (chunkSize <= 0)
            {
                throw new Exception("The chunk size must be greater than 0.");
            }

            // Split the text into chunks
            string[] chunks = new string[(int)Math.Ceiling((double)text.Length / chunkSize)];
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i] = text.Substring(i * chunkSize, Math.Min(chunkSize, text.Length - i * chunkSize));
            }

            return chunks;
        }


        //algorithm to encript and decrept message 
        private static string Encrypt(string text, string key)
        {
            // Make sure the key is valid
            int[,] keyMatrix = GetKeyMatrix(key);
            if (!CheckKeyMatrix(keyMatrix))
            {
                throw new Exception("The key is not valid.");
            }

            // Split the text into chunks
            string[] chunks = Split(text, keyMatrix.GetLength(0));

            // Encrypt the chunks
            string result = "";
            for (int i = 0; i < chunks.Length; i++)
            {
                // Pad the chunk if necessary
                if (chunks[i].Length < keyMatrix.GetLength(0))
                {
                    chunks[i] = chunks[i].PadRight(keyMatrix.GetLength(0), 'X');
                }

                // Get the vector
                int[] vector = GetVector(chunks[i]);

                // Multiply the key matrix and the vector
                int[] encryptedVector = MultiplyMatrixAndVector(keyMatrix, vector);

                // Convert the vector to a string
                string encryptedChunk = "";
                for (int j = 0; j < encryptedVector.Length; j++)
                {
                    encryptedChunk += GetCharacter(encryptedVector[j]);
                }

                // Add the encrypted chunk to the result
                result += encryptedChunk;
            }

            return result;
        }
        //Decrypt
        private static string Decrypt(string text, string key)
        {
            // Make sure the key is valid
            int[,] keyMatrix = GetKeyMatrix(key);
            if (!CheckKeyMatrix(keyMatrix))
            {
                throw new Exception("The key is not valid.");
            }

            // Split the text into chunks
            string[] chunks = Split(text, keyMatrix.GetLength(0));

            // Decrypt the chunks
            string result = "";
            for (int i = 0; i < chunks.Length; i++)
            {
                // Get the vector
                int[] vector = GetVector(chunks[i]);

                // Multiply the inverse key matrix and the vector
                int[] decryptedVector = MultiplyMatrixAndVector(GetInverseKeyMatrix(keyMatrix), vector);

                // Convert the vector to a string
                string decryptedChunk = "";
                for (int j = 0; j < decryptedVector.Length; j++)
                {
                    decryptedChunk += GetCharacter(decryptedVector[j]);
                }

                // Add the decrypted chunk to the result
                result += decryptedChunk;
            }

            return result;
        }
        //GetInverseKeyMatrix
        private static int[,] GetInverseKeyMatrix(int[,] keyMatrix)
        {
            // Make sure the key matrix is square
            if (keyMatrix.GetLength(0) != keyMatrix.GetLength(1))
            {
                throw new Exception("The key matrix is not square.");
            }

            // Make sure the key matrix is 2x2 or 3x3
            if (keyMatrix.GetLength(0) < 2 || keyMatrix.GetLength(0) > 3)
            {
                throw new Exception("The key matrix size is not supported.");
            }

            // Calculate the determinant
            int determinant = GetDeterminant(keyMatrix);

            // Calculate the inverse determinant
            int inverseDeterminant = 0;
            for (int i = 0; i < 26; i++)
            {
                if ((determinant * i) % 26 == 1)
                {
                    inverseDeterminant = i;
                    break;
                }
            }

            // Calculate the inverse key matrix
            int[,] inverseKeyMatrix = new int[keyMatrix.GetLength(0), keyMatrix.GetLength(1)];
            if (keyMatrix.GetLength(0) == 2)
            {
                inverseKeyMatrix[0, 0] = (inverseDeterminant * keyMatrix[1, 1]) % 26;
                inverseKeyMatrix[0, 1] = (inverseDeterminant * -keyMatrix[0, 1]) % 26;
                inverseKeyMatrix[1, 0] = (inverseDeterminant * -keyMatrix[1, 0]) % 26;
                inverseKeyMatrix[1, 1] = (inverseDeterminant * keyMatrix[0, 0]) % 26;
            }
            else if (keyMatrix.GetLength(0) == 3)
            {
                inverseKeyMatrix[0, 0] = (inverseDeterminant * (keyMatrix[1, 1] * keyMatrix[2, 2] - keyMatrix[1, 2] * keyMatrix[2, 1])) % 26;
                inverseKeyMatrix[0, 1] = (inverseDeterminant * (keyMatrix[0, 2] * keyMatrix[2, 1] - keyMatrix[0, 1] * keyMatrix[2, 2])) % 26;
                inverseKeyMatrix[0, 2] = (inverseDeterminant * (keyMatrix[0, 1] * keyMatrix[1, 2] - keyMatrix[0, 2] * keyMatrix[1, 1])) % 26;
                inverseKeyMatrix[1, 0] = (inverseDeterminant * (keyMatrix[1, 2] * keyMatrix[2, 0] - keyMatrix[1, 0] * keyMatrix[2, 2])) % 26;
                inverseKeyMatrix[1, 1] = (inverseDeterminant * (keyMatrix[0, 0] * keyMatrix[2, 2] - keyMatrix[0, 2] * keyMatrix[2, 0])) % 26;
                inverseKeyMatrix[1, 2] = (inverseDeterminant * (keyMatrix[0, 2] * keyMatrix[1, 0] - keyMatrix[0, 0] * keyMatrix[1, 2])) % 26;
                inverseKeyMatrix[2, 0] = (inverseDeterminant * (keyMatrix[1, 0] * keyMatrix[2, 1] - keyMatrix[1, 1] * keyMatrix[2, 0])) % 26;
                inverseKeyMatrix[2, 1] = (inverseDeterminant * (keyMatrix[0, 1] * keyMatrix[2, 0] - keyMatrix[0, 0] * keyMatrix[2, 1])) % 26;
                inverseKeyMatrix[2, 2] = (inverseDeterminant * (keyMatrix[0, 0] * keyMatrix[1, 1] - keyMatrix[0, 1] * keyMatrix[1, 0])) % 26;
                


            }
            //MultiplyMatrixAndVector
            private static int[] MultiplyMatrixAndVector(int[,] matrix, int[] vector)
            {
                // Make sure the matrix and vector are compatible
                if (matrix.GetLength(1) != vector.Length)
                {
                    throw new Exception("The matrix and vector are not compatible.");
                }

                // Multiply the matrix and the vector
                int[] result = new int[matrix.GetLength(0)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        result[i] += matrix[i, j] * vector[j];
                    }
                    result[i] = result[i] % 26;
                }

                return result;
            }
            //GetDeterminant
            private static int GetDeterminant(int[,] matrix)
            {
                // Make sure the matrix is square
                if (matrix.GetLength(0) != matrix.GetLength(1))
                {
                    throw new Exception("The matrix is not square.");
                }

                // Make sure the matrix is 2x2 or 3x3
                if (matrix.GetLength(0) < 2 || matrix.GetLength(0) > 3)
                {
                    throw new Exception("The matrix size is not supported.");
                }

                // Calculate the determinant
                int determinant = 0;
                if (matrix.GetLength(0) == 2)
                {
                    determinant = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) % 26;
                }
                else if (matrix.GetLength(0) == 3)
                {
                    determinant = (matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
                                   matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                                   matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0])) % 26;
                }

                return determinant;
            }
            //GetKeyMatrix
            
        }