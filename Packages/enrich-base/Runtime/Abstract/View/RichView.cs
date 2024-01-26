using Rich.Base.Runtime.Abstract.Root;
using Rich.Base.Runtime.Concrete.Handler.UnityObject;
using Sirenix.OdinInspector;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.View
{
    public abstract class RichView : MonoBehaviour, IView
    {
        [SerializeField]
        protected bool _showDebug;
        
        

        protected virtual InititializationType initType
        {
            get => InititializationType.Bubble;
        }
        /// Leave this value true most of the time. If for some reason you want
        /// a view to exist outside a context you can set it to false. The only
        /// difference is whether an error gets generated.
        private bool _requiresContext = true;
        public bool requiresContext
        {
            get
            {
                return _requiresContext;
            }
            set
            {
                _requiresContext = value;
            }
        }
        
        protected enum InititializationType
        {
            Bubble,
            Singleton
        }

        /// Determines the type of event the View is bubbling to the Context
        protected enum OperationType
        {
            Add,
            Remove,
            Enable,
            Disable
        }

        /// A flag for allowing the View to register with the Context
        /// In general you can ignore this. But some developers have asked for a way of disabling
        ///  View registration with a checkbox from Unity, so here it is.
        /// If you want to expose this capability either
        /// (1) uncomment the commented-out line immediately below, or
        /// (2) subclass View and override the autoRegisterWithContext method using your own custom (public) field.
       
        protected bool registerWithContext = true;
        virtual public bool autoRegisterWithContext
        {
            get { return registerWithContext; }
            set { registerWithContext = value; }
        }
        
        [ReadOnly][ShowInInspector][ShowIf("_showDebug")]
        public bool registeredWithContext { get; set; }
        
        public bool shouldRegister { get { return enabled && gameObject.activeInHierarchy; } }
        
        /// Recurses through Transform.parent to find the GameObject to which ContextView is attached
        /// Has a loop limit of 100 levels.
        /// By default, raises an Exception if no Context is found.
        virtual protected void StartContextOperation(MonoBehaviour view, OperationType type, bool finalTry)
        {
            IContext context = null;
            switch(initType)
            {
                case InititializationType.Bubble : FindContextRecursively(out context);
                    break;
                case InititializationType.Singleton : context = ViewOperationHandler.FirstRichMVCContext;
                    break;
            }
            
            if(context == null)
            {
                Debug.LogError("Context cannot be found by " + name, gameObject);
                return;
            }
            
            bool success = DoContextOperation(type,context);

            if (success)
            {
                return;
            }
            
            if (requiresContext && finalTry && type == OperationType.Add)
            {
                //last ditch. If there's a Context anywhere, we'll use it!
                if (Context.firstContext != null)
                {
                    Context.firstContext.AddView(view);
                    registeredWithContext = true;
                    return;
                }
                
                throw new MediationException("No Context Found!",
                    MediationExceptionType.NO_CONTEXT);
            }
        }
        
        private bool FindContextRecursively(out IContext context)
        {
            const int LOOP_MAX = 100;
            int loopLimiter = 0;
            Transform trans = transform;
            context = null;
            while (trans.parent != null && loopLimiter < LOOP_MAX)
            {
                loopLimiter++;
                trans = trans.parent;
                if (trans.gameObject.GetComponent<IRichMVCRoot>() != null)
                {
                    IRichMVCRoot root = trans.gameObject.GetComponent<IRichMVCRoot>();
                    if(root != null)
                    {
                        if(_showDebug)
                        Debug.Log("Root found = " + root.name, gameObject);
                    }
                    if (root.context != null)
                    {
                        context = root.context;
                        
                        if(_showDebug)
                        Debug.Log("Context found");
                        return true;
                    }
                }
            }
            return false;
        }
        
        private bool DoContextOperation(OperationType type, IContext context)
        {
            bool success = true;
            
            if(context == null)
            {
                if(_showDebug)
                Debug.LogError("Context is Null!");
                return false;
            }

            switch (type)
            {
                case OperationType.Add:
                    context.AddView(this);
                    registeredWithContext = true;
                    break;
                case OperationType.Remove:
                    context.RemoveView(this);
                    break;
                case OperationType.Enable:
                    context.EnableView(this);
                    break;
                case OperationType.Disable:
                    context.DisableView(this);
                    break;
                default:
                    success = false;
                    break;
            }
            
            return success;
        }
        
        public void Initialize()
        {
            if(registeredWithContext)
            {
                Debug.Log("You are trying to register a view that has already been registered with context!",gameObject);
                return;
            }
            StartContextOperation(this, OperationType.Add, false);
        }
        
        /// A MonoBehaviour Awake handler.
        /// The View will attempt to connect to the Context at this moment.
        protected virtual void Awake()
        {
            if (autoRegisterWithContext && !registeredWithContext && shouldRegister)
                StartContextOperation(this, OperationType.Add, false);
        }

        /// A MonoBehaviour Start handler
        /// If the View is not yet registered with the Context, it will 
        /// attempt to connect again at this moment.
        protected virtual void Start()
        {
            if (autoRegisterWithContext && !registeredWithContext && shouldRegister)
                StartContextOperation(this, OperationType.Add, true);
        }

        /// A MonoBehaviour OnDestroy handler
        /// The View will inform the Context that it is about to be
        /// destroyed.
        protected virtual void OnDestroy()
        {
            if(registeredWithContext)
                StartContextOperation(this, OperationType.Remove, false);
        }

        /// A MonoBehaviour OnEnable handler
        /// The View will inform the Context that it was enabled
        protected virtual void OnEnable()
        {
            if(registeredWithContext)
                StartContextOperation(this, OperationType.Enable, false);
        }

        /// A MonoBehaviour OnDisable handler
        /// The View will inform the Context that it was disabled
        protected virtual void OnDisable()
        {
            if(registeredWithContext)
                StartContextOperation(this, OperationType.Disable, false);
        }
    }
}