namespace DaltonLima.Terminal
{
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