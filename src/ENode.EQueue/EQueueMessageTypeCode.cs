namespace ENode.EQueue
{
    public enum EQueueMessageTypeCode
    {
        CommandMessage = 1,
        DomainEventStreamMessage = 2,
        ExceptionMessage = 3,
        CommandExecutedMessage = 11,
        DomainEventHandledMessage = 12,
    }
}