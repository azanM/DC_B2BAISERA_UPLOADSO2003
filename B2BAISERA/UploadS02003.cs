using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using B2BAISERA.Log;
using B2BAISERA.Models.Providers;
using B2BAISERA.Properties;
using System.Net;
using B2BAISERA.Models;
using B2BAISERA.Models.EFServer;
using System.Diagnostics;

namespace B2BAISERA
{
    public partial class UploadS02003 : Form
    {
        private static string fileType = "S02003";
        private bool acknowledge;
        private string ticketNo = "";
        private string message = "";

        public UploadS02003()
        {
            InitializeComponent();
        }

        //private void UploadS02003_Load(object sender, EventArgs e)
        //{
        //    LoginAuthentication();
        //    if (acknowledge == false || ticketNo == string.Empty)
        //    {
        //        //close
        //    }
        //    else Upload();
        //}

        private void UploadS02003_Load(object sender, EventArgs e)
        {
            LogEvent logEvent = new LogEvent();
            TransactionProvider transactionProvider = new TransactionProvider();
            List<CUSTOM_S02003_TEMP> tempHS = new List<CUSTOM_S02003_TEMP>();
            List<CUSTOM_S02003_TEMP> tempHSChecked = new List<CUSTOM_S02003_TEMP>();
            try
            {
                transactionProvider.DeleteAllTempHSS02003();
                do
                {
                    tempHS = transactionProvider.CancelPOSeraToAI();
                    tempHSChecked = transactionProvider.CheckingHistoryHS(tempHS);
                    transactionProvider.DeleteAllTempHSS02003();
                    if (tempHSChecked.Count > 0)
                    {
                        LoginAuthentication();
                        if (acknowledge == false || ticketNo == string.Empty)
                        {
                            //close
                        }
                        else Upload(tempHSChecked);
                        //Upload(tempHSChecked);

                        tempHS = new List<CUSTOM_S02003_TEMP>();
                        tempHSChecked = new List<CUSTOM_S02003_TEMP>();
                        //tempHS = transactionProvider.CancelPOSeraToAI();
                        tempHSChecked = transactionProvider.CheckingHistoryHS(tempHS);
                        transactionProvider.DeleteAllTempHSS02003();

                    }
                } while (tempHSChecked.Count > 0);

                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", "Upload Finish.", fileType, "SERA");
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
            }
            catch (Exception ex)
            {
                LblResult.Text = ex.Message;
                LblAcknowledge.Text = "";
                LblTicketNo.Text = "";
                LblMessage.Text = "";

                //logevent login failed
                logEvent.WriteDBLog("", "UploadS02003_Load", false, "", ex.Message, fileType, "SERA");
                transactionProvider.DeleteAllTempHSS02003();

                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
            }
        }

        private void LoginAuthentication()
        {
            LogEvent logEvent = new LogEvent();
            TransactionProvider transactionProvider = new TransactionProvider();
            try
            {
                using (WsProd.B2BAIWebServiceDMZ wsB2B = new WsProd.B2BAIWebServiceDMZ())
                {

                    var User = transactionProvider.GetUser("SERA", "SERA", "B2BAITAG");
                    if (User != null)
                    {
                        var loginReq = new WsProd.LoginRequest();
                        loginReq.UserName = User.UserCode;
                        loginReq.Password = User.PassCode;
                        loginReq.ClientTag = User.ClientTag;

                        //WebProxy myProxy = new WebProxy(Resources.WebProxyAddress, true);
                        //myProxy.Credentials = new NetworkCredential(Resources.NetworkCredentialUserName, Resources.NetworkCredentialPassword, Resources.NetworkCredentialProxy);

                        //WebProxy myProxy = new WebProxy();
                        //myProxy.Credentials = new NetworkCredential(Resources.NetworkCredentialUserName, Resources.NetworkCredentialPassword, Resources.NetworkCredentialProxy);
                        //myProxy.Credentials = new NetworkCredential("backup", "serasibackup", "trac.astra.co.id");
                        //myProxy.Credentials = new NetworkCredential("rika009692", "mickey1988", "trac.astra.co.id");
                        //myProxy.Credentials = new NetworkCredential("genrpt", "serasera", "trac.astra.co.id");
                        //wsB2B.Proxy = myProxy;

                        var wsResult = wsB2B.LoginAuthentication(loginReq);
                        acknowledge = wsResult.Acknowledge;
                        ticketNo = wsResult.TicketNo;
                        message = wsResult.Message;
                    }

                    LblResult.Text = "Service Result = ";
                    LblAcknowledge.Text = "Acknowledge : " + acknowledge;
                    LblTicketNo.Text = "TicketNo : " + ticketNo;
                    LblMessage.Text = "Message :" + message;

                    //logevent login succeded
                    logEvent.WriteDBLog("B2BAIWebServiceDMZ", "LoginAuthentication", acknowledge, ticketNo, message, fileType, "SERA");
                }
            }
            catch (Exception ex)
            {
                LblResult.Text = ex.Message;
                LblAcknowledge.Text = "";
                LblTicketNo.Text = "";
                LblMessage.Text = "";

                //logevent login failed
                logEvent.WriteDBLog("B2BAIWebServiceDMZ", "LoginAuthentication", acknowledge, ticketNo, "webservice message : " + message + ". exception message : " + ex.Message, fileType, "SERA");

                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
            }
        }

        private void Upload(List<CUSTOM_S02003_TEMP> tempHSChecked)
        {
            LogEvent logEvent = new LogEvent();
            TransactionProvider transactionProvider = new TransactionProvider();
            TransactionViewModel transaction = null;
            WsProd.TransactionData[] transactionDataArray = null;
            List<S02003HSViewModel> transactionDataDetailHS = new List<S02003HSViewModel>();
            List<string> arrHSIS = null;

            try
            {
                ////1.Get HS/IS FROM EPROC+STREAMLINER //2.INSERT INTO TEMP_HS + TEMP_IS //3.GET FROM TEMP_HS + TEMP_IS
                //var tempHS = transactionProvider.CancelPOSeraToAI();

                ////CHECKING EVER SEND TO UPLOAD BY PONUMBER
                //var tempHSChecked = transactionProvider.CheckingHistoryHS(tempHS);

                //4.INSERT INTO LOG TRANSACTION HEADER DETAIL
                var intResult = transactionProvider.InsertLogTransactionS02003(tempHSChecked);

                //5.GET DATA FROM LOG TRANSACTION HEADER DETAIL 
                if (intResult != 0)
                {
                    //a.GET TRANSACTION
                    transaction = transactionProvider.GetTransactionS02003();

                    //b.GET TRANSACTION DATA
                    if (transaction != null)
                    {
                        //transactionData = transactionProvider.GetTransactionData(transaction.ID);
                        transactionDataArray = transactionProvider.GetTransactionDataArray(transaction.ID);

                        //c.GET TRANSACTIONDATA DETAIL / HS-IS
                        for (int i = 0; i < transactionDataArray.Count(); i++)
                        {
                            var DataDetailHS = transactionProvider.GetTransactionDataDetailHSS02003(transactionDataArray[i].ID);
                            //var DataDetailIS = transactionProvider.GetTransactionDataDetailIS(transactionDataArray[i].ID);
                            for (int j = 0; j < DataDetailHS.Count; j++)
                            {
                                transactionDataDetailHS.Add(DataDetailHS[j]);
                                //masukan ke array
                                arrHSIS = new List<string>();
                                arrHSIS.Add(transactionProvider.ConcateStringHSS02003(DataDetailHS[j]));

                                //masukan ke transactionDataArray.
                                transactionDataArray[i].Data = arrHSIS.ToArray();
                                transactionDataArray[i].DataLength = arrHSIS.Count;
                            }
                        }
                        //6.SEND TO WEB SERVICE
                        using (WsProd.B2BAIWebServiceDMZ wsB2B = new WsProd.B2BAIWebServiceDMZ())
                        {
                            WsProd.UploadRequest uploadRequest = new WsProd.UploadRequest();
                            var lastTicketNo = transactionProvider.GetLastTicketNo(fileType);
                            uploadRequest.TicketNo = lastTicketNo; //from session ticketNo login
                            uploadRequest.ClientTag = Resources.ClientTag;
                            uploadRequest.transactionData = transactionDataArray;

                            //WebProxy myProxy = new WebProxy(Resources.WebProxyAddress, true);
                            //myProxy.Credentials = new NetworkCredential(Resources.NetworkCredentialUserName, Resources.NetworkCredentialPassword, Resources.NetworkCredentialProxy);

                            //WebProxy myProxy = new WebProxy();
                            //myProxy.Credentials = new NetworkCredential(Resources.NetworkCredentialUserName, Resources.NetworkCredentialPassword, Resources.NetworkCredentialProxy);
                            //myProxy.Credentials = new NetworkCredential("backup", "serasibackup", "trac.astra.co.id");
                            //myProxy.Credentials = new NetworkCredential("rika009692", "mickey1988", "trac.astra.co.id");
                            //myProxy.Credentials = new NetworkCredential("genrpt", "serasera", "trac.astra.co.id");
                            //wsB2B.Proxy = myProxy;

                            var wsResult = wsB2B.UploadDocument(uploadRequest);
                            acknowledge = wsResult.Acknowledge;
                            ticketNo = wsResult.TicketNo;
                            message = wsResult.Message;
                        }
                    }
                    else if (transaction == null)
                    {
                        // add taks kill when transaction = null : by fhi 04.06.2014
                        logEvent.WriteDBLog("", "UploadS02003_Load", false, "", "transaction == null", fileType, "SERA");
                        Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                        //end
                    }
                }
                else if (intResult == 0)
                {
                    //delete temp table 
                    transactionProvider.DeleteAllTempHSS02003();
                    acknowledge = false;
                    ticketNo = "";
                    message = "No Data Upload.";

                    // add taks kill when data = null : by fhi 04.06.2014
                    logEvent.WriteDBLog("", "UploadS02003_Load", false, "", "intResult == 0", fileType, "SERA");
                    Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                    //end
                }

                LblResult.Text = "Service Result = ";
                LblAcknowledge.Text = "Acknowledge : " + acknowledge;
                LblTicketNo.Text = "TicketNo : " + ticketNo;
                LblMessage.Text = "Message :" + message;

                //logevent login succeded
                logEvent.WriteDBLog("B2BAIWebServiceDMZ", "UploadDocumentS02003", acknowledge, ticketNo, message, fileType, "SERA");


            }
            catch (Exception ex)
            {
                LblResult.Text = ex.Message;
                LblAcknowledge.Text = "";
                LblTicketNo.Text = "";
                LblMessage.Text = "";

                //logevent login failed
                logEvent.WriteDBLog("B2BAIWebServiceDMZ", "UploadDocumentS02003", acknowledge, ticketNo, "webservice message : " + message + ". exception message : " + ex.Message, fileType, "SERA");

                // add taks kill when failed : by fhi 04.06.2014
                Process.Start("taskkill.exe", "/f /im B2BAISERA_S02003.exe");
                //end
            }
        }
    }
}
