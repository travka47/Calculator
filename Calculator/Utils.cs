using System.Collections.Generic;

namespace Calculator {
    public static class Utils {
        public enum OperationType {
            Digit,
            Binary,
            Unary,
            Clean,
            Memory,
            Output,
            FloatingPoint,
            Undefined
        }

        public static readonly Dictionary<string, OperationType> TypesMap =
            new Dictionary<string, OperationType> {
                {"1", OperationType.Digit},
                {"2", OperationType.Digit},
                {"3", OperationType.Digit},
                {"4", OperationType.Digit},
                {"5", OperationType.Digit},
                {"6", OperationType.Digit},
                {"7", OperationType.Digit},
                {"8", OperationType.Digit},
                {"9", OperationType.Digit},
                {"0", OperationType.Digit},
                {"+", OperationType.Binary},
                {"*", OperationType.Binary},
                {"/", OperationType.Binary},
                {"-", OperationType.Binary},
                {"1/x", OperationType.Unary},
                {"√", OperationType.Unary},
                {"±", OperationType.Unary},
                {"%", OperationType.Unary},
                {"🠔", OperationType.Clean},
                {"C", OperationType.Clean},
                {"CE", OperationType.Clean},
                {"MC", OperationType.Memory},
                {"MR", OperationType.Memory},
                {"MS", OperationType.Memory},
                {"M+", OperationType.Memory},
                {"M-", OperationType.Memory},
                {"=", OperationType.Output},
                {",", OperationType.FloatingPoint},
                {"&", OperationType.Undefined},
            };
    } 
}