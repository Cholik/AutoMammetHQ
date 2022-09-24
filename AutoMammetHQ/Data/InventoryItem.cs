namespace AutoMammetHQ.Data
{
    internal class InventoryItem
    {
        public uint Id { get; }

        public string Name { get; }

        public InventoryItem(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
