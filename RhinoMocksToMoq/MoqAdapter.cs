using System;
using System.Linq.Expressions;
using Moq;

namespace Rhino.Mocks
{
    class MoqAdapter<T, TR> where T : class
    {
        private readonly Mock<T> _mock;
        private readonly Expression<Func<T, TR>> _expression;

        public MoqAdapter(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            _mock = mock;
            _expression = expression;
        }

        public void Setup(TR result, Repeat<T,TR> repeat)
        {
            if (result != null)
            {
                if (repeat == null)
                {
                    _mock.Setup(_expression).Returns(result);
                }
                else
                {
                    switch (repeat.Type)
                    {
                        case RepeatType.Once:
                            _mock.SetupSequence(_expression).Returns(result).Throws(new Exception("Should call once"));
                            break;
                        case RepeatType.Twice:
                            _mock.SetupSequence(_expression).Returns(result).Returns(result).Throws(new Exception("Should call only twice"));
                            break;
                        case RepeatType.Any:
                            _mock.Setup(_expression).Returns(result);
                            break;
                        case RepeatType.AtLeastOnce:
                            _mock.Setup(_expression).Returns(result).Verifiable();
                            break;
                        case RepeatType.Never:
                            _mock.Setup(_expression).Throws(new Exception("Should never call"));
                            break;
                        case RepeatType.Exact:
                            var sequence = _mock.SetupSequence(_expression);
                            for (var i = 0; i < repeat.ExactCount; i++)
                            {
                                sequence = sequence.Returns(result);
                            }
                            sequence.Throws(new Exception($"Should call only {repeat.ExactCount} times"));
                            break;
                    }
                }
            }
        }

        public void SetupReturnInOrder(params TR[] results)
        {
            _mock.Setup(_expression).ReturnsInOrder(results);
        }

        public void Throws(Exception exception)
        {
            _mock.Setup(_expression).Throws(exception);
        }
    }

    class MoqAdapter<T> where T : class
    {
        private readonly Mock<T> _mock;
        private readonly Expression<Action<T>> _expression;

        public MoqAdapter(Mock<T> mock, Expression<Action<T>> expression)
        {
            _mock = mock;
            _expression = expression;
        }

        public void Setup(Repeat<T> repeat)
        {
            if (repeat == null)
            {
                _mock.Setup(_expression);
            }
            else
            {
                switch (repeat.Type)
                {
                    case RepeatType.Once:
                        _mock.SetupSequence(_expression).Pass().Throws(new Exception("Should call once"));
                        break;
                    case RepeatType.Twice:
                        _mock.SetupSequence(_expression).Pass().Pass().Throws(new Exception("Should call only twice"));
                        break;
                    case RepeatType.Any:
                        _mock.Setup(_expression);
                        break;
                    case RepeatType.AtLeastOnce:
                        _mock.Setup(_expression).Verifiable();
                        break;
                    case RepeatType.Never:
                        _mock.Setup(_expression).Throws(new Exception("Should never call"));
                        break;
                    case RepeatType.Exact:
                        var sequence = _mock.SetupSequence(_expression);
                        for (var i = 0; i < repeat.ExactCount; i++)
                        {
                            sequence = sequence.Pass();
                        }
                        sequence.Throws(new Exception($"Should call only {repeat.ExactCount} times"));
                        break;
                }
            }
        }

        public void Throws(Exception exception)
        {
            _mock.Setup(_expression).Throws(exception);
        }
    }
}