namespace AutoMammetHQ.Model
{
    internal class InventoryItemCategory
    {
        public uint Id { get; }
        
        public string Name { get; }

        public InventoryItemCategory(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
