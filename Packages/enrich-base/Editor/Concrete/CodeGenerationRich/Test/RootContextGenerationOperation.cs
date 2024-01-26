using System;
using System.IO;
using System.Text;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Code;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Test;
using UnityEditor;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich
{
    [CreateAssetMenu(fileName = "RootContextGenerationOperation", menuName = "Rich/Admin/Code Generation Operations/RootContextGenerationOperation")]
    public class RootContextGenerationOperation
        : CodeGenerationOperation<
            RootContextGenerationOperation,
            RootContextGenerationOperation.StartArgs,
            RootContextGenerationOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
            public string Type;
        }

        public struct OperateArgs
        {
            
        }

        private string _type;
        private string _name;
        private string _testScriptsPath;
        private const string NamespacePlaceholder = "%TemplateNS%";
        private const string ClassnamePlaceholder = "%Template%";

        protected override void OnBegin(StartArgs arg)
        {
            _type = arg.Type;
            _name = arg.Name;
            //Creating scripts folder for test code.
            _testScriptsPath = DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectTestRootPath + "/" + _name, "Scripts");
        }

        protected override void OnOperate(OperateArgs arg)
        {
            AddScript(_testScriptsPath, _name, TemplateType.Root, _type);
            AddScript(_testScriptsPath, _name, TemplateType.Context, _type);
        }
        
        private void AddScript(string path, string screenName, TemplateType templateType, string testType)
        {
            var data = LoadTemplate(templateType);
            data = data.Replace(NamespacePlaceholder, path.Replace("/", "."));
            data = data.Replace(ClassnamePlaceholder, screenName + testType + "Test");
            data = data.Replace("%Name%", screenName);
            data = data.Replace("%Type%", testType);
            CodeUtilities.SaveFile(data, path + "/" + screenName + testType + "Test" + templateType + ".cs");

            UnityEngine.Object rootScript = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path + "/" + screenName + testType + "Test" + templateType + ".cs");
            AssetDatabase.SetLabels(rootScript, new string[] { "Exclude" });
        }
        
        private string LoadTemplate(TemplateType type)
        {
            try
            {
                string data = string.Empty;
                string path = _sharedSettings.TestTemplatePath + "/Template" + type + ".txt";
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return string.Empty;
            }
        }
    }
}