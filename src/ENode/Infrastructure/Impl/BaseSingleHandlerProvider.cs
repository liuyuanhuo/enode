using System;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECommon.Components;

namespace ENode.Infrastructure.Impl
{

    public abstract class BaseSingleHandlerProvider<THandlerInterface> : IAssemblyInitializer where THandlerInterface : class
    {
        private readonly IDictionary<Type, object> _handlerDict = new Dictionary<Type, object>();

        public void Initialize(params Assembly[] assemblies)
        {
            foreach (var handlerType in assemblies.SelectMany(assembly => assembly.GetTypes().Where(IsHandler)))
            {
                if (!TypeUtils.IsComponent(handlerType))
                {
                    throw new Exception(string.Format("Handler [type={0}] should be marked as component.", handlerType.FullName));
                }
                RegisterHandler(handlerType);
            }
        }

        public THandlerInterface GetHandler(Type type)
        {
            foreach (var key in _handlerDict.Keys.Where(key => key.IsAssignableFrom(type)))
            {
                return _handlerDict[key] as THandlerInterface;
            }
            return null;
        }
        public bool IsHandler(Type type)
        {
            return type != null && type.IsClass && !type.IsAbstract && ScanHandlerInterfaces(type).Any();
        }
        protected abstract Type GetHandlerGenericInterfaceType();
        protected abstract Type GetHandlerProxyType();
        
        private void RegisterHandler(Type handlerType)
        {
            foreach (var handlerInterfaceType in ScanHandlerInterfaces(handlerType))
            {
                var argumentType = handlerInterfaceType.GetGenericArguments().Single();
                var handlerProxyType = GetHandlerProxyType().MakeGenericType(argumentType);
                object handler;
                if (!_handlerDict.TryGetValue(argumentType, out handler))
                {
                    _handlerDict.Add(argumentType, Activator.CreateInstance(handlerProxyType, new[] { ObjectContainer.Resolve(handlerType) }));
                }
            }
        }
        private IEnumerable<Type> ScanHandlerInterfaces(Type type)
        {
            return type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == GetHandlerGenericInterfaceType());
        }
    }
}
