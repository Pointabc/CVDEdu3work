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
      
    }

    public virtual bool CanComplete(Sungero.Workflow.Client.CanExecuteResultActionArgs e)
    {
      return true;
    }

  }

}