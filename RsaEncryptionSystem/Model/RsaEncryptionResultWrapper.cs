namespace RsaEncryptionSystem.Model;

public class RsaEncryptionResultWrapper
{
    public string EncryptedText { get; set; }
    public int N { get; set; } // n = p * q
    public int E { get; set; } // public component e
}