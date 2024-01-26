using Rich.Base.Runtime.Abstract.Injectable.Provider;

namespace Rich.Base.Runtime.Abstract.Injectable.Process
{
    /// <summary>
    /// You must start these processes after getting them.
    /// This type of processes get updated and probably needs to check something regularly or update something regularly. If you need to stop updating for some reason use ProcessProvider to return it will cancel updating.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UpdatedProcess<T> : ContinuousProcess where T : UpdatedProcess<T>
    {
        [Inject]
        public IUpdateProvider UpdateProvider { get; set; }

        private bool _isListeningUpdater;

        protected sealed override void DoStart()
        {
            OnStart(out bool shouldReturn);
            if (shouldReturn)
            {
                ProcessProvider.Return(this as T);
            }
            else
            {
                if (_isListeningUpdater) return;
                UpdateProvider.AddUpdate(Update);
                UpdateProvider.AddFixedUpdate(FixedUpdate);
                UpdateProvider.AddLateUpdate(LateUpdate);
                _isListeningUpdater = true;
            }
        }

        protected virtual void OnStart(out bool shouldReturn)
        {
            shouldReturn = false;
        }

        private void Update()
        {
            OnUpdate(out bool shouldReturn);
            if (shouldReturn)
            {
                ProcessProvider.Return(this as T);
            }
        }

        protected virtual void OnUpdate(out bool shouldReturn)
        {
            shouldReturn = false;
        }

        private void FixedUpdate()
        {
            OnFixedUpdate(out bool shouldReturn);
            if (shouldReturn)
            {
                ProcessProvider.Return(this as T);
            }
        }

        protected virtual void OnFixedUpdate(out bool shouldReturn)
        {
            shouldReturn = false;
        }

        private void LateUpdate()
        {
            OnLateUpdate(out bool shouldReturn);
            if (shouldReturn)
            {
                ProcessProvider.Return(this as T);
            }
        }

        protected virtual void OnLateUpdate(out bool shouldReturn)
        {
            shouldReturn = false;
        }

        public override void OnReturn()
        {
            base.OnReturn();
            if (_isListeningUpdater)
            {
                UpdateProvider.RemoveUpdate(Update);
                UpdateProvider.RemoveFixedUpdate(FixedUpdate);
                UpdateProvider.RemoveLateUpdate(LateUpdate);
                _isListeningUpdater = false;
            }
        }
    }
}