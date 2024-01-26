using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich
{
    [CreateAssetMenu(fileName = "CodeGenerationSettings", menuName = "Rich/Admin/Code Generation Settings")]
    public class CodeGenerationSettings : ScriptableObject
    {
        [SerializeField] private CodeGenerationOperationConfig _screenCodeGenerationConfig;
        public CodeGenerationOperationConfig ScreenCodeGenerationConfig
        {
            get => _screenCodeGenerationConfig;
        }
        
        [SerializeField][BoxGroup("Rich-Base Paths")]
        private DefaultAsset _testTemplateFolderInfo;
        public string TestTemplatePath
        {
            get => GetFolderInfoFolderPath(_testTemplateFolderInfo);
        }
        [SerializeField][BoxGroup("Rich-Base Paths")]
        private DefaultAsset _screenTemplateFolderInfo;
        public string TemplatePrefabPath
        {
            get => GetFolderInfoFolderPath(_screenTemplateFolderInfo);
        }
        [SerializeField][BoxGroup("Rich-Base Paths")]
        private DefaultAsset _codeTemplateFolderInfo;
        public string CodeTemplatePath
        {
            get => GetFolderInfoFolderPath(_codeTemplateFolderInfo);
        }
        
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectViewPath;
        public string ProjectViewPath
        {
            get => _projectViewPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectMediatorPath;
        public string ProjectMediatorPath
        {
            get => _projectMediatorPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectConstantsPath;
        public string ProjectConstantsPath
        {
            get => _projectConstantsPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectTestRootPath;
        public string ProjectTestRootPath
        {
            get => _projectTestRootPath;
        }
        [SerializeField][BoxGroup("Project Paths")][FolderPath]
        private string _projectResourcesPath;
        public string ProjectResourcesPath
        {
            get => _projectResourcesPath;
        }
        private string GetFolderInfoFolderPath(UnityEngine.Object asset)
        {
            return AssetDatabase.GetAssetPath(asset).Replace("/" + asset.name + ".folderInfo","");
        }
    }
}