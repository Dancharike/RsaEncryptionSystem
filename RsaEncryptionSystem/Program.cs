using RsaEncryptionSystem.Interface;
using RsaEncryptionSystem.Model;
using RsaEncryptionSystem.Service;

namespace RsaEncryptionSystem;

public class Program
{
    static void Main(string[] args)
    {
        IRsaEncryptionService rsaService = new RsaEncryptionService();
        IFileService fileService = new FileService();

        Console.WriteLine("RSA Encryption/Decryption System (without cryptographic library)");
        Console.Write("Enter the prime number p (<= 1000): ");
        
        int p = int.Parse(Console.ReadLine());
        Console.Write("Enter a prime number q (<= 1000): ");
        
        int q = int.Parse(Console.ReadLine());
        Console.Write("Enter the original number (plaintext): ");
        
        int plaintext = int.Parse(Console.ReadLine());

        // encryption
        RsaEncryptionResult encryptionResult = rsaService.Encrypt(p, q, plaintext);
        Console.WriteLine($"\nEncryption complete.\nPublic key: (n = {encryptionResult.N}, e = {encryptionResult.E})\nEncrypted number: {encryptionResult.CipherText}");

        // saving to file
        string filePath = "rsa_encrypted.txt";
        fileService.Save(filePath, encryptionResult);
        Console.WriteLine($"Data saved to file: {filePath}\nThe file can be fount at: {Environment.CurrentDirectory}");

        // reading from file and decryption
        RsaEncryptionResult loadedData = fileService.Read(filePath);
        int decrypted = rsaService.Decrypt(loadedData);
        Console.WriteLine($"Decrypted number: {decrypted}");
    }
}