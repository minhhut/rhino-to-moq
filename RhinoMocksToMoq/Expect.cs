using System;
using System.Linq.Expressions;
using Moq;

namespace Rhino.Mocks
{
    public interface IExpect<T> where T : class
    {
        IRepeat<T> Repeat { get; }

        void Throw(Exception exception);

        IExpect<T> SetupWithMoq();
    }
    
    public interface IExpect<T, TR> where T : class
    {
        IRepeat<T, TR> Repeat { get; }

        IExpect<T, TR> Return(TR result);

        void ReturnInOrder(params TR[] results);

        void Throw(Exception exception);
        
        IExpect<T, TR> SetupWithMoq();
    }

    public class Expect<T, TR> : IExpect<T, TR> where T : class
    {
        private readonly MoqAdapter<T, TR> _moqAdapter;
        
        private Repeat<T, TR> _repeat;

        private TR _result;

        private bool _isResultAssigned;

        public Expect(Mock<T> mock, Expression<Func<T, TR>> expression)
        {
            _moqAdapter = new MoqAdapter<T, TR>(mock, expression);
        }

        public IExpect<T, TR> Return(TR result)
        {
            if (_isResultAssigned)
            {
                throw new InvalidOperationException("Return should be setup only once");
            }

            _isResultAssigned = true;
            _result = result;

            return SetupWithMoq();
        }

        public void Throw(Exception exception)
        {
            _moqAdapter.Throws(exception);
        }

        public IExpect<T, TR> SetupWithMoq()
        {
            _moqAdapter.Setup(_result, _repeat);
            return this;
        }

        public void ReturnInOrder(params TR[] results)
        {
            _moqAdapter.SetupReturnInOrder(results);
        }

        public IRepeat<T, TR> Repeat
        {
            get
            {
                if (_repeat != null)
                {
                    throw new InvalidOperationException("Repeat should be setup only once");
                }
                return _repeat = new Repeat<T, TR>(this);
            }
        }
    }

    public class Expect<T> : IExpect<T> where T : class
    {
        private readonly MoqAdapter<T> _mockAdapter;

        private Repeat<T> _repeat;

        public Expect(Mock<T> mock, Expression<Action<T>> expression)
        {
            _mockAdapter = new MoqAdapter<T>(mock, expression);
        }

        public void Throw(Exception exception)
        {
            _mockAdapter.Throws(exception);
        }

        public IExpect<T> SetupWithMoq()
        {
            _mockAdapter.Setup(_repeat);
            return this;
        }

        public IRepeat<T> Repeat
        {
            get
            {
                if (_repeat != null)
                {
                    throw new InvalidOperationException("Repeat should setup only once");
                }
                return _repeat = new Repeat<T>(this);
            }
        }
    }
}