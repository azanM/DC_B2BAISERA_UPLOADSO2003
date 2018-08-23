using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using B2BAISERA.Models.DataAccess;
using B2BAISERA.Helper;
using System.Data.SqlClient;
using System.Data;
using B2BAISERA.Models.EFServer;
using System.Data.EntityClient;

namespace B2BAISERA.Log
{
    public class LogEvent : DataAccessBase
    {
        public LogEvent()
            : base()
        {
        }

        public LogEvent(EProcEntities context)
            : base(context)
        {
        }

        //B2BAISERAEntities ctx = new B2BAISERAEntities(Repository.ConnectionStringEF);

        //public void WriteDBLog(string WebServiceName, string MethodName, bool Acknowledge, string TicketNo, string Message, string UserName)
        //{
        //    Response response = new Response();
        //    response.WebServiceName = WebServiceName;
        //    response.MethodName = MethodName;
        //    response.Acknowledge = Acknowledge;
        //    response.TicketNo = TicketNo;
        //    response.Message = Message;
        //    EntityHelper.SetAuditForInsert(response, UserName);
        //    ctx.Responses.AddObject(response);
        //    ctx.SaveChanges();
        //}

        public void WriteDBLog(string webServiceName, string methodName, bool acknowledge, string ticketNo, string message, string fileType, string userName)
        {
            CUSTOM_LOG log = new CUSTOM_LOG();
            log.WebServiceName = webServiceName;
            log.MethodName = methodName;
            log.Acknowledge = acknowledge;
            log.TicketNo = ticketNo;
            log.Message = message;
            log.FileType = fileType;
            EntityHelper.SetAuditForInsert(log, userName);
            entities.CUSTOM_LOG.AddObject(log);
            entities.SaveChanges();
        }
    }
}
