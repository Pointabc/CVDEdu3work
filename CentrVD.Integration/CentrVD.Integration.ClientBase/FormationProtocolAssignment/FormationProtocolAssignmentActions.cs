using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.FormationProtocolAssignment;

namespace CentrVD.Integration.Client
{
  partial class FormationProtocolAssignmentActions
  {
    public virtual void DoneLessMinutes(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      
    }

    public virtual bool CanDoneLessMinutes(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

    public virtual void SendForRevote(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      
    }

    public virtual bool CanSendForRevote(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

    public virtual void Complete(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      _obj.Save();
      var protocol = CentrVD.Integration.AbsenteeVotingTasks.As(_obj.Task).ProtocolGroup.OfficialDocuments.FirstOrDefault();
      if (protocol == null)
      {
        // Создать новый документ с типом и видом указанным в задании
        // TODO VS пока добавляем любой документ
        var document = Sungero.Docflow.OfficialDocuments.GetAll().FirstOrDefault();
        var task = CentrVD.Integration.AbsenteeVotingTasks.As(_obj.Task);
        task.ProtocolGroup.OfficialDocuments.Add(document);
        // Выдать права
        PublicFunctions.Module.GrantRights(task, Sungero.Core.DefaultAccessRightsTypes.Read);
        document.AccessRights.Grant(_obj.Author, Sungero.Core.DefaultAccessRightsTypes.FullAccess);
      }
      else
      {
        // Протокол есть, создать новую версию, в которую записывается сформированный отчет
        var template = Sungero.Docflow.DocumentTemplates.GetAll().Where(d => d.HasVersions).OrderByDescending(d => d.Id).FirstOrDefault();
        Sungero.Content.Shared.ElectronicDocumentUtils.CreateVersionFrom(protocol, template);
        protocol.Save();
        //protocol.
      }
      //Sungero.Docflow.PublicFunctions.OfficialDocument.AddRelatedDocumentsToAttachmentGroup(document, _obj.OtherGroup);
    }

    public virtual bool CanComplete(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

  }

}