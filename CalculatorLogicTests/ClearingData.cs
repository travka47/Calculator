using System.Collections.Generic;
using Calculator.Controllers;
using Calculator.Models;
using NUnit.Framework;

namespace CalculatorLogicTests
{
    [TestFixture]
    public class ClearingData
    {
        private State _state;
        
        [SetUp]
        public void Setup()
        {
            _state = new State { Input = "0", History = new Stack<string>() };
        }

        [Test]
        public void ClearInput()
        {
            CalculatorController.Dispatch(ref _state,"5");
            CalculatorController.Dispatch(ref _state,"+");
            Assert.AreEqual(5, _state.LeftOperand);
            
            CalculatorController.Dispatch(ref _state,"6");
            Assert.AreEqual("6", _state.Input);
            
            CalculatorController.Dispatch(ref _state, "CE");
            Assert.IsNotNull(_state.Input);
            Assert.IsNotNull(_state.LeftOperand);
            Assert.AreEqual("0", _state.Input);
            Assert.AreEqual(5, _state.LeftOperand);
        }

        [Test]
        public void ClearAllData()
        {
            CalculatorController.Dispatch(ref _state,"5");
            CalculatorController.Dispatch(ref _state, "+");
            Assert.AreEqual(5, _state.LeftOperand);
            
            CalculatorController.Dispatch(ref _state, "6");
            Assert.AreEqual("6", _state.Input);
            
            CalculatorController.Dispatch(ref _state,"C");
            Assert.AreEqual("0", _state.Input);
            Assert.AreEqual(0, _state.History.Count);
        }
    }
}