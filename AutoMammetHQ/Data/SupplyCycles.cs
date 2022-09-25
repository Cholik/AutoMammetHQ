namespace AutoMammetHQ.Data
{
    internal static class SupplyCycles
    {
        internal struct SupplyDemandShift
        {
            public Supply Supply { get; set; }

            public DemandShift DemandShift { get; set; }

            public SupplyDemandShift(Supply supply, DemandShift demandShift)
            {
                Supply = supply;
                DemandShift = demandShift;
            }
        }

        internal struct SupplyDemandShiftCycle
        {
            public SupplyDemandShift[] Weak { get; set; }
            
            public SupplyDemandShift[] Strong { get; set; }
        }

        internal static SupplyDemandShiftCycle[] Cycles = new SupplyDemandShiftCycle[]
        {
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                }
            },
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                }
            },
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                }
            },
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                }
            },
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Decreasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                }
            },
            new SupplyDemandShiftCycle()
            {
                Weak = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Increasing),
                },
                Strong = new SupplyDemandShift[]
                {
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Any),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Plummeting),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.None),
                    new SupplyDemandShift(Supply.Sufficient,   DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Insufficient, DemandShift.Skyrocketing),
                    new SupplyDemandShift(Supply.Nonexistent,  DemandShift.Skyrocketing),
                }
            },
        };
    }
}
