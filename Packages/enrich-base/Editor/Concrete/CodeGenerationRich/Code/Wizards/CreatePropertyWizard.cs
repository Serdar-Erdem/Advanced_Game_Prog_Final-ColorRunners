using JetBrains.Annotations;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList;
using UnityEditor;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
    public class CreatePropertyWizard : ScriptableWizard
  {
    public string Name;

    public ReorderableContextList ContextList;

    [MenuItem("Assets/Code/Property")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add Property Panel", typeof(CreatePropertyWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(Name))
        return;

      var folder = CodeUtilities.GetSelectedFolder(TemplateType.Property);

      if (!CodeUtilities.HasSelectedFolder())
      {
        folder = CodeUtilities.GetSelectedFolder(TemplateType.Property.ToString());
        if (AssetDatabase.IsValidFolder(folder))
          AssetDatabase.CreateFolder(folder, Name);
        folder = folder + "/" + Name;
      }

      Template.Build(TemplateType.Property).Name(Name).Save(folder);
      Template.Build(TemplateType.PropertyCommand).Name(Name).Save(folder);
      var template = Template.Build(TemplateType.PropertyBuilder).Name(Name);
      template.Save(folder);

      AddToContext(template.Ns);
      AddKey();

      AssetDatabase.Refresh();
    }

    private void AddToContext(string ns)
    {
      foreach (ContextVo vo in ContextList.List)
      {
        string contextPath = CodeUtilities.GetContextPath(vo.Context);
        Template context = new Template(contextPath, TemplateType.Context);
        context.Command("PropertyKeys." + Name + "Property", Name + "PropertyCommand");
        context.Import(ns);
        context.Import("Project.Properties");
        context.Save();
      }
    }

    private void AddKey()
    {
      string contextPath = "Assets/Scripts/Project/Properties/PropertyKeys.cs";
      Template keyFile = new Template(contextPath, TemplateType.Property);
      keyFile.PropKey(Name).Save();
    }
  }
}