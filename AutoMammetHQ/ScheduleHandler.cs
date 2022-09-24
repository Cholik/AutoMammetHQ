using System.Collections.Generic;
using System.Linq;
using AutoMammetHQ.Data;

namespace AutoMammetHQ
{
    internal class ScheduleHandler
    {
        private readonly Handicraft[] handicrafts;
        private readonly SupplyAndDemand[] supplyAndDemand;

        private int currentGroove;

        private Dictionary<Supply, decimal> supplyModifiers = new Dictionary<Supply, decimal>()
        {
            {  Supply.Nonexistent, 1.6M },
            {  Supply.Insufficient, 1.3M },
            {  Supply.Sufficient, 1.0M },
            {  Supply.Surplus, 0.8M },
            {  Supply.Overflowing, 0.6M },
        };

        private Dictionary<Popularity, decimal> popularityModifiers = new Dictionary<Popularity, decimal>()
        {
            {  Popularity.VeryHigh, 1.4M },
            {  Popularity.High, 1.2M },
            {  Popularity.Average, 1.0M },
            {  Popularity.Low, 0.8M },
            {  Popularity.None, 0.0M },
        };

        public ScheduleHandler(Handicraft[] handicrafts, SupplyAndDemand[] supplyAndDemand)
        {
            this.handicrafts = handicrafts;
            this.supplyAndDemand = supplyAndDemand;

            currentGroove = 0;
        }

        internal IEnumerable<Schedule> GetSchedules()
        {
            var schedules = new List<Schedule>();

            foreach (var handicraft in handicrafts)
            {
                schedules.AddRange(GetSchedules(new List<Handicraft> { handicraft }));
            }

            return schedules;
        }

        private IEnumerable<Schedule> GetSchedules(IEnumerable<Handicraft> scheduleHandicrafts)
        {
            var schedules = new List<Schedule>();
            var lastHandicraft = scheduleHandicrafts.Last();

            var apa = handicrafts
                .Where(x => x.Id != lastHandicraft.Id &&
                            x.Categories.Intersect(lastHandicraft.Categories).Any())
                .ToList();

            foreach (var nextHandicraft in handicrafts
                .Where(x => x.Id != lastHandicraft.Id &&
                            x.Categories.Intersect(lastHandicraft.Categories).Any()))
            {
                var handicraftList = new List<Handicraft>(scheduleHandicrafts)
                    .Append(nextHandicraft)
                    .ToList();

                var craftingTime = handicraftList.Sum(x => x.CraftingTime);

                if (craftingTime < 24)
                {
                    schedules.AddRange(GetSchedules(handicraftList));
                }
                else if (craftingTime == 24)
                {
                    var score = ComputeScore(handicraftList);
                    schedules.Add(new Schedule(handicraftList, score));
                }
            }

            return schedules;
        }

        private decimal ComputeScore(List<Handicraft> handicrafts)
        {
            decimal score = 0;
            int groove = currentGroove;

            for (int i = 0; i < handicrafts.Count(); i++)
            {
                int efficiencyModifier = i == 0 ? 1 : 2;
                var supplyModifier = supplyModifiers[supplyAndDemand.First(x => x.Handicraft == handicrafts[i]).Supply];
                var demandModifier = popularityModifiers[supplyAndDemand.First(x => x.Handicraft == handicrafts[i]).Popularity];
                decimal grooveModifier = 1 + ((decimal)groove / 100);

                score += handicrafts[i].BasePrice * efficiencyModifier * supplyModifier * demandModifier * grooveModifier;

                groove++;
            }

            return score;
        }
    }
}
