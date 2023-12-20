using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration
{
  partial class AbsenteeVotingTaskClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      Functions.AbsenteeVotingTask.FillSubject(_obj);
    }

  }
}