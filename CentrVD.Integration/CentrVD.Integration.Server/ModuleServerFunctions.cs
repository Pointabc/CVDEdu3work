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
    /// Выдать всем участникам Заочного голосования права на документ на согласование и протокол.
    /// </summary>
    [Public]
    public static void GrantRights(CentrVD.Integration.IAbsenteeVotingTask _obj, Guid rightType)
    {
      // Выдать права на ЧТЕНИЕ для документа на согласование
      var document = _obj.DocumentForVotingGroup.OfficialDocuments.FirstOrDefault();
      var protocol = _obj.ProtocolGroup.OfficialDocuments.FirstOrDefault();
      var recipients = _obj.CommitteeMembers.Select(m => m.Member).ToList();
      
      if (document != null)
      {
        document.AccessRights.Grant(_obj.Chairman, rightType);
        document.AccessRights.Grant(_obj.Secretary, rightType);
        foreach(Sungero.Company.IEmployee member in recipients)
          document.AccessRights.Grant(member, rightType);
        document.AccessRights.Save();
      }
      
      if (protocol != null)
      {
        protocol.AccessRights.Grant(_obj.Chairman, rightType);
        protocol.AccessRights.Grant(_obj.Secretary, rightType);
        foreach(Sungero.Company.IEmployee member in recipients)
          protocol.AccessRights.Grant(member, rightType);
        protocol.AccessRights.Save();
      }
    }

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