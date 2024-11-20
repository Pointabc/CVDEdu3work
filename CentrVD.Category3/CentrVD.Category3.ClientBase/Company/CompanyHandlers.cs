using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.Company;

namespace CentrVD.Category3
{
  partial class CompanyClientHandlers
  {

    public override void Refresh(Sungero.Presentation.FormRefreshEventArgs e)
    {
      base.Refresh(e);
      
      _obj.State.Properties.LegalName.IsEnabled = _obj.LegalName == null ? true : false;
      _obj.State.Properties.TIN.IsEnabled = _obj.TIN == null ? true : false;
    }

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      base.Showing(e);
      
      _obj.State.Properties.Name.IsEnabled = CentrVD.Category3.PublicFunctions.Company.Remote.IsExistsContractualDocuments(_obj);
    }

  }
}