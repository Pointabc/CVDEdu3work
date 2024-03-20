using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace CentrVD.Category3.Module.Docflow.Server
{
  partial class ModuleFunctions
  {
    
    /// <summary>
    /// Получить отметку о регистрации.
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <returns>Изображение отметки о регистрации в виде html.</returns>
    [Public]
    public override string GetRegistrationStampAsHtml(Sungero.Docflow.IOfficialDocument document)
    {
      var regNumber = document.RegistrationNumber;
      var regDate = document.RegistrationDate;
      var businessUnit = document.BusinessUnit.Name;
      
      if (string.IsNullOrWhiteSpace(regNumber) || regDate == null)
        return string.Empty;
      
      string html;
      using (Sungero.Core.CultureInfoExtensions.SwitchTo(TenantInfo.Culture))
      {
        html = Resources.HtmlStampTemplateForRegistrationStamp;
        html = Resources.HtmlStampTemplateForSignatureCustom;
        var regNum = string.Join(" ", regNumber, "77777");
        html = html.Replace("{RegNumber}", regNum);
        html = html.Replace("{RegDate}", regDate.Value.ToShortDateString());
        html = html.Replace("{BusinessUnit}", businessUnit);
      }
      
      return html;
    }
    
    /// <summary>
    /// Получить отметку об ЭП для сертификата из подписи.
    /// </summary>
    /// <param name="signature">Подпись.</param>
    /// <returns>Изображение отметки об ЭП для сертификата в виде HTML.</returns>
    /// <description>
    /// Пример перекрытия отметки о квалифицированной ЭП для всех документов.
    /// В отметке о квалифицированной ЭП изменены логотип и пропорции заголовка.
    /// Также в отметку добавлены дата и время подписания.
    /// Цвет отметки изменен на фиолетовый.
    /// </description>
    public override string GetSignatureMarkForCertificateAsHtml(Sungero.Domain.Shared.ISignature signature, 
                                                                Sungero.Docflow.Structures.StampSetting.ISignatureStampParams signatureStampParams)
    {
      if (signature == null)
        return string.Empty;

      var certificate = signature.SignCertificate;
      if (certificate == null)
        return string.Empty;

      var certificateSubject = this.GetCertificateSubject(signature);

      var signatoryFullName = string.Format("{0} {1}", certificateSubject.Surname, certificateSubject.GivenName).Trim();

      if (string.IsNullOrEmpty(signatoryFullName))
        signatoryFullName = certificateSubject.CounterpartyName;

      string html = Resources.HtmlStampTemplateForCertificateCustom;
      html = html.Replace("{SignatoryFullName}", signatoryFullName);
      html = html.Replace("{Thumbprint}", certificate.Thumbprint.ToLower());
      var validity = string.Format("{0} {1} {2} {3}", Sungero.Company.Resources.From,
                                   certificate.NotBefore.Value.ToShortDateString(),
                                   Sungero.Company.Resources.To,
                                   certificate.NotAfter.Value.ToShortDateString());
      html = html.Replace("{Validity}", validity);
      html = html.Replace("{SigningDate}", signature.SigningDate.ToString("g"));

      return html;
    }
    
    /// <summary>
    /// Получить отметку об ЭП для подписи.
    /// </summary>
    /// <param name="signature">Подпись.</param>
    /// <returns>Изображение отметки об ЭП для подписи в виде HTML.</returns>
    /// <description>
    /// Пример перекрытия отметки о простой ЭП для всех документов.
    /// В отметке о простой ЭП изменены логотип и пропорции заголовка.
    /// Также в отметку добавлены дата и время подписания.
    /// Цвет отметки изменeн на фиолетовый.
    /// </description>
    public override string GetSignatureMarkForSimpleSignatureAsHtml(Sungero.Domain.Shared.ISignature signature, 
                                                                    Sungero.Docflow.Structures.StampSetting.ISignatureStampParams signatureStampParams)
    {
      if (signature == null)
        return string.Empty;

      var signatoryFullName = signature.SignatoryFullName;
      var signatoryId = signature.Signatory.Id;
      
      string html = Resources.HtmlStampTemplateForSignatureCustom;
      html = html.Replace("{SignatoryFullName}", signatoryFullName);
      html = html.Replace("{SignatoryId}", signatoryId.ToString());
      html = html.Replace("{SigningDate}", signature.SigningDate.ToString("g"));
      
      return html;
    }
    
  }
}