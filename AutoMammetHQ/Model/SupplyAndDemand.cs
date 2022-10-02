namespace AutoMammetHQ.Model
{
    internal class SupplyAndDemand
    {
        public Handicraft Handicraft { get; set; } = new Handicraft();

        public Popularity Popularity { get; set; }

        public Supply Supply { get; set; }

        public DemandShift DemandShift { get; set; }

        public Popularity PredictedDemand { get; set; }

        public SupplyAndDemand()
        {
        }

        public SupplyAndDemand(SupplyAndDemand supplyAndDemand)
        {
            Handicraft = supplyAndDemand.Handicraft;
            Popularity = supplyAndDemand.Popularity;
            Supply = supplyAndDemand.Supply;
            DemandShift = supplyAndDemand.DemandShift;
            PredictedDemand = supplyAndDemand.PredictedDemand;
        }
    }
}
