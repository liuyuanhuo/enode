using System;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ENode.Eventing;

namespace ENode.Domain.CompatibleStore.Impl
{

    public class CompatibleStoreHandlerProvider : BaseSingleHandlerProvider<ICompatibleStoreHandler>, ISingleHandlerProvider<ICompatibleStoreHandler>
    {
        protected override Type GetHandlerGenericInterfaceType()
        {
            return typeof(ICompatibleStoreHandler<>);
        }
        protected override Type GetHandlerProxyType()
        {
            return  typeof(CompatibleStoreHandlerWrapper<>);
        }
    }
}
