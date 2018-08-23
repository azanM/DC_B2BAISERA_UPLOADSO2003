using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA.Models
{
    public class S02003HSViewModel
    {
        public int ID
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public Nullable<int> VersionPOSera
        {
            get;
            set;
        }

        public Nullable<int> DataVersion
        {
            get;
            set;
        }

        public string KodeCabangAI
        {
            get;
            set;
        }

        public string ReasonRejection
        {
            get;
            set;
        }
        public Nullable<int> TransactionDataID
        {
            get;
            set;
        }
        public string CompanyCodeAI
        {
            get;
            set;
        }
    }
}
