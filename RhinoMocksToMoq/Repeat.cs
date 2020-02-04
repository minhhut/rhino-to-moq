using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Moq;
using Moq.Language;

namespace Rhino.Mocks
{
    public enum RepeatType
    {
        Any,
        Once,
        Twice,
        AtLeastOnce,
        Never,
        Exact
    }

    public interface IRepeat<T> where T : class
    {
        IExpect<T> Any();

        IExpect<T> Once();

        IExpect<T> Twice();

        IExpect<T> AtLeastOnce();

        IExpect<T> Never();

        IExpect<T> Times(int count);
    }

    public interface IRepeat<T, TR> where T : class
    {
        IExpect<T, TR> Any();

        IExpect<T, TR> Once();

        IExpect<T, TR> Twice();

        IExpect<T, TR> AtLeastOnce();

        IExpect<T, TR> Never();

        IExpect<T, TR> Times(int count);
    }

    public class Repeat<T, TR> : IRepeat<T, TR> where T : class
    {
        private readonly IExpect<T, TR> _expect;
        
        public int ExactCount { get; private set; }

        public RepeatType Type { get; private set; }

        public Repeat(IExpect<T, TR> expect)
        {
            _expect = expect;
        }

        public IExpect<T, TR> Any()
        {
            Type = RepeatType.Any;
            return _expect.SetupWithMoq();
        }

        public IExpect<T, TR> Once()
        {
            Type = RepeatType.Once;
            return _expect.SetupWithMoq();
        }

        public IExpect<T, TR> Twice()
        {
            Type = RepeatType.Twice;
            return _expect.SetupWithMoq();
        }

        public IExpect<T, TR> AtLeastOnce()
        {
            Type = RepeatType.AtLeastOnce;
            return _expect.SetupWithMoq();
        }

        public IExpect<T, TR> Never()
        {
            Type = RepeatType.Never;
            return _expect.SetupWithMoq();
        }

        public IExpect<T, TR> Times(int count)
        {
            Type = RepeatType.Exact;
            ExactCount = count;
            return _expect.SetupWithMoq();
        }
    }

    public class Repeat<T> : IRepeat<T> where T : class
    {
        private readonly IExpect<T> _expect;

        public int ExactCount { get; private set; }

        public RepeatType Type { get; private set; }

        public Repeat(IExpect<T> expect)
        {
            _expect = expect;
        }

        public IExpect<T> Once()
        {
            Type = RepeatType.Once;
            return _expect.SetupWithMoq();
        }

        public IExpect<T> Twice()
        {
            Type = RepeatType.Twice;
            return _expect.SetupWithMoq();
        }

        public IExpect<T> Any()
        {
            Type = RepeatType.Any;
            return _expect.SetupWithMoq();
        }

        public IExpect<T> AtLeastOnce()
        {
            Type = RepeatType.AtLeastOnce;
            return _expect.SetupWithMoq();
        }

        public IExpect<T> Never()
        {
            Type = RepeatType.Never;
            return _expect.SetupWithMoq();
        }

        public IExpect<T> Times(int count)
        {
            Type = RepeatType.Exact;
            ExactCount = count;
            return _expect.SetupWithMoq();
        }
    }
}