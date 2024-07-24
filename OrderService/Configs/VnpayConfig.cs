using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Configs
{
    public class VnpayConfig
    {
        public static string VnpPayUrl { get; } = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public static string VnpReturnUrl { get; } = "http://localhost:3000/payment-vnpay/result";
        public static string VnpTmnCode { get; } = "6E3KRNR8";
        public static string SecretKey { get; } = "HG4B7ZUPCJQVVPLBKDOHY7048I5SJI6B";
        public static string VnpApiUrl { get; } = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
        public static string VnpVersion { get; } = "2.1.0";
        public static string VnpCommand { get; } = "pay";

        public static string Sha256(string message)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string HashAllFields(IDictionary<string, string> fields)
        {
            var sortedFields = fields.OrderBy(f => f.Key).ToList();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sortedFields.Count; i++)
            {
                var field = sortedFields[i];
                if (!string.IsNullOrEmpty(field.Value))
                {
                    sb.Append(field.Key).Append("=").Append(field.Value);
                    if (i < sortedFields.Count - 1)
                    {
                        sb.Append("&");
                    }
                }
            }
            return HmacSHA512(SecretKey, sb.ToString());
        }

        public static string HmacSHA512(string key, string data)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(data);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public static string GetIpAddress(HttpRequest request)
        {
            try
            {
                string ipAddress = request.Headers["X-FORWARDED-FOR"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                return $"Invalid IP: {ex.Message}";
            }
        }

        public static string GetRandomNumber(int len)
        {
            Random rnd = new Random();
            string chars = "0123456789";
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }
            return sb.ToString();
        }
    }
}
