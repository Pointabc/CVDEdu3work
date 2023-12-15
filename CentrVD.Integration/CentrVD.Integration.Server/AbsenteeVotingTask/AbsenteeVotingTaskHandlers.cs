using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration
{
  partial class AbsenteeVotingTaskServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      Functions.AbsenteeVotingTask.FillSubject(_obj);
    }
  }

}