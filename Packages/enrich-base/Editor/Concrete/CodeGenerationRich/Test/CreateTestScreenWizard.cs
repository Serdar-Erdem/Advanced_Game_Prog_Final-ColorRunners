using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Test
{
    [InitializeOnLoad]
    public class CreateViewTemplateListener
    {
        private static CodeGenerationSettings _settings;
        
        static CreateViewTemplateListener()
        {
            if (EditorPrefs.GetBool("CreateTestScreenWizard"))
            {
                EditorPrefs.SetBool("CreateTestScreenWizard", false);

                string name = EditorPrefs.GetString("Name");
                string testType = EditorPrefs.GetString("Type");
                EditorPrefs.DeleteKey("Name");
                EditorPrefs.DeleteKey("Type");
                EditorPrefs.DeleteKey("ScriptRootPath");
                
                _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
                
                //Instantiate root GameObject.
                if(!TryInstantiateRootInstance(name,testType,out GameObject instanceRootGameObject))
                {
                    Debug.LogError("Could not instantiate root instance!");
                    return;
                }

                //Creating the skeleton gameObject of panel.
                GameObject screenGameObject = CreateScreenViewGameObject(name, testType);

                //Turning skeleton gameObject to a usable prefab.
                CreatePrefabOnPathFromInstance(_settings.ProjectResourcesPath + "/" + testType, name,screenGameObject, out GameObject usableScreenPrefab);

                //Re-assigning to prefab instance.
                screenGameObject = PrefabUtility.InstantiatePrefab(usableScreenPrefab) as GameObject;

                //Using Layer0 we will delete any screens that exist.
                GameObject screenContainer = GameObject.Find("Layer0");
                if (screenContainer.transform.childCount > 0)
                    Object.DestroyImmediate(screenContainer.transform.GetChild(0).gameObject);

                if (screenGameObject != null)
                {
                    screenGameObject.transform.SetParent(screenContainer.transform);
                    screenGameObject.GetComponent<RectTransform>().localScale = Vector3.one;
                }

                EditorSceneManager.MarkAllScenesDirty();
            }
        }

        private static bool TryInstantiateRootInstance(string panelName, string testType,out GameObject rootInstance)
        {
            Object root = AssetDatabase.LoadAssetAtPath(_settings.TemplatePrefabPath + "/" + "ScreenRootTemplate.prefab", typeof(GameObject));
            rootInstance = PrefabUtility.InstantiatePrefab(root) as GameObject;
            
            if(rootInstance == null)
            {
                return false;
            }
            
            rootInstance.name = panelName + "Root";
            
            //Adding root component.
            Type type = GetTypeFromName(panelName + testType + "TestRoot");
            Component rootComponent = rootInstance.AddComponent(type);
            return true;
        }

        private static GameObject CreateScreenViewGameObject(string panelName, string testType)
        {
            GameObject screenGameObject = new GameObject(panelName);
            screenGameObject.AddComponent<RectTransform>();
            screenGameObject.GetComponent<RectTransform>().localScale = Vector3.one;

            Type componentType = GetTypeFromName(panelName + testType + "View");
            screenGameObject.AddComponent(componentType);
            return screenGameObject;
        }
        
        private static void CreatePrefabOnPathFromInstance(string prefabPath, string prefabName, GameObject instance,out GameObject prefab)
        {
            DirectoryHelpers.EnsurePathExistence(prefabPath);

            string screenPrefabPath = prefabPath + "/" + prefabName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(instance, screenPrefabPath);
            Object.DestroyImmediate(instance);

            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(screenPrefabPath);
        }

        private static Type GetTypeFromName(string classNameWithNameSpace)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = a.GetTypes();
                foreach (Type type in types)
                {
                    if (type.AssemblyQualifiedName != null && type.AssemblyQualifiedName.Contains(classNameWithNameSpace))
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }


    public class CreateTestScreenWizard : ScriptableWizard
    {
        public string Type = "Screen";

        public string Name = string.Empty;

        public string[] ViewEventList;

        private string _testScriptsPath;

        private string _testControllersPath;

        private string _testRootPath;

        private string _keyPath;

        [MenuItem("Tools/Rich/Create Test Screen")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Panel", typeof(CreateTestScreenWizard), "Create");
        }

        private static CodeGenerationSettings _settings;
        private CodeGenerationOperationConfig _operationConfig;

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
            _operationConfig = _settings.ScreenCodeGenerationConfig;

            //Create necessary start args.
            Dictionary<Type, object> startArgs = new Dictionary<Type, object>()
            {
                {
                    typeof(ScreenViewMediatorGenerationOperation),
                    new ScreenViewMediatorGenerationOperation.StartArgs()
                    {
                        Name = this.Name,
                        Type = this.Type,
                        ViewEventList = this.ViewEventList
                    }
                },
                {
                    typeof(RootContextGenerationOperation),
                    new RootContextGenerationOperation.StartArgs()
                    {
                        Name = this.Name,
                        Type = this.Type
                    }
                },
                {
                    typeof(GameScreensCodeOperation),
                    new GameScreensCodeOperation.StartArgs()
                    {
                        Name = this.Name
                    }
                },
                {
                    typeof(CreateSceneGenerationOperation),
                    new CreateSceneGenerationOperation.StartArgs()
                    {
                        Name = this.Name
                    }
                }
            };
            
            foreach (CodeGenerationOperation operation in _operationConfig.Operations)
            {
                operation.Begin(startArgs);
            }
            
            //Create necessary operate args.
            Dictionary<Type, object> operateArgs = new Dictionary<Type, object>()
            {
                
            };
            
            foreach (CodeGenerationOperation operation in _operationConfig.Operations)
            {
                operation.Operate(operateArgs);
            }

            //This is for InitializeOnLoad code above.
            EditorPrefs.SetBool("CreateTestScreenWizard", true);
            EditorPrefs.SetString("Name", Name);
            EditorPrefs.SetString("Type", Type);

            AssetDatabase.Refresh();
        }
    }
}