namespace DaltonLima.Terminal
{
    public class TerminalCommandBase
    {
        public Command Command { get; }

        protected TerminalCommandBase(Command command)
        {
            Command = command;
        }
        
        protected TerminalCommandBase(string id, string description, string format)
        {
            var command = new Command {Id = id, Description = description, Format = format};
            Command = command;
        }
        
        
    }
}