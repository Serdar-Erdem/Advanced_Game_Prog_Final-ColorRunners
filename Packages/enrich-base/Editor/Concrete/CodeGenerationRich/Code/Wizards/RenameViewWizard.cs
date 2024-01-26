using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.Wizards
{
  public class RenameViewWizard : ScriptableWizard
  {
    public string NewName;

    [MenuItem("Assets/Code/Rename View")]
    [UsedImplicitly]
    private static void RenameViewUtility()
    {
      DisplayWizard("Rename View", typeof(RenameViewWizard), "Rename");
    }

    [MenuItem("Assets/Code/Rename View", true)]
    public static bool RenameViewValidation()
    {
      if (Selection.objects.Length == 1)
      {
        string assetPath = AssetDatabase.GetAssetPath(Selection.objects[0]);

        if (assetPath.Contains("/Tests/Screen/") && !assetPath.Contains(".") && assetPath.Split('/').Length == 4)
        {
          return true;
        }

        return false;
      }

      return false;
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(NewName))
        return;

      string assetPath = AssetDatabase.GetAssetPath(Selection.objects[0]);
      string[] split = assetPath.Split('/');
      string viewName = split[split.Length - 1];

      string[] assets = AssetDatabase.FindAssets(viewName);
      foreach (string asset in assets)
      {
        string path = AssetDatabase.GUIDToAssetPath(asset);

        if (path.Contains(".cs"))
        {
          string code = CodeUtilities.LoadScript(path);
          Debug.Log(code);
          code = code.Replace(viewName, NewName);
          Debug.Log(code);
          CodeUtilities.SaveFile(code, path);
        }

        AssetDatabase.RenameAsset(path, path.Replace(viewName, NewName));
      }

      AssetDatabase.Refresh();
    }
  }
}