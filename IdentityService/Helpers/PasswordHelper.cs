using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Helpers
{
    public class PasswordHelper
    {
        // Tạo mật khẩu băm (hash) sử dụng PBKDF2 với salt
        public static string HashPassword(string password)
        {
            // Tạo ngẫu nhiên một salt (mã muối)
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Sử dụng PBKDF2 với HMACSHA256 để mã hóa mật khẩu với salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // Lấy 32 byte từ kết quả băm

            // Gộp salt và hash vào cùng một mảng
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            // Chuyển đổi thành chuỗi base64 để lưu trữ
            return Convert.ToBase64String(hashBytes);
        }

        // Hàm kiểm tra mật khẩu có khớp với băm đã lưu trữ hay không
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Chuyển đổi hash từ chuỗi base64 thành byte[]
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Lấy salt từ hash
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Tạo lại hash từ mật khẩu nhập vào với salt đã lưu
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // So sánh hash nhập vào với hash đã lưu
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false; // Nếu không khớp thì trả về false
            }

            return true; // Mật khẩu đúng
        }
    }

}
