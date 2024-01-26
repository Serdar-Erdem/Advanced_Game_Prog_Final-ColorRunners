namespace Rich.Base.Runtime.Abstract.Injectable.Process
{
    public abstract class ContinuousProcess : Process
    {
        private bool _isRunning;

        public bool IsStarted
        {
            get => _isRunning;
        }

        public void Start()
        {
            if (_isRunning) return;
            
            DoStart();
            _isRunning = true;
        }

        protected abstract void DoStart();

        public override void OnReturn()
        {
            base.OnReturn();
            _isRunning = false;
        }
    }
}