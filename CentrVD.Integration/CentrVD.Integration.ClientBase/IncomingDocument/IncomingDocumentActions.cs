using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.IncomingDocument;

namespace CentrVD.Integration.Client
{
  partial class IncomingDocumentActions
  {
    public virtual void AddStampOnPublicVersion(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      // Проверить, что преобразование уже запущено, чтобы не запускать еще раз при повторном нажатии.
      // Вместо этого будет показан диалог о том, что преобразование в процессе.
      int convertingVersionIdParamValue = -1;
      bool addingRegistrationStampIsInProcess = e.Params.TryGetValue(Sungero.Docflow.Constants.OfficialDocument.ConvertingVersionId, out convertingVersionIdParamValue) &&
        convertingVersionIdParamValue == _obj.LastVersion.Id;
      
      // Преобразование в PDF.
      Sungero.Docflow.Structures.OfficialDocument.ConversionToPdfResult result = null;
      if (!addingRegistrationStampIsInProcess)
      {
        // Проверки возможности преобразования и наложения отметки.
        result = null; //Sungero.Docflow.Functions.IncomingDocumentBase.ValidatePdfConvertibilityByExtension(_obj);
        if (!result.HasErrors)
        {
          //var position = null; //Sungero.Docflow.Functions.IncomingDocumentBase.ShowAddRegistrationStampDialog(_obj);
          //if (position == null)
            //return;
          
          result = null; //Sungero.Docflow.PublicFunctions.IncomingDocumentBase.Remote.AddRegistrationStamp(_obj, position.RightIndent, position.BottomIndent);
          
          if (result.IsOnConvertion)
            e.Params.AddOrUpdate(Sungero.Docflow.Constants.OfficialDocument.ConvertingVersionId, _obj.LastVersion.Id);
          
          // Успешная интерактивная конвертация.
          if (!result.HasErrors && result.IsFastConvertion)
          {
            Dialogs.NotifyMessage(CentrVD.Category3.OfficialDocuments.Resources.ConvertionDone);
            return;
          }
        }
      }
      
      // Сообщение об ошибке при асинхронном преобразовании.
      if (!addingRegistrationStampIsInProcess && result.HasErrors)
      {
        Dialogs.NotifyMessage(result.ErrorMessage);
        return;
      }
    }

    public virtual bool CanAddStampOnPublicVersion(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}