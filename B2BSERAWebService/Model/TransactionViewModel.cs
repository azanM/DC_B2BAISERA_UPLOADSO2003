using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class TransactionViewModel
    {
        public int ID { get; set; }
        public string TicketNo { get; set; }
        public string ClientTag { get; set; }

        public string CreatedWho { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string ChangedWho { get; set; }
        public DateTime ChangedWhen { get; set; }

        public int TransactionDataID { get; set; }
        public string TransGUID { get; set; }
        public string DocumentNumber { get; set; }
        public string FileType { get; set; }
        public string IPAddress { get; set; }
        public string DestinationUser { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
        public int DataLength { get; set; }
        public string RowStatus { get; set; }

        public int TransactionDataDetailID { get; set; }
        public string Data { get; set; }
    }
}