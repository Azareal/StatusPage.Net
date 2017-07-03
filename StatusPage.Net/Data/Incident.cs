using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPage.Net.Data
{
    public class Incident
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StatusMessage> Messages { get; set; }
        public int? SiteId { get; set; }
        public Site Site { get; set; }
    }
}
