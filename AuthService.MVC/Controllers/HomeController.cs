using AuthService.MVC.AsyncServices;
using AuthService.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageProducer _messageProducer;

        public HomeController(ILogger<HomeController> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        public IActionResult Index()
        {
            _messageProducer.SendMessage<string>("OK");
            return View();
        }
    }
}
