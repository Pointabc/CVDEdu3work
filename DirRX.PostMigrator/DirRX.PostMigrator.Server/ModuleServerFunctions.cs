using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using System.Security.Cryptography.X509Certificates;

namespace DirRX.PostMigrator.Server
{
	public class ModuleFunctions
	{
	  #region НОР
    
    /// <summary>
    /// Получить ИД НОР.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static string GetIdBusinessUnits()
    {
      var command = string.Format(Queries.Module.SelectMigratedRecipient, Constants.Module.Discriminators.BusinessUnitDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
    }
    
    /// <summary>
    /// Пересохранить НОР.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static bool ProcessBusinessUnitBlock(string idBusinessUnitsBlock)
    {
      var list = ConvertStringToIntList(idBusinessUnitsBlock, Constants.Module.Separator);
      Logger.DebugFormat("Количество обрабатываемых НОР в блоке: {0}", list.Count);

      var result = true;
      var successSaved = new List<int>();
      var businessUnits = Sungero.Company.BusinessUnits.GetAll().Where(p => list.Contains(p.Id));
      foreach (var item in businessUnits)
      {
        item.Note += string.Empty;
        try
        {
          item.Save();
          successSaved.Add(item.Id);
        }
        catch(Exception ex)
        {
          Logger.DebugFormat("Не удалось сохранить НОР \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
          SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.BusinessUnitDiscriminator.ToString(), Constants.Module.Resaved);
          result = false;
        }
      }
      
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.BusinessUnitDiscriminator.ToString(), Constants.Module.Resaved);
      return result;
    }
    
    #endregion
    
	  #region Подразделения
    
    /// <summary>
    /// Получить ИД подразделений.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static string GetIdDepartments()
    {
      var command = string.Format(Queries.Module.SelectMigratedRecipient, Constants.Module.Discriminators.DepartmentDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
    }
    
    /// <summary>
    /// Пересохранить подразделения.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static bool ProcessDepartmentBlock(string idDepartmentsBlock)
    {
      var list = ConvertStringToIntList(idDepartmentsBlock, Constants.Module.Separator);
      Logger.DebugFormat("Количество обрабатываемых подразделений в блоке: {0}", list.Count);

      var result = true;
      var successSaved = new List<int>();
      var departments = Sungero.Company.Departments.GetAll().Where(p => list.Contains(p.Id));
      foreach (var item in departments)
      {
        item.Note += string.Empty;
        try
        {
          item.Save();
          successSaved.Add(item.Id);
        }
        catch(Exception ex)
        {
          Logger.DebugFormat("Не удалось сохранить подразделение \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
          SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.DepartmentDiscriminator.ToString(), Constants.Module.Resaved);
          result = false;
        }
      }
      
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.DepartmentDiscriminator.ToString(), Constants.Module.Resaved);
      return result;
    }
    
    #endregion

    #region Персоны
    
    /// <summary>
    /// Получить ИД персон.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static string GetIdPersons()
    {
      var command = string.Format(Queries.Module.SelectMigratedCounterparty, Constants.Module.Discriminators.PersonDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
    }
    
    /// <summary>
    /// Процедура перезаписи персон поблочно.
    /// </summary>
    /// <param name="persons">Структурированный набор данных персонё.</param>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual bool ProcessPersonBlock(string idPersonsBlock)
    {
      var list = ConvertStringToIntList(idPersonsBlock, Constants.Module.Separator);
      Logger.DebugFormat("Количество обрабатываемых персон в блоке: {0}", list.Count);

      var result = true;
      var successSaved = new List<int>();
      var persons = Sungero.Parties.People.GetAll().Where(p => list.Contains(p.Id));
      foreach (var item in persons)
      {
        item.Note += string.Empty;
        try
        {
          item.Save();
          successSaved.Add(item.Id);
        }
        catch(Exception ex)
        {
          Logger.DebugFormat("Не удалось сохранить персону \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
          SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.PersonDiscriminator.ToString(), Constants.Module.Resaved);
          result = false;
        }
      }
      
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.PersonDiscriminator.ToString(), Constants.Module.Resaved);
      return result;
    }
    
    #endregion
    
    #region Учетные записи
    
    /// <summary>
		/// Получить ИД учетных записей.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public static string GetIdLogins()
		{
		  var values = Sungero.CoreEntities.Logins.GetAll().Select(i => i.Id).ToList();
		  return ConvertListToString(values, Constants.Module.Separator);
		}
		
    /// <summary>
		/// Пересохранить логины.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public virtual bool ProcessLoginBlock(string idLoginsBlock)
		{
      var list = ConvertStringToIntList(idLoginsBlock, Constants.Module.Separator);
			Logger.Debug(String.Format("Количество обрабатываемых учетных записей в блоке: {0}", list.Count()));

			var result = true;
			var successSaved = new List<int>();
			var logins = Sungero.CoreEntities.Logins.GetAll().Where(p => list.Contains(p.Id));
			logins = logins.Where(x=>x.LoginName != "Administrator");
			logins = logins.Where(x=>x.LoginName != "Integration Service");
			logins = logins.Where(x=>x.LoginName != "Adviser");
			logins = logins.Where(x=>x.LoginName != "Service User");
			foreach (var item in logins)
			{
				item.LoginName += string.Empty;
				try
				{
					item.Save();
					successSaved.Add(item.Id);
				}
				catch(Exception ex)
				{
					Logger.DebugFormat("Не удалось сохранить учетную запись \"{0}\". Внутренняя ошибка: {1}", item.LoginName, ex.ToString());
					SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.LoginDiscriminator.ToString(), Constants.Module.Resaved);
					result = false;
				}
			}
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.LoginDiscriminator.ToString(), Constants.Module.Resaved);
			return result;
		}
    #endregion
    
    #region Роли
    /// <summary>
		/// Получить ИД ролей.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public static string GetIdRoles()
		{
      var command = string.Format(Queries.Module.SelectMigratedRecipient, Constants.Module.Discriminators.RoleDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
		}
		
		/// <summary>
		/// Пересохранить роли.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public virtual bool ProcessRoleBlock(string idRolesBlock)
		{
      var list = ConvertStringToIntList(idRolesBlock, Constants.Module.Separator);
			Logger.Debug(String.Format("Количество обрабатываемых ролей в блоке: {0}", list.Count()));

			var result = true;
			var successSaved = new List<int>();
			var roles = Sungero.CoreEntities.Roles.GetAll().Where(p => list.Contains(p.Id));
			roles = roles.Where(x => x.IsSystem != true);
			foreach (var item in roles)
			{
				item.Description += string.Empty;
				try
				{
					item.Save();
					successSaved.Add(item.Id);
				}
				catch(Exception ex)
				{
					Logger.DebugFormat("Не удалось сохранить роль \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
					SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.RoleDiscriminator.ToString(), Constants.Module.Resaved);
					result = false;
				}
			}
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.RoleDiscriminator.ToString(), Constants.Module.Resaved);
			return result;
		}
    #endregion

    #region Сотрудники
    
    /// <summary>
    /// Получить ИД сотрудников.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public static string GetIdEmployees()
    {
      var command = string.Format(Queries.Module.SelectMigratedRecipient, Constants.Module.Discriminators.EmployeeDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
    }
    
    /// <summary>
    /// Пересохранить сотрудников.
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual bool ProcessEmployeesBlock(string idEmployeesBlock)
    {
      var list = ConvertStringToIntList(idEmployeesBlock, Constants.Module.Separator);
      Logger.DebugFormat("Количество обрабатываемых сотрудников в блоке: {0}", list.Count);

      var result = true;
      var successSaved = new List<int>();
      var employees = Sungero.Company.Employees.GetAll().Where(p => list.Contains(p.Id));
      foreach (var item in employees)
      {
        item.Note += string.Empty;
        try
        {
          item.Save();
          successSaved.Add(item.Id);
        }
        catch(Exception ex)
        {
          Logger.DebugFormat("Не удалось сохранить сотрудника \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
          SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.EmployeeDiscriminator.ToString(), Constants.Module.Resaved);
          result = false;
        }
      }
      
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.EmployeeDiscriminator.ToString(), Constants.Module.Resaved);
      return result;
    }
    
    #endregion
    
    #region Группы регистрации
    /// <summary>
		/// Получить ИД групп регистрации.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public static string GetIdRegistrationGroups()
		{			
      var command = string.Format(Queries.Module.SelectMigratedRecipient, Constants.Module.Discriminators.RegistrationGroupDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
		}
		
		/// <summary>
		/// Пересохранить групп регистрации.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public virtual bool ProcessRegistrationGroupBlock(string idRegistrationGroupsBlock)
		{
      var list = ConvertStringToIntList(idRegistrationGroupsBlock, Constants.Module.Separator);
			Logger.Debug(String.Format("Количество обрабатываемых групп регистрации в блоке: {0}", list.Count()));

			var result = true;
			var successSaved = new List<int>();
			var registrationGroups = Sungero.Docflow.RegistrationGroups.GetAll().Where(p => list.Contains(p.Id));
			foreach (var item in registrationGroups)
			{
				item.Description += string.Empty;
				try
				{
					item.Save();
					successSaved.Add(item.Id);
				}
				catch(Exception ex)
				{
					Logger.DebugFormat("Не удалось сохранить группу регистрации \"{0}\". Внутренняя ошибка: {1}", item.Name, ex.ToString());
					SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.RegistrationGroupDiscriminator.ToString(), Constants.Module.Resaved);
					result = false;
				}
			}
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.RegistrationGroupDiscriminator.ToString(), Constants.Module.Resaved);
			return result;
		}
    #endregion

    #region Сертификаты
		/// <summary>
		/// Получить ИД цифровых сертификатов.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public static string GetIdCertificates()
		{
      var command = string.Format(Queries.Module.SelectCertificates, Constants.Module.Discriminators.CertificateDiscriminator.ToString());
      return ConvertListToString(ExecuteSQLSelect(command), Constants.Module.Separator);
		}
		
		/// <summary>
		/// Пересохранить сертификаты.
		/// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
		public virtual bool ProcessCertificateBlock(string idCertificatesBlock)
		{
      var list = ConvertStringToIntList(idCertificatesBlock, Constants.Module.Separator);
			Logger.Debug(String.Format("Количество обрабатываемых цифровых сертификатов в блоке: {0}", list.Count()));

			var result = true;
			var successSaved = new List<int>();
			var certificates = Sungero.CoreEntities.Certificates.GetAll().Where(p => list.Contains(p.Id));
			foreach (var item in certificates)
			{
				var x509Certificate = new X509Certificate2(item.X509Certificate);
				item.Issuer = x509Certificate.GetNameInfo(X509NameType.SimpleName, true);
				item.NotAfter = x509Certificate.NotAfter;
				item.NotBefore = x509Certificate.NotBefore;
				item.Subject = x509Certificate.GetNameInfo(X509NameType.SimpleName, false);
				item.Thumbprint = x509Certificate.Thumbprint;
				if (!item.Enabled.HasValue)
					item.Enabled = item.NotAfter.HasValue && item.NotAfter.Value > Calendar.Now;
				
				try
				{
					item.Save();
					successSaved.Add(item.Id);
				}
				catch(Exception ex)
				{
					Logger.DebugFormat("Не удалось сохранить цифровой сертификат \"{0}\". Внутренняя ошибка: {1}", item.DisplayValue, ex.ToString());
					SetDatabookItemError(item.Id, ex.Message, Constants.Module.Discriminators.CertificateDiscriminator.ToString(), Constants.Module.Resaved);
					result = false;
				}
			}
      SetDatabookItemStatus(successSaved, true, Constants.Module.Discriminators.CertificateDiscriminator.ToString(), Constants.Module.Resaved);
			return result;
		}
		
    #endregion

    #region Служебные функции
    
    /// <summary>
    /// Конвертировать строку в список.
    /// </summary>
    /// <param name="text">Строка.</param>
    /// <param name="separator">Разделитель.</param>
    /// <returns>Список элементов строки.</returns>
    private static List<int> ConvertStringToIntList(string text, string separator)
    {
      if (string.IsNullOrWhiteSpace(text))
        return new List<int>();
      else
      {
        return text.Split(separator.ToCharArray()).Select(int.Parse).ToList();
      }
    }

    /// <summary>
    /// Конвертировать список в строку.
    /// </summary>
    /// <param name="list">Список.</param>
    /// <param name="separator">Разделитель.</param>
    /// <returns>Список элементов строки.</returns>
    private static string ConvertListToString(List<int> list, string separator)
    {
      return string.Join<int>(separator, list);
    }
    
    /// <summary>
    /// Установить статус объектов в временной таблице миграции.
    /// </summary>
    /// <param name="itemIdList">Список ИД объектов.</param>
    /// <param name="status">Статус.</param>
    /// <param name="discriminator">Дискриминатор сущности.</param>
    /// <param name="column">Столбец статуса.</param>
    private static void SetDatabookItemStatus(List<int> itemIdList, bool status, string discriminator, string column)
    {
      if (itemIdList.Count > 0)
      {
        var sqlValue = status ? "1" : "0";
        var args = new object[] {column, sqlValue, ConvertListToSqlList(itemIdList), discriminator};
        Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(
          Queries.Module.SetDatabookItemStatus, args);
      }
    }
    
    /// <summary>
    /// Сохранить ошибку объекта в временной таблице миграции.
    /// </summary>
    /// <param name="itemId">ИД объекта.</param>
    /// <param name="errorMessage">Текс ошибки.</param>
    /// <param name="discriminator">Дискриминатор сущности.</param>
    /// <param name="column">Столбец статуса.</param>
    private static void SetDatabookItemError(int itemId, string errorMessage, string discriminator, string column)
    {
      
      var args = new object[] {column, errorMessage, itemId, discriminator};
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(
        Queries.Module.SetDatabookItemError, args);
    }
    
    /// <summary>
    /// Конвертировать список в SQL список.
    /// </summary>
    /// <param name="list">Список.</param>
    /// <returns>Строка SQL списка.</returns>
    public static string ConvertListToSqlList(List<int> list)
    {
      var count = list.Count;
      var result = string.Empty;
      switch (count)
      {
        case 0:
          throw AppliedCodeException.Create("Ошибка конвертации списка в SQL-список: список пуст.");
        case 1:
          result = string.Format("'{0}'", list[0]);
          break;
        default:
          var lastIndex = list.Count - 1;
          var stringBuilder = new System.Text.StringBuilder();
          for (int i = 0; i < lastIndex; i++)
          {
            stringBuilder.AppendFormat("'{0}', ", list[i]);
          }
          stringBuilder.AppendFormat("'{0}'", list[lastIndex]);
          result = stringBuilder.ToString();
          break;
      }
      return result;
    }
    
    /// <summary>
    /// Получить список ИД из временной таблицы миграции по дискриминатору.
    /// </summary>
    /// <param name="commandText">Константу с дискриминатором сущности.</param>
    private static List<int> GetListIdByDiscriminator(string discriminator)
    {
      var commandText = string.Format(Queries.Module.GetListIdByDiscriminator, discriminator);
      return ExecuteSQLSelect(commandText);
    }
    
    /// <summary>
    /// Получить список ИД мигрированных сертификатов.
    /// </summary>
    /// <param name="commandText">Константу с дискриминатором сущности.</param>
    private static List<int> GetListCertificates(string discriminator)
    {
      var commandText = string.Format(Queries.Module.GetListCertificates, discriminator);
      return ExecuteSQLSelect(commandText);
    }
    
    /// <summary>
    /// Запрос вернет список значений из первой колонки.
    /// </summary>
    /// <param name="query">Селект на выборку данных</param>
    /// <returns>Список занчений полученный в запросе.</returns>
    private static List<int> ExecuteSQLSelect(string query)
    {
      var listResult = new List<int>();
      using (var command = SQL.GetCurrentConnection().CreateCommand())
      {
        command.CommandText = query;
        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
            listResult.Add(reader.GetInt32(0));
        }
      }
      return listResult;
    }
    
    /// <summary>
    /// Выполнить запрос к базе данных.
    /// </summary>
    /// <param name="query">Запрос.</param>
    public static void ExecuteSqlQuery(string query)
    {
      using (var command = SQL.GetCurrentConnection().CreateCommand())
      {
        command.CommandText = query;
        command.ExecuteNonQuery();
      }
    }
    
    #endregion
	}
}