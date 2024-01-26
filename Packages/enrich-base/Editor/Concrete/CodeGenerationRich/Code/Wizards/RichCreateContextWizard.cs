using JetBrains.Annotations;
using UnityEditor;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
  public class RichCreateContextWizard : ScriptableWizard
  {
    public string Name;

    [MenuItem("Assets/Rich/Context")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add Context Panel", typeof(RichCreateContextWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(Name))
        return;

      if (!CodeUtilities.HasSelectedFolder())
      {
        var template = Template.Build(TemplateType.Context).Name(Name);
        template.Save();
        Template.Build(TemplateType.Root).Name(Name).Import(template.Ns).Save();

        AssetDatabase.Refresh();
        return;
      }

      Template.Build(TemplateType.Context).Name(Name).Save();
      Template.Build(TemplateType.Root).Name(Name).Save();

      AssetDatabase.Refresh();
    }
  }
}