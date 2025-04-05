using RsaEncryptionSystem.Interface;
using RsaEncryptionSystem.Model;

namespace RsaEncryptionSystem.Service;

public class RsaEncryptionService : IRsaEncryptionService
{
    public RsaEncryptionResult Encrypt(int p, int q, int plaintext)
    {
        int n = p * q;
        int phi = (p - 1) * (q - 1);
        int e = ChooseE(phi); // choose a public exponent e such that gcd(e, phi) == 1

        int ciphertext = ModExp(plaintext, e, n);
        return new RsaEncryptionResult { N = n, E = e, CipherText = ciphertext};
    }

    public int Decrypt(RsaEncryptionResult result)
    {
        int n = result.N;
        int e = result.E;

        (int p, int q) = Factorize(n);
        int phi = (p - 1) * (q - 1);
        int d = ModInverse(e, phi);
        
        int plaintext = ModExp(result.CipherText, d, n);
        return plaintext;
    }

    // choose the minimum number e (2 ≤ e < phi) such that gcd(e, phi) == 1
    private int ChooseE(int phi)
    {
        for (int e = 2; e < phi; e++)
        {
            if (Gcd(e, phi) == 1)
            {
                return e;
            }
        }
        throw new Exception("Could not find a suitable value for e!");
    }

    // Euclidean algorithm for finding the GCD
    private int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // find the modular inverse of a number using the extended Euclidean algorithm
    private int ModInverse(int a, int m)
    {
        (int g, int x, int y) = ExtendedGcd(a, m);
        if (g != 1)
        {
            throw new Exception("The modular inverse does not exist!");
        }
        return (x % m + m) % m;
    }

    // exponentiation by squaring
    private int ModExp(int baseVal, int exponent, int modulus)
    {
        int result = 1;
        baseVal = baseVal % modulus;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result = (result * baseVal) % modulus;
            }
            exponent >>= 1;
            baseVal = (baseVal * baseVal) % modulus;
        }
        return result;
    }

    // extended Euclidean Algorithm
    private (int, int, int) ExtendedGcd(int a, int b)
    {
        if (a == 0)
        {
            return (b, 0, 1);
        }
        (int g, int x1, int y1) = ExtendedGcd(b % a, a);
        int x = y1 - (b / a) * x1;
        int y = x1;
        return (g, x, y);
    }

    // simple factorization
    // iterate over divisors from 2 to sqrt(n)
    private (int, int) Factorize(int n)
    {
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                return (i, n / i);
            }
        }
        throw new Exception("Factorization failed!");
    }
}