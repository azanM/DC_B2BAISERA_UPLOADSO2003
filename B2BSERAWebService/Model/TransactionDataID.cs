using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class TransactionDataID
    {
        public string TransGUID { get; set; }
        public string DocumentNumber { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
        public string TransStatus { get; set; }
        public string LogMessage { get; set; }
    }
}