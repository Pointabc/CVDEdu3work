using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class TaskWithAttachmentsClientHandlers
  {

    public override void BeforeExecute(Sungero.Reporting.Client.BeforeExecuteEventArgs e)
    {
      // Создать диалог с запросом периода дат.
      var dialog = Dialogs.CreateInputDialog("Параметры отчета");
      var startDate = dialog.AddDate("Начальная дата", true , Calendar.Today.AddDays(-180));
      var endDate = dialog.AddDate("Конечная дата", true, Calendar.Today);
      
      if (dialog.Show() != DialogButtons.Ok)
        e.Cancel = true;
      // Передать указанные значения в параметры отчета.
      TaskWithAttachments.StartDate = startDate.Value.Value;
      TaskWithAttachments.EndDate = endDate.Value.Value;
    }

  }
}