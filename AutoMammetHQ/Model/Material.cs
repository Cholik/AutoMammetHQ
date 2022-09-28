namespace AutoMammetHQ.Model
{
    internal class Material
    {
        public uint Amount { get; }

        public InventoryItem InventoryItem { get; }

        public Material(uint amount, InventoryItem inventoryItem)
        {
            Amount = amount;
            InventoryItem = inventoryItem;
        }
    }
}
