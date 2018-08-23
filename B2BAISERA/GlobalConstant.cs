
#define production

//#define development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BAISERA
{
    public static class GlobalConstant
    {
    #if production
        public const string UriGlobal = "http://apps.astra.co.id/";
        public const string UrlActionLoginAuthentication = "http://apps.astra.co.id/LoginAuthentication";
        public const string UrlActionUploadDocument = "http://apps.astra.co.id/UploadDocument";
        public const string UrlActionDownloadDocument = "http://apps.astra.co.id/DownloadDocument";
        public const string UrlActionDownloadDocumentByCheckPoint = "http://apps.astra.co.id/DownloadDocumentByCheckPoint";
        public const string UrlActionUpdateDocumentStatus = "http://apps.astra.co.id/UpdateDocumentStatus";
        public const string UrlActionDownloadDocumentStatus = "http://apps.astra.co.id/DownloadDocumentStatus";
        public const string UrlActionDownloadDocumentMonitoring = "http://apps.astra.co.id/DownloadDocumentMonitoring";
        public const string UrlActionDownloadCollection = "http://apps.astra.co.id/DownloadCollection";
        public const string UrlActionDownloadDocumentWithDocNoQty = "http://apps.astra.co.id/DownloadDocumentWithDocNoQty";
        public const string UrlActionDownloadDocumentMonitoringWithDocNoQty = "http://apps.astra.co.id/DownloadDocumentMonitoringWithDocNoQty";

#endif
#if development
        public const string UriGlobal = "devUri";
        public const string UrlActionLoginAuthentication = "http://apps.astra.co.id/LoginAuthentication";
        public const string UrlActionUploadDocument = "http://apps.astra.co.id/UploadDocument";
        public const string UrlActionDownloadDocument = "http://apps.astra.co.id/DownloadDocument";
        public const string UrlActionDownloadDocumentByCheckPoint = "http://apps.astra.co.id/DownloadDocumentByCheckPoint";
        public const string UrlActionUpdateDocumentStatus = "http://apps.astra.co.id/UpdateDocumentStatus";
        public const string UrlActionDownloadDocumentStatus = "http://apps.astra.co.id/DownloadDocumentStatus";
        public const string UrlActionDownloadDocumentMonitoring = "http://apps.astra.co.id/DownloadDocumentMonitoring";
        public const string UrlActionDownloadCollection = "http://apps.astra.co.id/DownloadCollection";
        public const string UrlActionDownloadDocumentWithDocNoQty = "http://apps.astra.co.id/DownloadDocumentWithDocNoQty";
        public const string UrlActionDownloadDocumentMonitoringWithDocNoQty = "http://apps.astra.co.id/DownloadDocumentMonitoringWithDocNoQty";
#endif

    }
}
