using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Integration.Structures.EmployeeSignedDocumentsReportNew
{

  /// <summary>
  /// 
  /// </summary>
  partial class Record 
  {
    public string ReportSessionId { get; set; }
    public string JobTitle { get; set; }
    public string Employee1 { get; set; }
    public string Department { get; set; }
    public string BusinessUnit1 { get; set; }
    public string DocumentName { get; set; }
    public string DocumentLink1 { get; set; }
    public string SignedResult { get; set; }
    public DateTime? DateSign { get; set; }
  }

}