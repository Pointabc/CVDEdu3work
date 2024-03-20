using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Category3.OfficialDocument;

namespace CentrVD.Category3.Server
{
  partial class OfficialDocumentFunctions
  {
    /// <summary>
    /// Создать версию документа в формате pdf и добавить штамп
    /// </summary>
    [Remote, Public]
    public void ConvertToPdfAndAddSignatureMark(bool isSignatureMark)
    {
      var signInfo = GetSignInfo(_obj);
      
      string htmlStamp = GetStampInfo(_obj).HTML;
      System.IO.Stream pdfDocumentStream = null;
      using (var inputStream = new System.IO.MemoryStream(Sungero.Docflow.PublicFunctions.Module.GetBinaryData(_obj.LastVersion.Body)))
      {
        try
        {
          pdfDocumentStream = Sungero.Docflow.IsolatedFunctions.PdfConverter.GeneratePdf(inputStream, _obj.LastVersion.AssociatedApplication.Extension);
          if (!string.IsNullOrEmpty(htmlStamp))
          {
            pdfDocumentStream = Sungero.Docflow.IsolatedFunctions.PdfConverter.AddSignatureStamp(pdfDocumentStream,
                                                                                                 _obj.LastVersion.AssociatedApplication.Extension,
                                                                                                 htmlStamp, Sungero.Docflow.Resources.SignatureMarkAnchorSymbol,
                                                                                                 Sungero.Docflow.Constants.Module.SearchablePagesLimit);
            //: Sungero.Docflow.IsolatedFunctions.PdfConverter.AddRegistrationStamp(pdfDocumentStream, htmlStamp, 1, rightIndent, bottomIndent);
            
            // Создать новую версию документа.
            _obj.CreateVersionFrom(pdfDocumentStream, "pdf");
            _obj.Save();
            pdfDocumentStream.Close();
          }
        }
        catch (Exception ex)
        {
          if (ex is AppliedCodeException)
            Logger.Error(Sungero.Docflow.Resources.PdfConvertErrorFormat(_obj.Id), ex.InnerException);
          else
            Logger.Error(Sungero.Docflow.Resources.PdfConvertErrorFormat(_obj.Id), ex);
        }
      }
    }
    
    /// <summary>
    /// Получить изображение штампа в виде HTML.
    /// </summary>
    public Structures.Docflow.OfficialDocument.HtmlInfo GetStampInfo(CentrVD.Category3.IOfficialDocument document)
    {
      var info = Structures.Docflow.OfficialDocument.HtmlInfo.Create(string.Empty, false, string.Empty);
      var signInfo = GetSignInfo(document);
      
      if (!signInfo.HasError)
      {
        info.HTML = CentrVD.Category3.Module.Docflow.Resources.Stamp.ToString() // Ресурс с HTML-кодом штампа выше
          .Replace("{Logo}", CentrVD.Category3.Module.Docflow.Resources.SignatureStampSampleLogo)    // Logo предприятия в формате Base64
          .Replace("{Owner}", signInfo.Owner)
          .Replace("{BusinessUnit}", signInfo.BusinessUnit)
          .Replace("{SignDate}", signInfo.SignDate);
      }
      
      return info;
    }
    
    /// <summary>
    /// Данные последней утверждающей подписи документа.
    /// </summary>
    public static Structures.Docflow.OfficialDocument.SignInfo GetSignInfo(CentrVD.Category3.IOfficialDocument document)
    {
      var info = Structures.Docflow.OfficialDocument.SignInfo.Create();
      //var signature = Signatures.Get(document.LastVersion).Where(s => s.SignatureType == SignatureType.Approval && s.SignCertificate != null).OrderByDescending(s => s.SigningDate).FirstOrDefault();
      var signature = Signatures.Get(document.LastVersion).Where(s => s.SignatureType == SignatureType.Approval).OrderByDescending(s => s.SigningDate).FirstOrDefault();
      if (signature == null)
      {
        // Документ не подписан утверждающей подписью с использованием сертификата.
        info.HasError = true;
        info.Error = "Документ не подписан утверждающей подписью с использованием сертификата";
        return info;
      }
      
      //info.Owner = signature.SignCertificate.SubjectName;
      info.Owner = signature.SignatoryFullName;
      info.BusinessUnit = Sungero.Company.Employees.As(signature.Signatory).Department.BusinessUnit.DisplayValue;
      info.SignDate = signature.SigningDate.ToShortDateString();
      
      return info;
    }
    
    /// <summary>
    /// Преобразовать документ в PDF с наложением отметки об ЭП.
    /// </summary>
    /// <returns>Результат преобразования.</returns>
    [Remote, Public]
    public void MyConvertToPdfWithSignatureMark()
    {
      base.ConvertToPdfAndAddSignatureMark(_obj.LastVersion.Id);
    }
    
  }
}