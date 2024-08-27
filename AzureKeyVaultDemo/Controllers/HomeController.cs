using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultDemo.Models;
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
        private readonly IConfiguration _configuration;
        private readonly SecretClient client;
        const string keyVaultName = "";
        const string kvUri = "";

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;

            client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        }

        public IActionResult Index()
        {
            var result = client.GetPropertiesOfSecrets();

            var secrets = new List<KeyVaultSecretModel>();

            foreach(var secret in result)
            {
                var secretModel = new KeyVaultSecretModel()
                {
                    SecretName = secret.Name
                };

                secrets.Add(secretModel);
            } 
             
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
            var result = await client.SetSecretAsync(model.SecretName, model.SecretValue);
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string secretName)
        {

            var secret = await client.GetSecretAsync(secretName);

            var model = new KeyVaultSecretModel()
            {
                SecretName = secretName,
                SecretValue = secret.Value.Value
            };

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string secretName)
        {

            DeleteSecretOperation operation = await client.StartDeleteSecretAsync(secretName);
            
            await operation.WaitForCompletionAsync();

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
