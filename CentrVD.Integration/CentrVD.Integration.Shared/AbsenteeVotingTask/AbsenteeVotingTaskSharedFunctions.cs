using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AbsenteeVotingTask;

namespace CentrVD.Integration.Shared
{
  partial class AbsenteeVotingTaskFunctions
  {
    
    public static void FillSubject(CentrVD.Integration.IAbsenteeVotingTask _obj)
    {
      // Заполняется автоматически по формату Заочное голосование по документу: <Имя документа на голосование>.
      var name = "Заочное голосование по документу: ";
      var document = _obj.DocumentForVotingGroup.OfficialDocuments.FirstOrDefault();
      if (document != null)
        name += document.DisplayValue;
      if (string.IsNullOrWhiteSpace(name))
        name = "<Имя будет сформировано автоматически по содержанию и другим реквизитам задачи>";
      _obj.Subject = Sungero.Docflow.PublicFunctions.Module.TrimSpecialSymbols(name.Trim());
    }

  }
}