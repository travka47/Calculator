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
                    s.Input += content;
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
                case Utils.OperationType.Percent:
                    //s.LeftOperand = s.LeftOperand +-*/ s.RightOperand * %
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
            s.Input = s.LeftOperand.ToString();
            s.InputState = InputState.None;
        }

        private static void DispatchUnary(ref State s, string content) {
            switch (content) {
                case "1/x":
                    break;
                case "√":
                    break;
                case "±":
                    s.LeftOperand = -Convert.ToDouble(s.Input);
                    s.Input = s.LeftOperand.ToString();
                    break;
            }
        }

        private static void DispatchClean(ref State s, string content) {
            switch (content) {
                case "C":
                    break;
                case "CE":
                    break;
            }
        }

        private static void DispatchMemory(ref State s, string content) {
            switch (content) {
                case "MS":
                    break;
                case "MR":
                    break;
                case "MC":
                    break;
                case "M+":
                    break;
                case "M-":
                    break;
            }
        }

    }
}