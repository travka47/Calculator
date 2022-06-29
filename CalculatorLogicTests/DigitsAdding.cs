using System.Collections.Generic;
using NUnit.Framework;
using Calculator.Controllers;
using Calculator.Models;

namespace CalculatorLogicTests
{
    public class DigitsAddingTests
    {
        private State _state;
        
        [SetUp]
        public void Setup()
        {
            _state = new State { Input = "0", History = new Stack<string>() };
        }

        [Test]
        public void SingleDigit()
        {
            CalculatorController.Dispatch(ref _state, "5"); 
            
            Assert.AreEqual("5", _state.Input); 
        }
        
        [Test]
        public void ManyDigits()
        {
            CalculatorController.Dispatch(ref _state, "5"); 
            CalculatorController.Dispatch(ref _state, "4"); 
            CalculatorController.Dispatch(ref _state, "2"); 
            
            Assert.AreEqual("542", _state.Input); 
        }
        
        [Test]
        public void SingleFloatingPoint()
        {
           CalculatorController.Dispatch(ref _state, ",");
           
           Assert.AreEqual("0,", _state.Input);
        }
        
        [Test]
        public void DigitsWithSingleFloatingPoint()
        {
            CalculatorController.Dispatch(ref _state, "4");
            CalculatorController.Dispatch(ref _state, ",");
            CalculatorController.Dispatch(ref _state, "2");
            
            Assert.AreEqual("4,2", _state.Input);
        }
        
        [Test]
        public void TryAddManyFloatingPoints()
        {
            CalculatorController.Dispatch(ref _state, "4");
            CalculatorController.Dispatch(ref _state, ",");
            CalculatorController.Dispatch(ref _state, ",");
            CalculatorController.Dispatch(ref _state, ",");
            CalculatorController.Dispatch(ref _state, "2");
            
            Assert.AreEqual("4,2", _state.Input);
        }
        [Test]
        public void WithLeadingZeroes()
        {
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "0");
            CalculatorController.Dispatch(ref _state, "5");
            
            Assert.AreEqual("5", _state.Input);
        }
    }
}