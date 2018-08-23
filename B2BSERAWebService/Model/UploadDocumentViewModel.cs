using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class UploadDocumentViewModel
    {
        public bool Acknowledge { get; set; }
        public string TicketNo { get; set; }
        public string Message { get; set; }
    }
}