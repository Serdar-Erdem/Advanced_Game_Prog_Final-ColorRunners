using Rich.Base.Runtime.Abstract.Injectable.Provider;

namespace Rich.Base.Runtime.Abstract.Injectable.Process
{
    public abstract class Process
    {
        [Inject]
        public IProcessProvider ProcessProvider { get; set; }

        private bool _isActive; 
        /// <summary>
        /// Called by ProcessProvider when created. After injected.
        /// </summary>
        public virtual void OnCreate()
        {
            
        }
        
        /// <summary>
        /// Called by ProcessProvider when get.
        /// </summary>
        public virtual void OnGet()
        {
            _isActive = true;
        }

        /// <summary>
        /// Called by ProcessProvider when returning.
        /// </summary>
        public virtual void OnReturn()
        {
            _isActive = false;
        }
    }
}