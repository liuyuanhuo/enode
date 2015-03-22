using System;
using System.Collections.Generic;

namespace ENode.Infrastructure
{
    /// <summary>Represents a provider to provide the single handler.
    /// </summary>
    public interface ISingleHandlerProvider<THandlerInterface> where THandlerInterface : class
    {
        /// <summary>Get all the handler for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        THandlerInterface GetHandler(Type type);

        /// <summary>Check whether a given type is a handler type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsHandler(Type type);
    }
}
