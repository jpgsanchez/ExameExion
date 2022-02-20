using System.Collections.Generic;

namespace EXIONTEST.ENTITIES.Models
{
    public class MOResponse
    {
        public balance current_balance { get; set; }

        //public List<string> business_errors { get; set; }
    }

    public class balance
    {
        public decimal cash { get; set; }

        public List<issuers> issuers { get; set; }
    }

    public class issuers
    {
        public string issuer_name { get; set; }

        public int total_shares { get; set; }

        public decimal share_price { get; set; }

        public string business_errors { get; set; }

        //public List<string> business_errors { get; set; }
    }
}