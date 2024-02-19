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
    public string ReportSessionld { get; set; }
    public string JobTitle { get; set; }
    public string Employee { get; set; }
    public string Department { get; set; }
    public string BusinessUnit { get; set; }
    public string DocumentName { get; set; }
    public string DocumentLink { get; set; }
    public int? SignedResult { get; set; }
    //public DateTime SignedResult { get; set; }
  }

}