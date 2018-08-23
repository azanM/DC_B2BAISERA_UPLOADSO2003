using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using B2BSERAWebService.Model;
using System.Globalization;
using B2BSERAWebService.Model.DataAccess;
using B2BSERAWebService.Logic;
using Microsoft.Practices.Unity;
using B2BSERAWebService.Model.Providers;
using System.Threading;

namespace B2BSERAWebService
{
    /// <summary>
    /// Summary description for WsB2BAISERA
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://apps.astra.co.id/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsB2BAISERA : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        
        private static Mutex mut = new Mutex(false, "JobSchedulerMutex");

        #region Login Authentication
        [WebMethod]
        public B2BSERAWebService.Model.LoginAuthenticationViewModel LoginAuthentication(B2BSERAWebService.Model.loginRequest loginRequest)
        {
            //bool ranJob = false; mut.WaitOne();
            try
            {
                //if (loginRequest != null)
                if (ValidateRequest(loginRequest.UserName, loginRequest.Password, loginRequest.ClientTag))
                {
                    return LoginAuthenticationResponse();
                }
                else return LoginAuthenticationResponseError();
            }
            catch (Exception ex)
            {
                //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                //logEvent.WriteDBLog("WsB2BAISERA", "LoginAuthentication", false, "", "Error " + ex.Message, "");
                return LoginAuthenticationResponseError(ex);
            }
        }

        //for testing LoginAuthentication
        private bool ValidateRequest(string UserName, string Password, string ClientTag)
        {
            try
            {
                //B2BAISERAEntities db = new B2BAISERAEntities();
                //var User = (from o in db.Users
                //            where o.UserName == UserName && o.Password == Password && o.ClientTag == ClientTag
                //            select o).FirstOrDefault();

                TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                var User = transactionProvider.GetUser(UserName, Password, ClientTag);

                if (User != null)
                {
                    //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                    //logEvent.WriteDBLog("WsB2BAISERA", "LoginAuthentication ValidateRequest", true, "", "Succeeded Validate Request LoginAuthentication.", UserName);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                //logEvent.WriteDBLog("WsB2BAISERA", "LoginAuthentication ValidateRequest", false, "", "Error Validate Request LoginAuthentication. " + ex.Message, UserName);
                return false;
            }
        }

        private B2BSERAWebService.Model.LoginAuthenticationViewModel LoginAuthenticationResponse()
        {
            LoginAuthenticationViewModel LoginAuthenticationResult = new B2BSERAWebService.Model.LoginAuthenticationViewModel()
            {
                Acknowledge = true,
                TicketNo = "EDE283E6BE1A866452AC76F106CC5CB80BA1902F0BBA8C019F27D3E5025C931139B8E385AD37F6E1CDA0FB029A1198E17DD968A5C711A1FFFB72F33405F34B8EA545F4CE02F7B8B83369C36AC83BF0FAE422B15B919D294047AEC92AF8DC67A0",
                Message = "Login Succeeded"
            };

            return LoginAuthenticationResult;
        }

        private B2BSERAWebService.Model.LoginAuthenticationViewModel LoginAuthenticationResponseError()
        {
            LoginAuthenticationViewModel LoginAuthenticationResult = new B2BSERAWebService.Model.LoginAuthenticationViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Login Failed"
            };

            return LoginAuthenticationResult;
        }

        private B2BSERAWebService.Model.LoginAuthenticationViewModel LoginAuthenticationResponseError(Exception ex)
        {
            LoginAuthenticationViewModel LoginAuthenticationResult = new B2BSERAWebService.Model.LoginAuthenticationViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Login Failed"
            };

            return LoginAuthenticationResult;
        }
        #endregion

        #region Upload Document
        [WebMethod]
        public B2BSERAWebService.Model.UploadDocumentViewModel UploadDocument(B2BSERAWebService.Model.uploadRequest uploadRequest)
        {
            try
            {
                if (ValidateUploadDocumentRequest(uploadRequest))
                {
                    TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                    transactionProvider.InsertTransaction(uploadRequest);

                    return UploadDocumentResponse();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                //logEvent.WriteDBLog("WsB2BAISERA", "UploadDocument", false, "", "Error Validate FileType or IPAdress or DetinationUser. " + ex.Message, "");
                return UploadDocumentResponseError(ex);
            }
        }

        //for testing UploadDocument
        private bool ValidateUploadDocumentRequest(B2BSERAWebService.Model.uploadRequest uploadRequest)
        {
            try
            {
                if (!string.IsNullOrEmpty(uploadRequest.TicketNo) && uploadRequest.ClientTag == "B2BAITAG_DEV")
                {
                    //checking filetype, ipaddress, destinationuser : registered on db or not.
                    if (ValidateFileTypeIPAdressDetinationUser(uploadRequest))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateFileTypeIPAdressDetinationUser(B2BSERAWebService.Model.uploadRequest uploadRequest)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i < uploadRequest.transactionData.Count(); i++)
                {
                    TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                    var fileType = transactionProvider.GetFileTypeName(uploadRequest.transactionData[i].FileType);
                    var ipAddress = transactionProvider.GetIPAddress(uploadRequest.transactionData[i].IPAddress);
                    var destinationUser = transactionProvider.GetUser(uploadRequest.transactionData[i].DestinationUser);

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

        private B2BSERAWebService.Model.UploadDocumentViewModel UploadDocumentResponse()
        {
            B2BSERAWebService.Model.UploadDocumentViewModel UploadDocumentResult = new B2BSERAWebService.Model.UploadDocumentViewModel()
            {
                Acknowledge = true,
                TicketNo = "EDE283E6BE1A866452AC76F106CC5CB80BA1902F0BBA8C019F27D3E5025C931139B8E385AD37F6E1CDA0FB029A1198E17DD968A5C711A1FFFB72F33405F34B8EA545F4CE02F7B8B83369C36AC83BF0FAE422B15B919D294047AEC92AF8DC67A0",
                Message = "Upload Document Succeeded"
            };

            return UploadDocumentResult;
        }

        private B2BSERAWebService.Model.UploadDocumentViewModel UploadDocumentResponseError(Exception ex)
        {
            B2BSERAWebService.Model.UploadDocumentViewModel UploadDocumentResult = new B2BSERAWebService.Model.UploadDocumentViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Error Validate FileType or IPAdress or DetinationUser. " + ex.Message
            };

            return UploadDocumentResult;
        }
        #endregion
        
        #region Download Document
        [WebMethod]
        public B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocument(B2BSERAWebService.Model.downloadRequest downloadRequest)
        {
            try
            {
                var listData = ValidateDownloadDocumentRequest(downloadRequest);
                if (listData.Count > 0)
                {
                    return DownloadDocumentResponse(listData);
                }
                else return DownloadDocumentResponseFailed();
            }
            catch (Exception ex)
            {
                //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                //logEvent.WriteDBLog("WsB2BAISERA", "UploadDocument", false, "", "Error Validate FileType or IPAdress or DetinationUser. " + ex.Message, "");
                return DownloadDocumentResponseError(ex);
            }
        }

        //for testing DownloadDocument
        private List<TransactionViewModel> ValidateDownloadDocumentRequest(B2BSERAWebService.Model.downloadRequest downloadRequest)
        {
            List<TransactionViewModel> listTransaction = new List<TransactionViewModel>();
            List<TransactionDataDetailViewModel> listTransactionDataDetail = new List<TransactionDataDetailViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(downloadRequest.TicketNo)
                    && downloadRequest.ClientTag == "B2BAITAG_DEV")
                {
                    TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                    listTransaction = transactionProvider.SearchDocumentTransactionData(downloadRequest).ToList();

                    //if (listTransaction.Count > 0)
                    //{
                    //    for (int i = 0; i < listTransaction.Count; i++)
                    //    {
                    //        listTransaction[i].Data
                    //    }
                    //    //listTransactionDataDetail = transactionProvider.SearchDocumentTransactionDataDetail(
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listTransaction;
        }
        
        private B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocumentResponse(List<TransactionViewModel> listData)
        {
            DownloadDocumentViewModel DownloadDocumentResult = new DownloadDocumentViewModel();
            DownloadDocumentResult.Acknowledge = true;
            DownloadDocumentResult.TicketNo = "EDE283E6BE1A866452AC76F106CC5CB80BA1902F0BBA8C019F27D3E5025C931139B8E385AD37F6E1CDA0FB029A1198E17DD968A5C711A1FFFB72F33405F34B8EA545F4CE02F7B8B83369C36AC83BF0FAE422B15B919D294047AEC92AF8DC67A0";
            DownloadDocumentResult.Message = "Download Data Succeeded";


            List<TransactionDataModel> trans = new List<TransactionDataModel>();
            for (int i = 0; i < listData.Count; i++)
            {                
                TransactionDataModel transData = new TransactionDataModel();
                transData.ID = listData[i].TransactionDataID;
                transData.TransGUID = listData[i].TransGUID;
                transData.DocumentNumber = listData[i].DocumentNumber;
                transData.FileType = listData[i].FileType;
                transData.IPAddress = listData[i].IPAddress;
                transData.DestinationUser = listData[i].DestinationUser;
                transData.DataLength = listData[i].DataLength;

                //TODO 2013-09-05: BUILD { Data = HS|PONUMBER|||||||| }
                List<string> listString = new List<string>();
                TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                listString = transactionProvider.SearchTransDetail(listData[i].TransactionDataID);

                transData.Data = listString.ToArray();

                trans.Add(transData);
            }
            DownloadDocumentResult.transactionData = trans;

            return DownloadDocumentResult;
        }

        private B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocumentResponseFailed()
        {
            B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocumentResult = new B2BSERAWebService.Model.DownloadDocumentViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Download Data Failed."
            };

            return DownloadDocumentResult;
        }

        private B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocumentResponseError(Exception ex)
        {
            B2BSERAWebService.Model.DownloadDocumentViewModel DownloadDocumentResult = new B2BSERAWebService.Model.DownloadDocumentViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Error on " + ex.Message
            };

            return DownloadDocumentResult;
        }
        #endregion

        #region Update Document Status
        [WebMethod]
        public B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentStatus(B2BSERAWebService.Model.updateStatusRequest updateStatusRequest)
        {
            try
            {
                if (validateUpdateDocumentStatus(updateStatusRequest))
                {
                    TransactionProvider transactionProvider = UnityContainerHelper.Container.Resolve<TransactionProvider>();
                    for (int i = 0; i < updateStatusRequest.transactionDataID.Count(); i++)
                    {
                        var transactionData = transactionProvider.GetTransactionData(updateStatusRequest.transactionDataID[i].TransGUID, updateStatusRequest.transactionDataID[i].DocumentNumber, updateStatusRequest.transactionDataID[i].Key1, updateStatusRequest.transactionDataID[i].Key2, updateStatusRequest.transactionDataID[i].Key3);
                        if (transactionData != null)
                        {
                            //transactionProvider.UpdateDocumentStatus(updateStatusRequest);
                            transactionProvider.UpdateDocumentStatus(updateStatusRequest.transactionDataID[i].TransGUID, updateStatusRequest.transactionDataID[i].DocumentNumber, updateStatusRequest.transactionDataID[i].Key1, updateStatusRequest.transactionDataID[i].Key2, updateStatusRequest.transactionDataID[i].Key3, updateStatusRequest.transactionDataID[i].TransStatus);
                        }
                        if (transactionData == null)
                        {
                            return UpdateDocumentResponseNull();
                        }
                    }
                    return UpdateDocumentResponse();
                }
                else return UpdateDocumentResponseFailed();
            }
            catch (Exception ex)
            {
                //LogEvent logEvent = UnityContainerHelper.Container.Resolve<LogEvent>();
                //logEvent.WriteDBLog("WsB2BAISERA", "UpdateDocumentStatus", false, "", "Error " + ex.Message, "");
                return UpdateDocumentResponseError(ex);
            }            
        }

        private bool validateUpdateDocumentStatus(updateStatusRequest updateStatusRequest)
        {
            if (!string.IsNullOrEmpty(updateStatusRequest.TicketNo) && updateStatusRequest.ClientTag == "B2BAITAG_DEV")
            {
                return true;
            }
            return false;
        }

        private B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResponse()
        {
            B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResult = new B2BSERAWebService.Model.UpdateDocumentViewModel()
            {
                Acknowledge = true,
                TicketNo = "EDE283E6BE1A866452AC76F106CC5CB80BA1902F0BBA8C019F27D3E5025C931139B8E385AD37F6E1CDA0FB029A1198E17DD968A5C711A1FFFB72F33405F34B8EA545F4CE02F7B8B83369C36AC83BF0FAE422B15B919D294047AEC92AF8DC67A0",
                Message = "Update Document Succeeded"
            };

            return UpdateDocumentResult;
        }

        private UpdateDocumentViewModel UpdateDocumentResponseNull()
        {
            B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResult = new B2BSERAWebService.Model.UpdateDocumentViewModel()
            {
                Acknowledge = true,
                TicketNo = "EDE283E6BE1A866452AC76F106CC5CB80BA1902F0BBA8C019F27D3E5025C931139B8E385AD37F6E1CDA0FB029A1198E17DD968A5C711A1FFFB72F33405F34B8EA545F4CE02F7B8B83369C36AC83BF0FAE422B15B919D294047AEC92AF8DC67A0",
                Message = "No Data Updated"
            };

            return UpdateDocumentResult;
        }

        private B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResponseFailed()
        {
            B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResult = new B2BSERAWebService.Model.UpdateDocumentViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Update Document Failed"
            };

            return UpdateDocumentResult;
        }

        private B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResponseError(Exception ex)
        {
            B2BSERAWebService.Model.UpdateDocumentViewModel UpdateDocumentResult = new B2BSERAWebService.Model.UpdateDocumentViewModel()
            {
                Acknowledge = false,
                TicketNo = "",
                Message = "Update Document Error On : " + ex.Message
            };

            return UpdateDocumentResult;
        }
        #endregion
        
    }
}
