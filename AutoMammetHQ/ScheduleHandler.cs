using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMammetHQ.Model;

namespace AutoMammetHQ
{
    internal class ScheduleHandler
    {
        private readonly Handicraft[] handicrafts;
        private readonly SupplyAndDemand[] supplyAndDemand;

        private readonly int currentGroove;

        private readonly Dictionary<Supply, decimal> supplyModifiers = new()
        {
            {  Supply.Nonexistent, 1.6M },
            {  Supply.Insufficient, 1.3M },
            {  Supply.Sufficient, 1.0M },
            {  Supply.Surplus, 0.8M },
            {  Supply.Overflowing, 0.6M },
        };

        private readonly Dictionary<Popularity, decimal> popularityModifiers = new()
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

        internal WorkshopSchedules GetSchedules()
        {
            var cycle = GetNextCycle(DateTime.UtcNow);
            var schedules = new List<WorkshopSchedule>();

            if (cycle == 7)
            {
                return new WorkshopSchedules(schedules, cycle, true);
            }


            Parallel.ForEach(handicrafts, handicraft =>
            {
                schedules.AddRange(GetSchedules(new List<Handicraft> { handicraft }));
            });

            return new WorkshopSchedules(schedules, cycle, false);
        }

        private IEnumerable<WorkshopSchedule> GetSchedules(IEnumerable<Handicraft> scheduleHandicrafts)
        {
            var schedules = new List<WorkshopSchedule>();
            var lastHandicraft = scheduleHandicrafts.Last();

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
                    schedules.Add(new WorkshopSchedule(handicraftList, score));
                }
            }

            return schedules;
        }

        private static int GetNextCycle(DateTime time)
        {
            return (int)(time.AddDays(-1).AddHours(-8).DayOfWeek) + 1;
        }

        private decimal ComputeScore(List<Handicraft> handicrafts)
        {
            var usedHandicrafts = new Dictionary<Handicraft, (int Count, SupplyAndDemand SupplyAndDemand)>();

            decimal score = 0;
            var groove = currentGroove;

            for (var i = 0; i < handicrafts.Count; i++)
            {
                SupplyAndDemand supplyDemand;

                if (!usedHandicrafts.ContainsKey(handicrafts[i]))
                {
                    supplyDemand = new SupplyAndDemand(supplyAndDemand.First(x => x.Handicraft == handicrafts[i]));
                    usedHandicrafts.Add(handicrafts[i], (0, supplyDemand));
                }
                else
                {
                    (_, supplyDemand) = usedHandicrafts[handicrafts[i]];
                }

                var efficiencyModifier = i == 0 ? 1 : 2;

                var supplyModifier = supplyDemand.Supply == Supply.Insufficient && supplyDemand.DemandShift == DemandShift.Skyrocketing
                    ? supplyModifiers[Supply.Nonexistent]
                    : supplyModifiers[Supply.Sufficient];
                var demandModifier = popularityModifiers[supplyDemand.Popularity];

                var grooveModifier = 1 + ((decimal)groove / 100);

                score += handicrafts[i].BasePrice * efficiencyModifier * supplyModifier * demandModifier * grooveModifier;

                groove++;

                usedHandicrafts[handicrafts[i]] = (usedHandicrafts[handicrafts[i]].Count + 1, supplyDemand);
    
                if (usedHandicrafts[handicrafts[i]].Count >= 2 && (int)supplyDemand.Supply < 4)
                {
                    supplyDemand.Supply++;
                }
            }

            return score;
        }
    }
}
