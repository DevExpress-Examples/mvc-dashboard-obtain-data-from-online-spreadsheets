using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Native.Excel;
using DevExpress.SpreadsheetSource;
using GoogleApi;

namespace DXWebApplication7.Controllers {
    public class HomeController : Controller {
        public HomeController() {
        }

        public ActionResult Index() {
            DashboardConfig.AccessToken = (string)Session["access_token"];
            return View();
        }

        public ActionResult SignInGoogle() {
            return Redirect(GoogleOAuth.BuildAuthUrl(DashboardConfig.ClientInfo));
        }

        public ActionResult GoogleRedirectCode() {
            var result = Request.QueryString;
            if(result["state"] == DashboardConfig.ClientInfo.State) {
                AuthorizedResponse authorizedResponse = GoogleOAuth.ExchangeCode(result["code"], DashboardConfig.ClientInfo);
                Session["access_token"] = authorizedResponse.AccessToken;
                DashboardConfig.AccessToken = authorizedResponse.AccessToken;
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public ActionResult ChooseFile() {
            if(Session["access_token"] == null || GoogleOAuth.ValidateToken(Session["access_token"].ToString()).Error != null) {
                return RedirectToAction("Index");
            }

            const string excelMimeType = "application/vnd.google-apps.spreadsheet";
            object token = Session["access_token"];
            using(var googleSheets = new GoogleSheetsService(DashboardConfig.ClientInfo, token != null ? token.ToString() : null)) {
                ViewBag.Files = googleSheets.ListFiles().Files.Where(f => f.MimeType == excelMimeType).ToArray();
            }

            return View("ChooseFile");
        }

        public ActionResult ChooseSheetByUrl(string url) {
            return ChooseSheet(GoogleSheetsUrlHelper.ExtractIdFromUrl(url));
        }

        public ActionResult ChooseSheet(string id) {
            if(Session["access_token"] == null || GoogleOAuth.ValidateToken(Session["access_token"].ToString()).Error != null) {
                return RedirectToAction("Index");
            }

            object token = Session["access_token"];
            using(var googleSheets = new GoogleSheetsService(DashboardConfig.ClientInfo, token != null ? token.ToString() : null)) {
                Stream fileStream = googleSheets.GetFileStream(id);
                using(ISpreadsheetSource spreadsheetSource = ExcelDataLoaderHelper.CreateSource(fileStream, ExcelDocumentFormat.Xlsx, null, new ExcelSourceOptions())) {
                    ViewBag.Sheets = spreadsheetSource.Worksheets.Select(f => f.Name).ToArray();
                    ViewBag.Tables = spreadsheetSource.Tables.Select(t => t.Name).ToArray();
                    ViewBag.Ranges = spreadsheetSource.DefinedNames.Where(dn => !dn.IsHidden).ToArray();
                    ViewBag.FileID = id;
                }
            }

            return View("ChooseSheet");
        }

        public ActionResult AddDataSource(string id, string sheetId, string tableId, string rangeId, string scope) {
            string name = new PrefixNameGenerator("Google Sheets Source", " ", 1)
                .GenerateName(n => ((IDataSourceStorage)DashboardConfig.DataSourceStorage).GetDataSourcesID().Contains(n));
            ExcelSettingsBase excelSettings = null;
            if(!string.IsNullOrEmpty(sheetId))
                excelSettings = new ExcelWorksheetSettings(sheetId);
            else if(!string.IsNullOrEmpty(tableId))
                excelSettings = new ExcelTableSettings(tableId);
            else if(!string.IsNullOrEmpty(rangeId))
                excelSettings = new ExcelDefinedNameSettings(rangeId, scope);
            if(excelSettings != null) {
                var excelDataSource = new DashboardExcelDataSource {
                    Name = name,
                    SourceOptions = new ExcelSourceOptions(excelSettings),
                    StreamDocumentFormat = ExcelDocumentFormat.Xlsx,
                    FileName = id,
                };

                DashboardConfig.DataSourceStorage.RegisterDataSource(name, excelDataSource.SaveToXml());
            }

            return View("Index");
        }
    }
}