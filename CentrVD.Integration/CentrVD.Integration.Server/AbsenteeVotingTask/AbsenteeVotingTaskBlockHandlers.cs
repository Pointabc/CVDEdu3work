using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Workflow;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration.Server.AbsenteeVotingTaskBlocks
{
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