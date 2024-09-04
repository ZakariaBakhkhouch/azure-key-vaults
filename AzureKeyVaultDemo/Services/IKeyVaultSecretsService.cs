using AzureKeyVaultDemo.Models;

namespace AzureKeyVaultDemo.Services
{
    public interface IKeyVaultSecretsService
    {
        Task<List<KeyVaultSecretModel>> GetAllSecrets();
        Task<KeyVaultSecretModel> GetSecretValue(string secretName);
        Task AddSecret(KeyVaultSecretModel secretModel);
        Task DeleteSecret(string secretName);
    }
}
