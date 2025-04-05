namespace RsaEncryptionSystem.Model;

public class RsaEncryptionResult
{
    public int N { get; set; }          // n = p * q
    public int E { get; set; }          // public exhibitor
    public int CipherText { get; set; } // cyphered number
}