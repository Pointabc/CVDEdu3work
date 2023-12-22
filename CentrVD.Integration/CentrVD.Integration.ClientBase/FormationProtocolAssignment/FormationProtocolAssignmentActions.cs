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
      var task = CentrVD.Integration.AbsenteeVotingTasks.As(_obj.Task);
      var protocol = task.ProtocolGroup.OfficialDocuments.FirstOrDefault();
      if (protocol != null)
        Dialogs.ShowMessage(string.Format("По результатам голосования будет сформирован и отправлен на подпись новый документ {0}", protocol.Name));
    }

    public virtual bool CanComplete(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

  }

}