using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StatusPage.Net.Data;

namespace StatusPage.Net.Models.StatusViewModels
{
    public class CreateIncidentViewModel
    {
        [Required]
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        [Required]
        [Display(Name = "Initial Message")]
        public string InitialMessage { get; set; }
        [Required]
        public StatusMessageType InitialMessageType { get; set; } = StatusMessageType.Down;
    }
}
