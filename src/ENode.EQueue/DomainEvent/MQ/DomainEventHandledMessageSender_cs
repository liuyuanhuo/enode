using System.Text;
using ECommon.Components;
using ECommon.Serializing;
using ENode.Infrastructure;
using EQueue.Clients.Producers;
using EQueue.Protocols;
using EQueueMessage = EQueue.Protocols.Message;
using ECommon.IO;

namespace ENode.EQueue
{
    public class DomainEventHandledMessageSender
    {
        private const string DefaultDomainEventHandledMessageSenderProcuderId = "DomainEventHandledMessageSender";
        private readonly Producer _producer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly SendQueueMessageService _sendMessageService;
        private readonly IOHelper _ioHelper;

        public Producer Producer { get { return _producer; } }

        public DomainEventHandledMessageSender(string id = null, ProducerSetting setting = null)
        {
            _producer = new Producer(id ?? DefaultDomainEventHandledMessageSenderProcuderId, setting ?? new ProducerSetting());
            _jsonSerializer = ObjectContainer.Resolve<IJsonSerializer>();
            _sendMessageService = new SendQueueMessageService();
            _ioHelper = ObjectContainer.Resolve<IOHelper>();
        }

        public DomainEventHandledMessageSender Start()
        {
            _producer.Start();
            return this;
        }
        public DomainEventHandledMessageSender Shutdown()
        {
            _producer.Shutdown();
            return this;
        }
        public void Send(DomainEventHandledMessage message, string topic)
        {
            var messageJson = _jsonSerializer.Serialize(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageJson);
            var equeueMessage = new EQueueMessage(topic, (int)EQueueMessageTypeCode.DomainEventHandledMessage, messageBytes);
            _ioHelper.TryIOAction("DomainEventHandledMessage", () => messageJson, () =>
            {
                _ioHelper.TryIOAction(() => _producer.SendAsync(equeueMessage, message.CommandId), "DomainEventHandledMessageAsync");
            }, 0);
        }
        public void SendAsync(DomainEventHandledMessage message, string topic)
        {
            var messageJson = _jsonSerializer.Serialize(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageJson);
            var equeueMessage = new EQueueMessage(topic, (int)EQueueMessageTypeCode.DomainEventHandledMessage, messageBytes);
            SendMessageAsync(equeueMessage, messageJson, message.CommandId, 0);
        }

        private void SendMessageAsync(EQueueMessage message, string messageJson, string routingKey, int retryTimes)
        {
            _ioHelper.TryAsyncActionRecursively<AsyncTaskResult>("PublishDomainEventHandledMessageAsync",
            () => _sendMessageService.SendMessageAsync(_producer, message, routingKey),
            currentRetryTimes => SendMessageAsync(message, messageJson, routingKey, currentRetryTimes),
            null,
            () => string.Format("[message:{0}]", messageJson),
            null,
            retryTimes);
        }
    }
}
