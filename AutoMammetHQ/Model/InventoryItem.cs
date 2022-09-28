namespace AutoMammetHQ.Model
{
    internal class InventoryItem
    {
        public uint Id { get; }

        public string Name { get; }

        public InventoryItemCategory Category { get;  }

        public InventoryItem(uint id, string name, InventoryItemCategory category)
        {
            Id = id;
            Name = name;
            Category = category;
        }
    }
}
