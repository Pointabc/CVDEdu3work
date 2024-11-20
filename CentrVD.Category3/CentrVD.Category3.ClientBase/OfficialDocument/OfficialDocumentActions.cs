using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.OfficialDocument;

namespace CentrVD.Category3.Client
{
  partial class OfficialDocumentActions
  {
    public virtual void FindTasksWithDocument(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var version = _obj.Versions.Single(v => v.Id == _obj.LastVersion.Id);
      byte[] receipt;
      using (var memory = new System.IO.MemoryStream())
      {
        version.Body.Read().CopyTo(memory);
        receipt = memory.ToArray();
      }

      
      var tasks = CentrVD.Category3.PublicFunctions.OfficialDocument.Remote.FindTasksWithDocument(_obj);
      tasks.ShowModal();
    }

    public virtual bool CanFindTasksWithDocument(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}