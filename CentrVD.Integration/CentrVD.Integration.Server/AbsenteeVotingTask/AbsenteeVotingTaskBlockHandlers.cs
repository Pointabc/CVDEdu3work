using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Workflow;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration.Server.AbsenteeVotingTaskBlocks
{
  partial class ProtocolGenerationTaskBlockHandlers
  {

    public virtual void ProtocolGenerationTaskBlockEnd(System.Collections.Generic.IEnumerable<CentrVD.Integration.IFormationProtocolAssignment> createdAssignments)
    {
      if (createdAssignments.FirstOrDefault().Result.HasValue && createdAssignments.FirstOrDefault().Result == CentrVD.Integration.FormationProtocolAssignment.Result.Complete)
      {
        var task = CentrVD.Integration.AbsenteeVotingTasks.As(_obj);
        var protocol = task.ProtocolGroup.OfficialDocuments.FirstOrDefault();
        if (protocol == null)
        {
          // Создать протокол и выдать права сотруднику выполнившему задание Полные права, остальным участникам на просмотр
          var template = Sungero.Content.ElectronicDocumentTemplates.GetAll(t => t.Name.Contains("Microsoft Word")).FirstOrDefault();
          // Создать простой документ.
          if (template != null)
          {
            protocol = Sungero.Docflow.SimpleDocuments.CreateFrom(template);
            protocol.Name = string.Format("Протокол, {0}", _block.VoteReport != null ? _block.VoteReport.DisplayValue : "Не определено");
            protocol.Save();
          }

          task.ProtocolGroup.OfficialDocuments.Add(protocol);
          task.Save();
        }
        else
        {
          // Создать версию протокола и выдать права сотруднику выполнившему задание На изменение, остальным на просмотр.
          var template = Sungero.Docflow.DocumentTemplates.GetAll().Where(d => d.HasVersions).OrderByDescending(d => d.Id).FirstOrDefault();
          Sungero.Content.Shared.ElectronicDocumentUtils.CreateVersionFrom(protocol, template);
          protocol.Save();
        }
        // Выдать права на протокол
        PublicFunctions.Module.GrantRights(task, Sungero.Core.DefaultAccessRightsTypes.Read);
        var performer = _block.Performers.FirstOrDefault();
        protocol.AccessRights.Grant(performer, Sungero.Core.DefaultAccessRightsTypes.FullAccess);
        protocol.AccessRights.Save();
      }
    }
  }

  partial class GrantRightsScriptBlockHandlers
  {

    public virtual void GrantRightsScriptBlockExecute()
    {
      var right = _block.AccessRight.Right.GetValueOrDefault();
      if (right != null && right.Value == "Change")
        PublicFunctions.Module.GrantRights(_obj, Sungero.Core.DefaultAccessRightsTypes.Change);
      else if (right != null && right.Value == "Read")
        PublicFunctions.Module.GrantRights(_obj, Sungero.Core.DefaultAccessRightsTypes.Read);
    }
  }

}