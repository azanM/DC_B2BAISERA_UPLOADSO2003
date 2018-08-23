using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.Practices.Unity;
using B2BSERAWebService.Helper;
using B2BSERAWebService.Model.DataAccess;
using B2BSERAWebService.Logic;
using B2BSERAWebService.Model.Providers;
using System.Data.Objects;

namespace B2BSERAWebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigureUnity();
        }

        void ConfigureUnity()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<Repository>(
                new HttpContextLifetimeManager<Repository>());

            container.RegisterType<LogEvent>(
                new HttpContextLifetimeManager<LogEvent>(),
                new InjectionProperty("Container", container));

            container.RegisterType<TransactionProvider>(
            new HttpContextLifetimeManager<TransactionProvider>(),
            new InjectionProperty("Container", container));

            container.RegisterType<ObjectContext, B2BAISERAEntities>(
                new HttpContextLifetimeManager<B2BAISERAEntities>(),
                new InjectionConstructor("name=B2BAISERAEntities"));


            UnityContainerHelper.Container = container;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}