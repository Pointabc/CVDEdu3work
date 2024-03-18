using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Meeting;

namespace CentrVD.Category3.Client
{
  partial class MeetingActions
  {
    public virtual void AddObserver(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var observersCount = _obj.Observers.Count;
      var recipients = Sungero.Company.PublicFunctions.Module.GetAllActiveNoSystemGroups()
        .Where(x => !Sungero.Company.BusinessUnits.Is(x));
      
      var observer = recipients.ShowSelect(Meetings.Resources.SelectDepartmentOrRole);
      CentrVD.Category3.PublicFunctions.Meeting.Remote.SetRecipientToObservers(_obj, observer);
      
      // Если были выбраны новые участники, то показать всплывающее сообщение с результатами выбора.
      if (observer != null)
        if (observersCount == _obj.Observers.Count)
          Sungero.Core.Dialogs.NotifyMessage(Sungero.Meetings.Meetings.Resources.MembersAlreadyEntered);
        else Sungero.Core.Dialogs.NotifyMessage(Sungero.Meetings.Meetings.Resources.NewMembersAdded);
    }

    public virtual bool CanAddObserver(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}