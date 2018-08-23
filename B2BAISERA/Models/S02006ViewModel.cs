using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02006ViewModel
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

        public string BillingNo
        {
            get;
            set;
        }

        public string KuitansiNo
        {
            get;
            set;
        }

        public Nullable<System.DateTime> KuitansiDate
        {
            get;
            set;
        }

        public string CurrencyCode
        {
            get;
            set;
        }

        public Nullable<decimal> AmountKuitansiDC
        {
            get;
            set;
        }

        public string BusinessAreaCode
        {
            get;
            set;
        }

        public string CustomerNo
        {
            get;
            set;
        }

        public string NomorSpes
        {
            get;
            set;
        }

        public string SalesOrderNo
        {
            get;
            set;
        }

        public string SalesmanNumber
        {
            get;
            set;
        }

        public string NomorFakturPajak
        {
            get;
            set;
        }

        public string ChasisNumber
        {
            get;
            set;
        }

        public string PONumberSERA
        {
            get;
            set;
        }

        public Nullable<decimal> VersionPOSERA
        {
            get;
            set;
        }

        public string KuitansiNoRef
        {
            get;
            set;
        }

        public Nullable<System.DateTime> KuitansiDateRef
        {
            get;
            set;
        }
    }
}
