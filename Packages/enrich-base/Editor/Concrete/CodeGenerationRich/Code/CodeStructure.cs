namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code
{
  public class CodeStructure
  {
    private static void AddContext(string name)
    {
      Template context = new Template(TemplateType.Context);
      context.Name(name).Save();

      Template bootstrap = new Template(TemplateType.Root);
      bootstrap.Name(name).Save();
    }
  }
}