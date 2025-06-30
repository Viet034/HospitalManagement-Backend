using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace SWP391_SE1914_ManageHospital.Ultility
{
    public class VNPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new();
        private readonly SortedList<string, string> _responseData = new();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _requestData.Add(key, value);
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                _responseData[key] = value;
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var query = new StringBuilder();
            foreach (var kv in _requestData)
            {
                query.Append($"{kv.Key}={Uri.EscapeDataString(kv.Value)}&");
            }

            var rawData = query.ToString().TrimEnd('&');
            var signData = HmacSHA512(hashSecret, rawData);
            var fullUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";

            return fullUrl;
        }

        public bool ValidateSignature(string receivedHash, string hashSecret)
        {
            var raw = _responseData
                .Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                .Select(kv => $"{kv.Key}={kv.Value}");

            var rawData = string.Join("&", raw);
            var computedHash = HmacSHA512(hashSecret, rawData);

            return string.Equals(computedHash, receivedHash, StringComparison.OrdinalIgnoreCase);
        }

        private string HmacSHA512(string key, string data)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
