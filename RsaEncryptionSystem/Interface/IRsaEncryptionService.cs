using RsaEncryptionSystem.Model;

namespace RsaEncryptionSystem.Interface;

public interface IRsaEncryptionService
{
    RsaEncryptionResult Encrypt(int p, int q, int plaintext);
    int Decrypt(RsaEncryptionResult result);
}