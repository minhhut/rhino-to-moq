using System;
using System.Collections.Generic;
using System.Linq;
using Moq.Language.Flow;

namespace RhinoMocksToMoq
{
    public static class MoqExtensions
    {
        public static IReturnsResult<TMock> ReturnsInOrder<TMock, TResult>(this ISetup<TMock, TResult> setup, params Func<TResult>[] valueFunctions) where TMock : class
        {
            var functionQueue = new Queue<Func<TResult>>(valueFunctions);
            return setup.Returns(() => functionQueue.Dequeue()());
        }

        public static void ReturnsInOrder<TMock, TResult>(this ISetup<TMock, TResult> setup, List<TResult> results) where TMock : class
        {
            ReturnsInOrder(setup, results.Select(result => new Func<TResult>(() => result)).ToArray());
        }
    }
}