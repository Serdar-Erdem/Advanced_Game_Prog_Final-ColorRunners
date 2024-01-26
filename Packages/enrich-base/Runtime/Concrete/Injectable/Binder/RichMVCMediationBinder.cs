using System;
using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Injectable.Binder;
using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Sirenix.OdinInspector;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Binder
{
	public class RichMVCMediationBinder : AbstractRichMVCMediationBinder
    {
        public RichMVCMediationBinder ()
		{
		}

        [ShowInInspector]
        private Dictionary<IView, MediatorLite> _mediatorLites = new Dictionary<IView, MediatorLite>(); 

		protected override bool HasMediator(IView view, Type mediatorType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mediatorType))
			{
				MonoBehaviour mono = view as MonoBehaviour;
				return mono.GetComponent(mediatorType) != null;
			}
			else
			{
				if (_mediatorLites.ContainsKey(view))
				{
					return _mediatorLites[view] != null;
				}
				else return false;
			}
		}

		/// Create a new Mediator object based on the mediatorType on the provided view
		protected override object CreateMediator(IView view, Type mediatorType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mediatorType))
			{
				MonoBehaviour mono = view as MonoBehaviour;
				return mono.gameObject.AddComponent(mediatorType);
			}
			else
			{
				_mediatorLites.Add(view, (MediatorLite) Activator.CreateInstance(mediatorType));
				return _mediatorLites[view];
			}
		}

		/// Destroy the Mediator on the provided view object based on the mediatorType
		protected override IMediator DestroyMediator(IView view, Type mediatorType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mediatorType))
			{
				MonoBehaviour mono = view as MonoBehaviour;
				IMediator mediator = mono.GetComponent(mediatorType) as IMediator;
				if (mediator != null)
					mediator.OnRemove();
				return mediator;
			}
			else
			{
				IMediator mediator = _mediatorLites[view];
				mediator.OnRemove();
				_mediatorLites[view] = null;
				_mediatorLites.Remove(view);
				return mediator;
			}
		}

		protected override object EnableMediator(IView view, Type mediatorType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mediatorType))
			{
				MonoBehaviour mono = view as MonoBehaviour;
				IMediator mediator = mono.GetComponent(mediatorType) as IMediator;
				if (mediator != null)
					mediator.OnEnabled();
				
				return mediator;
			}
			else
			{
				IMediator mediator = _mediatorLites[view];
				mediator.OnEnabled();
				return mediator;
			}
		}

		protected override object DisableMediator(IView view, Type mediatorType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mediatorType))
			{
				MonoBehaviour mono = view as MonoBehaviour;
				IMediator mediator = mono.GetComponent(mediatorType) as IMediator;
				if (mediator != null)
					mediator.OnDisabled();

				return mediator;
			}
			else
			{
				IMediator mediator = _mediatorLites[view];
				mediator.OnDisabled();
				return mediator;
			}
		}

		protected override void ThrowNullMediatorError(Type viewType, Type mediatorType)
		{
			throw new MediationException("The view: " + viewType.ToString() 
			                                          + " is mapped to mediator: " 
			                                          + mediatorType.ToString() 
			                                          + ". AddComponent resulted in null, which probably means " 
			                                          + mediatorType.ToString().Substring(mediatorType.ToString().LastIndexOf(".") + 1) 
			                                          + " is not a MonoBehaviour.", MediationExceptionType.NULL_MEDIATOR);
		}
    }
}