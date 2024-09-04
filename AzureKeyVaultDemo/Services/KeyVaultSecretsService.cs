using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace AzureKeyVaultDemo.Services
{
    public class KeyVaultSecretsService : IKeyVaultSecretsService
    {
        private readonly IConfiguration _configuration;
        private readonly SecretClient client;

        public KeyVaultSecretsService(IConfiguration configuration)
        {
            _configuration = configuration;

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                },
            };

            var clientCredential = new ClientSecretCredential(tenantId: _configuration["KeyVault:TenantId"], clientId: _configuration["KeyVault:ClientId"], clientSecret: _configuration["KeyVault:ClientSecret"]);

            client = new SecretClient(new Uri(_configuration["KeyVault:KeyVaultURL"]!), clientCredential, options);
        }

        public async Task AddSecret(KeyVaultSecretModel secretModel)
        {
            var result = await client.SetSecretAsync(secretModel.SecretName, secretModel.SecretValue);
        }

        public async Task DeleteSecret(string secretName)
        {
            var operation = await client.PurgeDeletedSecretAsync(secretName);
        }

        public async Task<List<KeyVaultSecretModel>> GetAllSecrets()
        {
            var result = client.GetPropertiesOfSecretsAsync();

            var secrets = new List<KeyVaultSecretModel>();
            
            await foreach (var secret in result)
            {
                var secretModel = new KeyVaultSecretModel()
                {
                    Id = secret.Id.AbsoluteUri,
                    SecretName = secret.Name,
                    CreatedOn = secret.CreatedOn,
                    ExpiresOn = secret.ExpiresOn,
                    Enabled = secret.Enabled
                };
                
                secrets.Add(secretModel);
            }

            return secrets;
        }

        public async Task<KeyVaultSecretModel> GetSecretValue(string secretName)
        {
            var secret = await client.GetSecretAsync(secretName);

            var keyVaultSecret = new KeyVaultSecretModel()
            {
                SecretName = secretName,
                SecretValue = secret.Value.Value
            };

            return keyVaultSecret;
        }
    }
}
