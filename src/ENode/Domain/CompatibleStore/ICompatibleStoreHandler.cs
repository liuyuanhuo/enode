using ENode.Eventing;
using ENode.Infrastructure;
using System;

namespace ENode.Domain.CompatibleStore
{

    public interface ICompatibleStoreHandler : IHandlerProxy
    {
        CompatibleStyle GetCompatibleStyle();
        DomainEventStream GetAggregateRestoreEventStream(string aggregateRootId);
        //bool SaveAggregateRoot(IAggregateRoot aggregateRoot);
    }

    public interface ICompatibleStoreHandler<in TAggregateRootType>
        where TAggregateRootType : class, IAggregateRoot
    {
        CompatibleStyle GetCompatibleStyle(TAggregateRootType nullObject);
        DomainEventStream GetAggregateRestoreEventStream(string aggregateRootId, TAggregateRootType nullObject);
        //bool SaveAggregateRoot(TAggregateRootType aggregateRoot);
    }
}
