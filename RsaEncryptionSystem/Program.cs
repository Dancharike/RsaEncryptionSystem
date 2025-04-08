using RsaEncryptionSystem.Interface;
using RsaEncryptionSystem.Model;
using RsaEncryptionSystem.Service;

namespace RsaEncryptionSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRsaEncryptionService rsaService = new RsaEncryptionService();
            IFileService fileService = new FileService();

            Console.WriteLine("RSA Encryption/Decryption System (with letter-based encryption)");
            Console.WriteLine("Choose preferred mode:");
            Console.WriteLine("1. Encrypt text");
            Console.WriteLine("2. Decrypt text from file");
            Console.Write("Enter your choice number: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                // encryption mode
                Console.Write("Enter p: ");
                int p = int.Parse(Console.ReadLine());
                
                Console.Write("Enter q: ");
                int q = int.Parse(Console.ReadLine());
                
                Console.Write("Enter plaintext: ");
                string plaintext = Console.ReadLine();

                // encrypt text character by character
                string encryptedText = EncryptText(rsaService, p, q, plaintext);
                Console.WriteLine("\nEncrypted Text:");
                Console.WriteLine(encryptedText);

                // save to a file with parameters p and q
                Console.Write("Enter the path to save the encrypted text: ");
                string filePath = Console.ReadLine();
                try
                {
                    RsaEncryptionResultWrapper wrapper = new RsaEncryptionResultWrapper
                    {
                        EncryptedText = encryptedText,
                        P = p,
                        Q = q
                    };
                    fileService.SaveWrapper(filePath, wrapper);
                    Console.WriteLine("Data successfully saved to file: " + filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving file: " + ex.Message);
                }
            }
            else if (choice == "2")
            {
                // decryption mode
                Console.Write("Enter the path to the encrypted text file: ");
                string filePath = Console.ReadLine();
                RsaEncryptionResultWrapper wrapper;
                try
                {
                    wrapper = fileService.ReadWrapper(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file: " + ex.Message);
                    return;
                }

                string encryptedText = wrapper.EncryptedText;
                int p = wrapper.P;
                int q = wrapper.Q;

                string decryptedText = DecryptText(rsaService, p, q, encryptedText);
                Console.WriteLine("\nDecrypted Text:");
                Console.WriteLine(decryptedText);
            }
            else
            {
                Console.WriteLine("Incorrect mode selection.");
            }
        }

        // encrypts text character by character: each character is converted to its numeric code and encrypted with RSA
        static string EncryptText(IRsaEncryptionService rsaService, int p, int q, string plaintext)
        {
            var parts = new System.Collections.Generic.List<string>();
            // convert a character to a number (its Unicode code, which is the same as ASCII for standard Latin characters)
            foreach (char c in plaintext)
            {
                int num = (int)c;
                RsaEncryptionResult result = rsaService.Encrypt(p, q, num);
                parts.Add(result.CipherText.ToString());
            }
            return string.Join(" ", parts);
        }

        // decrypts text: splits the string on spaces, decrypts each number and collects symbols
        static string DecryptText(IRsaEncryptionService rsaService, int p, int q, string encryptedText)
        {
            int n = p * q;
            int e = GetE(p, q);
            var parts = encryptedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string decrypted = "";
            foreach (var part in parts)
            {
                int cipher = int.Parse(part);
                RsaEncryptionResult result = new RsaEncryptionResult { N = n, E = e, CipherText = cipher };
                int decryptedNumber = rsaService.Decrypt(result);
                decrypted += (char)decryptedNumber;
            }
            return decrypted;
        }

        // function for defining public exponent e
        static int GetE(int p, int q)
        {
            int phi = (p - 1) * (q - 1);
            for (int e = 2; e < phi; e++)
            {
                if (Gcd(e, phi) == 1)
                    return e;
            }
            throw new Exception("Unable to determine public exponent e.");
        }

        static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
