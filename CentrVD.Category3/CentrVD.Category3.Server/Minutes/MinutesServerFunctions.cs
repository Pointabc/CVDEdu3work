using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Minutes;

namespace CentrVD.Category3.Server
{
  partial class MinutesFunctions
  {
    
    /// <summary>
    /// Получить список наблюдателей совещания с должностью.
    /// </summary>
    /// <param name="minutes">Протокол.</param>
    /// <returns>Список участников совещания с должностью.</returns>
    [Converter("GetMeetingObserversWithJobTitle")]
    public static string GetMeetingObserversWithJobTitle(CentrVD.Category3.IMinutes minutes)
    {
      if (minutes.Meeting == null)
        return null;
      
      return CentrVD.Category3.PublicFunctions.Meeting.Remote.GetMeetingObserversString(CentrVD.Category3.Meetings.As(minutes.Meeting), true, true);
    }
    
    /// <summary>
    /// Получить список наблюдателей совещания без должности.
    /// </summary>
    /// <param name="minutes">Протокол.</param>
    /// <returns>Список наблюдателей совещания.</returns>
    [Converter("GetMeetingObservers")]
    public static string GetMeetingObserversFullMinutes(CentrVD.Category3.IMinutes minutes)
    {
      if (minutes.Meeting == null)
        return null;
      
      return CentrVD.Category3.PublicFunctions.Meeting.Remote.GetMeetingObserversString(CentrVD.Category3.Meetings.As(minutes.Meeting), true, false);
    }
  }
  
}