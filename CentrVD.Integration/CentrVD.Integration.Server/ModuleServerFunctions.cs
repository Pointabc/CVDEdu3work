using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration.Server
{
  public class ModuleFunctions
  {

    /// <summary>
    /// Заполнить SQL таблицу для формирования отчета "Сотрудник подписал документы".
    /// </summary>
    /// <param name="reportSessionId">Идентификатор отчета.</param>
    [Public]
    public static void UpdateEmployeeSignedDocumentReportTableV2(string reportSessionId)
    {
      var commandText = CentrVD.Integration.Queries.EmployeeSignedDocumentsReportNew.DataSourceNew;
      var table = new List<Structures.EmployeeSignedDocumentsReportNew.Record>();
      using (var command = SQL.CreateConnection().CreateCommand())
      {
        command.CommandText = commandText;
        using(System.Data.IDataReader reader = command.ExecuteReader())
        {
          var cnt = 0;
          while (reader.Read())
          {
            var v0 = reader.GetString(0); // JobTitle
            var v1 = reader.GetString(1); // Employee
            var v2 = reader.GetString(2); // Department
            var v3 = reader.GetString(3); // BusinessUnit
            var v4 = reader.GetString(4); // DocName 
            //var v5 = reader.GetString(5); // DocLink
            var v5 = reader.GetInt32(5); //
            var v7 = reader.GetDateTime(6); //
            
            var tableLine = new Structures.EmployeeSignedDocumentsReportNew.Record();
            tableLine.ReportSessionld = reportSessionId; 
            tableLine.JobTitle = reader.GetString(0); // JobTitle
            tableLine.Employee = reader.GetString(1); // Employee
            tableLine.Department = reader.GetString(2); // Department
            tableLine.BusinessUnit = reader.GetString(3); // BusinessUnit
            tableLine.DocumentName = reader.GetString(4); // DocName
            tableLine.DocumentLink = "";//reader.GetString(5) ?? "";
            tableLine.SignedResult = reader.GetInt32(5);
            //tableLine.D = reader.GetDateTime(7);
            table.Add(tableLine);
            //cnt++;
          }
        }
      }
      
      // Записываем в таблицу
      Sungero.Docflow.PublicFunctions.Module.WriteStructuresToTable(Constants.EmployeeSignedDocumentsReportNew.SourceTableNameVersionV2, table);
      
      /*using (var command = SQL.CreateConnection().CreateCommand())
      {
        /*var jobTitle = "Разработчик";
        var department = "ИТ отдел";
        var documentName = Sungero.Docflow.OfficialDocuments.GetAll().FirstOrDefault().Name;
        var signedResult = true;
        
        command.CommandText = commandText;
        SQL.AddParameter(command, "@reportSessionId",  reportSessionId, System.Data.DbType.String);
        SQL.AddParameter(command, "@jobTitle",  jobTitle, System.Data.DbType.String);
        SQL.AddParameter(command, "@department",  department, System.Data.DbType.String);
        SQL.AddParameter(command, "@documentName",  documentName, System.Data.DbType.String);
        SQL.AddParameter(command, "@signedResult",  signedResult, System.Data.DbType.Boolean);
        
        command.ExecuteNonQuery();
      }*/
    }
    
    /// <summary>
    /// Заполнить SQL таблицу для формирования отчета "Сотрудник подписал документы".
    /// </summary>
    /// <param name="reportSessionId">Идентификатор отчета.</param>
    [Public]
    public static void UpdateEmployeeSignedDocumentReportTable(string reportSessionId)
    {
      var commandText = CentrVD.Integration.Queries.EmployeeSignedDocumentsReport.InsertIntoReportTable;
      
      using (var command = SQL.CreateConnection().CreateCommand())
      {
        //var separator = ", ";
        /*var errorString = Docflow.PublicFunctions.Module.GetSignatureValidationErrorsAsString(signature, separator);
        var signErrors = string.IsNullOrWhiteSpace(errorString)
          ? Reports.Resources.ApprovalSheetReport.SignStatusActive
          : Docflow.PublicFunctions.Module.ReplaceFirstSymbolToUpperCase(errorString.ToLower());
        var resultString = Functions.ApprovalTask.GetEndorsingResultFromSignature(signature, false);
        var comment = Docflow.Functions.Module.HasApproveWithSuggestionsMark(signature.Comment)
          ? Docflow.Functions.Module.RemoveApproveWithSuggestionsMark(signature.Comment)
          : signature.Comment;*/
        //var employeeName = "Вася";
        var jobTitle = "Разработчик";
        var department = "ИТ отдел";
        var documentName = Sungero.Docflow.OfficialDocuments.GetAll().FirstOrDefault().Name;
        var signedResult = true;
        
        command.CommandText = commandText;
        SQL.AddParameter(command, "@reportSessionId",  reportSessionId, System.Data.DbType.String);
        SQL.AddParameter(command, "@jobTitle",  jobTitle, System.Data.DbType.String);
        SQL.AddParameter(command, "@department",  department, System.Data.DbType.String);
        SQL.AddParameter(command, "@documentName",  documentName, System.Data.DbType.String);
        SQL.AddParameter(command, "@signedResult",  signedResult, System.Data.DbType.Boolean);
        
        command.ExecuteNonQuery();
      }
    }
    
    /// <summary>
    /// Заполнить SQL таблицу для формирования отчета "Лист согласования".
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <param name="reportSessionId">Идентификатор отчета.</param>
    [Public]
    public static void UpdateApprovalSheetReportTable(string reportSessionId)
    {
      var commandText = CentrVD.Integration.Queries.TestReport.InsertIntoTestReportTable;
      
      using (var command = SQL.GetCurrentConnection().CreateCommand())
      {
        //var separator = ", ";
        /*var errorString = Docflow.PublicFunctions.Module.GetSignatureValidationErrorsAsString(signature, separator);
        var signErrors = string.IsNullOrWhiteSpace(errorString)
          ? Reports.Resources.ApprovalSheetReport.SignStatusActive
          : Docflow.PublicFunctions.Module.ReplaceFirstSymbolToUpperCase(errorString.ToLower());
        var resultString = Functions.ApprovalTask.GetEndorsingResultFromSignature(signature, false);
        var comment = Docflow.Functions.Module.HasApproveWithSuggestionsMark(signature.Comment)
          ? Docflow.Functions.Module.RemoveApproveWithSuggestionsMark(signature.Comment)
          : signature.Comment;*/
        var employeeName = "Вася";
        command.CommandText = commandText;
        SQL.AddParameter(command, "@reportSessionId",  reportSessionId, System.Data.DbType.String);
        SQL.AddParameter(command, "@employeeName",  employeeName, System.Data.DbType.String);
        
        command.ExecuteNonQuery();
      }
    }

    /// <summary>
    /// Выдать всем участникам Заочного голосования права на документ на согласование и протокол.
    /// </summary>
    [Public]
    public static void GrantRights(CentrVD.Integration.IAbsenteeVotingTask _obj, Guid rightType)
    {
      // Выдать права на ЧТЕНИЕ для документа на согласование
      var document = _obj.DocumentForVotingGroup.OfficialDocuments.FirstOrDefault();
      var protocol = _obj.ProtocolGroup.OfficialDocuments.FirstOrDefault();
      var recipients = _obj.CommitteeMembers.Select(m => m.Member).ToList();
      
      if (document != null)
      {
        document.AccessRights.Grant(_obj.Chairman, rightType);
        document.AccessRights.Grant(_obj.Secretary, rightType);
        foreach(Sungero.Company.IEmployee member in recipients)
          document.AccessRights.Grant(member, rightType);
        document.AccessRights.Save();
      }
      
      if (protocol != null)
      {
        protocol.AccessRights.Grant(_obj.Chairman, rightType);
        protocol.AccessRights.Grant(_obj.Secretary, rightType);
        foreach(Sungero.Company.IEmployee member in recipients)
          protocol.AccessRights.Grant(member, rightType);
        protocol.AccessRights.Save();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Тема совещания.</param>
    /// <param name="datetime">Дата совещания со временем.</param>
    /// <param name="duration">Длительность совещания.</param>
    /// <param name="location">Место совещания.</param>
    [Public(WebApiRequestType = RequestType.Post)]
    public void CreateMeeting(string name, string datetime, double duration, string location)
    {
      var meeting = Sungero.Meetings.Meetings.Create();
      meeting.Name = name;
      meeting.Duration = duration;
      DateTime dt;
      meeting.DateTime = Calendar.TryParseDateTime(datetime, out dt) ? DateTime.Parse(datetime) : Calendar.Now;
      meeting.Location = location;
      meeting.Save();
    }

  }
}