using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using B2BSERAWebService.Model.DataAccess;
using B2BSERAWebService.Helper;
using B2BSERAWebService.Logic;
using Microsoft.Practices.Unity;

namespace B2BSERAWebService.Model.Providers
{
    public class TransactionProvider : BaseProvider
    {
        private Repository repository;

        public TransactionProvider(Repository repository)
        {
            this.repository = repository;
        }

        public void InsertTransaction(B2BSERAWebService.Model.uploadRequest uploadRequest)
        {
            try
            {
                Transaction transaction = new Transaction();
                transaction.TicketNo = uploadRequest.TicketNo;
                transaction.ClientTag = uploadRequest.ClientTag;
                EntityHelper.SetAuditForInsert(transaction, "SERA");
                repository.Add(transaction);

                for (int i = 0; i < uploadRequest.transactionData.Count(); i++)
                {
                    TransactionData transactionData = new TransactionData();
                    transactionData.Transaction = transaction;
                    transactionData.TransGUID = uploadRequest.transactionData[i].TransGUID;
                    transactionData.DocumentNumber = uploadRequest.transactionData[i].DocumentNumber;
                    transactionData.FileType = uploadRequest.transactionData[i].FileType;
                    transactionData.IPAddress = uploadRequest.transactionData[i].IPAddress;
                    transactionData.DestinationUser = uploadRequest.transactionData[i].DestinationUser;
                    transactionData.Key1 = uploadRequest.transactionData[i].Key1;
                    transactionData.Key2 = uploadRequest.transactionData[i].Key2;
                    transactionData.Key3 = uploadRequest.transactionData[i].Key3;
                    transactionData.DataLength = uploadRequest.transactionData[i].DataLength;
                    transactionData.RowStatus = string.IsNullOrEmpty(uploadRequest.transactionData[i].RowStatus) ? "Inprogress" : uploadRequest.transactionData[i].RowStatus;
                    EntityHelper.SetAuditForInsert(transactionData, "SERA");
                    repository.Add(transactionData);

                    for (int j = 0; j < uploadRequest.transactionData[i].DataLength; j++)
                    {
                        TransactionDataDetail transactionDataDetail = new TransactionDataDetail();
                        transactionDataDetail.TransactionData = transactionData;
                        //TODO 10-09-2013: { Data = HS|ponumber||||||||}
                        transactionDataDetail.Data = "{ Data = " + uploadRequest.transactionData[i].Data[j] + "}"; //uploadRequest.transactionData[i].Data[j];
                        repository.Add(transactionDataDetail);
                    }
                }
                repository.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFileTypeName(string fileType)
        {
            var documentFileType = repository.Single<DocumentFileType>(o => o.FileTypeName == fileType).FileTypeName;
            return documentFileType != null ? documentFileType : "";
        }

        public string GetIPAddress(string ipAddress)
        {
            var documentIPAddress = repository.Single<DocumentIPAddress>(o => o.IPAddress == ipAddress).IPAddress;
            return documentIPAddress != null ? documentIPAddress : "";
        }

        public string GetUser(string userName)
        {
            var user = repository.Single<User>(o => o.UserName == userName).UserName;
            return user != null ? user : "";
        }

        public User GetUser(string userName, string password, string clientTag)
        {
            var User = (from o in repository.GetQuery<User>()
                     where o.UserName == userName && o.Password == password && o.ClientTag == clientTag
                     select o).FirstOrDefault();

            return User;
            //return repository.Single<User>(o => o.UserName == user && o.Password == password && o.ClientTag == clientTag).UserName;
        }

        public bool ValidateFileTypeIPAdressDetinationUser(B2BSERAWebService.Model.uploadRequest uploadRequest)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i < uploadRequest.transactionData.Count(); i++)
                {
                    B2BAISERAEntities db = new B2BAISERAEntities();

                    var fileType = (from o in db.DocumentFileTypes
                                    where o.FileTypeName == uploadRequest.transactionData[i].FileType
                                    select o.FileTypeName).FirstOrDefault();

                    var ipAddress = (from o in db.DocumentIPAddresses
                                     where o.IPAddress == uploadRequest.transactionData[i].IPAddress
                                     select o.IPAddress).FirstOrDefault();

                    var destinationUser = (from o in db.Users
                                           where o.UserName == uploadRequest.transactionData[i].DestinationUser
                                           select o.UserName).FirstOrDefault();

                    if (string.IsNullOrEmpty(fileType) || string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(destinationUser))
                    {
                        flag = false;
                        break;
                    }
                    else flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region test download
        //public List<TransactionViewModel> DownloadDocument(B2BSERAWebService.Model.downloadRequest downloadRequest)
        //{
        //    //just info :
        //    //1.all has value
        //    //2.jika filetype kosong, maka pilih semua yg destination user == user login
        //    //3.jika sourceuser kosong, maka pilih semua yg destination user == user login
        //    //4.jika status kosong, maka pilih yg status = new
        //    //5.jika TransDateFrom dan TransDateTo diisi, maka range date. //jika salah satu diisi, maka equalnya.
        //    //end just info :

        //    //1.all has value
        //    List<TransactionViewModel> query = null;
        //    if (!string.IsNullOrEmpty(downloadRequest.FileType)
        //        && !string.IsNullOrEmpty(downloadRequest.SourceUser)
        //        && !string.IsNullOrEmpty(downloadRequest.Status)
        //        && downloadRequest.TransDateFrom != null
        //        && downloadRequest.TransDateTo != null)
        //    {
        //        query = (from o in repository.GetQuery<Transaction>()

        //                 join p in repository.GetQuery<TransactionData>()
        //                 on o.ID equals p.TransactionID

        //                 join q in repository.GetQuery<TransactionDataDetail>()
        //                 on p.ID equals q.TransactionDataID

        //                 where p.FileType == downloadRequest.FileType
        //                 && p.DestinationUser == downloadRequest.SourceUser
        //                 && p.RowStatus == downloadRequest.Status
        //                 && o.CreatedWhen >= downloadRequest.TransDateFrom
        //                 && o.CreatedWhen <= downloadRequest.TransDateTo

        //                 select new TransactionViewModel()
        //                 {
        //                     ID = o.ID,
        //                     TicketNo = o.TicketNo,
        //                     ClientTag = o.ClientTag,
        //                     CreatedWho = o.CreatedWho,
        //                     CreatedWhen = o.CreatedWhen,
        //                     ChangedWho = o.ChangedWho,
        //                     ChangedWhen = o.ChangedWhen,

        //                     TransactionDataID = p.ID,
        //                     TransGUID = p.TransGUID,
        //                     DocumentNumber = p.DocumentNumber,
        //                     FileType = p.FileType,
        //                     IPAddress = p.IPAddress,
        //                     DestinationUser = p.DestinationUser,
        //                     Key1 = p.Key1,
        //                     Key2 = p.Key2,
        //                     Key3 = p.Key3,
        //                     DataLength = p.DataLength,
        //                     RowStatus = p.RowStatus,

        //                     TransactionDataDetailID = q.ID,
        //                     Data = q.Data
        //                 }).ToList();
        //    }

        //    //2.jika filetype kosong, maka pilih semua yg destination user == user login
        //    //3.jika sourceuser kosong, maka pilih semua yg destination user == user login
        //    else if ((string.IsNullOrEmpty(downloadRequest.FileType) || string.IsNullOrEmpty(downloadRequest.SourceUser))
        //        && !string.IsNullOrEmpty(downloadRequest.Status)
        //        && downloadRequest.TransDateFrom != null
        //        && downloadRequest.TransDateTo != null)
        //    {
        //        query = (from o in repository.GetQuery<Transaction>()

        //                 join p in repository.GetQuery<TransactionData>()
        //                 on o.ID equals p.TransactionID

        //                 join q in repository.GetQuery<TransactionDataDetail>()
        //                 on p.ID equals q.TransactionDataID

        //                 where p.DestinationUser == downloadRequest.SourceUser //user login 
        //                 && p.RowStatus == downloadRequest.Status
        //                 && o.CreatedWhen >= downloadRequest.TransDateFrom
        //                 && o.CreatedWhen <= downloadRequest.TransDateTo

        //                 select new TransactionViewModel()
        //                 {
        //                     ID = o.ID,
        //                     TicketNo = o.TicketNo,
        //                     ClientTag = o.ClientTag,
        //                     CreatedWho = o.CreatedWho,
        //                     CreatedWhen = o.CreatedWhen,
        //                     ChangedWho = o.ChangedWho,
        //                     ChangedWhen = o.ChangedWhen,

        //                     TransactionDataID = p.ID,
        //                     TransGUID = p.TransGUID,
        //                     DocumentNumber = p.DocumentNumber,
        //                     FileType = p.FileType,
        //                     IPAddress = p.IPAddress,
        //                     DestinationUser = p.DestinationUser,
        //                     Key1 = p.Key1,
        //                     Key2 = p.Key2,
        //                     Key3 = p.Key3,
        //                     DataLength = p.DataLength,
        //                     RowStatus = p.RowStatus,

        //                     TransactionDataDetailID = q.ID,
        //                     Data = q.Data
        //                 }).ToList();
        //    }


        //    //4.jika status kosong, maka pilih yg status = new
        //    else if (!string.IsNullOrEmpty(downloadRequest.FileType)
        //        && !string.IsNullOrEmpty(downloadRequest.SourceUser)
        //        && string.IsNullOrEmpty(downloadRequest.Status)
        //        && downloadRequest.TransDateFrom != null
        //        && downloadRequest.TransDateTo != null)
        //    {
        //        query = (from o in repository.GetQuery<Transaction>()

        //                 join p in repository.GetQuery<TransactionData>()
        //                 on o.ID equals p.TransactionID

        //                 join q in repository.GetQuery<TransactionDataDetail>()
        //                 on p.ID equals q.TransactionDataID

        //                 where p.FileType == downloadRequest.FileType
        //                 && p.DestinationUser == downloadRequest.SourceUser
        //                 && p.RowStatus == "New"
        //                 && o.CreatedWhen >= downloadRequest.TransDateFrom
        //                 && o.CreatedWhen <= downloadRequest.TransDateTo

        //                 select new TransactionViewModel()
        //                 {
        //                     ID = o.ID,
        //                     TicketNo = o.TicketNo,
        //                     ClientTag = o.ClientTag,
        //                     CreatedWho = o.CreatedWho,
        //                     CreatedWhen = o.CreatedWhen,
        //                     ChangedWho = o.ChangedWho,
        //                     ChangedWhen = o.ChangedWhen,

        //                     TransactionDataID = p.ID,
        //                     TransGUID = p.TransGUID,
        //                     DocumentNumber = p.DocumentNumber,
        //                     FileType = p.FileType,
        //                     IPAddress = p.IPAddress,
        //                     DestinationUser = p.DestinationUser,
        //                     Key1 = p.Key1,
        //                     Key2 = p.Key2,
        //                     Key3 = p.Key3,
        //                     DataLength = p.DataLength,
        //                     RowStatus = p.RowStatus,

        //                     TransactionDataDetailID = q.ID,
        //                     Data = q.Data
        //                 }).ToList();
        //    }

        //    return query;
        //}
        #endregion
        
        public IEnumerable<TransactionViewModel> SearchDocument(B2BSERAWebService.Model.downloadRequest downloadRequest)
        {
            var query = (from o in repository.GetQuery<Transaction>()

                         join p in repository.GetQuery<TransactionData>()
                         on o.ID equals p.TransactionID

                         join q in repository.GetQuery<TransactionDataDetail>()
                         on p.ID equals q.TransactionDataID

                         select new TransactionViewModel()
                         {
                             ID = o.ID,
                             TicketNo = o.TicketNo,
                             ClientTag = o.ClientTag,
                             CreatedWho = o.CreatedWho,
                             CreatedWhen = o.CreatedWhen,
                             ChangedWho = o.ChangedWho,
                             ChangedWhen = o.ChangedWhen,

                             TransactionDataID = p.ID,
                             TransGUID = p.TransGUID,
                             DocumentNumber = p.DocumentNumber,
                             FileType = p.FileType,
                             IPAddress = p.IPAddress,
                             DestinationUser = p.DestinationUser,
                             Key1 = p.Key1,
                             Key2 = p.Key2,
                             Key3 = p.Key3,
                             DataLength = p.DataLength,
                             RowStatus = p.RowStatus,

                             TransactionDataDetailID = q.ID,
                             Data = q.Data
                         });

            //1.all has value
            if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType 
                    && o.DestinationUser == downloadRequest.SourceUser 
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //2.jika filetype kosong, maka pilih semua yg destination user == user login
            //3.jika sourceuser kosong, maka pilih semua yg destination user == user login
            else if ((string.IsNullOrEmpty(downloadRequest.FileType) || string.IsNullOrEmpty(downloadRequest.SourceUser))
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.DestinationUser == downloadRequest.SourceUser // user login
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //4.jika status kosong, maka pilih yg status = New
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == "New"
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //5.a. jika TransDateFrom dan TransDateTo diisi, maka range date. //jika salah satu diisi, maka equalnya. TransDateFrom isi
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo == null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen == downloadRequest.TransDateFrom);
            }

            //5.b. jika TransDateFrom dan TransDateTo diisi, maka range date. //jika salah satu diisi, maka equalnya. TransDateTo isi
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom == null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen == downloadRequest.TransDateTo);
            }
            return query;
        }

        public IEnumerable<TransactionViewModel> SearchDocumentTransactionData(B2BSERAWebService.Model.downloadRequest downloadRequest)
        {
            var query = (from o in repository.GetQuery<Transaction>()

                         join p in repository.GetQuery<TransactionData>()
                         on o.ID equals p.TransactionID

                         select new TransactionViewModel()
                         {
                             ID = o.ID,
                             TicketNo = o.TicketNo,
                             ClientTag = o.ClientTag,
                             CreatedWho = o.CreatedWho,
                             CreatedWhen = o.CreatedWhen,
                             ChangedWho = o.ChangedWho,
                             ChangedWhen = o.ChangedWhen,

                             TransactionDataID = p.ID,
                             TransGUID = p.TransGUID,
                             DocumentNumber = p.DocumentNumber,
                             FileType = p.FileType,
                             IPAddress = p.IPAddress,
                             DestinationUser = p.DestinationUser,
                             Key1 = p.Key1,
                             Key2 = p.Key2,
                             Key3 = p.Key3,
                             DataLength = p.DataLength,
                             RowStatus = p.RowStatus
                         });

            //1.all has value
            if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //2.jika filetype kosong, maka pilih semua yg destination user == user login
            //3.jika sourceuser kosong, maka pilih semua yg destination user == user login
            else if ((string.IsNullOrEmpty(downloadRequest.FileType) || string.IsNullOrEmpty(downloadRequest.SourceUser))
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.DestinationUser == downloadRequest.SourceUser // user login
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //4.jika status kosong, maka pilih yg status = New
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == "New"
                    && o.CreatedWhen >= downloadRequest.TransDateFrom
                    && o.CreatedWhen <= downloadRequest.TransDateTo);
            }

            //5.a. jika TransDateFrom dan TransDateTo diisi, maka range date. //jika salah satu diisi, maka equalnya. TransDateFrom isi
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom != null
                && downloadRequest.TransDateTo == null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen == downloadRequest.TransDateFrom);
            }

            //5.b. jika TransDateFrom dan TransDateTo diisi, maka range date. //jika salah satu diisi, maka equalnya. TransDateTo isi
            else if (!string.IsNullOrEmpty(downloadRequest.FileType)
                && !string.IsNullOrEmpty(downloadRequest.SourceUser)
                && !string.IsNullOrEmpty(downloadRequest.Status)
                && downloadRequest.TransDateFrom == null
                && downloadRequest.TransDateTo != null)
            {
                query = query.Where(o => o.FileType == downloadRequest.FileType
                    && o.DestinationUser == downloadRequest.SourceUser
                    && o.RowStatus == downloadRequest.Status
                    && o.CreatedWhen == downloadRequest.TransDateTo);
            }
            return query;
        }

        public List<string> SearchTransDetail(int TransactionDataID)
        {
            List<string> listString = new List<string>();

            var query = (from o in repository.GetQuery<TransactionDataDetail>()
                         where o.TransactionDataID == TransactionDataID
                         select new TransactionDataDetailViewModel() 
                         {
                            ID = o.ID,
                            TransactionDataID = o.TransactionDataID,
                            Data = o.Data
                         }).ToList();

            if (query.Count > 0)
            {
                for (int i = 0; i < query.Count; i++)
                {
                    listString.Add(query[i].Data);
                }
            }
            return listString;
        }

        public void UpdateDocumentStatus(updateStatusRequest updateStatusRequest)
        {
            for (int i = 0; i < updateStatusRequest.transactionDataID.Count(); i++)
            {
                TransactionData transactionData =
                    repository.Single<TransactionData>(o => o.TransGUID == updateStatusRequest.transactionDataID[i].TransGUID
                    && o.DocumentNumber == updateStatusRequest.transactionDataID[i].DocumentNumber
                    && o.Key1 == updateStatusRequest.transactionDataID[i].Key1
                    && o.Key2 == updateStatusRequest.transactionDataID[i].Key2
                    && o.Key3 == updateStatusRequest.transactionDataID[i].Key3);
                if (transactionData != null)
                {
                    transactionData.RowStatus = updateStatusRequest.transactionDataID[i].TransStatus;
                    EntityHelper.SetAuditForUpdate(transactionData, "SERA");
                    repository.Update(transactionData);
                    repository.UnitOfWork.SaveChanges();
                }
                else if (transactionData == null)
                {
                    break;
                }
            }
        }

        public TransactionData GetTransactionData(string transGUID, string documentNumber, string key1, string key2, string key3)
        {
            TransactionData transactionData =
                    repository.Single<TransactionData>(o => o.TransGUID == transGUID
                    && o.DocumentNumber == documentNumber
                    && o.Key1 == key1
                    && o.Key2 == key2
                    && o.Key3 == key3);
            return transactionData;
        }

        public void UpdateDocumentStatus(string transGUID, string docNumber, string key1, string key2, string key3, string status)
        {
            TransactionData transactionData =
                    repository.Single<TransactionData>(o => o.TransGUID == transGUID
                    && o.DocumentNumber == docNumber
                    && o.Key1 == key1
                    && o.Key2 == key2
                    && o.Key3 == key3);
            transactionData.RowStatus = status;
            EntityHelper.SetAuditForUpdate(transactionData, "SERA");
            repository.Update(transactionData);
            repository.UnitOfWork.SaveChanges();
        }
    }
}