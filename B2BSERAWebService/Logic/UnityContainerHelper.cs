using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace B2BSERAWebService.Logic
{
    public class UnityContainerHelper
    {
        public static IUnityContainer Container
        {
            get
            {
                return HttpContext.Current.Application["container"] as UnityContainer;
            }
            set
            {
                HttpContext.Current.Application["container"] = value;
            }
        }
    }
}