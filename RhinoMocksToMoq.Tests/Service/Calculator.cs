namespace RhinoMocksToMoq
{
    public interface ICalculator
    {
        void Reset();

        int Add(int a, int b);

        int Random();
    }

    class CalculatorService : ICalculator
    {
        private readonly ICalculator _calculator;

        public CalculatorService(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public int Add(int a, int b)
        {
            return _calculator.Add(a, b);
        }

        public int Random()
        {
            return _calculator.Random();
        }

        public void Reset()
        {
            _calculator.Reset();
        }
    }
}