using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02004ViewModel
    {
        public int ID
        {
            get;
            set;
        }
        
        public Nullable<int> TransactionDataID
        {
            get;
            set;
        }

        public Nullable<decimal> VersionPOSERA
        {
            get;
            set;
        }

        public Nullable<decimal> DataVersion
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string ReasonRejection
        {
            get;
            set;
        }
    }
}
