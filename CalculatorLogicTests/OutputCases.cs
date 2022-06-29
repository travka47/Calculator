using System.Collections.Generic;
using Calculator.Controllers;
using Calculator.Models;
using NUnit.Framework;

namespace CalculatorLogicTests
{
    [TestFixture]
    public class OutputCases
    {
        private State _state;

        [SetUp]
        public void Setup()
        {
            _state = new State{Input = "0", History = new Stack<string>()};
        }
        
        [Test]
        public void WithoutAnyData()
        {
            CalculatorController.Dispatch(ref _state, "=");

            Assert.AreEqual("0", _state.Input);
        }

        [Test]
        public void WithSingleOperandAndOperation()
        {
            CalculatorController.Dispatch(ref _state, "4");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "=");

            Assert.AreEqual("8", _state.Input);
        }

        [Test]
        public void ManyTimesWithSingleOperandAndOperation()
        {
            CalculatorController.Dispatch(ref _state, "4");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "=");
            CalculatorController.Dispatch(ref _state, "=");
            CalculatorController.Dispatch(ref _state, "=");

            Assert.AreEqual("16", _state.Input);
        }

        [Test]
        public void WithTwoOperands()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "=");

            Assert.AreEqual("11", _state.Input);
        }


        [Test]
        public void ManyTimesWithTwoOperands()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "=");
            CalculatorController.Dispatch(ref _state, "=");
            CalculatorController.Dispatch(ref _state, "=");

            Assert.AreEqual("23", _state.Input);
        }

        [Test]
        public void InputAfterOutput()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "=");
            
            CalculatorController.Dispatch(ref _state, "2");
            
            Assert.AreEqual("2", _state.Input);
        }
    }
}