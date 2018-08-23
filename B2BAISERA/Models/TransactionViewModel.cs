using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class TransactionViewModel
    {
        public virtual int ID { get; set; }

        public virtual string TicketNo { get; set; }

        public virtual string ClientTag { get; set; }

        public string CreatedWho { get; set; }

        public Nullable<System.DateTime> CreatedWhen { get; set; }

        public string ChangedWho { get; set; }

        public Nullable<System.DateTime> ChangedWhen { get; set; }
    }
}
