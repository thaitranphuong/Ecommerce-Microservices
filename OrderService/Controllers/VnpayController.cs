using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Configs;
using OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
        public class VnPayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var vnpCompare = CompareInfo.GetCompareInfo("en-US");
                return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
            }
        }

        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = VnpayConfig.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }

        [HttpGet("create_payment")] //https://localhost:5011/api/Vnpay/create_payment?amount=1000000&locale=vn
        public IActionResult CreatePayment([FromQuery] long amount, [FromQuery] string locale)
        {
            try
            {
                var vnpParams = new SortedList<string, string>
                {
                    { "vnp_Version", VnpayConfig.VnpVersion },
                    { "vnp_Command", VnpayConfig.VnpCommand },  
                    { "vnp_TmnCode", VnpayConfig.VnpTmnCode },
                    { "vnp_Amount", (amount * 100).ToString() },
                    { "vnp_BankCode", "NCB" },
                    { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_IpAddr", "123.123.123.123" },
                    { "vnp_Locale", locale },
                    { "vnp_OrderInfo", $"Thanh toan don hang 123" },
                    { "vnp_OrderType", "other" },
                    { "vnp_ReturnUrl", VnpayConfig.VnpReturnUrl },
                    { "vnp_TxnRef", VnpayConfig.GetRandomNumber(8) },
                    { "vnp_ExpireDate", DateTime.UtcNow.AddHours(7).AddMinutes(15).ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) },
                };

                _requestData = new SortedList<string, string>(new VnPayCompare());

                foreach (var param in vnpParams)
                {
                    _requestData.Add(param.Key, param.Value);
                }

                var dto = new VnpayDto
                {
                    Status = "200",
                    Message = "successfully",
                    Url = CreateRequestUrl(VnpayConfig.VnpPayUrl, VnpayConfig.HashSecret)
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }
    }
}
