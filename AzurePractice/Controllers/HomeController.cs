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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKeyVaultManager _keyVaultManager;

        public HomeController(ILogger<HomeController> logger, IKeyVaultManager keyVaultManager)
        {
            _logger = logger;
            _keyVaultManager = keyVaultManager;
        }

        public async Task<IActionResult> Index()
        {
            var connectionString = await _keyVaultManager.GetSecret("ConnectionString");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
