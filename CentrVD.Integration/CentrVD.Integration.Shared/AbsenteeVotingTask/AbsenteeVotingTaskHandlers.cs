using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration
{
  partial class AbsenteeVotingTaskSharedHandlers
  {

    public virtual void DocumentForVotingGroupDeleted(Sungero.Workflow.Interfaces.AttachmentDeletedEventArgs e)
    {
      Functions.AbsenteeVotingTask.FillSubject(_obj);
    }

    public virtual void DocumentForVotingGroupAdded(Sungero.Workflow.Interfaces.AttachmentAddedEventArgs e)
    {
      Functions.AbsenteeVotingTask.FillSubject(_obj);
    }

    public virtual void DocumentForVotingGroupCreated(Sungero.Workflow.Interfaces.AttachmentCreatedEventArgs e)
    {
      Functions.AbsenteeVotingTask.FillSubject(_obj);
    }

  }
}