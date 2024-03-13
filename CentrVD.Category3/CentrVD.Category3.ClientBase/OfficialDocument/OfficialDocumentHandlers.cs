using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.OfficialDocument;

namespace CentrVD.Category3
{
  partial class OfficialDocumentClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      base.Showing(e);
      _obj.State.Pages.PreviewPage.IsVisible = true;
      //var pview = _obj.Info.State.Pages.PreviewPage;
      //Sungero.Domain.Shared.InplacePageState. pview.
      //_obj.State.Pages.PropertyChanged.Method.CustomAttributes.
      _obj.State.Controls.Preview.HighlightAreas.Add(Colors.Common.Green, 1, 105, 102, 46, 12, 210, 297);
    }

  }
}