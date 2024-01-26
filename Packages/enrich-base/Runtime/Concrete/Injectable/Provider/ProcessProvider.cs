using System;
using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using strange.extensions.injector.api;

namespace Rich.Base.Runtime.Concrete.Injectable.Provider
{
    public class ProcessProvider : IProcessProvider
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        protected Dictionary<Type, HashSet<Rich.Base.Runtime.Abstract.Injectable.Process.Process>> _activeDictionary = new Dictionary<Type, HashSet<Rich.Base.Runtime.Abstract.Injectable.Process.Process>>(); 
        protected Dictionary<Type, List<Rich.Base.Runtime.Abstract.Injectable.Process.Process>> _inactiveDictionary = new Dictionary<Type, List<Rich.Base.Runtime.Abstract.Injectable.Process.Process>>(); 

        //This will create or get an inactive one.
        public T Get<T>() where T : Rich.Base.Runtime.Abstract.Injectable.Process.Process, new()
        {
            T instance;
            Type type = typeof(T);

            if (!_inactiveDictionary.ContainsKey(type))
            {
                Create<T>();
            }
            
            if (_inactiveDictionary[type].Count == 0)
            {
                Create<T>();
            }
            
            instance = _inactiveDictionary[type][0] as T;
            _inactiveDictionary[type].Remove(instance);
            _activeDictionary[type].Add(instance);
            
            //Debug.Log("Total Inactive = " + _inactiveDictionary[type].Count);
            //Debug.Log("Total Active = " + _activeDictionary[type].Count);
            
            instance.OnGet();
            
            return instance;
        }

        public bool Return<T>(T process) where T: Rich.Base.Runtime.Abstract.Injectable.Process.Process
        {
            Type type = typeof(T);

            bool success;
            if (_activeDictionary.ContainsKey(type))
            {
                if (_activeDictionary[type].Contains(process))
                {
                    _activeDictionary[type].Remove(process);

                    if (!_inactiveDictionary.ContainsKey(type))
                    {
                        _inactiveDictionary.Add(type, new List<Rich.Base.Runtime.Abstract.Injectable.Process.Process>());
                    }
                    
                    _inactiveDictionary[type].Add(process);
                    process.OnReturn();
                    success = true;
                }
                else
                {
                    success = false;
                }
            
                //Debug.Log("On Return Total Inactive, Type " + type + " = " + _inactiveDictionary[type].Count);
                //Debug.Log("On Return Total Active, Type " + type + " = " +  _activeDictionary[type].Count);
            }
            else
            {
                success = false;
            }

            return success;
        }

        private void Create<T>() where T : Rich.Base.Runtime.Abstract.Injectable.Process.Process, new()
        {
            T instance = new T();
            Type type = typeof(T);
            InjectionBinder.injector.Inject(instance);

            if (!_inactiveDictionary.ContainsKey(type))
            {
                _inactiveDictionary.Add(type,new List<Rich.Base.Runtime.Abstract.Injectable.Process.Process>());
            }

            if (!_activeDictionary.ContainsKey(type))
            {
                _activeDictionary.Add(type,new HashSet<Rich.Base.Runtime.Abstract.Injectable.Process.Process>());
            }
            
            _inactiveDictionary[type].Add(instance);
            instance.OnCreate();
        }

        public void ClearInactive()
        {
            _inactiveDictionary.Clear();
        }
    }
}