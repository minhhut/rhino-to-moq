using Xunit;
using Rhino.Mocks;

namespace RhinoMocksToMoq.Tests
{
    public class ActionTests
    {
        [Fact]
        public void Once()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.Once();

            var calculatorService = new CalculatorService(calculator);
            calculatorService.Reset();

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void Twice()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.Twice();

            var calculatorService = new CalculatorService(calculator);
            calculatorService.Reset();
            calculatorService.Reset();

            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Any(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.Any();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                calculator.Reset();
            }

            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void AtLeastOnce(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.AtLeastOnce();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                calculator.Reset();
            }

            if (times == 0)
            {
                Assert.Throws<Moq.MockException>(() => calculator.VerifyAllExpectations());
            }
            else 
            {
                calculator.VerifyAllExpectations();
            }
        }

        [Fact]
        public void Never()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.Never();

            var calculatorService = new CalculatorService(calculator);
            
            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Times(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Reset()).Repeat.Times(times);

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                calculatorService.Reset();
            }
            
            calculator.VerifyAllExpectations();
        }
    }
}
