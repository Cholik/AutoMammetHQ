namespace AutoMammetHQ.Data
{
    internal class Category
    {
        public uint Id { get; }
        
        public string Name { get; }

        public Category(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
