using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public interface IKeyVaultManager

    {
        public Task<string> GetSecret(string secretName);
        Task<string> GetKey();
    }
}
