using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02001ISViewModel
    {
        public int ID
        {
            get;
            set;
        }

        public int TransactionDataID
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public Nullable<System.DateTime> PODate
        {
            get;
            set;
        }

        public string AccessoriesNumberAI
        {
            get;
            set;
        }

        public string AccessoriesNumberSERA
        {
            get;
            set;
        }

        public string AccessoriesDescriptionSERA
        {
            get;
            set;
        }

        public Nullable<decimal> QtyAccessories
        {
            get;
            set;
        }
    }
}
