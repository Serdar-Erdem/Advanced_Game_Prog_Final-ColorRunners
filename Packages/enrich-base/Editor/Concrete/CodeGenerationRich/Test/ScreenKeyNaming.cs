using System.Linq;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Test
{
    public static class ScreenKeyNaming
    {
        public const string SCREEN_KEY_PREFIX = "PanelKey";

        public static string GetPrefixedName(string screenName)
        {
            return "PanelKey" + "_" + screenName;
        }

        public static string GetScreenName(string prefixedName)
        {
            return prefixedName.Split('_').Last();
        }

        public static string GetPathWithExtension(string path,string screenName)
        {
            return path + "/" + GetPrefixedName(screenName) + ".asset";
        }
    }
}