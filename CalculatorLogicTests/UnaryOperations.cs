using System.Collections.Generic;
using Calculator.Controllers;
using Calculator.Models;
using NUnit.Framework;

namespace CalculatorLogicTests
{
    [TestFixture]
    public class UnaryOperations
    {
        private State _state;

        [SetUp]
        public void SetUp()
        {
            _state = new State{Input = "0", History = new Stack<string>()};
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase("5", "0,2")]
        public void Reciprocal(string num, string expected)
        {
            CalculatorController.Dispatch(ref _state, num);
            CalculatorController.Dispatch(ref _state, "1/x");

            Assert.AreEqual(expected, _state.Input);
        }

        [Test]
        [TestCase("9", "3")]
        [TestCase("4", "2")]
        [TestCase("5", "2,23606797749979")]
        public void Sqrt(string num, string expected)
        {
            CalculatorController.Dispatch(ref _state, num);
            CalculatorController.Dispatch(ref _state, "√");
            
            Assert.AreEqual(expected, _state.Input);
        }

        [Test]
        [TestCase("5", "-5")]
        public void Negate(string num, string expected)
        {
            CalculatorController.Dispatch(ref _state, num);
            CalculatorController.Dispatch(ref _state, "±");
           
            Assert.AreEqual(expected, _state.Input);
        }

        [Test]
        public void GetPercentWithoutLeftOperand()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "%");
            
            Assert.AreEqual("0", _state.Input);
        }

        [Test]
        public void GetPercentWithRightOperand()
        {
            CalculatorController.Dispatch(ref _state, "1");
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "1");
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "%");
            
            Assert.AreEqual("10", _state.Input);
        }
    }
}