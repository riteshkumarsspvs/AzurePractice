using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public class ServiceBusQueue : IServiceBusQueue
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusQueue(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        public async Task<string> GetMessage()
        {
            var serviceBusReceiver = _serviceBusClient.CreateReceiver("servicebusqueue");
            var message = await serviceBusReceiver.ReceiveMessageAsync();

            await serviceBusReceiver.CompleteMessageAsync(message);

            return message.Body.ToString();
        }

        public async Task PutMessageAsync()
        {
            var serviceBusSender = _serviceBusClient.CreateSender("servicebusqueue");
            var serviceBusMessage = new ServiceBusMessage { Body = BinaryData.FromString("Message from app") };
            await serviceBusSender.SendMessageAsync(serviceBusMessage);
            await serviceBusSender.CloseAsync();
        }
    }
}
