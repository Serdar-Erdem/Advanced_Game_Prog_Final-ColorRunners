using JetBrains.Annotations;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList;
using UnityEditor;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
    public enum CreateViMaEnType
  {
    View,
    Manager,
    Entity
  }

  public class CreateViMaEnWizard : ScriptableWizard
  {
    public string Name;

    public CreateViMaEnType Type = CreateViMaEnType.View;

    public ReorderableContextList ContextList;

    [MenuItem("Assets/Code/View, Manager or Entity")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add View/Manager/Entity Panel", typeof(CreateViMaEnWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(Name))
        return;

      var folder = CodeUtilities.GetSelectedFolder(Type.ToString());

      if (!CodeUtilities.HasSelectedFolder())
      {
        folder = CodeUtilities.GetSelectedFolder(Type.ToString());
        if (AssetDatabase.IsValidFolder(folder))
          AssetDatabase.CreateFolder(folder, Name);
        folder = folder + "/" + Name;
      }

      var extra = string.Empty;
      if (Type == CreateViMaEnType.Manager)
        extra = Type.ToString();

      Template.Build(TemplateType.Mediator).Name(Name + extra).Save(folder);
      var template = Template.Build(TemplateType.View).Name(Name + extra);
      template.Save(folder);

      AddToContext(template.Ns);

      AssetDatabase.Refresh();
    }

    private void AddToContext(string ns)
    {
      foreach (ContextVo vo in ContextList.List)
      {
        string contextPath = CodeUtilities.GetContextPath(vo.Context);
        Template context = new Template(contextPath, TemplateType.Context);
        context.Mediate(Name).Import(ns).Save();
      }
    }
  }
}