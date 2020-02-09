using Xunit;
using Rhino.Mocks;
using System;

namespace RhinoMocksToMoq.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void Method()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3);

            var sum = new CalculatorService(calculator).Add(1, 2);

            Assert.Equal(3, sum);
            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void Once()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Once();

            var sum = new CalculatorService(calculator).Add(1, 2);

            Assert.Equal(3, sum);
            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void Twice()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Twice();

            var calculatorService = new CalculatorService(calculator);
            var sum = calculatorService.Add(1, 2);
            var sum2 = calculatorService.Add(1, 2);

            Assert.Equal(3, sum);
            Assert.Equal(3, sum2);
            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Any(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Any();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
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
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.AtLeastOnce();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
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
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Never();

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
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Times(times);

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
            }

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ReturnInOrder()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Random()).ReturnInOrder(new int[] {1, 2, 3, 4});

            var calculatorService = new CalculatorService(calculator);
            
            Assert.Equal(1, calculator.Random());
            Assert.Equal(2, calculator.Random());
            Assert.Equal(3, calculator.Random());
            Assert.Equal(4, calculator.Random());
            
            calculator.VerifyAllExpectations();                        
        }

        [Fact]
        public void MulitpleReturnExpectationsOnSameMock()
        {
            const uint InnerFunctionResult = 0xdeadbeef;
            Func<int, TimeSpan, uint> innerFunction = MockRepository.GenerateMock<Func<int, TimeSpan, uint>>();

            for (var i = 0; i < 20; ++i)
            {
                innerFunction.Expect(f => f(i, TimeSpan.FromDays(i))).Return(InnerFunctionResult);
            }

            for (var i = 0; i < 20; ++i)
            {
                var result = innerFunction(i, TimeSpan.FromDays(i));
                Assert.Equal(InnerFunctionResult, result);
            }
        }
    }
}
