using System.Collections.Generic;
using Calculator.Controllers;
using Calculator.Models;
using NUnit.Framework;

namespace CalculatorLogicTests
{
    [TestFixture]
    public class BinaryOperations
    {
        private State _state;

        [SetUp]
        public void Setup()
        {
            _state = new State { Input = "0", History = new Stack<string>()};
        }

        [Test]
        public void OperationsSequenceWithoutOutput()
        {
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "3");
            CalculatorController.Dispatch(ref _state, "-");
            
            Assert.AreEqual("9", _state.Input);
        }
        
        [Test]
        public void OperationsSequenceWithSignChanging()
        {
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "/");
            CalculatorController.Dispatch(ref _state, "3");
            CalculatorController.Dispatch(ref _state, "*");
            CalculatorController.Dispatch(ref _state, "-");
            
            Assert.AreEqual("2", _state.Input);
        }
        
        [Test]
        public void LongOperationsSequenceWithSignChanging()
        {
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "/");
            CalculatorController.Dispatch(ref _state, "3");
            CalculatorController.Dispatch(ref _state, "-");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "-");
            
            Assert.AreEqual("2", _state.Input);
            
            CalculatorController.Dispatch(ref _state, "9");
            CalculatorController.Dispatch(ref _state, "/");
            
            Assert.AreEqual("-7", _state.Input);
        }

        [Test]
        public void SingleOperationWithOutput()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "=");
            
            Assert.AreEqual("11", _state.Input);
        }
        
        [Test]
        public void SingleOperationWithOutputAndSignChanging()
        {
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "+");
            CalculatorController.Dispatch(ref _state, "/");
            CalculatorController.Dispatch(ref _state, "*");
            CalculatorController.Dispatch(ref _state, "6");
            CalculatorController.Dispatch(ref _state, "=");
            
            Assert.AreEqual("30", _state.Input);
        }

        [Test]
        public void OperationsSequenceWithOutput()
        {
            CalculatorController.Dispatch(ref _state, "7");
            CalculatorController.Dispatch(ref _state, "-");
            CalculatorController.Dispatch(ref _state, "9");
            CalculatorController.Dispatch(ref _state, "=");
            
            Assert.AreEqual("-2", _state.Input);
            
            CalculatorController.Dispatch(ref _state, "/");
            CalculatorController.Dispatch(ref _state, "2");
            CalculatorController.Dispatch(ref _state, "=");
            
            Assert.AreEqual("-1", _state.Input);
        }
    }
}