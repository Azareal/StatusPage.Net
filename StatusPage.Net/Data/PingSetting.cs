using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPage.Net.Data
{
    public class PingSetting
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public float IntervalSeconds { get; set; }
        [StringLength(255)]
        public string Site { get; set; }
        public bool Visible { get; set; }
        public ICollection<Ping> Pings { get; set; }
    }
}
