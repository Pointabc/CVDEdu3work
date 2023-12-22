using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.CheckProtocolAssignment;

namespace CentrVD.Integration.Client
{
  partial class CheckProtocolAssignmentActions
  {
    public virtual void Complete(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      if (_obj.Status == CentrVD.Integration.CheckProtocolAssignment.Status.InProcess)
      {
        var task = CentrVD.Integration.AbsenteeVotingTasks.As(_obj.Task);
        var protocol = task.ProtocolGroup.OfficialDocuments.FirstOrDefault();
        var lastVersion = protocol.LastVersion;
        if (lastVersion != null)
          Sungero.Core.Signatures.Approve(protocol.LastVersion, "Подписано.");
      }
    }

    public virtual bool CanComplete(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

  }

}