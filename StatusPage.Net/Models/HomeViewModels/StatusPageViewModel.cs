using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPage.Net.Data;

namespace StatusPage.Net.Models.HomeViewModels
{
    public class StatusPageViewModel
    {
        public List<StatusMessage> StatusMessages { get; set; }
        public IEnumerable<IGrouping<PingSetting, Ping>> Pings { get; set; }
    }
}
