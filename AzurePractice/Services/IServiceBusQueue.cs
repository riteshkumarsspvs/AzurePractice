using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public interface IServiceBusQueue
    {
        Task<string> GetMessage();

        Task PutMessageAsync();
    }
}
