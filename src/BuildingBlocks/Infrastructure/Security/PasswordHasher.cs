using Contracts.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // Tạo một chuỗi salt 128-bit sử dụng ngẫu nhiên các byte mạnh mẽ
        byte[] salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(salt);
        }

        // Dùng PBKDF2 để tạo khóa con 256-bit (sử dụng HMACSHA256 với 100,000 lần lặp)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        return hashed;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Giải mã chuỗi hash để lấy salt và số lần lặp
        byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[128 / 8];
        Array.Copy(decodedHashedPassword, 0, salt, 0, salt.Length);

        // Hash lại mật khẩu nhập vào với salt và số lần lặp đã lưu
        string rehashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        // So sánh chuỗi hash mới với chuỗi hash đã lưu
        return rehashedPassword == hashedPassword;
    }
}
