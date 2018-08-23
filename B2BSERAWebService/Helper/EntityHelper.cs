using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace B2BSERAWebService.Helper
{
    public static class EntityHelper
    {
        public static void SetAuditForInsert(dynamic entity, string userName)
        {
            entity.ChangedWho = userName;
            entity.ChangedWhen = DateTime.Now;
            entity.CreatedWho = userName;         
            entity.CreatedWhen = DateTime.Now;
        }

        public static void SetAuditForUpdate(dynamic entity, string userName)
        {
            entity.ChangedWho = userName;
            entity.ChangedWhen = DateTime.Now;
        }
    }

    public static class EntityExtension
    {
        public static string ToTraceString<T>(this IQueryable<T> t)
        {

            // try to cast to ObjectQuery<T>

            ObjectQuery<T> oqt = t as ObjectQuery<T>;

            if (oqt != null)

                return oqt.ToTraceString();

            return "";

        }
    }
}
