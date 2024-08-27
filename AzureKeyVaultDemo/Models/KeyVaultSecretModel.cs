using System.ComponentModel.DataAnnotations;

namespace AzureKeyVaultDemo.Models
{
    public class KeyVaultSecretModel
    {
        [Required]
        public string? SecretName { get; set; }

        [Required]
        public string? SecretValue { get; set; }
    }
}
