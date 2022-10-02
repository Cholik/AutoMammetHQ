using System;

namespace AutoMammetHQ.Model
{
    internal class Handicraft
    {
        public uint Id { get; }

        public string Name { get; } = string.Empty;

        public int CraftingTime { get; }

        public int BasePrice { get; }

        public HandicraftCategory[] Categories { get; } = Array.Empty<HandicraftCategory>();

        public Material[] Materials { get; } = Array.Empty<Material>();

        public Handicraft()
        {
        }

        public Handicraft(
            uint id,
            string name,
            int craftingTime,
            int basePrice,
            HandicraftCategory[] categories,
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
