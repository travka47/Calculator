using System.Collections.Generic;
using Calculator.Controllers;
using Calculator.Models;
using NUnit.Framework;

namespace CalculatorLogicTests
{
    [TestFixture]
    public class MemoryOperations
    {
        private State _state;
        
        [SetUp]
        public void Setup()
        {
            _state = new State { Input = "0", History = new Stack<string>()};
        }

        [Test]
        public void MemorySave()
        {
            CalculatorController.Dispatch(ref _state, "8");
            CalculatorController.Dispatch(ref _state, "MS");
            
            Assert.IsNotNull(_state.MemoryOperand);
            Assert.AreEqual(8, _state.MemoryOperand);
        }
        
        [Test]
        public void MemorySaveAndRead()
        {
            
            CalculatorController.Dispatch(ref _state, "8");
            CalculatorController.Dispatch(ref _state, "MS");
            
            Assert.IsNotNull(_state.MemoryOperand);
            Assert.AreEqual(8, _state.MemoryOperand);
            
            CalculatorController.Dispatch(ref _state, "CE");
            CalculatorController.Dispatch(ref _state, "MR");
        }
        
        [Test]
        public void MemorySaveAndPlus()
        {
            CalculatorController.Dispatch(ref _state, "8");
            CalculatorController.Dispatch(ref _state, "MS");
            
            Assert.IsNotNull(_state.MemoryOperand);
            Assert.AreEqual(8, _state.MemoryOperand);
            
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "M+");
            
            Assert.AreEqual(13, _state.MemoryOperand);
        }
        
        [Test]
        public void MemorySaveAndMinus()
        {
            CalculatorController.Dispatch(ref _state, "8");
            CalculatorController.Dispatch(ref _state, "MS");
            
            Assert.IsNotNull(_state.MemoryOperand);
            Assert.AreEqual(8, _state.MemoryOperand);
            
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "M-");
            
            Assert.AreEqual(3, _state.MemoryOperand);
        }
        
        [Test]
        public void TryInputAfterOperation()
        {
            
            CalculatorController.Dispatch(ref _state, "8");
            CalculatorController.Dispatch(ref _state, "MS");
            CalculatorController.Dispatch(ref _state, "5");
            
            Assert.IsNotNull(_state.MemoryOperand);
            Assert.AreEqual(8, _state.MemoryOperand);
            Assert.AreEqual("5", _state.Input);
            
            CalculatorController.Dispatch(ref _state, "5");
            CalculatorController.Dispatch(ref _state, "M-");
            CalculatorController.Dispatch(ref _state, "9");
            
            Assert.AreEqual(-47, _state.MemoryOperand);
            Assert.AreEqual("9", _state.Input);
        }
    }
}