using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class downloadRequest
    {
        public string TicketNo { get; set; }
        public string ClientTag { get; set; }
        public string FileType { get; set; }
        public string SourceUser { get; set; }
        public string Status { get; set; }
        public DateTime TransDateFrom { get; set; }
        public DateTime TransDateTo { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }
}