using RsaEncryptionSystem.Model;

namespace RsaEncryptionSystem.Interface;

public interface IRsaEncryptionService
{
    // find public key (n, e) for p and q
    (int n, int e) GetPublicKeyParameters(int p, int q);
    RsaEncryptionResult EncryptWithKey(int n, int e, int plaintext);
    int Decrypt(RsaEncryptionResult result);
}