using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StatusPage.Net.Data;

namespace StatusPage.Net.Models.StatusViewModels
{
    public class CreateStatusMessageViewModel
    {
        [Required]
        public int IncidentId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public StatusMessageType Status { get; set; } = StatusMessageType.Down;
        public DateTime? DateTime { get; set; }
    }
}
