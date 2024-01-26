using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code
{
  public class Template
  {
    private const string rootFolder = "Assets/Modules/Core/Library/Rich/rich-base/Scripts/Editor/Code/Templates/";
    private const string extension = ".txt";

    private string _data;

    private string _classname;

    private string _namespace;

    private string _path;

    private TemplateType _type;

    public static Template Build(TemplateType type)
    {
      return new Template(type);
    }

    public static Template Build(TemplateType type, string data)
    {
      return new Template(type, data);
    }

    public Template(TemplateType type, string data)
    {
      _type = type;
      _data = data;
      _data = Data.Replace("%TYPE%", type.ToString());
    }

    public Template(TemplateType type)
    {
      _type = type;
      _data = CodeUtilities.LoadScript(rootFolder + type + extension);
      _data = Data.Replace("%TYPE%", type.ToString());
    }

    public Template(string path, TemplateType type)
    {
      _type = type;
      _path = path;
      _data = CodeUtilities.LoadScript(path);
    }

    public string Data
    {
      get { return _data; }
    }

    public TemplateType Type
    {
      get { return _type; }
    }

    public string Ns
    {
      get { return _namespace; }
    }

    public Template Name(string value)
    {
      if (_type == TemplateType.Interface)
      {
        _classname = "I" + value;
        _data = Data.Replace("%NAME%", _classname);
      }
      else
      {
        _classname = value + _type;
        _data = Data.Replace("%NAME%", value);
      }

      return this;
    }

    public Template Import(string value)
    {
      var key = "%IMPORT%";
      var residentKey = "//%IMPORTPOINT%";
      var line = "using " + value.Replace("/", ".") + ";\r";
      if (_data.Contains(line))
        return this;

      _data = _data.Replace(key, line + key);
      _data = _data.Replace(residentKey, line + residentKey);
      return this;
    }

    public Template Inject(string value, bool crosscontext = true)
    {
      var key = "//%INJECTIONPOINT%";
      var line = "injectionBinder.Bind<I" + value;
      line += ">().To<" + value;
      line += ">().ToSingleton()";
      if (crosscontext)
        line += ".CrossContext()";
      line += ";\r" + key;
      _data = _data.Replace(key, line);
      Debug.Log(line);
      return this;
    }

    public Template Mediate(string value)
    {
      var key = "//%MEDIATIONPOINT%";
      var line = "mediationBinder.Bind<" + value;
      line += "View>().To<" + value;
      line += "Mediator>();\r" + key;
      _data = _data.Replace(key, line);
      return this;
    }

    public Template PropKey(string value)
    {
      var key = "//%KEYPOINT%";
      var line = "public const string " + value;
      line += "Property = \"" + value;
      line += "Property\";\r" + key;
      _data = _data.Replace(key, line);
      return this.PropType(value);
    }

    private Template PropType(string value)
    {
      var key = "//%TYPEMAPPOINT%";
      var line = "_typeMap.Add(" + value;
      line += "Property, PropertyType." + "None";
      line += ");\r" + key;
      _data = _data.Replace(key, line);
      return this;
    }

    public Template Command(string eventName, string command)
    {
      var key = "//%COMMANDPOINT%";
      var line = "commandBinder.Bind(" + eventName + ").To<" + command + ">();\r" + key;
      _data = _data.Replace(key, line);
      return this;
    }

    public Template StartCommand(string command)
    {
      return Command("ContextEvent.START", command);
    }

    public void Save(string folder)
    {
      _namespace = folder.Replace("/", ".");
      _data = Data.Replace("%NAMESPACE%", _namespace);
      _data = _data.Replace("%IMPORT%", "");
      CodeUtilities.SaveFile(_data, folder + "/" + _classname + ".cs");
    }

    public void Save()
    {
      if (!string.IsNullOrEmpty(_path))
      {
        CodeUtilities.SaveFile(_data, _path);
        return;
      }

      var folder = CodeUtilities.GetSelectedFolder(_type);
      _namespace = folder.Replace("/", ".");
      _data = Data.Replace("%NAMESPACE%", _namespace);
      _data = _data.Replace("%IMPORT%", "");
      CodeUtilities.SaveFile(_data, folder + "/" + _classname + ".cs");
    }
  }
}