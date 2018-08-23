using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using B2BSERAWebService.Model;
using Infrastructure.Data;
using B2BSERAWebService.Model.DataAccess;
using B2BSERAWebService.Model.Providers;
using B2BSERAWebService.Helper;

namespace B2BSERAWebService.Logic
{
    public class LogEvent : BaseProvider
    {
        private Repository repository;

        public LogEvent(Repository repository)
        {
            this.repository = repository;
        }

        public LogEvent()
        {
            // TODO: Complete member initialization
        }

        #region OleDB
        
        //string ConnectionString = ConfigurationManager.AppSettings["ConnStringB2BAISERA"];

        //public void WriteDBLog(string WebServiceName, string MethodName, bool Acknowledge, string TicketNo, string Message, string UserName)
        //{
        //    OleDbConnection DbConnection = new OleDbConnection(ConnectionString);
        //    try
        //    {
        //        OleDbCommand DBCommand;
        //        DbConnection.Open();
        //        string strQuery = null;
        //        strQuery = "INSERT INTO Response ";
        //        strQuery = strQuery + "(WebServiceName, MethodName, Acknowledge, TicketNo, Message, CreatedWho, CreatedWhen, ChangedWho, ChangedWhen)";
        //        strQuery = strQuery + "Values ( getdate() ,'" + MethodName + "','" + Acknowledge + "','" + TicketNo + "','" + Message + "','" + UserName + "', getdate() ,'" + UserName + "', getdate() )";
        //        DBCommand = new OleDbCommand(strQuery, DbConnection);
        //        DBCommand.CommandType = CommandType.Text;
        //        DBCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        DbConnection.Close();
        //    }
        //}
        #endregion

        public void WriteDBLog(string WebServiceName, string MethodName, bool Acknowledge, string TicketNo, string Message, string UserName)
        {
            Response response = new Response();
            response.WebServiceName = WebServiceName;
            response.MethodName = MethodName;
            response.Acknowledge = Acknowledge;
            response.TicketNo = TicketNo;
            response.Message = Message;
            EntityHelper.SetAuditForInsert(response, UserName);
            repository.Add(response);
            repository.UnitOfWork.SaveChanges();
        }
    }
}