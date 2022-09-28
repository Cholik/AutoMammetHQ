namespace AutoMammetHQ.Model
{
    internal class HandicraftCategory
    {
        public uint Id { get; }
        
        public string Name { get; }

        public HandicraftCategory(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
