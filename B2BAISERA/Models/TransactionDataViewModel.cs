using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class TransactionDataViewModel
    {
        public int ID { get; set; }

        public int TransactionID { get; set; }

        public int AIID { get; set; }

        public string TransGUID { get; set; }

        public string DocumentNumber { get; set; }

        public string FileType { get; set; }

        public string IPAddress { get; set; }

        public string DestinationUser { get; set; }

        public string Key1 { get; set; }

        public string Key2 { get; set; }

        public string Key3 { get; set; }

        public Nullable<int> DataLength { get; set; }

        public string[] Data { get; set; }

        public string TransStatus { get; set; }

        public string LogMessage { get; set; }

        public string CreatedWho { get; set; }

        public Nullable<System.DateTime> CreatedWhen { get; set; }

        public string ChangedWho { get; set; }

        public Nullable<System.DateTime> ChangedWhen { get; set; }
    }
}
