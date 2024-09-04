using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultDemo.Models;
using AzureKeyVaultDemo.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using System;
using System.Diagnostics;

namespace AzureKeyVaultDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKeyVaultSecretsService _keyVaultSecretsService;
        //const string keyVaultName = "MyPersonalInfos";

        public HomeController(ILogger<HomeController> logger, IKeyVaultSecretsService keyVaultSecretsService)
        {
            _logger = logger;
            _keyVaultSecretsService = keyVaultSecretsService;
        }

        public async Task<IActionResult> Index()
        {
            var secrets = await _keyVaultSecretsService.GetAllSecrets();

            return View(secrets);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(KeyVaultSecretModel model)
        {
            await _keyVaultSecretsService.AddSecret(model);
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string secretName)
        {
            var result = await _keyVaultSecretsService.GetSecretValue(secretName);

            return View(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string secretName)
        {
            await _keyVaultSecretsService.DeleteSecret(secretName);
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
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
