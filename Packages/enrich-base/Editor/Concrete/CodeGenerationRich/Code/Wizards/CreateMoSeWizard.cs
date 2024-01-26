using JetBrains.Annotations;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList;
using UnityEditor;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
  public enum CreateMoSeType
  {
    Model,
    Service
  }

  public class CreateMoSeWizard : ScriptableWizard
  {
    public string Name;

    public CreateMoSeType Type = CreateMoSeType.Model;

    public ReorderableContextList ContextList;

    [MenuItem("Assets/Code/Model or Service")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add Model/Service Panel", typeof(CreateMoSeWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(Name))
        return;

      var type = TemplateType.Model;
      if (Type == CreateMoSeType.Service)
        type = TemplateType.Service;

      if (!CodeUtilities.HasSelectedFolder())
      {
        var folder = CodeUtilities.GetSelectedFolder(type);

        if (AssetDatabase.IsValidFolder(folder))
          AssetDatabase.CreateFolder(folder, Name);

        var path = folder + "/" + Name;
        Template.Build(type).Name(Name).Save(path);
        Template.Build(TemplateType.Interface).Name(Name + type).Save(path);

        AddToContext(type, path);

        AssetDatabase.Refresh();
        return;
      }

      Template.Build(type).Name(Name).Save();
      var template = Template.Build(TemplateType.Interface);
      template.Name(Name + type).Save();

      AddToContext(type, template.Ns);

      AssetDatabase.Refresh();
    }

    private void AddToContext(TemplateType type, string ns)
    {
      foreach (ContextVo vo in ContextList.List)
      {
        string contextPath = CodeUtilities.GetContextPath(vo.Context);
        Template context = new Template(contextPath, TemplateType.Context);
        context.Inject(Name + type).Import(ns).Save();
      }
    }
  }
}