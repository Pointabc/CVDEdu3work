using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class EmployeeSignedDocumentsReportClientHandlers
  {

    public override void BeforeExecute(Sungero.Reporting.Client.BeforeExecuteEventArgs e)
    {
      EmployeeSignedDocumentsReport.ReportSessionId = Guid.NewGuid().ToString();
      CentrVD.Integration.PublicFunctions.Module.UpdateEmployeeSignedDocumentReportTable(EmployeeSignedDocumentsReport.ReportSessionId);
      
      // Создать диалог с запросом периода дат.
      var dialog = Dialogs.CreateInputDialog("Параметры отчета Сотрудник подписал документы");
      var employees = dialog.AddSelectMany("Сотудник", false, Sungero.Company.Employees.GetAll().FirstOrDefault());
      var departments = dialog.AddSelectMany("Подразделения", false, Sungero.Company.Departments.GetAll().FirstOrDefault());
      var roles = dialog.AddSelectMany("Роли", false, Sungero.CoreEntities.Roles.GetAll().FirstOrDefault());
      var startDate = dialog.AddDate("Дата от", true , Calendar.Today.AddDays(-30));
      var endDate = dialog.AddDate("Дата no", true, Calendar.Today);
      dialog.AddHyperlink("Статус");
      var isSign = dialog.AddBoolean("Подписан с использованием сертификата");
      
      if (dialog.Show() != DialogButtons.Ok)
        e.Cancel = true;
      
      // Передать указанные значения в параметры отчета.
      EmployeeSignedDocumentsReport.StartDate = startDate.Value.Value;
      EmployeeSignedDocumentsReport.EndDate = endDate.Value.Value;
    }

  }
}