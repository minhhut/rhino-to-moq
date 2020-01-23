using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;

namespace RhinoMocksToMoq
{
    public interface IExpect<T> where T : class
    {
        IRepeat<T> Repeat { get; set; }

        void Throw(Exception exception);
    }

    public interface IExpect<T, TR> where T : class
    {
        void Return(TR result);

        IRepeat<T, TR> Repeat { get; set; }
    }
    
    public class Expect<T, TR> : IExpect<T, TR> where T : class
    {
        private readonly Mock<T> _mock;
        private readonly Expression<Func<T, TR>> _expression;

        private static ConcurrentDictionary<Tuple<object, string>, List<TR>> RESULT_STORE = new ConcurrentDictionary<Tuple<object, string>, List<TR>>();

        public Expect(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            _mock = mock;
            _expression = expression;

            Repeat = new Repeat<T, TR>(this);
        }

        public void Return(TR result)
        {
            var repeat = (Repeat<T, TR>) Repeat;

            switch (repeat.RepeatType)
            {
                case RepeatType.Any:
                    _mock.Setup(_expression).Returns(result);
                    break;
                case RepeatType.Once:
                    RESULT_STORE.AddOrUpdate(new Tuple<object, string>(_mock.Object, _expression.ToString()), expression =>
                    {
                        _mock.Setup(_expression).Returns(result);
                        return new List<TR>() {result};
                    }, (expression, resultList) =>
                    {
                        resultList.Add(result);
                        _mock.Setup(_expression).ReturnsInOrder(resultList);
                        return resultList;
                    });
                    break;
                case RepeatType.AtLeastOnce:
                    _mock.Setup(_expression).Returns(result).Verifiable("Should call at lease once");
                    break;
                case RepeatType.Never:
                    _mock.Setup(_expression).Throws(new Exception("Should never be called"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{repeat.RepeatType} is not implemented");
            }
        }

        public IRepeat<T, TR> Repeat { get; set; }
    }

    public class Expect<T> : IExpect<T> where T : class
    {
        private readonly Mock<T> _mock;

        public IRepeat<T> Repeat { get; set; }

        public Expression<Action<T>> Expression { get; }

        public Expect(Mock<T> mock, Expression<Action<T>> expression)
        {
            this._mock = mock;            
            Expression = expression;
            Repeat = new Repeat<T>(mock, expression);
        }

        public void Throw(Exception exception)
        {
            _mock.Setup(Expression).Throws(exception);
        }
    }
}