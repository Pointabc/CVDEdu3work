using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration
{
  partial class TaskWithAttachmentsServerHandlers
  {

    public virtual IQueryable<Sungero.Docflow.IOfficialDocument> GetDocs()
    {
      return Sungero.Docflow.OfficialDocuments.GetAll();
    }

  }
}