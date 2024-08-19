using Azure.Messaging.ServiceBus;
using ServiceContracts.AzureBusService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AzureBusService
{
    public class AzureBusProducerService : IAzureBusProducerService
    {
        public async Task<List<ProducerAzureBus>> ProduceMessages(string connectionstring, string topicName)
        {
            ServiceBusClient client;
            ServiceBusSender sender;

            client = new ServiceBusClient(connectionstring);
            sender = client.CreateSender(topicName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            List<ProducerAzureBus> messages = new List<ProducerAzureBus>();

            for (int i = 0; i < 5; i++)
            {
                ProducerAzureBus message = new ProducerAzureBus();
                message.Topic = topicName;
                message.Guid = Guid.NewGuid();
                message.Message = $"Message with id : {message.Guid}";

                if (!messageBatch.TryAddMessage(new ServiceBusMessage(message.Message)))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }

                messages.Add(message);
            }

            await sender.SendMessagesAsync(messageBatch);
            return messages;

        }
    }

    
}
