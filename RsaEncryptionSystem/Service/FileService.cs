using RsaEncryptionSystem.Interface;
using RsaEncryptionSystem.Model;

namespace RsaEncryptionSystem.Service;

public class FileService : IFileService
{
    public void Save(string filePath, RsaEncryptionResult data)
    {
        // save the data as a string: n e ciphertext
        string line = $"{data.N} {data.E} {data.CipherText}";
        File.WriteAllText(filePath, line);
    }

    public RsaEncryptionResult Read(string filePath)
    {
        string line = File.ReadAllText(filePath);
        var parts = line.Split(' ');
        if (parts.Length < 3)
        {
            throw new Exception("Invalid file format!");
        }
        
        int n = int.Parse(parts[0]);
        int e = int.Parse(parts[1]);
        int ciphertext = int.Parse(parts[2]);
        
        return new RsaEncryptionResult { N = n, E = e, CipherText = ciphertext };
    }
}