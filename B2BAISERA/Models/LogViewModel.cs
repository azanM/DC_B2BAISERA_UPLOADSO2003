using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class LogViewModel
    {
        public int ID
        {
            get;
            set;
        }

        public string WebServiceName
        {
            get;
            set;
        }

        public string MethodName
        {
            get;
            set;
        }

        public bool Acknowledge
        {
            get;
            set;
        }

        public string TicketNo
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string CreatedWho
        {
            get;
            set;
        }

        public System.DateTime CreatedWhen
        {
            get;
            set;
        }

        public string ChangedWho
        {
            get;
            set;
        }

        public System.DateTime ChangedWhen
        {
            get;
            set;
        }
    }
}
