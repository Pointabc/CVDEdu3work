using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration.Server
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Тема совещания.</param>
    /// <param name="datetime">Дата совещания со временем.</param>
    /// <param name="duration">Длительность совещания.</param>
    /// <param name="location">Место совещания.</param>
    [Public(WebApiRequestType = RequestType.Post)]
    public void CreateMeeting(string name, string datetime, double duration, string location)
    {
      var meeting = Sungero.Meetings.Meetings.Create();
      meeting.Name = name;
      meeting.Duration = duration;
      DateTime dt;
      meeting.DateTime = Calendar.TryParseDateTime(datetime, out dt) ? DateTime.Parse(datetime) : Calendar.Now;
      meeting.Location = location;
      meeting.Save();
    }

  }
}