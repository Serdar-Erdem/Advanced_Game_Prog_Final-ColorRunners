using strange.extensions.context.api;
using strange.extensions.mediation.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Mediator
{
    [System.Serializable]
    public class MediatorLite : IMediator
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        public MediatorLite()
        {
        }

        /**
         * Fires directly after creation and before injection
         */
        virtual public void PreRegister()
        {
        }

        /**
         * Fires after all injections satisifed.
         *
         * Override and place your initialization code here.
         */
        virtual public void OnRegister()
        {
        }

        /**
         * Fires on removal of view.
         *
         * Override and place your cleanup code here
         */
        virtual public void OnRemove()
        {
        }

        /**
         * Fires on enabling of view.
         */
        virtual public void OnEnabled()
        {
        }

        /**
         * Fires on disabling of view.
         */
        virtual public void OnDisabled()
        {
        }
    }
}