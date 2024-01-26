using System;
using Rich.Base.Runtime.Abstract.Injectable.Binder;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using Rich.Base.Runtime.Concrete.Handler.UnityObject;
using Rich.Base.Runtime.Concrete.Injectable.Binder;
using Rich.Base.Runtime.Concrete.Injectable.Provider;
using Rich.Base.Runtime.Extensions;
using Rich.Base.Runtime.Signals;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.implicitBind.api;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.sequencer.api;
using strange.extensions.sequencer.impl;
using strange.framework.api;
using strange.framework.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Context
{
    public class RichMVCContext : CrossContext
    {
        /// A Binder that maps Views to Mediators
        public IRichMVCMediationBinder mediationBinder { get; set; }
        
        /// A Binder that maps Signals to Commands
        public ICommandBinder commandBinder{get;set;}
        
        /// A Binder that serves as the Event bus for the Context
        public IEventDispatcher dispatcher{get;set;}
        
        //Interprets implicit bindings
        public IImplicitBinder implicitBinder { get; set; }

        /// A Binder that maps Events to Sequences
        public ISequencer sequencer{get;set;}
        
        //Provides unity update events.
        public IUpdateProvider updateProvider { get; set; }
        
        //Provides injected processes.
        public IProcessProvider processProvider { get; set; }


        /// A list of Views Awake before the Context is fully set up.
        protected static ISemiBinding viewCache = new SemiBinding();

        private GameObject _rootObject;
        public GameObject RootObject{get => _rootObject;}

        protected CoreContextSignals CoreContextSignals;

        public RichMVCContext()
        {
            
        }
        
        public RichMVCContext(GameObject root)
        {
            if (firstContext == null || firstContext.GetContextView() == null)
            {
                firstContext = this;
            }
            else
            {
                firstContext.AddContext(this);
            }
            _rootObject = root;
            SetContextView(root);
            addCoreComponents();
            instantiateCoreComponents();
        }

        public void Initialize(GameObject root)
        {
            if (firstContext == null || firstContext.GetContextView() == null)
            {
                firstContext = this;
            }
            else
            {
                firstContext.AddContext(this);
            }
            _rootObject = root;
            SetContextView(root);
            addCoreComponents();
            instantiateCoreComponents();
        }

        //In-case need for change its overriden with same setup.
        public override IContext Start()
        {
            mapBindings();
            postBindings();
            return this;
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
	        
            injectionBinder.Bind<IInstanceProvider>().Bind<IInjectionBinder>().ToValue(injectionBinder);
            injectionBinder.Bind<IContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
            
            //Command binder that works with signals.
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
            
            //Special mediation binder, that doesn't allow views to be injected.
            injectionBinder.Bind<IRichMVCMediationBinder>().To<RichMVCMediationBinder>().ToSingleton();
            
            //This binding is for local dispatchers
            injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>();
            //This binding is for the common system bus
            injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton().ToName(ContextKeys.CONTEXT_DISPATCHER);
            
            injectionBinder.Bind<ISequencer>().To<EventSequencer>().ToSingleton();
            injectionBinder.Bind<IImplicitBinder>().To<RichMVCImplicitBinder>().ToSingleton();

            //Update events from unity
            injectionBinder.Bind<IUpdateProvider>().To<UpdateProvider>().ToSingleton();

            //injected process provider
            injectionBinder.Bind<IProcessProvider>().To<ProcessProvider>().ToSingleton();
        }
        protected override void instantiateCoreComponents()
        {
            base.instantiateCoreComponents();
            
            injectionBinder.Bind<CoreContextSignals>().ToSingleton();

            CoreContextSignals = injectionBinder.GetInstance<CoreContextSignals>();
            
            injectionBinder.Bind<GameObject>().ToValue(contextView).ToName(ContextKeys.CONTEXT_VIEW);
            
            commandBinder = injectionBinder.GetInstance<ICommandBinder>() as ICommandBinder;
            
            //Our rich mvc bindings.
            mediationBinder = injectionBinder.GetInstance<IRichMVCMediationBinder>() as IRichMVCMediationBinder;
            //For more streamlined and fast initializations. Non bubble ups will use this!
            ViewOperationHandler.Init(this);
			
            dispatcher = injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER) as IEventDispatcher;
            
            sequencer = injectionBinder.GetInstance<ISequencer>() as ISequencer;
            implicitBinder = injectionBinder.GetInstance<IImplicitBinder>() as IImplicitBinder;

            updateProvider = injectionBinder.GetInstance<IUpdateProvider>();

            processProvider = injectionBinder.GetInstance<IProcessProvider>();

            (dispatcher as ITriggerProvider).AddTriggerable(commandBinder as ITriggerable);
            (dispatcher as ITriggerProvider).AddTriggerable(sequencer as ITriggerable);
        }

        protected override void postBindings()
        {
            //It's possible for views to fire their Awake before bindings. This catches any early risers and attaches their Mediators.
            mediateViewCache();
            //Ensure that all Views underneath the ContextView are triggered
            mediationBinder.ActivateRoot((contextView as GameObject));
        }

        /// Fires ContextEvent.START
        /// Whatever Command/Sequence you want to happen first should 
        /// be mapped to this event.
        public override void Launch()
        {
            dispatcher.Dispatch(ContextEvent.START);
            CoreContextSignals.Start.Dispatch();
        }
		
        /// Gets an instance of the provided generic type.
        /// Always bear in mind that doing this risks adding
        /// dependencies that must be cleaned up when Contexts
        /// are removed.
        override public object GetComponent<T>()
        {
            return GetComponent<T>(null);
        }

        /// Gets an instance of the provided generic type and name from the InjectionBinder
        /// Always bear in mind that doing this risks adding
        /// dependencies that must be cleaned up when Contexts
        /// are removed.
        override public object GetComponent<T>(object name)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<T>(name);
            if (binding != null)
            {
                return injectionBinder.GetInstance<T>(name);
            }
            return null;
        }
		
        override public void AddView(object view)
        {
            if (mediationBinder != null)
            {
                mediationBinder.Trigger(MediationEvent.AWAKE, view as IView);
            }
            else
            {
                cacheView(view as MonoBehaviour);
            }
        }
		
        override public void RemoveView(object view)
        {
            mediationBinder.Trigger(MediationEvent.DESTROYED, view as IView);
        }

        override public void EnableView(object view)
        {
            mediationBinder.Trigger(MediationEvent.ENABLED, view as IView);
        }

        override public void DisableView(object view)
        {
            mediationBinder.Trigger(MediationEvent.DISABLED, view as IView);
        }
        
        /// Caches early-riser Views.
        /// 
        /// If a View is on stage at startup, it's possible for that
        /// View to be Awake before this Context has finished initing.
        /// `cacheView()` maintains a list of such 'early-risers'
        /// until the Context is ready to mediate them.
        virtual protected void cacheView(MonoBehaviour view)
        {
            if (viewCache.constraint.Equals(BindingConstraintType.ONE))
            {
                viewCache.constraint = BindingConstraintType.MANY;
            }
            viewCache.Add(view);
        }
        
        /// Provide mediation for early-riser Views
        virtual protected void mediateViewCache()
        {
            if (mediationBinder == null)
                throw new ContextException("RichMVCContext cannot mediate views without a mediationBinder", ContextExceptionType.NO_MEDIATION_BINDER);
			
            object[] values = viewCache.value as object[];
            if (values == null)
            {
                return;
            }
            int aa = values.Length;
            for (int a = 0; a < aa; a++)
            {
                mediationBinder.Trigger(MediationEvent.AWAKE, values[a] as IView);
            }
            viewCache = new SemiBinding();
        }
        
        public override void OnRemove()
        {
            base.OnRemove();
            commandBinder.OnRemove();
        }

        protected void CrossContextEvent<T>()
        {
            var values = Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                crossContextBridge.Bind(value);
            }
        }
    }
}