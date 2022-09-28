using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMammetHQ.Model
{
    internal class Schedule : IEquatable<Schedule>
    {
        public Handicraft[] Handicrafts { get; set; }

        public decimal Score { get; }

        public Schedule(IEnumerable<Handicraft> handicrafts, decimal score)
        {
            Handicrafts = handicrafts.ToArray();
            Score = score;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Schedule);
        }

        public bool Equals(Schedule? other)
        {
            return other != null &&
                Enumerable.SequenceEqual(
                    Handicrafts.OrderBy(x => x.Name),
                    other.Handicrafts.OrderBy(x => x.Name));
        }

        public override int GetHashCode()
        {
            int hash = 17;

            foreach (var handicraft in Handicrafts.OrderBy(x => x.Name))
            {
                hash = hash * 23 + handicraft.GetHashCode();
            }

            return hash;
        }
    }
}
