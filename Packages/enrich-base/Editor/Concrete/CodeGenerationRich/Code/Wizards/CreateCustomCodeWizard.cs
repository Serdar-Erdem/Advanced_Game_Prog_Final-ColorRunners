using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
  public class CreateCustomCodeWizard : ScriptableWizard
  {
    public List<CodeVo> List;

    [MenuItem("Assets/Code/Custom")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add Custom Code Panel", typeof(CreateCustomCodeWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (List == null)
        return;

      foreach (CodeVo codeVo in List)
      {
        Template.Build(codeVo.Type).Name(codeVo.Name).Save();
      }

      AssetDatabase.Refresh();
    }
  }
}