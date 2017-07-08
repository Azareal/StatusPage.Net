using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPage.Net.Data;

namespace StatusPage.Net.Models.HomeViewModels
{
    public class StatusPageViewModel
    {
        public List<Incident> Incidents { get; set; }
        public IEnumerable<IGrouping<PingSetting, Ping>> Pings { get; set; }
        public List<DailyStatusSummary> DailyStatusSummaries { get; set; }
    }

    public class DailyStatusSummary
    {
        public DateTime Date { get; set; }
        public float HighPingPercentage { get; set; }
        public float DownTimePercentage { get; set; }
    }

    public class IncidentDailySummary
    {
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public TimeSpan DurationTimeSpan => TimeSpan.FromSeconds(Duration);
        public int Count { get; set; }
    }

    public class PingSummary
    {
        public DateTime Date { get; set; }
        public int HighPingCount { get; set; }
        public int Count { get; set; }
    }
}
