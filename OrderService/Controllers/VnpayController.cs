using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Configs;
using OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        [HttpGet("create_payment")] //https://localhost:5011/api/Vnpay/create_payment?amount=1000000&bankCode=NCB&locale=vn
        public IActionResult CreatePayment([FromQuery] long amount, [FromQuery] string bankCode, [FromQuery] string locale)
        {
            try
            {
                string orderType = "other";
                long amountInCents = amount * 100;
                string vnpTxnRef = VnpayConfig.GetRandomNumber(8);
                string vnpIpAddr = VnpayConfig.GetIpAddress(HttpContext.Request);
                string vnpTmnCode = VnpayConfig.VnpTmnCode;

                var vnpParams = new Dictionary<string, string>
            {
                { "vnp_Version", VnpayConfig.VnpVersion },
                { "vnp_Command", VnpayConfig.VnpCommand },
                { "vnp_TmnCode", vnpTmnCode },
                { "vnp_Amount", amountInCents.ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_BankCode", bankCode },
                { "vnp_TxnRef", vnpTxnRef },
                { "vnp_OrderInfo", $"Thanh toan don hang {vnpTxnRef}" },
                { "vnp_OrderType", orderType },
                { "vnp_ReturnUrl", VnpayConfig.VnpReturnUrl },
                { "vnp_IpAddr", vnpIpAddr },
                { "vnp_Locale", locale }
            };

                var cld = DateTime.UtcNow.AddHours(7);
                string vnpCreateDate = cld.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                vnpParams["vnp_CreateDate"] = vnpCreateDate;

                cld = cld.AddMinutes(15);
                string vnpExpireDate = cld.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                vnpParams["vnp_ExpireDate"] = vnpExpireDate;

                var sortedParams = vnpParams.OrderBy(p => p.Key);

                List<string> fieldNames = vnpParams.Keys.Cast<string>().ToList();
                fieldNames.Sort();
                StringBuilder hashData = new StringBuilder();
                StringBuilder query = new StringBuilder();
                foreach (string fieldName in fieldNames)
                {
                    string fieldValue = vnpParams[fieldName]?.ToString();
                    if (!string.IsNullOrEmpty(fieldValue))
                    {
                        // Build hash data
                        hashData.Append(fieldName);
                        hashData.Append('=');
                        hashData.Append(HttpUtility.UrlEncode(fieldValue, Encoding.ASCII));

                        // Build query
                        query.Append(HttpUtility.UrlEncode(fieldName, Encoding.ASCII));
                        query.Append('=');
                        query.Append(HttpUtility.UrlEncode(fieldValue, Encoding.ASCII));
                        if (fieldName != fieldNames.Last())
                        {
                            query.Append('&');
                            hashData.Append('&');
                        }
                    }
                }

                string queryUrl = query.ToString();
                string vnp_SecureHash = VnpayConfig.HmacSHA512(VnpayConfig.SecretKey, hashData.ToString());
                queryUrl += "&vnp_SecureHash=" + vnp_SecureHash;
                string paymentUrl = VnpayConfig.VnpPayUrl + "?" + queryUrl;

                var dto = new VnpayDto
                {
                    Status = "OK",
                    Message = "successfully",
                    Url = paymentUrl
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpGet("payment_result")]
        public IActionResult PaymentResult([FromQuery] int vnpAmount, [FromQuery] string vnpOrderInfo, [FromQuery] string vnpResponseCode)
        {
            if (vnpResponseCode == "00")
            {
                return Ok($"Thanh toan thanh cong. So tien: {vnpAmount / 100} VND, Thong tin giao dich: {vnpOrderInfo}");
            }
            else
            {
                return BadRequest("Loi thanh toan, vui long thanh toan lai.");
            }
        }
    }
}
