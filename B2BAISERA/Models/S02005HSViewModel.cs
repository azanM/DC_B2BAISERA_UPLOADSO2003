using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02005HSViewModel
    {
        public int ID
        {
            get;
            set;
        }

        public string KodeCabangAI
        {
            get;
            set;
        }

        public string CashReceiptNumber
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public Nullable<double> AmountFromLC
        {
            get;
            set;
        }

        public Nullable<System.DateTime> TanggalTerimaTagihan
        {
            get;
            set;
        }

        public Nullable<System.DateTime> TanggalValidasi
        {
            get;
            set;
        }

        public Nullable<double> CodeGroup
        {
            get;
            set;
        }

        public string EquipmentNumber
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
