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

                // find public parameters one time
                var (n, e) = rsaService.GetPublicKeyParameters(p, q);

                // encrypt text character by character with using specific e
                string encryptedText = EncryptText(rsaService, n, e, plaintext);
                Console.WriteLine("\nEncrypted Text:");
                Console.WriteLine(encryptedText);

                // save to a file with parameters p and q
                Console.Write("Enter the path to save the encrypted text: ");
                string filePath = Console.ReadLine();
                try
                {
                    var wrapper = new RsaEncryptionResultWrapper
                    {
                        EncryptedText = encryptedText,
                        N = n,
                        E = e
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

                string decryptedText = DecryptText(rsaService, wrapper.N, wrapper.E, encryptedText);
                Console.WriteLine("\nDecrypted Text:");
                Console.WriteLine(decryptedText);
            }
            else
            {
                Console.WriteLine("Incorrect mode selection.");
            }
        }

        // encrypts text character by character: each character is converted to its numeric code and encrypted with RSA
        static string EncryptText(IRsaEncryptionService rsaService, int n, int e, string plaintext)
        {
            var parts = new System.Collections.Generic.List<string>();
            // convert a character to a number (its Unicode code, which is the same as ASCII for standard Latin characters)
            foreach (char c in plaintext)
            {
                int num = (int)c;
                RsaEncryptionResult result = rsaService.EncryptWithKey(n, e, num);
                parts.Add(result.CipherText.ToString());
            }
            return string.Join(" ", parts);
        }

        // decrypts text: splits the string on spaces, decrypts each number and collects symbols
        static string DecryptText(IRsaEncryptionService rsaService, int n, int e, string encryptedText)
        {
            var parts = encryptedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string decrypted = "";
            foreach (var part in parts)
            {
                int cipher = int.Parse(part);
                var result = new RsaEncryptionResult { N = n, E = e, CipherText = cipher };
                int decryptedNumber = rsaService.Decrypt(result);
                decrypted += (char)decryptedNumber;
            }
            return decrypted;
        }
    }
}
