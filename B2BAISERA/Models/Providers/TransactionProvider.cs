using System;
using System.Collections.Generic;
using System.Linq;
//using B2BAISERA.Models.DataAccess;
//using B2BAISERA.Helper;
//using B2BAISERA.Logic;
using Microsoft.Practices.Unity;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
using B2BAISERA.Models.EFServer;
using B2BAISERA.Helper;
using System.Data.EntityClient;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using B2BAISERA.Log;

namespace B2BAISERA.Models.Providers
{
    public class TransactionProvider : DataAccessBase
    {
        public TransactionProvider()
            : base()
        {
        }

        public TransactionProvider(EProcEntities context)
            : base(context)
        {
        }

        #region MAIN
        //B2BAISERAEntities ctx = new B2BAISERAEntities(Repository.ConnectionStringEF);

        //public User GetUser(string userName, string password, string clientTag)
        //{
        //    var User = (from o in ctx.Users
        //                where o.UserName == userName && o.Password == password && o.ClientTag == clientTag
        //                select o).FirstOrDefault();

        //    return User;
        //}

        public CUSTOM_USER GetUser(string userName, string password, string clientTag)
        {
            var user = (from o in entities.CUSTOM_USER
                        where o.UserName == userName && o.Password == password && o.ClientTag == clientTag
                        select o).FirstOrDefault();

            return user;
        }

        public string GetLastTicketNo(string fileType)
        {
            var result = "";
            try
            {
                var query = (entities.CUSTOM_LOG
                    .Where(log => (log.Acknowledge == true) && (log.FileType == fileType))
                    .Select(log => new LogViewModel
                    {
                        ID = log.ID,
                        WebServiceName = log.WebServiceName,
                        MethodName = log.MethodName,
                        TicketNo = log.TicketNo,
                        Message = log.Message
                    })
                    ).OrderByDescending(log => log.ID).FirstOrDefault();

                result = query != null ? Convert.ToString(query.TicketNo) : "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region UPLOAD
                
        #region S02003
        public string ConcateStringHSS02003(S02003HSViewModel HS)
        {
            StringBuilder strHS = new StringBuilder(1000);
            strHS.Append("HS|");
            strHS.Append(HS.PONumber);
            strHS.Append("|");
            strHS.Append(HS.VersionPOSera);
            strHS.Append("|");
            strHS.Append(HS.DataVersion);
            strHS.Append("|");
            strHS.Append(HS.KodeCabangAI);
            strHS.Append("|");
            strHS.Append(HS.ReasonRejection);

            return strHS.ToString();
        }

        public string ConcateStringHSS02003(CUSTOM_S02003_TEMP HS)
        {
            StringBuilder strHS = new StringBuilder(1000);
            strHS.Append("HS|");
            strHS.Append(HS.PONumber);
            strHS.Append("|");
            strHS.Append(HS.VersionPOSera);
            strHS.Append("|");
            strHS.Append(HS.DataVersion);
            strHS.Append("|");
            strHS.Append(HS.KodeCabangAI);
            strHS.Append("|");
            strHS.Append(HS.ReasonRejection);

            return strHS.ToString();
        }

        public List<CUSTOM_S02003_TEMP> CancelPOSeraToAI()
        {
            LogEvent logEvent = new LogEvent();
            List<CUSTOM_S02003_TEMP> listTempHS = new List<CUSTOM_S02003_TEMP>();
            try
            {
                listTempHS = entities.sp_CancelPOSeraToAI().ToList();
                return listTempHS;
            }
            catch (Exception ex)
            {
                //add task kill when error 0 : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;               
            }
        }

        public int InsertLogTransactionS02003(List<CUSTOM_S02003_TEMP> listTempHS)
        {
            LogEvent logEvent = new LogEvent();
            int result = 0;
            try
            {
                if (listTempHS.Count > 0)
                {
                    //insert into CUSTOM_TRANSACTION
                    CUSTOM_TRANSACTION transaction = new CUSTOM_TRANSACTION();
                    transaction.TicketNo = "";
                    transaction.ClientTag = "";
                    EntityHelper.SetAuditForInsert(transaction, "SERA");
                    entities.CUSTOM_TRANSACTION.AddObject(transaction);

                    var countListTempHS = listTempHS.Count;
                    //var countListTempIS = listTempIS.Count;

                    for (int i = 0; i < listTempHS.Count; i++)
                    {
                        //insert into CUSTOM_TRANSACTIONDATA
                        CUSTOM_TRANSACTIONDATA transactionData = new CUSTOM_TRANSACTIONDATA();
                        transactionData.CUSTOM_TRANSACTION = transaction;
                        transactionData.TransGUID = Guid.NewGuid().ToString();
                        transactionData.DocumentNumber = listTempHS[i].PONumber;
                        transactionData.FileType = "S02003";
                        transactionData.IPAddress = "118.97.80.12"; //"127.0.0.1";
                        transactionData.DestinationUser = "AI";
                        //transactionData.Key1 = "0002"; //company toyota
                        transactionData.Key1 = listTempHS[i].CompanyCodeAI;
                        transactionData.Key2 = listTempHS[i].KodeCabangAI; 
                        transactionData.Key3 = "";
                        transactionData.DataLength = null;
                        transactionData.RowStatus = "";
                        EntityHelper.SetAuditForInsert(transactionData, "SERA");
                        entities.CUSTOM_TRANSACTIONDATA.AddObject(transactionData);

                        //CHECK IF DATA HS BY PONUMBER SUDAH ADA, DELETE DULU BY ID, supaya tidak redundant ponumber nya
                        var poNumb = listTempHS[i].PONumber;
                        var query = (from o in entities.CUSTOM_S02003
                                     where o.PONumber == poNumb
                                     select o).ToList();
                        if (query.Count > 0)
                        {
                            for (int d = 0; d < query.Count; d++)
                            {
                                //delete
                                var delID = query[d].ID;
                                CUSTOM_S02003 delHS = entities.CUSTOM_S02003.Single(o => o.ID == delID);
                                entities.CUSTOM_S02003.DeleteObject(delHS);
                            }
                        }

                        //insert into CUSTOM_S02003
                        CUSTOM_S02003 DataDetailHS = new CUSTOM_S02003();
                        DataDetailHS.CUSTOM_TRANSACTIONDATA = transactionData;
                        DataDetailHS.PONumber = listTempHS[i].PONumber;
                        DataDetailHS.VersionPOSera = listTempHS[i].VersionPOSera;
                        DataDetailHS.DataVersion = listTempHS[i].DataVersion;
                        DataDetailHS.KodeCabangAI = listTempHS[i].KodeCabangAI;
                        DataDetailHS.ReasonRejection = listTempHS[i].ReasonRejection;
                        //start add identitas penambahan row custom_s02003 : by fhi 04.06.2014
                        DataDetailHS.dibuatOleh = "system";
                        DataDetailHS.dibuatTanggal = DateTime.Now;
                        DataDetailHS.diubahOleh = "system";
                        DataDetailHS.diubahTanggal = DateTime.Now;
                        //end
                        entities.CUSTOM_S02003.AddObject(DataDetailHS);

                        //build HS separator
                        var strHS = ConcateStringHSS02003(listTempHS[i]);

                        //insert into CUSTOM_TRANSACTIONDATADETAIL for HS
                        CUSTOM_TRANSACTIONDATADETAIL transactionDataDetail = new CUSTOM_TRANSACTIONDATADETAIL();
                        transactionDataDetail.CUSTOM_TRANSACTIONDATA = transactionData;
                        transactionDataDetail.Data = strHS;

                        //start add identitas penambahan row CUSTOM_TRANSACTIONDATADETAIL : by fhi 04.06.2014
                        transactionDataDetail.dibuatOleh = "system";
                        transactionDataDetail.dibuatTanggal = DateTime.Now;
                        transactionDataDetail.diubahOleh = "system";
                        transactionDataDetail.diubahTanggal = DateTime.Now;
                        //end

                        entities.CUSTOM_TRANSACTIONDATADETAIL.AddObject(transactionDataDetail);
                    }
                    entities.SaveChanges();
                    //todo: delete temp table
                    for (int y = 0; y < countListTempHS; y++)
                    {
                        DeleteTempHSS02003(listTempHS[y]);
                    }
                    result = 1;
                }
                else
                {
                    //add task kill when failed : by  fhi 04.06.2014
                    logEvent.WriteDBLog("", "UploadS02003_Load", false, "", "listTempHS.Count <= 0", "S02003", "SERA");
                    Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                    //end
                }
            }
            catch (Exception ex)
            {
                //add task kill when failed : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;
            }
            return result;
        }

        public TransactionViewModel GetTransactionS02003()
        {
            LogEvent logEvent = new LogEvent();
            TransactionViewModel transaction = null;
            try
            {
                DateTime dateNow = DateTime.Now.Date;
                transaction = (from h in entities.CUSTOM_TRANSACTION
                               join d in entities.CUSTOM_TRANSACTIONDATA
                               on h.ID equals d.TransactionID
                               where d.FileType == "S02003" && h.CreatedWhen >= dateNow
                               select new TransactionViewModel()
                               {
                                   ID = h.ID
                               }).OrderByDescending(z => z.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //add task kill when failed : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;
            }
            return transaction;
        }

        public WsProd.TransactionData[] GetTransactionDataArray(int? transactionID)
        {
            LogEvent logEvent = new LogEvent();
            WsProd.TransactionData[] transactionData = null;
            try
            {
                transactionData = (from o in entities.CUSTOM_TRANSACTIONDATA
                                   //join p in entities.CUSTOM_S02001_HS
                                   //on o.ID equals p.TransactionDataID
                                   //join q in entities.CUSTOM_S02001_IS
                                   //on o.ID equals q.TransactionDataID
                                   //join s in entities.CUSTOM_TRANSACTIONDATADETAIL
                                   //on o.ID equals s.TransactionDataID
                                   where o.TransactionID == transactionID
                                   select new WsProd.TransactionData
                                   {
                                       ID = o.ID,
                                       TransGUID = o.TransGUID,
                                       DocumentNumber = o.DocumentNumber,
                                       FileType = o.FileType,
                                       IPAddress = o.IPAddress,
                                       DestinationUser = o.DestinationUser,
                                       Key1 = o.Key1,
                                       Key2 = o.Key2,
                                       Key3 = o.Key3,
                                       //DataLength = 
                                       //Data = ConcateStringHS()//new ArrayOfString { s.Data, "","" }
                                   }).ToArray();
            }
            catch (Exception ex)
            {
                //add task kill when failed : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;
            }
            return transactionData;
        }

        public List<S02003HSViewModel> GetTransactionDataDetailHSS02003(int? transactionDataID)
        {
            LogEvent logEvent = new LogEvent();
            List<S02003HSViewModel> dataHS = null;
            try
            {
                dataHS = (entities.CUSTOM_S02003
                          .Where(o => o.TransactionDataID == transactionDataID)
                          .Select(o => new S02003HSViewModel
                          {
                              ID = o.ID,
                              TransactionDataID = o.TransactionDataID == null ? null : o.TransactionDataID,
                              PONumber = o.PONumber,
                              VersionPOSera = o.VersionPOSera,
                              DataVersion = o.DataVersion,
                              KodeCabangAI = o.KodeCabangAI,
                              ReasonRejection = o.ReasonRejection
                          }).ToList());
            }
            catch (Exception ex)
            {
                //add task kill when failed : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;
            }
            return dataHS;
        }

        public int DeleteTempHSS02003(CUSTOM_S02003_TEMP tempHS)
        {
            int result = 1;
            try
            {
                EntityCommand cmd = new EntityCommand("EProcEntities.sp_DeleteTempHSS02003", entityConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PONUMBER", DbType.String).Value = tempHS.PONumber;
                OpenConnection();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                result = 0;

                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }
        #endregion

        #endregion
        
        public List<CUSTOM_S02003_TEMP> CheckingHistory(List<CUSTOM_S02003_TEMP> tempHS)
        {
            List<CUSTOM_S02003_TEMP> listDataHS = new List<CUSTOM_S02003_TEMP>();
            try
            {
                if (tempHS.Count > 0)
                {
                    //select data HS yang tidak ada di history
                    var query = (from o in tempHS
                                 where !entities.CUSTOM_S02003.Any(e => o.PONumber == e.PONumber)
                                 select o).ToList();
                    listDataHS = query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDataHS;
        }

        public List<CUSTOM_S02003_TEMP> CheckingHistoryHS(List<CUSTOM_S02003_TEMP> tempHS)
        {
            LogEvent logEvent = new LogEvent();
            List<CUSTOM_S02003_TEMP> listDataHS = new List<CUSTOM_S02003_TEMP>();
            try
            {
                if (tempHS.Count > 0)
                {
                    listDataHS = tempHS;
                    var existingRow = (from o in tempHS
                                       where entities.CUSTOM_S02003.Any(e => o.PONumber == e.PONumber)
                                       select new S02003HSViewModel
                                       {
                                           PONumber = o.PONumber,
                                           VersionPOSera = o.VersionPOSera,
                                           DataVersion = o.DataVersion,
                                           KodeCabangAI = o.KodeCabangAI,
                                           ReasonRejection = o.ReasonRejection,
                                           CompanyCodeAI = o.CompanyCodeAI
                                       }).ToList();

                    for (int i = 0; i < existingRow.Count; i++)
                    {
                        string existPoNumber = existingRow[i].PONumber;
                        var q = (from o in entities.CUSTOM_S02003
                                 where o.PONumber == existPoNumber
                                 select new S02003HSViewModel
                                 {
                                     PONumber = o.PONumber,
                                     VersionPOSera = o.VersionPOSera,
                                     DataVersion = o.DataVersion,
                                     KodeCabangAI = o.KodeCabangAI,
                                     ReasonRejection = o.ReasonRejection
                                 }).SingleOrDefault();
                        var compare = EqualS02003HS(existingRow[i], q);

                        //jika ada yg beda di salah satu field atau lebih, maka update list 
                        if (!compare)
                        {
                            //removeat(index)
                            var rowDel = (from o in listDataHS
                                          where o.PONumber == existingRow[i].PONumber
                                          select o).SingleOrDefault();
                            listDataHS.Remove(rowDel);

                            CUSTOM_S02003_TEMP row = new CUSTOM_S02003_TEMP();
                            row.PONumber = existingRow[i].PONumber;
                            row.VersionPOSera = q.VersionPOSera != null ? q.VersionPOSera + 1 : existingRow[i].VersionPOSera != null ? existingRow[i].VersionPOSera : 1;
                            row.DataVersion = q.DataVersion != null ? q.DataVersion + 1 : existingRow[i].DataVersion != null ? existingRow[i].DataVersion : 1;
                            row.KodeCabangAI = existingRow[i].KodeCabangAI;
                            row.ReasonRejection = existingRow[i].ReasonRejection;
                            row.CompanyCodeAI = existingRow[i].CompanyCodeAI;
                            listDataHS.Add(row);
                        }
                        else
                        {
                            //removeat(index)
                            var rowDel = (from o in listDataHS
                                          where o.PONumber == existingRow[i].PONumber
                                          select o).SingleOrDefault();
                            listDataHS.Remove(rowDel);
                        }
                    }
                }
                //add task kill when tempHS.Count <= 0 : by  fhi 04.06.2014
                else
                {
                    logEvent.WriteDBLog("", "UploadS02003_Load", false, "", "tempHS.Count <= 0", "S02003", "SERA");
                    Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                }
                //end
            }
            catch (Exception ex)
            {
                //add task kill when error : by  fhi 04.06.2014
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, "S02003", "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
                throw ex;
            }
            return listDataHS;
        }

        public bool EqualS02003HS(S02003HSViewModel item1, S02003HSViewModel item2)
        {
            //jika value sama return true, jika value beda return false
            if (item1 == null && item2 == null)
                return true;
            else if ((item1 != null && item2 == null) || (item1 == null && item2 != null))
                return false;

            var PONumber1 = !string.IsNullOrEmpty(item1.PONumber) ? item1.PONumber : "";
            var KodeCabangAI1 = !string.IsNullOrEmpty(item1.KodeCabangAI) ? item1.KodeCabangAI : "";
            var ReasonRejection1 = !string.IsNullOrEmpty(item1.ReasonRejection) ? item1.ReasonRejection : "";


            var PONumber2 = !string.IsNullOrEmpty(item2.PONumber) ? item2.PONumber : "";
            var KodeCabangAI2 = !string.IsNullOrEmpty(item2.KodeCabangAI) ? item2.KodeCabangAI : "";
            var ReasonRejection2 = !string.IsNullOrEmpty(item2.ReasonRejection) ? item2.ReasonRejection : "";

            return PONumber1.Equals(PONumber2) &&
                KodeCabangAI1.Equals(KodeCabangAI2) &&
                ReasonRejection1.Equals(ReasonRejection2) ;
        }

        public int DeleteAllTempHSS02003()
        {
            int result = 1;
            try
            {
                EntityCommand cmd = new EntityCommand("EProcEntities.sp_DeleteAllTempHSS02003", entityConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                OpenConnection();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                result = 0;

                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }
    }
}