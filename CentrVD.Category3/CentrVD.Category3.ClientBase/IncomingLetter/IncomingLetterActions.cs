using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.IncomingLetter;

namespace CentrVD.Category3.Client
{
  partial class IncomingLetterActions
  {
    public virtual void AddStampOnPublicVersion(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      CentrVD.Category3.PublicFunctions.OfficialDocument.Remote.ConvertToPdfAndAddSignatureMark(_obj);
      //base.ConvertToPdf(e);
      //base.AddRegistrationStamp(e);
    }

    public virtual bool CanAddStampOnPublicVersion(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}