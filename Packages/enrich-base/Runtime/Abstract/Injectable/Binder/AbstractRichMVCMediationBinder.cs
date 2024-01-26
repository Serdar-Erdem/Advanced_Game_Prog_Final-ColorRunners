using System;
using Rich.Base.Runtime.Abstract.Root;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using strange.framework.api;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Injectable.Binder
{
    public abstract class AbstractRichMVCMediationBinder : strange.framework.impl.Binder, IRichMVCMediationBinder
    {
        [Inject]
        public IInjectionBinder injectionBinder { private get; set; }
        
        public override IBinding GetRawBinding ()
        {
            return new MediationBinding (resolver) as IBinding;
        }
        
        public void Trigger(MediationEvent evt, IView view)
        {
            Type viewType = view.GetType();
            IMediationBinding binding = GetBinding (viewType) as IMediationBinding;
            if (binding != null)
            {
                switch(evt)
                {
                    case MediationEvent.AWAKE:
                        MapView (view, binding);
                        break;
                    case MediationEvent.DESTROYED:
                        UnmapView (view, binding);
                        break;
                    case MediationEvent.ENABLED:
                        EnableView (view, binding);
                        break;
                    case MediationEvent.DISABLED:
                        DisableView (view, binding);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("Hello there, you are trying to bind " + view + " without a mediation. This is not allowed.");
            }
        }
        
        public void ActivateRoot(GameObject root)
        {
            if (root == null)
            {
                Debug.LogError("Context Root either invalid or given null.Context Root cannot be null!");
                return;
            }
            
            injectionBinder.injector.Inject(root.GetComponent<IRichMVCRoot>(), false);
        }
         
        protected virtual void ApplyMediationToView(IMediationBinding binding, IView view, Type mediatorType)
        {
            bool isTrueMediator = IsTrueMediator(mediatorType);
            if (!isTrueMediator || !HasMediator(view, mediatorType))
            {
                Type viewType = view.GetType();
                object mediator = CreateMediator(view, mediatorType);
                if (mediator == null)
                    ThrowNullMediatorError (viewType, mediatorType);
                if (isTrueMediator)
                    ((IMediator)mediator).PreRegister();
                Type typeToInject = (binding.abstraction == null || binding.abstraction.Equals(BindingConst.NULLOID)) ? viewType : binding.abstraction as Type;
                injectionBinder.Bind(typeToInject).ToValue(view).ToInject(false);
                injectionBinder.injector.Inject(mediator);
                injectionBinder.Unbind(typeToInject);
                if (isTrueMediator)
                {
                    ((IMediator)mediator).OnRegister();
                }
            }
        }
        
        protected virtual bool IsTrueMediator(Type mediatorType)
        {
            return typeof(IMediator).IsAssignableFrom(mediatorType);
        }
        
        new public IMediationBinding Bind<T> ()
        {
            return base.Bind<T> () as IMediationBinding;
        }

        public IMediationBinding BindView<T>()
        {
            return base.Bind<T> () as IMediationBinding;
        }
        
        virtual protected void MapView(IView view, IMediationBinding binding)
        {
            Type viewType = view.GetType();

            if (bindings.ContainsKey (viewType))
            {
                object[] values = binding.value as object[];
                int aa = values.Length;
                for (int a = 0; a < aa; a++)
                {
                    Type mediatorType = values [a] as Type;
                    if (mediatorType == viewType)
                    {
                        throw new MediationException(viewType + "mapped to itself. The result would be a stack overflow.", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
                    }
                    ApplyMediationToView (binding, view, mediatorType);
                }
            }
        }
        
        virtual protected void UnmapView(IView view, IMediationBinding binding)
        {
            TriggerInBindings(view, binding, DestroyMediator);
        }

        /// Enables a mediator when its view is enabled
        virtual protected void EnableView(IView view, IMediationBinding binding)
        {
            TriggerInBindings(view, binding, EnableMediator);
        }

        /// Disables a mediator when its view is disabled
        virtual protected void DisableView(IView view, IMediationBinding binding)
        {
            TriggerInBindings(view, binding, DisableMediator);
        }
        
        virtual protected void TriggerInBindings(IView view, IMediationBinding binding, Func<IView, Type, object> method)
        {
            Type viewType = view.GetType();

            if (bindings.ContainsKey(viewType))
            {
                object[] values = binding.value as object[];
                int aa = values.Length;
                for (int a = 0; a < aa; a++)
                {
                    Type mediatorType = values[a] as Type;
                    method (view, mediatorType);
                }
            }			
        }
        
        protected abstract object CreateMediator(IView view, Type mediatorType);

        /// Destroy the Mediator on the provided view object based on the mediatorType
        protected abstract IMediator DestroyMediator(IView view, Type mediatorType);

        /// Calls the OnEnabled method of the mediator
        protected abstract object EnableMediator(IView view, Type mediatorType);

        /// Calls the OnDisabled method of the mediator
        protected abstract object DisableMediator(IView view, Type mediatorType);

        /// Whether or not an instantiated Mediator of this type exists
        protected abstract bool HasMediator(IView view, Type mediatorType);

        /// Error thrown when a Mediator can't be instantiated
        /// Abstract because this happens for different reasons. Allow implementing
        /// class to specify the nature of the error.
        protected abstract void ThrowNullMediatorError(Type viewType, Type mediatorType);
    }
}