using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Meeting;

namespace CentrVD.Category3.Server
{
  partial class MeetingFunctions
  {

    /// <summary>
    /// Создать уведомление участникам об изменении Даты и времени и Места совещания.
    /// </summary>
    [Remote]
    public void CreateNotification()
    {
      string subject;
      if (_obj.Location != null)
        subject = string.Format("Новая дата и время совещания {0}, место совещания {1}.", _obj.DateTime, _obj.Location);
      else
        subject = string.Format("Новая дата и время совещания {0}, место совещания {1}.", _obj.DateTime, "Неопределено");
      
      var recipients = _obj.Members.Select(e => e.Member).ToList();
      if (_obj.Secretary != null)
        recipients.Add(_obj.Secretary);
      if (_obj.President != null)
        recipients.Add(_obj.President);
      if (!recipients.Any())
        return;
      
      var notification = Sungero.Workflow.SimpleTasks.CreateWithNotices(subject, recipients.ToArray());
      notification.Start();
    }

  }
}