using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Model
{
    public interface IObjectPoolModel
    {
        /// <summary>
        /// Instansiate the pool objects
        /// </summary>
        /// <param name="key"> Unique key value to store on pool </param>
        /// <param name="prefab"> A Prefab to instansiate </param>
        /// <param name="count"> How many gameobject should be created </param>
        void Pool(string key, GameObject prefab, int count);

        /// <summary>
        /// Return a random gameobject from pool
        /// </summary>
        /// <param name="key"> Key of the desired pool object </param>
        /// <returns></returns>
        GameObject Get(string key);

        /// <summary>
        /// Use to disable and return the gameobject to pool
        /// </summary>
        /// <param name="obj"> Gameobject to be disabled </param>
        void Return(GameObject obj);

        /// <summary>
        /// Return all pool objects
        /// </summary>
        void HideAll();

        /// <summary>
        /// Check if pool has the key
        /// </summary>
        /// <returns></returns>
        bool Has(string key);
    }
}