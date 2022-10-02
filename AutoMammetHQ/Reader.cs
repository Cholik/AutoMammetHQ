using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMammetHQ.Model;
using Dalamud.Utility.Signatures;
using Lumina.Excel.GeneratedSheets;

namespace AutoMammetHQ
{
    // Code from Otter, just slight changes to fix warnings/get it plugin ready.
    public class Reader
    {
        [Signature("E8 ?? ?? ?? ?? 8B 50 10")]
        private readonly unsafe delegate* unmanaged<IntPtr> readerInstance = null!;

        private readonly Configuration config;
        private readonly Handicraft[] handicrafts;
        private readonly InventoryItem[] inventoryItems;

        internal Handicraft[] Handicrafts
        {
            get
            {
                return handicrafts;
            }
        }

        internal InventoryItem[] InventoryItems
        {
            get
            {
                return inventoryItems;
            }
        }

        public Reader(Plugin plugin)
        {
            Dalamud.Initialize(plugin.PluginInterface);
            SignatureHelper.Initialise(this);

            this.config = plugin.Configuration;

            var itemData = Dalamud.GameData
                .GetExcelSheet<Item>()!;

            var handicraftCategories = Dalamud.GameData
                .GetExcelSheet<MJICraftworksObjectTheme>()!
                .Select(x => new HandicraftCategory(x.RowId, x.Unknown0.ToString()))
                .ToList();

            var inventoryItemCategories = Dalamud.GameData
                .GetExcelSheet<MJIItemCategory>()!
                .Select(x => new InventoryItemCategory(x.RowId, x.Unknown0.ToString()))
                .ToList();

            inventoryItems = Dalamud.GameData
                .GetExcelSheet<MJIItemPouch>()!
                .Select(i => new InventoryItem(
                    i.RowId,
                    itemData.GetRow(i.Item.Row)?.Name.ToString() ?? string.Empty,
                    inventoryItemCategories.First(x => x.Id == i.Unknown1)
                ))
                .ToArray();

            var craftworksObjects = Dalamud.GameData
                .GetExcelSheet<MJICraftworksObject>()!
                .Where(x => !string.IsNullOrEmpty(itemData.GetRow(x.Item.Row)?.Name.ToString()));

            var handicrafts = new List<Handicraft>();

            foreach (var craftworksObject in craftworksObjects)
            {
                var categories = handicraftCategories
                    .Where(category => new uint[] { craftworksObject.Unknown1, craftworksObject.Unknown2 }.Contains(category.Id))
                    .Where(category => category.Id != 0)
                    .ToArray();

                var materials = new List<Material>();

                if (craftworksObject.Unknown4 != 0)
                {
                    materials.Add(
                        new Material(craftworksObject.Unknown5, inventoryItems.First(x => x.Id == craftworksObject.Unknown4)
                    ));
                }

                if (craftworksObject.Unknown6 != 0)
                {
                    materials.Add(
                        new Material(craftworksObject.Unknown7, inventoryItems.First(x => x.Id == craftworksObject.Unknown6)
                    ));
                }

                if (craftworksObject.Unknown8 != 0)
                {
                    materials.Add(
                        new Material(craftworksObject.Unknown9, inventoryItems.First(x => x.Id == craftworksObject.Unknown8)
                    ));
                }

                handicrafts.Add(
                    new Handicraft(
                        craftworksObject.RowId,
                        itemData.GetRow(craftworksObject.Item.Row)?.Name.ToString() ?? string.Empty,
                        craftworksObject.Unknown13,
                        craftworksObject.Unknown14,
                        categories,
                        materials.ToArray()
                    ));
            }

            this.handicrafts = handicrafts.ToArray();
        }

        internal unsafe bool IsSupplyAndDemandAvailable()
        {
            var instance = readerInstance();

            var sheet = Dalamud.GameData.GetExcelSheet<MJICraftworksPopularity>()!;

            for (var i = 1; i <= handicrafts.Length; ++i)
            {
                var supply = *(byte*)(instance + 0x2EA + i);
                var shift = supply & 0x7;
                supply = (byte)(supply >> 4);

                if (shift != 0 || supply != 0)
                {
                    return true;
                }
            }

            return false;
        }

        internal unsafe SupplyAndDemand[] GetSupplyAndDemand()
        {
            var instance = readerInstance();

            if (instance == IntPtr.Zero)
            {
                return Array.Empty<SupplyAndDemand>();
            }

            var supplyAndDemand = new List<SupplyAndDemand>();

            var sheet = Dalamud.GameData.GetExcelSheet<MJICraftworksPopularity>()!;

            var currentPopularity = sheet.GetRow(*(byte*)(instance + 0x2E8))!;
            var nextPopularity = sheet.GetRow(*(byte*)(instance + 0x2E9))!;

            for (var i = 1; i <= handicrafts.Length; ++i)
            {
                var supply = *(byte*)(instance + 0x2EA + i);
                var shift = supply & 0x7;
                supply = (byte)(supply >> 4);

                supplyAndDemand.Add(
                    new SupplyAndDemand
                    {
                        Handicraft = handicrafts[i - 1],
                        Popularity = GetPopularity(currentPopularity, i),
                        Supply = (Supply)supply,
                        DemandShift = (DemandShift)shift,
                        PredictedDemand = GetPopularity(nextPopularity, i),
                    });
            }

            return supplyAndDemand.ToArray();
        }

        private static Popularity GetPopularity(MJICraftworksPopularity pop, int idx)
        {
            var val = (byte?)pop.GetType()
                .GetProperty($"Unknown{idx}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)?
                .GetValue(pop, null);

            if (val != null)
            {
                return (Popularity)val.Value;
            }

            return Popularity.None;
        }
    }
}
