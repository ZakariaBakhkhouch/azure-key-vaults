using System.ComponentModel.DataAnnotations;

namespace AzureKeyVaultDemo.Models
{
    public class KeyVaultSecretModel
    {
        public string? Id { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }
        public bool? Enabled { get; set; }

        [Required]
        public string? SecretName { get; set; }

        [Required]
        public string? SecretValue { get; set; }
    }
}
