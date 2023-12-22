using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.SigningProtocolAssignment;

namespace CentrVD.Integration.Client
{
  partial class SigningProtocolAssignmentActions
  {
    public virtual void RetToInitiator(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      
    }

    public virtual bool CanRetToInitiator(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }


    public virtual void Complete(Sungero.Workflow.Client.ExecuteResultActionArgs e)
    {
      if (_obj.Status == CentrVD.Integration.SigningProtocolAssignment.Status.InProcess)
      {
        var task = CentrVD.Integration.AbsenteeVotingTasks.As(_obj.Task);
        var protocol = task.ProtocolGroup.OfficialDocuments.FirstOrDefault();
        if (protocol != null)
        {
          var lastVersion = protocol.LastVersion;
          if (lastVersion != null)
            Sungero.Core.Signatures.Approve(protocol.LastVersion, "Подписано.");
        }
      }
    }

    public virtual bool CanComplete(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

  }

}