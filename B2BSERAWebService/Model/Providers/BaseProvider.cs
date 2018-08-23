using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace B2BSERAWebService.Model.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseProvider
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        [Dependency]
        public IUnityContainer Container { get; set; }
    }
}