using ENode.Eventing;
using ENode.Infrastructure;

namespace ENode.Domain.CompatibleStore.Impl
{
    public class CompatibleStoreHandlerWrapper<TAggregateRootType> : ICompatibleStoreHandler
        where TAggregateRootType : class, IAggregateRoot
    {
        private readonly ICompatibleStoreHandler<TAggregateRootType> _aggregateInitEvent;

        public CompatibleStoreHandlerWrapper(ICompatibleStoreHandler<TAggregateRootType> aggregateInitEvent)
        {
            _aggregateInitEvent = aggregateInitEvent;
        }
        public object GetInnerHandler()
        {
            return _aggregateInitEvent;
        }
        public CompatibleStyle GetCompatibleStyle()
        {
            return _aggregateInitEvent.GetCompatibleStyle(null);
        }
        public DomainEventStream GetAggregateRestoreEventStream(string aggregateRootId)
        {
            return _aggregateInitEvent.GetAggregateRestoreEventStream(aggregateRootId, null);
        }

        //public bool SaveAggregateRoot(IAggregateRoot aggregateRoot)
        //{
        //    return _aggregateInitEvent.SaveAggregateRoot(aggregateRoot as TAggregateRootType);
        //}
    }
}
