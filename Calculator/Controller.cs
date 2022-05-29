using System;

namespace Calculator {
    public class Controller {
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
                    s.InputState = InputState.Clean;
                    s.Operation = content;
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
                    s.LeftOperand /= s.RightOperand;
                    break;
            }
            s.Operation = null;
            s.RightOperand = null;
            s.Input = s.LeftOperand is null ? "0" : s.LeftOperand.ToString();
            s.InputState = InputState.None;
        }

        private static void DispatchUnary(ref State s, string content) {
            switch (content) {
                case "%":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            s.LeftOperand = 0;
                            s.Input = s.LeftOperand.ToString();
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
                            s.LeftOperand = 1 / Convert.ToDouble(s.Input);
                            s.Input = s.LeftOperand.ToString();
                            break;
                        case InputState.IsRight:
                            s.RightOperand = 1 / Convert.ToDouble(s.Input);
                            s.Input = s.RightOperand.ToString();
                            break;
                    }
                    break;
                case "√":
                    switch (s.InputState) {
                        case InputState.IsLeft or InputState.None:
                            s.LeftOperand = Math.Sqrt(Convert.ToDouble(s.Input));
                            s.Input = s.LeftOperand.ToString();
                            break;
                        case InputState.IsRight:
                            s.RightOperand = Math.Sqrt(Convert.ToDouble(s.Input));
                            s.Input = s.RightOperand.ToString();
                            break;
                    }
                    break;
                case "±":
                    s.LeftOperand = -Convert.ToDouble(s.Input);
                    s.Input = s.LeftOperand.ToString();
                    break;
            }
        }

        private static void DispatchClean(ref State s, string content) {
            switch (content) {
                case "🠔":
                    s.Input = s.Input.Length > 1 ? s.Input.Remove(s.Input.Length - 1) : "0";
                    break;
                case "C":
                    s = new State { Input = "0", MemoryOperand = s.MemoryOperand};
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
                    break;
                case "MC":
                    s.MemoryOperand = 0;
                    break;
                case "M+":
                    s.MemoryOperand += Convert.ToDouble(s.Input);
                    s.InputState = InputState.None;
                    break;
                case "M-":
                    s.MemoryOperand -= Convert.ToDouble(s.Input);
                    s.InputState = InputState.None;
                    break;
            }
        }

    }
}