<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128579139/21.2.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T549666)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# BI Dashboard for ASP.NET MVC - How to Obtain Data for Web Dashboard from Online Spreadsheets

This is a sample project that connects Dashboard to a Google Drive spreadsheet. 

Follow the steps below to create a data source based on the online spreadsheet:

1. Set *MvcDashboard* as a startup project and launch the application.
2. Click the **Sign in GoogleDrive** button to sign into the Google Account and allow the current application to view files in your Google Drive. 
3. After redirection click the **Add New ExcelDataSource** button to select the required spreadsheet file and data (sheets, tables, or named ranges). 
The created data source is now available for your dashboards.

Note that this sample project is a prototype that shows the possibility of using a third-party API to extend the Web Dashboard functionality. We do not plan to make access to Google Drive a built-in feature in Web Dashboard in the near future, but you can use the provided project as a starting point to implement this functionality in your project.

## Files to Review

* [DashboardConfig.cs](./CS/MvcDashboard/App_Start/DashboardConfig.cs)
* [HomeController.cs](./CS/MvcDashboard/Controllers/HomeController.cs)
* [ChooseFile.cshtml](./CS/MvcDashboard/Views/Home/ChooseFile.cshtml)
* [ChooseSheet.cshtml](./CS/MvcDashboard/Views/Home/ChooseSheet.cshtml)
* [Index.cshtml](./CS/MvcDashboard/Views/Home/Index.cshtml)
* [Web.config](./CS/MvcDashboard/Web.config)
<!-- feedback -->
## Does This Example Address Your Development Requirements/Objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=mvc-dashboard-obtain-data-from-online-spreadsheets&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=mvc-dashboard-obtain-data-from-online-spreadsheets&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
