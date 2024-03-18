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
    /// Получить нумерованный список наблюдателей совещания.
    /// </summary>
    /// <param name="onlyMembers">Признак отображения только списка участников.</param>
    /// <param name="withJobTitle">Признак отображения должности наблюдателей.</param>
    /// <returns>Нумерованный список наблюдателей совещания.</returns>
    [Remote, Public]
    public virtual string GetMeetingObserversString(bool onlyObservers, bool withJobTitle)
    {
      var observers = _obj.Observers.Select(x => x.Observer).ToList();
      var employees = Sungero.Company.PublicFunctions.Module.Remote.GetEmployeesFromRecipientsRemote(observers);
      if (_obj.Secretary != null)
        employees.Insert(0, _obj.Secretary);
      if (_obj.President != null)
        employees.Insert(0, _obj.President);
      
      if (onlyObservers)
        employees = employees.Where(x => !Equals(x, _obj.President))
          .Where(x => !Equals(x, _obj.Secretary))
          .ToList();
      
      return Sungero.Company.PublicFunctions.Employee.Remote.GetEmployeesNumberedList(employees, withJobTitle);
    }
    
    /// <summary>
    /// Добавить получателей в наблюдателей совещания, исключая дублирующие записи.
    /// </summary>
    /// <param name="recipient">Реципиент.</param>
    [Public, Remote]
    public void SetRecipientToObservers(IRecipient recipient)
    {
      var employees = Sungero.Company.PublicFunctions.Module.Remote.GetEmployeesFromRecipientsRemote(new List<IRecipient> { recipient });
      
      var currentEmployees = _obj.Observers.Where(x => Sungero.Company.Employees.Is(x.Observer))
        .Select(x => Sungero.Company.Employees.As(x.Observer));
      employees = employees.Except(currentEmployees)
        .Where(x => !Equals(x, _obj.Secretary))
        .Where(x => !Equals(x, _obj.President))
        .ToList();
      
      foreach (var employee in employees)
        _obj.Observers.AddNew().Observer = employee;
    }
    
    /// <summary>
    /// Выдать права на документ участникам совещания.
    /// </summary>
    /// <param name="meeting">Совещание.</param>
    /// <param name="document">Документ.</param>
    [Public]
    public static void SetAccessRightsOnDocumentToObservers(CentrVD.Category3.IMeeting meeting, Sungero.Content.IElectronicDocument document)
    {
      if (meeting == null)
        return;
      
      if (document.AccessRights.StrictMode == AccessRightsStrictMode.Enhanced)
        return;
      
      var observers = meeting.Observers.Select(m => m.Observer).ToList();
      foreach (var observer in observers)
        if (!document.AccessRights.IsGrantedDirectly(DefaultAccessRightsTypes.Read, observer))
          document.AccessRights.Grant(observer, DefaultAccessRightsTypes.Read);
    }

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