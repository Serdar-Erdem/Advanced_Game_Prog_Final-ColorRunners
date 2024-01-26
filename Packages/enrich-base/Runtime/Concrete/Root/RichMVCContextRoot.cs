using System;
using Rich.Base.Runtime.Abstract.Root;
using Rich.Base.Runtime.Concrete.Context;
using strange.extensions.context.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Root
{
    public abstract class RichMVCContextRoot : MonoBehaviour, IRichMVCRoot
    {
        private IContext _context;
        public IContext context
        {
            get
            {
                if(_context == null)
                {
                    InitializeContext();
                }
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public bool requiresContext {get;set;}

        public bool registeredWithContext {get;set;}

        public bool autoRegisterWithContext{ get; set; }

        public bool shouldRegister
        {
            get => true;
        }

        protected abstract void InitializeContext();
        

        private void Awake()
        {
            if(_context == null)
            {
                InitializeContext();
            }
        }

        /// <summary>
        /// When a ContextView is Destroyed, automatically removes the associated Context.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (context != null && strange.extensions.context.impl.Context.firstContext != null)
                strange.extensions.context.impl.Context.firstContext.RemoveContext(context);
        }
    }

    public abstract class RichMVCContextRoot<TContext> : RichMVCContextRoot where TContext : RichMVCContext, new()
    {
        protected TContext _typedContext;

        protected sealed override void InitializeContext()
        {
            BeforeCreateContext();
            _typedContext = new TContext();
            context = _typedContext;
            _typedContext.Initialize(gameObject);
            AfterCreateBeforeStartContext();
            context.Start();
            AfterStartBeforeLaunchContext();
        }

        private void Start()
        {
            context.Launch();
            AfterLaunchContext();
        }

        protected virtual void BeforeCreateContext()
        {
            
        }

        protected virtual void AfterCreateBeforeStartContext()
        {
            
        }

        protected virtual void AfterStartBeforeLaunchContext()
        {
            
        }

        protected virtual void AfterLaunchContext()
        {
            
        }
    }
}