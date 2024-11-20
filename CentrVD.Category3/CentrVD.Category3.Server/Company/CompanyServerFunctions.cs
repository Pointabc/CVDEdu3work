using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Company;

namespace CentrVD.Category3.Server
{
  partial class CompanyFunctions
  {

    /// <summary>
    /// Существуют ли договорные документы для Организации
    /// </summary>
    [Remote, Public]
    public bool IsExistsContractualDocuments()
    {
      var contractualDocuments = Sungero.Contracts.ContractualDocuments.GetAll(d => Equals(d.Counterparty.Id, _obj.Id));
      
      return !contractualDocuments.Any() ? true : false;
    }

  }
}