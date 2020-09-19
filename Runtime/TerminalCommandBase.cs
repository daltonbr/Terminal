namespace Terminal
{
    public class TerminalCommandBase
    {
        private Command _command;
        public Command Command => _command;
    
        protected TerminalCommandBase(Command command)
        {
            _command = command;
        }
        
        protected TerminalCommandBase(string id, string description, string format)
        {
            var command = new Command {Id = id, Description = description, Format = format};
            _command = command;
        }
        
        
    }

    public struct Command
    {
        public string Id { get;  set; }
        public string Description { get; set; }
        public string Format { get; set; }

        public Command(string id, string description, string format)
        {
            Id = id;
            Description = description;
            Format = format;
        }
    
    }
}