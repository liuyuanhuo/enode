using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENode.Domain.CompatibleStore
{
    public enum CompatibleStyle
    {
        EventSourcingOnly,
        RepositoryOnly,
        RepositoryThenEventSourcing
    }
}
