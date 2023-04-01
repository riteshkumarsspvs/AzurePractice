using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public class ServiceBusTopics : IServiceBusTopics
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusTopics(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        public async Task SendMessageInTopicsAsync()
        {
            try
            {
                var sender = _serviceBusClient.CreateSender("ritstopics");

                await sender.SendMessageAsync(new ServiceBusMessage { Body = BinaryData.FromString("send my data") });

                await sender.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
