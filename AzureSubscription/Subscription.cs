using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSubscription
{
    public class Subscription
    {
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public Subscription(ServiceBusClient serviceBusClient)
        {
            _serviceBusProcessor = serviceBusClient.CreateProcessor("ritstopics", "RitsSubscription", new ServiceBusProcessorOptions());
            Register().GetAwaiter().GetResult();
        }

        private async Task Register()
        {
            //try
            //{
                // add handler to process messages
                _serviceBusProcessor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await _serviceBusProcessor.StartProcessingAsync();

                // stop processing 
                //await _serviceBusProcessor.StopProcessingAsync();
            //}
            //finally
            //{
            //    // Calling DisposeAsync on client types is required to ensure that network
            //    // resources and other unmanaged objects are properly cleaned up.
            //    await _serviceBusProcessor.DisposeAsync();
            //    await _serviceBusProcessor.DisposeAsync();
            //}
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            //Console.WriteLine($"Received: {body} from subscription.");

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
