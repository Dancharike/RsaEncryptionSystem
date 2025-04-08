using RsaEncryptionSystem.Model;

namespace RsaEncryptionSystem.Interface;

public interface IFileService
{
    void Save(string filePath, RsaEncryptionResult data);
    RsaEncryptionResult Read(string filePath);
    
    void SaveWrapper(string filePath, RsaEncryptionResultWrapper data);
    RsaEncryptionResultWrapper ReadWrapper(string filePath);
}