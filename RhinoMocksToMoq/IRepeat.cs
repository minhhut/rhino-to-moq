using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Moq;
using Moq.Language;

namespace RhinoMocksToMoq
{
    public enum RepeatType
    {
        Any,
        Once,
        AtLeastOnce,
        Never,
        Count
    }

    public interface IRepeat<T> where T : class
    {
        void Once();

        void Twice();

        void Times(int count);
    }

    public interface IRepeat<T, TR> where T : class
    {
        IExpect<T, TR> Any();

        IExpect<T, TR> Once();

        IExpect<T, TR> AtLeastOnce();

        IExpect<T, TR> Never();
    }

    public class Repeat<T, TR> : IRepeat<T, TR> where T : class
    {
        private readonly Expect<T, TR> _expect;

        public RepeatType RepeatType { get; private set; }

        public Repeat(Expect<T, TR> expect)
        {
            _expect = expect;
        }

        public IExpect<T, TR> Any()
        {
            RepeatType = RepeatType.Any;
            return _expect;
        }

        public IExpect<T, TR> Once()
        {
            RepeatType = RepeatType.Once;
            return _expect;
        }

        public IExpect<T, TR> AtLeastOnce()
        {
            RepeatType = RepeatType.AtLeastOnce;
            return _expect;
        }

        public IExpect<T, TR> Never()
        {
            RepeatType = RepeatType.Never;
            return _expect;
        }
    }

    public class Repeat<T> : IRepeat<T> where T : class
    {
        private readonly Mock<T> _mock;
        private readonly Expression<Action<T>> _expression;

        private static readonly ConcurrentDictionary<LambdaExpression, ISetupSequentialAction> SETUPS = new ConcurrentDictionary<LambdaExpression, ISetupSequentialAction>();

        public Repeat(Mock<T> mock, Expression<Action<T>> expression)
        {
            _mock = mock;
            _expression = expression;
        }

        public void Once()
        {
            SETUPS.AddOrUpdate(_expression, _mock.SetupSequence(_expression).Pass(),
                (expression, action) => action.Pass());
        }

        public void Twice()
        {
            SETUPS.AddOrUpdate(_expression, _mock.SetupSequence(_expression).Pass().Pass(),
                (expression, action) => action.Pass().Pass());
        }

        public void Times(int count)
        {
            SETUPS.AddOrUpdate(_expression, expression =>
                {
                    var action = _mock.SetupSequence(_expression);
                    for (var i = 0; i < count; i++)
                    {
                        action = action.Pass();
                    }
                    return action;
                },
                (expression, action) =>
                {
                    for (var i = 0; i < count; i++)
                    {
                        action = action.Pass();
                    }
                    return action;
                });
        }
    }
}