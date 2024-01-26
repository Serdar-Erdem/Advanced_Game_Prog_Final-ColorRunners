using System;
using System.Collections.Generic;
using System.Reflection;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using Rich.Base.Runtime.Concrete.Root;
using Rich.Base.Runtime.Signals;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.implicitBind.api;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.extensions.sequencer.api;
using strange.framework.api;
using UnityEngine;

namespace Modules.SRDebuggerAndCamera.Controller
{
    public class SROptionsAllModelRegisterCommand : Command
    {
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
    
        public override void Execute()
        {
            HashSet<object> instanceSet = new HashSet<object>();

            RichMVCContextRoot[] roots = GameObject.FindObjectsOfType<RichMVCContextRoot>();

            foreach (RichMVCContextRoot root in roots)
            {
                Debug.Log("Adding models of " + root + " to SRDebugger");
            
                FieldInfo fieldInfo = typeof(CrossContextInjectionBinder).BaseType.GetField("bindings",BindingFlags.Instance| BindingFlags.NonPublic);
        
                if(fieldInfo == null) return;
            
                CrossContext context = root.context as CrossContext;
        
                InjectionBinder injectionBinder = context.injectionBinder as CrossContextInjectionBinder;
        
                if(injectionBinder == null) return;
        
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
            
                    typeName = mainKey.ToString();
            
                    object instance = injectionBinder.GetInstance(mainKeyType);
            
                    if(instanceSet.Contains(instance))
                    {
                        continue;
                    }
                
                    Debug.Log("Adding " + instance.ToString());
                    SRDebug.Instance.AddOptionContainer(instance);
                    instanceSet.Add(instance);
                }
            }
        
        
        }
    }
}