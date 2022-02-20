using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EXIONTEST.ENTITIES.Objects;

namespace EXIONTEST.ENTITIES.Models
{
    public class MCResponse
    {
        public int id { get; set; }

        public decimal cash { get; set; }

        public List<string> issuers { get; set; } = new List<string>();

        public List<string> errors { get; set; } = new List<string>();
    }
}