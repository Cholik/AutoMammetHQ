namespace AutoMammetHQ.Data
{
    internal class Handicraft
    {
        public uint Id { get; }

        public string Name { get; }

        public int CraftingTime { get; }

        public int BasePrice { get; }

        public Category[] Categories { get; }

        public Material[] Materials { get;  }

        public Handicraft(
            uint id,
            string name,
            int craftingTime,
            int basePrice,
            Category[] categories,
            Material[] materials)
        {
            Id = id;
            Name = name;
            CraftingTime = craftingTime;
            BasePrice = basePrice;
            Categories = categories;
            Materials = materials;
        }
    }
}
