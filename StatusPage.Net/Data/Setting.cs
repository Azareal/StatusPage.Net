using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPage.Net.Data
{
    public class Setting
    {
        [Key]
        [StringLength(120)]
        public string Id { get; set; }
        public string Value { get; set; }

        public Setting() { }

        public Setting(string id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
