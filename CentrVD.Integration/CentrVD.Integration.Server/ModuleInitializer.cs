using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace CentrVD.Integration.Server
{
  public partial class ModuleInitializer
  {

    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      var allUsers = Roles.AllUsers;
      Reports.AccessRights.Grant(Sungero.Docflow.Reports.GetApprovalRulesConsolidatedReport().Info, allUsers, DefaultReportAccessRightsTypes.Execute);
      
      CreateReportsTables();
    }
    
    #region Отчеты
    /// <summary>
    /// Создать таблицы для отчетов.
    /// </summary>
    public static void CreateReportsTables()
    {
      var reportTableName = CentrVD.Integration.Constants.EmployeeSignedDocumentsReport.SourceTableNameVersion2;
      var testReportTableName = CentrVD.Integration.Constants.EmployeeSignedDocumentsReport.TestSourceTableName;
      
      Sungero.Docflow.PublicFunctions.Module.DropReportTempTables(new[] { reportTableName, testReportTableName });
      
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(CentrVD.Integration.Queries.EmployeeSignedDocumentsReport.CreateReportTable, new[] { reportTableName });
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(CentrVD.Integration.Queries.TestReport.CreateTestReportTable, new[] { testReportTableName });
    }
    #endregion
    
  }
}
