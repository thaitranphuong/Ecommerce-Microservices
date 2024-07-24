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
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        [HttpGet("create_payment")] //https://localhost:5011/api/Vnpay/create_payment?amount=1000&bankCode=NCB&locale=vn
        public IActionResult CreatePayment([FromQuery] string amount, [FromQuery] string bankCode, [FromQuery] string locale)
        {
            try
            {
                string orderType = "other";
                long amountInCents = long.Parse(amount) * 100;
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
                var queryBuilder = new StringBuilder();
                var hashDataBuilder = new StringBuilder();
                foreach (var kvp in sortedParams)
                {
                    queryBuilder.Append(WebUtility.UrlEncode(kvp.Key)).Append('=').Append(WebUtility.UrlEncode(kvp.Value));
                    hashDataBuilder.Append(kvp.Key).Append('=').Append(kvp.Value);
                    if (!kvp.Equals(sortedParams.Last()))
                    {
                        queryBuilder.Append('&');
                        hashDataBuilder.Append('&');
                    }
                }

                string queryUrl = queryBuilder.ToString();
                string vnpSecureHash = VnpayConfig.HmacSHA512(VnpayConfig.SecretKey, hashDataBuilder.ToString());
                queryUrl += "&vnp_SecureHash=" + vnpSecureHash;
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
