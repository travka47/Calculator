using System;
using System.Collections.Generic;
using Calculator.Models;
using Calculator.Views;

namespace Calculator.Controllers {
    public class CalculatorController {
        public static void Dispatch(ref State s, string content) {
            switch (Utils.TypesMap[content]) {
                case Utils.OperationType.Digit:
                    if (s.InputState.Equals(InputState.None)) {
                        s.InputState = InputState.IsLeft;
                        s.Input = "";
                    }
                    if (s.InputState.Equals(InputState.Clean)) {
                        s.InputState = InputState.IsRight;
                        s.Input = "";
                    }
                    if (s.Input.Equals("0")) s.Input = content;
                    else s.Input += content;
                    break;
                case Utils.OperationType.Binary:
                    if (s.InputState.Equals(InputState.IsRight)) {
                        Logic(ref s);
                        DispatchBinary(ref s, s.Operation);
                    }
                    s.LeftOperand = Convert.ToDouble(s.Input);
                    s.Operation = content;
                    switch (s.InputState) {
                        case InputState.IsLeft:
                            if (s.History.Count.Equals(0)) {
                                s.History.Push(s.LeftOperand.ToString());
                            }
                            s.History.Push(s.Operation);
                            break;
                        case InputState.Clean:
                            s.History.Pop();
                            s.History.Push(s.Operation);
                            break;
                    }
                    s.InputState = InputState.Clean;
                    break;
                case Utils.OperationType.Unary:
                    DispatchUnary(ref s, content);
                    break;
                case Utils.OperationType.Clean:
                    DispatchClean(ref s, content);
                    break;
                case Utils.OperationType.Memory:
                    DispatchMemory(ref s, content);
                    break;
                case Utils.OperationType.Output:
                    Logic(ref s);
                    s.Storage.LastOperation = s.Operation;
                    s.Storage.LastOperand = s.RightOperand;
                    DispatchBinary(ref s, s.Operation);
                    break;
                case Utils.OperationType.FloatingPoint:
                    s.Input ??= "0";
                    if (!s.Input.Contains(",")) s.Input += content;
                    break;
            }
        }
        
        private static void Logic(ref State s) {
            switch (s.InputState) {
                case InputState.None:
                    s.Operation = s.Storage.LastOperation;
                    s.RightOperand = s.Storage.LastOperand;
                    break;
                case InputState.IsLeft:
                    s.LeftOperand = Convert.ToDouble(s.Input);
                    s.Operation = s.Storage.LastOperation;
                    s.RightOperand = s.Storage.LastOperand;
                    break;
                case InputState.Clean:
                    s.RightOperand = s.LeftOperand;
                    break;
                case InputState.IsRight:
                    s.RightOperand = Convert.ToDouble(s.Input);
                    break;
            }
        }

        private static void DispatchBinary(ref State s, string content) {
            switch (content) {
                case "+":
                    s.LeftOperand += s.RightOperand;
                    break;
                case "-":
                    s.LeftOperand -= s.RightOperand;
                    break;
                case "*":
                    s.LeftOperand *= s.RightOperand;
                    break;
                case "/":
                    if (s.RightOperand.Equals(0.0))
                        throw new InvalidOperationException("Деление на ноль невозможно");
                    s.LeftOperand /= s.RightOperand;
                    break;
            }
            s.Operation = null;
            s.RightOperand = null;
            s.History.Clear();
            s.Input = s.LeftOperand is null ? "0" : s.LeftOperand.ToString();
            s.InputState = InputState.None;
        }

        private static void DispatchUnary(ref State s, string content) {
            string pushing;
            switch (content) {
                case "%":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            s.LeftOperand = 0;
                            s.Input = s.LeftOperand.ToString();
                            break;
                        case InputState.Clean:
                            s.RightOperand = s.LeftOperand / 100 * s.LeftOperand;
                            s.Input = s.RightOperand.ToString();
                            s.InputState = InputState.IsRight;
                            break;
                        case InputState.IsRight:
                            s.RightOperand = s.LeftOperand / 100 * Convert.ToDouble(s.Input);
                            s.Input = s.RightOperand.ToString();
                            break;
                    }
                    break;
                case "1/x":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            if (Convert.ToDouble(s.Input).Equals(0))
                                throw new InvalidOperationException("Деление на ноль невозможно");
                            s.LeftOperand = 1 / Convert.ToDouble(s.Input);
                            break;
                        case InputState.Clean:
                            s.RightOperand = 1 / s.LeftOperand;
                            break;
                        case InputState.IsRight:
                            s.RightOperand = 1 / Convert.ToDouble(s.Input);
                            break;
                    }
                    HistoryLogic(ref s, content);
                    break;
                case "√":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            if (s.LeftOperand < 0)
                                throw new InvalidOperationException("Недопустимый ввод");
                            s.LeftOperand = Math.Sqrt(Convert.ToDouble(s.Input));
                            break;
                        case InputState.Clean:
                            s.RightOperand = Math.Sqrt(Convert.ToDouble(s.LeftOperand));
                            break;
                        case InputState.IsRight:
                            if (s.RightOperand < 0)
                                throw new InvalidOperationException("Недопустимый ввод");
                            s.RightOperand = Math.Sqrt(Convert.ToDouble(s.Input));
                            break;
                    }
                    HistoryLogic(ref s, content);
                    break;
                case "±":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            s.LeftOperand = -Convert.ToDouble(s.Input);
                            //s.Input = s.LeftOperand.ToString();
                            break;
                        case InputState.Clean:
                            s.RightOperand = -s.LeftOperand;
                            //s.Input = s.RightOperand.ToString();
                            //s.InputState = InputState.IsRight;
                            break;
                        case InputState.IsRight:
                            s.RightOperand = -Convert.ToDouble(s.Input);
                            //s.Input = s.RightOperand.ToString();
                            break;
                    }
                    HistoryLogic(ref s, content);
                    break;
            }
        }

        private static string WrapUnary(string operand, string operation) {
            var res = operation switch {
                "1/x" => $"reciproc({operand})",
                "√" => $"sqrt({operand})",
                "±" => $"negate({operand})",
                _ => ""
            };
            return res;
        }
        
        private static void HistoryLogic(ref State s, string operation) {
            string pushing;
            switch (s.InputState) {
                case InputState.IsLeft or InputState.None:
                    pushing = s.History.Count.Equals(0) ? s.Input : s.History.Pop();
                    s.History.Push(WrapUnary(pushing, operation));
                    s.Input = s.LeftOperand.ToString();
                    break;
                case InputState.Clean:
                    s.Input = s.RightOperand.ToString();
                    s.History.Push(WrapUnary(s.LeftOperand.ToString(), operation));
                    s.InputState = InputState.IsRight;
                    break;
                case InputState.IsRight:
                    pushing = "+-*/".Contains(s.History.Peek()) ? s.Input : s.History.Pop();
                    //if (!operation.Equals("±") & pushing != s.Input) {
                        s.History.Push(WrapUnary(pushing, operation));
                    //}
                    s.Input = s.RightOperand.ToString();
                    break;
            }
        }
        
        private static void DispatchClean(ref State s, string content) {
            switch (content) {
                case "🠔":
                    s.Input = s.Input.Length > 1 ? s.Input.Remove(s.Input.Length - 1) : "0";
                    break;
                case "C":
                    s = new State { Input = "0", History = new Stack<string>(), MemoryOperand = s.MemoryOperand};
                    break;
                case "CE":
                    s.Input = "0";
                    break;
            }
        }

        private static void DispatchMemory(ref State s, string content) {
            switch (content) {
                case "MS":
                    s.MemoryOperand = Convert.ToDouble(s.Input);
                    s.InputState = InputState.None;
                    break;
                case "MR":
                    s.Input = s.MemoryOperand.ToString();
                    s.InputState = s.Operation is null ? InputState.IsLeft : InputState.IsRight;
                    break;
                case "MC":
                    s.MemoryOperand = 0;
                    break;
                case "M+":
                    s.MemoryOperand += Convert.ToDouble(s.Input);
                    s.InputState = s.Operation is null ? InputState.None : InputState.Clean;
                    break;
                case "M-":
                    s.MemoryOperand -= Convert.ToDouble(s.Input);
                    s.InputState = s.Operation is null ? InputState.None : InputState.Clean;
                    break;
            }
        }

    }
}