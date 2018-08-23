using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02008ViewModel
    {
        public int ID
        {
            get;
            set;
        }

        public string PONumber
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

        public string SalesOrderNo
        {
            get;
            set;
        }

        public string ChassisNumberByVendor
        {
            get;
            set;
        }

        public Nullable<System.DateTime> DateEntryCarrosserieAccessories
        {
            get;
            set;
        }

        public Nullable<int> TransactionDataID
        {
            get;
            set;
        }
    }
}
