using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using System.Text;

namespace B2BAISERA.Models.Providers
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