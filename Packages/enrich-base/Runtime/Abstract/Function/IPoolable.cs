namespace Rich.Base.Runtime.Abstract.Function
{
    public interface IPoolable
    {
        /// <summary>
        /// It works when a object received from pool
        /// </summary>
        void OnGetFromPool();

        /// <summary>
        /// It works when a object returns to pool
        /// </summary>
        void OnReturnFromPool();

        /// <summary>
        /// Unique key value to identify pool object
        /// </summary>
        string PoolKey { get; set; }
    }
}