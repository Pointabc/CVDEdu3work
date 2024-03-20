using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Category3.Structures.Docflow.OfficialDocument
{
  
  /// <summary>
  /// Информация о получении штампа в виде HTML.
  /// </summary>
  partial class HtmlInfo
  {
    public string HTML { get; set; }
    public bool HasError { get; set; }
    public string Error { get; set; }
  }

  /// <summary>
  /// Информация о сертификате подписи.
  /// </summary>
  partial class SignInfo
  {
    public string Owner { get; set; }
    public string BusinessUnit { get; set; }
    public string SignDate { get; set; }
    public bool HasError { get; set; }
    public string Error { get; set; }
  }
  
}