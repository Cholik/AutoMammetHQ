using System.Collections.Generic;

namespace AutoMammetHQ.Model
{
    internal class WorkshopSchedules
    {
        public IEnumerable<WorkshopSchedule> Schedules { get; }

        public int Cycle { get; }

        public bool IsRestDay { get; }

        public WorkshopSchedules(IEnumerable<WorkshopSchedule> schedules, int cycle, bool isRestDay)
        {
            Schedules = schedules;
            Cycle = cycle;
            IsRestDay = isRestDay;
        }
    }
}
