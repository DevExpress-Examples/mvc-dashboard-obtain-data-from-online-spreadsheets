using System;
using System.IO;
using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Sql;
using System.Web.Hosting;
using DevExpress.DataAccess.Excel;
using DevExpress.DashboardCommon;
using GoogleApi;

namespace DXWebApplication7 {
    public static class DashboardConfig {
        static readonly ClientAuthenticationInfo clientInfo;

        public static DataSourceInMemoryStorage DataSourceStorage { get; set; }

        public static ClientAuthenticationInfo ClientInfo { get { return clientInfo; } }

        public static string AccessToken { get; set; }

        static DashboardConfig() {
            clientInfo = new GoogleClientAuthenticationInfo {
                ClientID = System.Configuration.ConfigurationManager.AppSettings["GoogleClientID"],
                ClientSecret = System.Configuration.ConfigurationManager.AppSettings["GoogleClientSecret"],
                RedirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"],
            };
        }

        public static void RegisterService(RouteCollection routes) {
            routes.MapDashboardRoute("dashboardControl");

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage("~/App_Data/Dashboards");
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);
            DataSourceStorage = new DataSourceInMemoryStorage();
            DashboardConfigurator.Default.SetDataSourceStorage(DataSourceStorage);
            DashboardConfigurator.Default.ExcelDataSourceBeforeFill += DefaultOnExcelDataSourceBeforeFill;
            DashboardConfigurator.Default.ConfigureDataReloadingTimeout += (s, e) => {
                e.DataReloadingTimeout = TimeSpan.FromSeconds(30);
            };
        }

        static void DefaultOnExcelDataSourceBeforeFill(object sender, ExcelDataSourceBeforeFillWebEventArgs args) {
            using(var googleSheets = new GoogleSheetsService(ClientInfo, AccessToken)) {
                Stream fileStream = googleSheets.GetFileStream(args.FileName);
                args.Stream = fileStream;
                args.StreamDocumentFormat = ExcelDocumentFormat.Xlsx;
            }
        }
    }
}