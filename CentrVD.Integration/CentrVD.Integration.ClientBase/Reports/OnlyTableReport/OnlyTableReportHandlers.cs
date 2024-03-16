using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class OnlyTableReportClientHandlers
  {

    public override void BeforeExecute(Sungero.Reporting.Client.BeforeExecuteEventArgs e)
    {
      // Создать диалог с запросом периода дат.
      var dialog = Dialogs.CreateInputDialog("Параметры отчета Сотрудник подписал документы");
      // TODO Показывает не все записи
      var recipients = dialog.AddSelectMany("Автор", false,  Sungero.CoreEntities.Recipients.GetAll().FirstOrDefault());
      var startDate = dialog.AddDate("Дата от", true , Calendar.Today.AddDays(-90));
      var endDate = dialog.AddDate("Дата no", true, Calendar.Today);
      List<string> list = new List<string>() { CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.ValueIsNotSet,
        CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Approved,
        CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Agreed };
      var agreedApproved = dialog.AddSelect("Согласовано/Утверждено", true, 0).From(list.ToArray());
      var isSignCert = dialog.AddBoolean("Подписан с использованием сертификата");
      
      if (dialog.Show() != DialogButtons.Ok)
        e.Cancel = true;
      
      // Передать указанные значения в параметры отчета.
      OnlyTableReport.StartDate = startDate.Value.Value;
      OnlyTableReport.EndDate = endDate.Value.Value;
      OnlyTableReport.RecipientsIds = CentrVD.Integration.PublicFunctions.Module.GetEmployeesIdsForQuery(recipients.Value.ToList());
    }

  }
}