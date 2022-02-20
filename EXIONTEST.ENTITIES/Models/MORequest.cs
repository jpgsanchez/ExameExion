using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXIONTEST.ENTITIES.Models
{
    public class MORequest
    {
        public DateTime timestamp { get; set; }

        public int operation { get; set; }

        public int issuer_id { get; set; }

        public int total_shares { get; set; }

        //public decimal share_price { get; set; }
    }
}