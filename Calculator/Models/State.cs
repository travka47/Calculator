using System.Collections.Generic;

namespace Calculator.Models;

public struct State {
    public string Input;
    public Stack<string> History;
    public InputState InputState;
    public double? LeftOperand;
    public double? RightOperand;
    public string Operation;
    public double MemoryOperand;
    public Storage Storage;
}
    
public struct Storage {
    public double? LastOperand;
    public string LastOperation;
}

public enum InputState {
    None,
    IsLeft,
    Clean,
    IsRight
}