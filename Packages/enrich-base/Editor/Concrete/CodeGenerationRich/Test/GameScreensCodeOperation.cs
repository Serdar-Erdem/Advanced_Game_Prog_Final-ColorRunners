using System;
using System.IO;
using System.Text;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Code;
using Rich.Base.Editor.Concrete.CodeGenerationRich.Test;
using UnityEditor;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich
{
    [CreateAssetMenu(fileName = "GameScreensCodeOperation", menuName = "Rich/Admin/Code Generation Operations/GameScreensCodeOperation")]
    public class GameScreensCodeOperation : CodeGenerationOperation
    <GameScreensCodeOperation,
        GameScreensCodeOperation.StartArgs,
        GameScreensCodeOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
        }
        public struct OperateArgs
        {
            
        }

        private string _name;

        protected override void OnBegin(StartArgs arg)
        {
            base.OnBegin(arg);
            DirectoryHelpers.EnsurePathExistence(_sharedSettings.ProjectConstantsPath);
            _name = arg.Name;
        }

        protected override void OnOperate(OperateArgs arg)
        {
            string gameScreensPath = _sharedSettings.ProjectConstantsPath + "/" + "GameScreens.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(gameScreensPath);

            string data;
            
            if (obj == null)
            {
                data = LoadTemplate();
                
                Debug.Log("File didn't existed generating...");
            }
            else
            {
                data = LoadFileOnPath(gameScreensPath);
                
                Debug.Log("Changing on existing file...");
            }
            
            if(data.Contains(_name))
            {
                Debug.Log("Constant already exists");
                return;
            }
            string addition="\r\t\t";
            addition+= "//-%Name%";
            addition+="\r\t\t";
            addition+="public const string %Name% = \"%Name%\";";
            addition+="\r\t\t";
            addition+="//-";
            addition+="\r\t\t";
            addition+="ADDPOINT";
            data = data.Replace("//*ADDITION*//",addition);
            data = data.Replace("%Name%", _name);
            data = data.Replace("ADDPOINT","//*ADDITION*//");
            CodeUtilities.SaveFile(data, gameScreensPath);
            Debug.Log("Added Constants");
        }
        
        private string LoadTemplate()
        {
            try
            {
                string data = string.Empty;
                string path = _sharedSettings.TestTemplatePath + "/TemplateGameScreens" + ".txt";
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
        
        private string LoadFileOnPath(string filePath)
        {
            try
            {
                Debug.Log("Loading File = " + filePath);
                string data = string.Empty;
                string path = filePath;
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
                Debug.LogException(e);
                return string.Empty;
            }
        }
    }
}