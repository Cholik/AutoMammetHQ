using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMammetHQ.Model
{
    internal class WorkshopSchedule : IEquatable<WorkshopSchedule>
    {
        public Handicraft[] Handicrafts { get; set; }

        public decimal Score { get; }

        public WorkshopSchedule(IEnumerable<Handicraft> handicrafts, decimal score)
        {
            Handicrafts = handicrafts.ToArray();
            Score = score;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WorkshopSchedule);
        }

        public bool Equals(WorkshopSchedule? other)
        {
            return other != null &&
                Enumerable.SequenceEqual(
                    Handicrafts.OrderBy(x => x.Name),
                    other.Handicrafts.OrderBy(x => x.Name));
        }

        public override int GetHashCode()
        {
            var hash = 17;

            foreach (var handicraft in Handicrafts.OrderBy(x => x.Name))
            {
                hash = (hash * 23) + handicraft.GetHashCode();
            }

            return hash;
        }
    }
}
