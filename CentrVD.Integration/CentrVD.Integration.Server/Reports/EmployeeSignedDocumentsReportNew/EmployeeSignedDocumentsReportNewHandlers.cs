using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class EmployeeSignedDocumentsReportNewServerHandlers
  {

    public override void BeforeExecute(Sungero.Reporting.Server.BeforeExecuteEventArgs e)
    {
      //var reportSessionId = System.Guid.NewGuid().ToString();
      //EmployeeSignedDocumentsReportNew.ReportSessionId = reportSessionId;
      
      EmployeeSignedDocumentsReportNew.ReportSessionId = Guid.NewGuid().ToString();
      CentrVD.Integration.PublicFunctions.Module.UpdateEmployeeSignedDocumentReportTableV2(EmployeeSignedDocumentsReportNew.ReportSessionId);
      
      // Тут заполняем таблицу Sungero_Reports_EmployeeSignedDocumentReportV2 данными из запроса
      //Functions.Module.ExecuteSQLCommandFormat(Queries.EmployeeSignedDocumentsReportNew.InsertIntoEmployeeTable,
      //                                         new object[] { Constants.EmployeeSignedDocumentsReportNew.SourceTableNameVersionV2, reportSessionId });
      
    }

    public override void AfterExecute(Sungero.Reporting.Server.AfterExecuteEventArgs e)
    {
      // TODO VS Восстановить после отладки
      //Sungero.Docflow.PublicFunctions.Module.DeleteReportData(Constants.EmployeeSignedDocumentsReport.SourceTableName, EmployeeSignedDocumentsReport.ReportSessionId);
    }
  }

}