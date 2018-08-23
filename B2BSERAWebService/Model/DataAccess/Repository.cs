using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Data.EntityFramework;
using System.Data.Objects;

namespace B2BSERAWebService.Model.DataAccess
{
    public class Repository : GenericRepository
    {
        public ObjectContext Context { get; set; }

        public Repository(ObjectContext ctx)
            : base(ctx)
        {
            this.Context = ctx;
        }
    }
}