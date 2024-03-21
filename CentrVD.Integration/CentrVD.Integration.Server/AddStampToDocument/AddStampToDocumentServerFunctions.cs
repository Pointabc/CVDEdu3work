using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using CentrVD.Integration.AddStampToDocument;

namespace CentrVD.Integration.Server
{
  partial class AddStampToDocumentFunctions
  {
    
    /// <summary>
    /// Выполнить сценарий.
    /// </summary>
    /// <param name="approvalTask">Задача на согласование по регламенту.</param>
    /// <returns>Результат выполнения сценария.</returns>
    public override Sungero.Docflow.Structures.ApprovalFunctionStageBase.ExecutionResult Execute(Sungero.Docflow.IApprovalTask approvalTask)
    {
      var result = base.Execute(approvalTask);
      var document = approvalTask.DocumentGroup.OfficialDocuments.SingleOrDefault();

      if (document == null)
        return this.GetErrorResult("Не найден документ.");

      if (document.DocumentKind == null)
        return this.GetErrorResult("Не найден вид документа.");
      
      //CentrVD.Category3.PublicFunctions.OfficialDocument.Remote.MyConvertToPdfWithSignatureMark(CentrVD.Category3.OfficialDocuments.As(document));
      CentrVD.Category3.PublicFunctions.OfficialDocument.Remote.ConvertToPdfAndAddSignatureMark(CentrVD.Category3.OfficialDocuments.As(document));

      return result;
    }
    
    public override void Validate(Sungero.Docflow.IApprovalRuleBase rule, List<Sungero.Docflow.IApprovalRuleBaseStages> stagesSequence, 
                                  Sungero.Docflow.IApprovalRuleBaseStages stage, Sungero.Domain.BeforeSaveEventArgs e)
    {
      base.Validate(rule, stagesSequence, stage, e);

      var reviewTaskStages = stagesSequence.Where(s => s.StageType == Sungero.Docflow.ApprovalRuleBaseStages.StageType.Sign);
      
      int indStamp = stagesSequence.ToList().IndexOf(reviewTaskStages.FirstOrDefault());
      var scenario = stagesSequence.Where(s => s.StageType == Sungero.Docflow.ApprovalRuleBaseStages.StageType.Function);
      int indScenario = stagesSequence.ToList().IndexOf(scenario.FirstOrDefault());
      
      // В одной ветке правила должно содержать этап подписание.
      if (!reviewTaskStages.Any())
        e.AddError(stage, "Необходимо сначало добавить этап подписания.");
      if (indStamp > indScenario)
        e.AddError(stage, "Этап подписания должен идти ранее этапа добавления штампа на подписанный документ.");
    }
    
  }
}