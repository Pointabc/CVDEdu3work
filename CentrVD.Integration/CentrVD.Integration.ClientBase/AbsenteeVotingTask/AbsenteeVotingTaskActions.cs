using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration.Client
{
  partial class AbsenteeVotingTaskActions
  {
    public override void Start(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      // Проверить наличие Документа на голосование
      var document = _obj.DocumentForVotingGroup.OfficialDocuments.FirstOrDefault();
      if (document == null)
      {
        e.AddError("Добавьте документ на голосование.");
        return;
      }
      
      base.Start(e);
    }

    public override bool CanStart(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return base.CanStart(e);
    }

  }

}