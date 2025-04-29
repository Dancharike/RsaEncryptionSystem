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

    public void SaveWrapper(string filePath, RsaEncryptionResultWrapper data)
    {
        // save in format of: encryptedText|p|q
        string line = $"{data.EncryptedText}|{data.N}|{data.E}";
        File.WriteAllText(filePath, line);
    }

    public RsaEncryptionResultWrapper ReadWrapper(string filePath)
    {
        string line = File.ReadAllText(filePath);
        var parts = line.Split('|');
        if (parts.Length < 3)
        {
            throw new Exception("Invalid file format for text wrapper!");
        }
        
        return new RsaEncryptionResultWrapper
        {
            EncryptedText = parts[0],
            N = int.Parse(parts[1]),
            E = int.Parse(parts[2]),
        };
    }
}