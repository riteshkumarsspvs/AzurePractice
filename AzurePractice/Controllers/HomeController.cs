using AzurePractice.Models;
using AzurePractice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKeyVaultManager _keyVaultManager;
        private readonly IBlobManager _blobManager;
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly IServiceBusTopics _serviceBusTopics;

        public HomeController(ILogger<HomeController> logger, IKeyVaultManager keyVaultManager, IBlobManager blobManager, IServiceBusQueue serviceBusQueue, IServiceBusTopics serviceBusTopics)
        {
            _logger = logger;
            _keyVaultManager = keyVaultManager;
            _blobManager = blobManager;
            _serviceBusQueue = serviceBusQueue;
            _serviceBusTopics = serviceBusTopics;
        }

        public async Task<IActionResult> Index()
        {
            var connectionString = await _keyVaultManager.GetSecret("ConnectionString");
            var key = await _keyVaultManager.GetKey();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult KeyVault()
        {
            return View();
        }

        public IActionResult ConfigurationManager()
        {
            return View();
        }

        public IActionResult Blob()
        {
            var value = _blobManager.GetBlob();
            return View();
        }

        public IActionResult ServiceBus()
        {
            _serviceBusQueue.PutMessageAsync();
            var value = _serviceBusQueue.GetMessage();
            return View();
        }

        public IActionResult Topics()
        {
            _serviceBusTopics.SendMessageInTopicsAsync();
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
