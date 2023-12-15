using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Meeting;

namespace CentrVD.Category3
{
  partial class MeetingServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      if ((_obj.State.Properties.DateTime.OriginalValue != _obj.DateTime ||
           _obj.State.Properties.Location.OriginalValue != _obj.Location) &&
          !_obj.State.IsInserted)
        Functions.Meeting.CreateNotification(_obj);
      base.BeforeSave(e);
    }
  }

}