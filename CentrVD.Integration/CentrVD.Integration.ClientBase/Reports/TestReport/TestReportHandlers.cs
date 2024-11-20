using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class TestReportClientHandlers
  {

    public override void BeforeExecute(Sungero.Reporting.Client.BeforeExecuteEventArgs e)
    {
      var date = DateTime.Parse("Apr 23 2024  3:52PM");
      
      // Создать диалог с запросом периода дат.
      var dialog = Dialogs.CreateInputDialog("Параметры отчета Сотрудник подписал документы");
      var flds = Sungero.CoreEntities.Folders.GetAll().ToList();
      var d = Sungero.CoreEntities.Folders.GetAll(f => !f.IsSpecial.Value).ShowSelectMany();
      //var d = Sungero.CoreEntities.Folders.GetAll().ShowSelectMany();
      //var d = Sungero.CoreEntities.Folders.GetAll();
      IQueryable<IFolder> folders = null;
      var hyperlink = dialog.AddHyperlink("Выберите папки");
      
      var fld = Sungero.CoreEntities.Folders.GetAll().FirstOrDefault();
      
      var name = fld.State.Properties.Name;
      //var name1 = fld.State.Properties.Name.GetLocalizedValue();
      //var n = CentrVD.Category3.Module.MobileApps.Server.PublicFunctions.MobileDevice.Remote.
      //var folders = Functions.MobileAppSetting.Remote.GetFolderNameWithIds(_obj);
      
      var fnames = new List<string>();
      //fnames.AddRange(flds.Select(f => f.Name));
      
      //var folders = dialog.AddSelect("Папки", false, 0).From(fnames.ToArray());
      hyperlink.SetOnExecute(() => {
                               folders = Sungero.CoreEntities.Folders.GetAll().ShowSelectMany();
                               //fnames.AddRange(d.Select(f => f.DisplayValue));
                               
                             });
      
      
      //fnames.AddRange(d.Where(f => !f.IsSpecial.Value).ToList());
      //var folders1 = dialog.AddSelect("Папки", false, Sungero.CoreEntities.Folders.GetAll().Where(f => !f.IsSpecial.Value));
      //var folders1 = dialog.AddSelect("Папки", false, Sungero.CoreEntities.Folders.Null);
      dialog.AddSelectMany("Папки", false, Sungero.CoreEntities.Folders.Null);
      dialog.AddSelect("Папки", false, Sungero.CoreEntities.Folders.Null);
      //var folders = dialog.AddSelect("Папки", false, 0).From(fnames.ToArray());
      dialog.AddDate("Дата и время", false).AsDateTime();
      
      // TODO Показывает не все записи
      var recipients = dialog.AddSelectMany("Автор", false,  Sungero.CoreEntities.Recipients.GetAll().FirstOrDefault());
      var startDate = dialog.AddDate("Дата от", true , Calendar.Today.AddDays(-90));
      var endDate = dialog.AddDate("Дата no", true, Calendar.Today);
      List<string> list = new List<string>() { CentrVD.Integration.Reports.Resources.TestReport.ValueIsNotSet,
        CentrVD.Integration.Reports.Resources.TestReport.Approved,
        CentrVD.Integration.Reports.Resources.TestReport.Agreed };
      var agreedApproved = dialog.AddSelect("Согласовано/Утверждено", true, 0).From(list.ToArray());
      var isSignCert = dialog.AddBoolean("Подписан с использованием сертификата");
      
      if (dialog.Show() != DialogButtons.Ok)
        e.Cancel = true;
      
      //folders.ShowModal();
      
      // Передать указанные значения в параметры отчета.
      TestReport.StartDate = startDate.Value.Value;
      TestReport.EndDate = endDate.Value.Value;
      
      TestReport.ReportSessionId = Guid.NewGuid().ToString();
      var authors = new List<IRecipient>();
      authors.AddRange(recipients.Value);
      CentrVD.Integration.PublicFunctions.Module.UpdateEmployeeSignedDocumentReportTableV3(TestReport.ReportSessionId,
                                                                                           authors, agreedApproved.Value,
                                                                                           isSignCert.Value.Value,
                                                                                           TestReport.StartDate,
                                                                                           TestReport.EndDate);
    }

  }
}