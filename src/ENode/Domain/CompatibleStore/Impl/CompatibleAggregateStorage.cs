using System;
using System.Collections.Generic;
using System.Linq;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Snapshoting;

namespace ENode.Domain.CompatibleStore.Impl
{
    public class CompatibleAggregateStorage : IAggregateStorage
    {
        private const int minVersion = 1;
        private const int maxVersion = int.MaxValue;
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly ISingleHandlerProvider<ICompatibleStoreHandler> _compatibleStoreHandlerProvider;
        private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotter _snapshotter;
        private readonly ITypeCodeProvider _aggregateRootTypeCodeProvider;

        public CompatibleAggregateStorage(
            IAggregateRootFactory aggregateRootFactory,
            ISingleHandlerProvider<ICompatibleStoreHandler> compatibleStoreHandlerProvider,
            IEventStore eventStore,
            ISnapshotStore snapshotStore,
            ISnapshotter snapshotter,
            ITypeCodeProvider aggregateRootTypeCodeProvider)
        {
            _aggregateRootFactory = aggregateRootFactory;
            _compatibleStoreHandlerProvider = compatibleStoreHandlerProvider;
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _snapshotter = snapshotter;
            _aggregateRootTypeCodeProvider = aggregateRootTypeCodeProvider;
        }

        public IAggregateRoot Get(Type aggregateRootType, string aggregateRootId)
        {
            if (aggregateRootId == null) throw new ArgumentNullException("aggregateRootId");

            var aggregateRoot = default(IAggregateRoot);

            CompatibleStyle compatibleStyle = CompatibleStyle.EventSourcingOnly;

            ICompatibleStoreHandler aggregateInitEventHandler = _compatibleStoreHandlerProvider.GetHandler(aggregateRootType);
            if (aggregateInitEventHandler != null)
            {
                compatibleStyle = aggregateInitEventHandler.GetCompatibleStyle();
            }

            if (compatibleStyle == CompatibleStyle.EventSourcingOnly)
            {
                if (TryGetFromSnapshot(aggregateRootId, aggregateRootType, out aggregateRoot))
                {
                    return aggregateRoot;
                }
            }

            if (TryGetFromCompatibleAggregateStore(aggregateRootId, aggregateRootType, aggregateInitEventHandler, compatibleStyle, out aggregateRoot))
            {
                return aggregateRoot;
            }

            return null;
        }

        #region Helper Methods

        private bool TryGetFromSnapshot(string aggregateRootId, Type aggregateRootType, out IAggregateRoot aggregateRoot)
        {
            aggregateRoot = null;

            var snapshot = _snapshotStore.GetLastestSnapshot(aggregateRootId, aggregateRootType);
            if (snapshot == null) return false;

            aggregateRoot = _snapshotter.RestoreFromSnapshot(snapshot);
            if (aggregateRoot == null) return false;

            if (aggregateRoot.UniqueId != aggregateRootId)
            {
                throw new Exception(string.Format("Aggregate root restored from snapshot not valid as the aggregate root id not matched. Snapshot aggregate root id:{0}, expected aggregate root id:{1}", aggregateRoot.UniqueId, aggregateRootId));
            }

            var aggregateRootTypeCode = _aggregateRootTypeCodeProvider.GetTypeCode(aggregateRootType);
            var eventStreamsAfterSnapshot = _eventStore.QueryAggregateEvents(aggregateRootId, aggregateRootTypeCode, snapshot.Version + 1, int.MaxValue);
            aggregateRoot.ReplayEvents(eventStreamsAfterSnapshot);
            return true;
        }

        private bool TryGetFromCompatibleAggregateStore(string aggregateRootId, Type aggregateRootType,
            ICompatibleStoreHandler aggregateInitHandler, CompatibleStyle compatibleStyle, out IAggregateRoot aggregateRoot)
        {
            aggregateRoot = null;
            var aggregateRootTypeCode = _aggregateRootTypeCodeProvider.GetTypeCode(aggregateRootType);

            if(compatibleStyle == CompatibleStyle.RepositoryOnly)
            {
                // Replay Repository Aggregate Init Event 
                DomainEventStream aggregateInitEvent = aggregateInitHandler.GetAggregateRestoreEventStream(aggregateRootId);
                if (aggregateInitEvent != null)
                {
                    if (aggregateInitEvent.AggregateRootId != aggregateRootId)
                    {
                        throw new Exception(string.Format("DomainEventStream.AggregateRootId error: {0}, expected aggregate root id:{1}", aggregateInitEvent.AggregateRootId, aggregateRootId));
                    }
                    aggregateRoot = _aggregateRootFactory.CreateAggregateRoot(aggregateRootType);
                    aggregateRoot.RestoreFromEvents(aggregateInitEvent);
                    return true;
                }
                return false;
            }
            else if (compatibleStyle == CompatibleStyle.RepositoryThenEventSourcing)
            {
                var versionStart = minVersion;

                // Replay Repository Aggregate Init Event 
                //IDomainEvent aggregateInitEvent = _compatibleAggregateStore.GetAggregateInitEvent(aggregateRootType, aggregateRootId);
                DomainEventStream aggregateInitEvent = aggregateInitHandler.GetAggregateRestoreEventStream(aggregateRootId);
                if (aggregateInitEvent != null)
                {
                    if (aggregateInitEvent.AggregateRootId != aggregateRootId)
                    {
                        throw new Exception(string.Format("DomainEventStream.AggregateRootId error: {0}, expected aggregate root id:{1}", aggregateInitEvent.AggregateRootId, aggregateRootId));
                    }
                    aggregateRoot = _aggregateRootFactory.CreateAggregateRoot(aggregateRootType);
                    aggregateRoot.RestoreFromEvents(aggregateInitEvent);
                    versionStart = aggregateInitEvent.Version + 1;
                }

                // Replay Event Sourcing Events
                var eventStreams = _eventStore.QueryAggregateEvents(aggregateRootId, aggregateRootTypeCode, versionStart, maxVersion);
                if (eventStreams == null || !eventStreams.Any())
                {
                    if (aggregateRoot != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                aggregateRoot.ReplayEvents(eventStreams);
                return true;
            }
            else //if(compatibleStyle == CompatibleAggregateInitStyle.EventSourcingOnly)
            {
                // Replay Event Sourcing Events
                var versionStart = minVersion;
                var eventStreams = _eventStore.QueryAggregateEvents(aggregateRootId, aggregateRootTypeCode, versionStart, maxVersion);
                if (eventStreams == null || !eventStreams.Any())
                {
                    return false;
                }
                aggregateRoot = _aggregateRootFactory.CreateAggregateRoot(aggregateRootType);
                aggregateRoot.ReplayEvents(eventStreams);
                return true;
            }
        }
        #endregion
    }
}
