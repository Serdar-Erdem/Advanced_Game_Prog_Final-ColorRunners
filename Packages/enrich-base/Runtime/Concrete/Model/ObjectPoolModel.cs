using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Function;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using strange.extensions.context.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Model
{
    public class ObjectPoolModel : IObjectPoolModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        private Dictionary<string, ObjectPoolVo> _poolVos;

        private Dictionary<string, Queue<GameObject>> _objectQueues;

        private GameObject container, newObj;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _poolVos = new Dictionary<string, ObjectPoolVo>();
            _objectQueues = new Dictionary<string, Queue<GameObject>>();
            container = new GameObject("PoolObjects");
            container.transform.SetParent(contextView.transform);
        }

        public void Pool(string key, GameObject prefab, int count)
        {
            //if (_poolVos.ContainsKey(key))
            //{
            //    Debug.LogWarning("Already have " + key + " pool.");
            //    return;
            //}
            if (prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("You cant create " + prefab.name + ". IPoolable class is missing");
                return;
            }
            var vo = new ObjectPoolVo
            {
                Key = key,
                Count = count,
                Prefab = prefab
            };

            if (!_poolVos.ContainsKey(vo.Key))
                _poolVos.Add(vo.Key, vo);

            Queue<GameObject> queue;

            if (!_objectQueues.ContainsKey(vo.Key))
            {
                queue = new Queue<GameObject>();
                _objectQueues.Add(vo.Key, queue);
            }
            else
            {
                queue = _objectQueues[vo.Key];
            }

            for (var i = 0; i < vo.Count; i++)
            {
                newObj = GameObject.Instantiate(vo.Prefab);
                newObj.GetComponent<IPoolable>().PoolKey = key;
                newObj.name = vo.Key;
                newObj.transform.SetParent(container.transform);
                newObj.SetActive(false);
                queue.Enqueue(newObj);
            }
        }

        public void HideAll()
        {
            foreach (KeyValuePair<string, Queue<GameObject>> pair in _objectQueues)
            {
                foreach (GameObject gameObject in pair.Value)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public GameObject Get(string key)
        {
            if (!_objectQueues.ContainsKey(key))
            {
                Debug.LogWarning("Not object in pool with key " + key);
                return null;
            }

            if (_objectQueues[key].Count == 0)
            {
                Debug.LogWarning("Not enough object in pool with key " + key + ". Instantiating.");
                ObjectPoolVo vo = _poolVos[key];
                Pool(vo.Key, vo.Prefab, 1);
            }

            newObj = _objectQueues[key].Dequeue();
            newObj.SetActive(true);
            newObj.GetComponent<IPoolable>().OnGetFromPool();
            return newObj;
        }

        public void Return(GameObject obj)
        {
            if (obj.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("You cant destroy " + obj.name + ". IPoolable class is missing");
                return;
            }

            if (!obj.activeInHierarchy)
                return;

            obj.GetComponent<IPoolable>().OnReturnFromPool();
            obj.transform.SetParent(container.transform);
            obj.SetActive(false);
            _objectQueues[obj.GetComponent<IPoolable>().PoolKey].Enqueue(obj);
        }

        public bool Has(string key)
        {
            return _poolVos.ContainsKey(key);
        }
    }
}