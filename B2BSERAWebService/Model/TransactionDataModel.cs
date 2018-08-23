using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class TransactionDataModel
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
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
        public string[] Data { get; set; }

    }
}