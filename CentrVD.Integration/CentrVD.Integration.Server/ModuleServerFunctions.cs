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
    public static void UpdateEmployeeSignedDocumentReportTableV2(string reportSessionId, List<IRecipient> usersAndGroups, string agreedApproved, bool isSignCert)
    {
      // Получаем всех сотрудников из подр, НОР, ролей и тд
      var users = new List<IRecipient>();
      foreach (var recipient in usersAndGroups)
      {
        if (!Groups.Is(recipient))
          users.Add(recipient);
        else
        {
          var group = Groups.As(recipient);
          var groupMembers = Groups.GetAllUsersInGroup(group)
            .Where(r => Sungero.Company.Employees.Is(r) && r.Status == Sungero.CoreEntities.DatabookEntry.Status.Active)
            .Select(r => Recipients.As(r));
          users.AddRange(groupMembers);
        }
      }
      
      var commandText = CentrVD.Integration.Queries.EmployeeSignedDocumentsReportNew.DataSourceNew;
      var table = new List<Structures.EmployeeSignedDocumentsReportNew.Record>();
      using (var command = SQL.CreateConnection().CreateCommand())
      {
        command.CommandText = commandText;
        using(System.Data.IDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            // Включаем с таблицу только сотрудников из recipients
            var employee = reader.GetString(1);
            var user = users.AsQueryable().Where(e => e.DisplayValue == employee).FirstOrDefault();
            if (user == null)
              continue;
            // Включаем в таблицу Согласовано или Утверждено в зависимости от выбора пользователя
            if (agreedApproved != CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.ValueIsNotSet)
            {
              var approved = reader.GetInt32(5);
              if (agreedApproved == CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Approved && approved != 1)
                continue;
              if (agreedApproved == CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Agreed && approved != 0)
                continue;
            }
            
            var documentId = reader.GetInt64(7); // DocumentID
            var document = Sungero.Docflow.OfficialDocuments.Get(documentId);
            
            var v0 = reader.GetString(0); // JobTitle
            var v1 = reader.GetString(1); // Employee
            var v2 = reader.GetString(2); // Department
            var v3 = reader.GetString(3); // BusinessUnit
            var v4 = reader.GetString(4); // DocName
            var v5 = reader.GetInt32(5); // Signed Result
            var v7 = reader.GetDateTime(6); // DateSign
           
            //var v9 = reader.GetBytes(8); // DateSign
            
            var tableLine = new Structures.EmployeeSignedDocumentsReportNew.Record();
            tableLine.ReportSessionId = reportSessionId;
            tableLine.JobTitle = reader.GetString(0); // JobTitle
            tableLine.Employee1 = reader.GetString(1); // Employee
            tableLine.Department = reader.GetString(2); // Department
            tableLine.BusinessUnit1 = reader.GetString(3); // BusinessUnit
            tableLine.DocumentName = reader.GetString(4); // DocName
            tableLine.DocumentLink1 = Sungero.Core.Hyperlinks.Get(document);
            if (document != null && isSignCert)
            {
              var signatures = Sungero.Core.Signatures.Get(document).Where(s => s.SignCertificate != null).FirstOrDefault();
              if (signatures == null)
                continue;
            }
            var signedResult = reader.GetInt32(5); // Signed Result
            tableLine.SignedResult = signedResult == 0 ? CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Agreed :
              CentrVD.Integration.Reports.Resources.EmployeeSignedDocumentsReportNew.Approved;
            tableLine.DateSign = reader.GetDateTime(6);
            table.Add(tableLine);
          }
        }
      }
      
      // Записываем структуру в таблицу
      Sungero.Docflow.PublicFunctions.Module.WriteStructuresToTable(Constants.EmployeeSignedDocumentsReportNew.SourceTableNameVersionV2, table);
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