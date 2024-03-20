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
      CreateStampInDocumentStage();
    }
    
    /// <summary>
    /// Создание записи нового типа сценария "Автонумерация служебных записок".
    /// </summary>
    public static void CreateStampInDocumentStage()
    {
      InitializationLogger.DebugFormat("Init: Create stage for add stamp in document.");
      if (CentrVD.Integration.AddStampToDocuments.GetAll().Any())
        return;

      var stage = CentrVD.Integration.AddStampToDocuments.Create();
      stage.Name = "Добавить штамп на публичную версию документа.";
      stage.TimeoutInHours = 4;
      stage.Save();

    }
    
    #region Отчеты
    /// <summary>
    /// Создать таблицы для отчетов.
    /// </summary>
    public static void CreateReportsTables()
    {
      var reportTableName = CentrVD.Integration.Constants.EmployeeSignedDocumentsReportNew.SourceTableNameVersionV2;
      var testReportTableName = CentrVD.Integration.Constants.EmployeeSignedDocumentsReport.TestSourceTableName;
      var reportTableNameVersion3 = CentrVD.Integration.Constants.EmployeeSignedDocumentsReport.TestSourceTableName;
      
      Sungero.Docflow.PublicFunctions.Module.DropReportTempTables(new[] { reportTableName, testReportTableName, reportTableNameVersion3 });
      
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(CentrVD.Integration.Queries.EmployeeSignedDocumentsReportNew.CreateReportTable, new[] { reportTableName });
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(CentrVD.Integration.Queries.TestReport.CreateTestReportTable, new[] { reportTableNameVersion3 });
    }
    #endregion
    
  }
}
