using Azure.Core;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public class KeyVaultManager : IKeyVaultManager
    {
        private readonly SecretClient _secretClient;
        private readonly KeyClient _keyClient;
        private readonly TokenCredential _tokenCredential;

        public KeyVaultManager(SecretClient secretClient, KeyClient keyClient, TokenCredential tokenCredential)
        {
            _secretClient = secretClient;
            _keyClient = keyClient;
            _tokenCredential = tokenCredential;
        }

        public async Task<string> GetSecret(string secretName)
        {
            try
            {
                KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);

                return keyValueSecret.Value;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetKey()
        {
            try
            {
                KeyVaultKey keyVaultKey = await _keyClient.GetKeyAsync("AzurePracKey");

                var cryptoClient = new CryptographyClient(keyVaultKey.Id, _tokenCredential);

                byte[] inputAsByteArray = Encoding.UTF8.GetBytes("test to encrypt");

                EncryptResult encryptResult = await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, inputAsByteArray);

                var encrypted = Convert.ToBase64String(encryptResult.Ciphertext);

                inputAsByteArray = Convert.FromBase64String(encrypted);

                DecryptResult decryptResult = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, inputAsByteArray);

                var decrypted = Encoding.Default.GetString(decryptResult.Plaintext);

                return decrypted;
            }
            catch
            {
                throw;
            }
        }

    }
}
