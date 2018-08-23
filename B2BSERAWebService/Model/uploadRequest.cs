using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class uploadRequest
    {
        public string TicketNo { get; set; }
        public string ClientTag { get; set; }
        public List<TransactionDataModel> transactionData { get; set; }
    }
}