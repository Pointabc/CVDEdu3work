using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Agenda;

namespace CentrVD.Category3
{
  partial class AgendaServerHandlers
  {

    public override void Saving(Sungero.Domain.SavingEventArgs e)
    {
      base.Saving(e);
      
      // Выдать права Наблюдателям на протокол совещания
      CentrVD.Category3.PublicFunctions.Meeting.SetAccessRightsOnDocumentToObservers(CentrVD.Category3.Meetings.As(_obj.Meeting), _obj);
    }
  }

}