using System;
using System.ComponentModel.DataAnnotations;

namespace StatusPage.Net.Data
{
    public class Ping
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public float ResponseTime { get; set; }
        public int PingSettingId { get; set; }
        public PingSetting PingSetting { get; set; }

        public Ping()
        {
            DateTime = DateTime.UtcNow;
        }
    }
}
