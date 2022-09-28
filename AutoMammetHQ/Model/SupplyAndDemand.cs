namespace AutoMammetHQ.Model
{
    internal class SupplyAndDemand
    {
        public Handicraft Handicraft { get; set; }

        public Popularity Popularity { get; set; }

        public Supply Supply { get; set; }

        public DemandShift DemandShift { get; set; }

        public Popularity PredictedDemand { get; set; }
    }
}
