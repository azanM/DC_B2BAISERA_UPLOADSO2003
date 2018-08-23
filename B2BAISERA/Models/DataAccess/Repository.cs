using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace B2BAISERA.Models.DataAccess
{
    public class Repository
    {
        public static string ServerName = "ASUS-PC\\SQLEXPRESS";
        public static string DatabaseName = "B2BAISERA";
        public static string UserDB = "sa";
        public static string PasswordUserDB = "admin123";
                
        public static string ConnectionStringEF = "metadata=res://*/Models.DataAccess.B2BAISERA.csdl|res://*/Models.DataAccess.B2BAISERA.ssdl|res://*/Models.DataAccess.B2BAISERA.msl;provider=System.Data.SqlClient;provider connection string='Data Source="+ ServerName +";Initial Catalog="+ DatabaseName +";Persist Security Info=True;User ID=" + UserDB + ";Password=" + PasswordUserDB + ";MultipleActiveResultSets=True'";
        //public static string ConnectionStringEF2 = ConfigurationSettings.AppSettings["ConnectionString"];

        public static string ServerNameServer = "TRAC58\\SQLEXPRESS";
        public static string DatabaseNameServer = "e-procurement";
        public static string UserDBServer = "sa";
        public static string PasswordUserDBServer = "@dm1n1strator";

        public static string conn = "metadata=res://*/Models.EFServer.EProc.csdl|res://*/Models.EFServer.EProc.ssdl|res://*/Models.EFServer.EProc.msl;provider=System.Data.SqlClient;provider connection string='Data Source=" + ServerNameServer + ";Initial Catalog=" + DatabaseNameServer + ";Persist Security Info=True;User ID=" + UserDBServer + ";Password=" + PasswordUserDBServer + ";MultipleActiveResultSets=True'"; //ConfigurationManager.ConnectionStrings["PROC1"].ConnectionString;
    }
}
