using System;
using System.Collections.Generic;
using System.Reflection;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using Rich.Base.Runtime.Concrete.Root;
using Rich.Base.Runtime.Signals;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.implicitBind.api;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.extensions.sequencer.api;
using strange.framework.api;
using UnityEditor;
using UnityEngine;

public class ModelViewer : OdinEditorWindow
{
    private RichMVCContextRoot _inspectedRoot;
    
    private readonly List<Type> _ignoredTypeList = new List<Type>()
    {
        typeof(ICommandBinder),
        typeof(IEventDispatcher),
        typeof(IImplicitBinder),
        typeof(ISequencer),
        typeof(IUpdateProvider),
        typeof(IProcessProvider),
        typeof(CrossContextBridge),
        typeof(IInstanceProvider),
        typeof(IInjectionBinder),
        typeof(CoreContextSignals)
    };
    
    [MenuItem("Tools/Rich/Model Viewer")]
    private static void OpenWindow()
    {
        GetWindow<ModelViewer>().Show();
    }
    protected override void DrawEditors()
    {
        base.DrawEditors();
        DrawModels();
    }

    private void DrawModels()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("Enter Play Mode to view Models.");
            return;
        }

        if (Selection.activeObject == null)
        {
            EditorGUILayout.LabelField("Select a RichMVCContext Root GameObject");
            return;
        }

        GameObject rootGameObject = Selection.activeObject as GameObject;
        if (rootGameObject == null)
        {
            EditorGUILayout.LabelField("Select a RichMVCContext Root GameObject");
            return;
        }

        _inspectedRoot = rootGameObject.GetComponent<RichMVCContextRoot>();
        if (_inspectedRoot == null)
        {
            EditorGUILayout.LabelField("Select a RichMVCContext Root GameObject");
            return;
        }

        SirenixEditorFields.UnityObjectField("Inspecting", _inspectedRoot, typeof(RichMVCContextRoot),true);
        SirenixEditorGUI.DrawThickHorizontalSeperator(5f,5f,5f);
        
        CrossContext context = _inspectedRoot.context as CrossContext;
        EditorGUILayout.Toggle("Context Found",context!=null);
        if(context == null) return;
        
        FieldInfo fieldInfo = typeof(CrossContextInjectionBinder).BaseType.GetField("bindings",BindingFlags.Instance| BindingFlags.NonPublic);
        
        EditorGUILayout.Toggle("Bindings Found",fieldInfo!=null);
        if(fieldInfo == null) return;
        
        InjectionBinder injectionBinder = context.injectionBinder as CrossContextInjectionBinder;
        
        EditorGUILayout.Toggle("InjectionBinder Casted",injectionBinder!=null);
        if(injectionBinder == null) return;
        
        SirenixEditorGUI.DrawThickHorizontalSeperator(5f,5f,5f);
        SirenixEditorGUI.Title("Bindings","",TextAlignment.Center,false);
        SirenixEditorGUI.BeginBox();
        
        object bindings = fieldInfo.GetValue(injectionBinder);
        Dictionary<object,Dictionary<object,IBinding>> bindingDictionaries = (Dictionary<object,Dictionary<object,IBinding>>)bindings;
        if(bindingDictionaries == null) return;

        foreach (object mainKey in bindingDictionaries.Keys)
        {
            string typeName;
            if (!(mainKey is Type))
            {
                continue;
            }
            
            Type mainKeyType = mainKey as Type;

            if (_ignoredTypeList.Contains(mainKeyType))
            {
                continue;
            }
            
            IBinding binding = injectionBinder.GetBinding(mainKeyType);

            if (binding == null)
            {
                continue;
            } 
            SirenixEditorGUI.BeginBox();
            
            typeName = mainKey.ToString();
            SirenixEditorGUI.Title(mainKeyType.Name,"",TextAlignment.Center,false);
            
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            object instance = injectionBinder.GetInstance(mainKeyType);
            if( GUILayout.Button("Inspect",GUILayout.Height(20), GUILayout.Width(80)))
            {
                OdinEditorWindow.InspectObject(instance);
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            
            SirenixEditorGUI.EndBox();
            SirenixEditorGUI.DrawHorizontalLineSeperator(2f,2f,2f);
        }
        SirenixEditorGUI.EndBox();
    }
}
