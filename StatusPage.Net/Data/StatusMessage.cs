using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPage.Net.Data
{
    public class StatusMessage
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public StatusMessageType Status { get; set; }
        public int IncidentId { get; set; }
        public Incident Incident { get; set; }
    }

    public enum StatusMessageType
    {
        Okay,
        Down,
        Investigating,
        Identified,
        Monitoring,
        WellShit
    }
}
