using System;
using Terminal;

public class TerminalCommand<T1> : TerminalCommandBase
{
    private Action<T1> _action;
    
    public TerminalCommand(Command command, Action<T1> action) : base(command)
    {
        // _command = command;
        _action = action;
    }
    
    public TerminalCommand(string id, string description , string format, Action<T1> action) : base(id, description, format)
    {
        _action = action;
    }

    public void Invoke(T1 value)
    {
        _action.Invoke(value);
    }
}

public class TerminalCommand : TerminalCommandBase
{
    private Action _action;

    public TerminalCommand(Command command, Action action) : base(command)
    {
        //_command = command;
        _action = action;
    }
    
    public TerminalCommand(string id, string description , string format, Action action) : base(id, description, format)
    {
        _action = action;
    }

    public void Invoke()
    {
        _action.Invoke();
    }
}