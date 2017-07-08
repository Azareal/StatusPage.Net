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

        public string Color
        {
            get
            {
                switch (Status)
                {
                    case StatusMessageType.Okay:
                        return "text-success";
                    case StatusMessageType.Down:
                        return "text-danger";
                    case StatusMessageType.Investigating:
                        return "text-danger";
                    case StatusMessageType.Identified:
                        return "text-warning";
                    case StatusMessageType.Monitoring:
                        return "text-info";
                    case StatusMessageType.WellShit:
                        return "text-danger";
                }
                return null;
            }
        }

        public string Icon
        {
            get
            {
                switch (Status)
                {
                    case StatusMessageType.Okay:
                        return "fa fa-check";
                    case StatusMessageType.Down:
                        return "fa fa-arrow-down";
                    case StatusMessageType.Investigating:
                        return "fa fa-search";
                    case StatusMessageType.Identified:
                        return "fa fa-crosshair";
                    case StatusMessageType.Monitoring:
                        return "fa fa-chart-bar";
                    case StatusMessageType.WellShit:
                        return "fa fa-poo";
                }
                return null;
            }
        }
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
