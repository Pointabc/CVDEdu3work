using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Minutes;

namespace CentrVD.Category3
{
  partial class MinutesServerHandlers
  {

    public override void Saving(Sungero.Domain.SavingEventArgs e)
    {
      base.Saving(e);
      
      // Выдать права наблюдателям на повестку совещания
      CentrVD.Category3.PublicFunctions.Meeting.SetAccessRightsOnDocumentToObservers(CentrVD.Category3.Meetings.As(_obj.Meeting), _obj);
    }
  }

}