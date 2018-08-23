using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BSERAWebService.Model
{
    public class loginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientTag { get; set; }
    }
}