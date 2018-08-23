using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Astra.Sms.Hso.Entities;
using System.Data.EntityClient;
//using Astra.Sms.Hso.Common;

namespace B2BAISERA.Models.EFServer
{
    public class DataAccessBase : IDisposable
    {
        protected EProcEntities entities;
        protected EntityConnection entityConnection;
        //protected AppResult Result;
        protected bool IsTransaction = false;

        protected DataAccessBase()
        {
            entities = new EProcEntities();
            entityConnection = (EntityConnection)entities.Connection;
            //Result = new AppResult();
        }

        protected DataAccessBase(EProcEntities context)
        {
            if (entities == null)
                entities = new EProcEntities();
            else
                entities = context;
            entityConnection = (EntityConnection)entities.Connection;
            //Result = new AppResult();
        }

        public void Dispose()
        {
            if (entities != null)
                entities.Dispose();
            if (entityConnection != null)
                entityConnection.Dispose();
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        
        protected void OpenConnection()
        {
            if (entityConnection.State != System.Data.ConnectionState.Open)
                entityConnection.Open();
        }

        protected void CloseConnection()
        {
            if (!IsTransaction)
            {
                if (entityConnection.State != System.Data.ConnectionState.Closed)
                    entityConnection.Close();
            }
        }
    }
}
