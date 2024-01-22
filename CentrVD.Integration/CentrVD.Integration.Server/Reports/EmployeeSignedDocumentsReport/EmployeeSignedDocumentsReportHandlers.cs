using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class EmployeeSignedDocumentsReportServerHandlers
  {

    public override void AfterExecute(Sungero.Reporting.Server.AfterExecuteEventArgs e)
    {
      // TODO VS Восстановить после отладки
      //Sungero.Docflow.PublicFunctions.Module.DeleteReportData(Constants.EmployeeSignedDocumentsReport.SourceTableName, EmployeeSignedDocumentsReport.ReportSessionId);
    }
  }

}