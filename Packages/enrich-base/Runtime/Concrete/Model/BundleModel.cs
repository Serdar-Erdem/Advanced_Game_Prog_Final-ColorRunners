using System;
using System.Collections;
using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Concrete.Promise;
using Rich.Base.Runtime.Concrete.Root;
using strange.extensions.context.api;
using UnityEngine;
using UnityEngine.Networking;

namespace Rich.Base.Runtime.Concrete.Model
{
    public class BundleModel : IBundleModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        protected Dictionary<string, BundleLoadData> _bundleMap;

        protected Dictionary<string, BundleLoadData> _pathDataMap;

        protected Dictionary<string, BundleDataVo> _bundleDataMap;

        protected Dictionary<string,Dictionary<int, string>> _bundleChapterData;
        //public List<AvatarVo> AvatarList { get; set; }

        protected bool _isBusy;

        protected Queue<BundleLoadData> _queue;

        protected int versionBase;

        protected Dictionary<BundleLoadData, Promise<BundleLoadData>> _promisesMap;

        protected MonoBehaviour root;


        [PostConstruct]
        public void OnPostConstruct()
        {
            Caching.ClearCache();
            _pathDataMap = new Dictionary<string, BundleLoadData>();
            _bundleMap = new Dictionary<string, BundleLoadData>();
            _bundleDataMap = new Dictionary<string, BundleDataVo>();
            _promisesMap = new Dictionary<BundleLoadData, Promise<BundleLoadData>>();
            _queue = new Queue<BundleLoadData>();
            _bundleChapterData = new Dictionary<string, Dictionary<int, string>>();
            versionBase = int.Parse(Application.version.Replace(".", ""));
            root = contextView.GetComponent<RichMVCContextRoot>();
        }

        public BundleLoadData GetBundleByName(string name)
        {
            if (!_bundleMap.ContainsKey(name))
            {
                Debug.LogWarning("Bundle not found by name :" + name);
                return null;
            }

            return _bundleMap[name];
        }

        public IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false)
        {
            var loadData = new BundleLoadData
            {
                Name = name,
                Load = load,
                Path = path,
                Vers = (uint)versionBase
            };
            return LoadBundle(loadData);
        }

        public IPromise<BundleLoadData> LoadBundle(BundleLoadData loadData)
        {
            Promise<BundleLoadData> promise = new Promise<BundleLoadData>();

            _promisesMap.Add(loadData, promise);

            if (_pathDataMap.ContainsKey(loadData.Path))
            {
                var predata = _pathDataMap[loadData.Path];
                //        Debug.Log(loadData.Path + " already loaded.");
                promise.Resolve(predata);
            }
            else
            {
                if (!_isBusy)
                {
                    _isBusy = true;
                    LoadBundleInner(loadData);
                }
                else
                {
                    //          Debug.Log("Queued " + loadData.Name);
                    _queue.Enqueue(loadData);
                }
            }

            return promise;
        }

        protected void LoadBundleInner(BundleLoadData data)
        {
            if (_pathDataMap.ContainsKey(data.Path))
            {
                //        _promisesMap[data].Reject(new BundleAlreadyLoadedException(data));
                _promisesMap[data].Resolve(_pathDataMap[data.Path]);
                CheckQueue();
                return;
            }

            _isBusy = true;
            root.StartCoroutine(LoadBundleWithUrl(data));
        }

        protected void CheckQueue()
        {
            _isBusy = false;
            if (_queue.Count > 0)
            {
                LoadBundleInner(_queue.Dequeue());
            }
        }

        protected IEnumerator LoadBundleWithUrl(BundleLoadData data)
        {
            while (!Caching.ready)
                yield return null;

            //Debug.Log(data.Path);
            using (UnityWebRequest www = new UnityWebRequest(data.Path))
            {
                DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, data.Vers, 0);
                //DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, data.Vers, 0);

                www.downloadHandler = handler;
                yield return www.SendWebRequest();

                if (www.error != null)
                {
                    var exception = new Exception(www.error);

                    _promisesMap[data].Reject(exception);
                    Debug.LogWarning(data.Name + " bundle loading problem. " + www.error + " " + data.Path);
                    CheckQueue();
                    yield break;
                }

                data.Bundle = handler.assetBundle;

                Clear(data.Name);
                _pathDataMap.Add(data.Path, data);
                _bundleMap.Add(data.Name, data);
                if (data.Load && data.Bundle != null)
                {
                    AsyncOperation loadOperation = data.Bundle.LoadAllAssetsAsync();
                    if (loadOperation != null)
                    {
                        while (!loadOperation.isDone)
                        {
                            yield return null;
                        }
                    }
                }

                CheckQueue();

                //Load Success
                _promisesMap[data].Resolve(data);
            }
        }

        public void Clear(string name, bool clearAll = true)
        {
            if (!_bundleMap.ContainsKey(name))
                return;

            var data = _bundleMap[name];

            if (data == null)
                return;

            _pathDataMap.Remove(data.Path);
            _bundleMap.Remove(data.Name);

            if (data.Bundle != null)
                data.Bundle.Unload(clearAll);

            data.Dispose();
        }

        public void ClearLayers(string[] names)
        {
            foreach (string t in names)
            {
                Clear(t);
            }
        }

        public void AddBundleData(string key, BundleDataVo vo)
        {
            if (!_bundleDataMap.ContainsKey(key))
            {
                _bundleDataMap.Add(key, vo);
            }
        }

        public GameObject GetPrefabByAssetName(string bundleKey, string assetKey)
        {
            try
            {
                AssetBundle bundle = _bundleMap[bundleKey].Bundle;
                GameObject go = bundle.LoadAsset<GameObject>(assetKey);
                go.transform.position = Vector3.zero;
                return go;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public Dictionary<string, BundleDataVo> GetBundleDatas()
        {
            return _bundleDataMap;
        }

        public void SetBundleData(Dictionary<string, BundleDataVo> dir)
        {
            _bundleDataMap = dir;
        }

        public void AddAssetBundleData(string key, Dictionary<int, string> val)
        {
            if(!_bundleChapterData.ContainsKey(key))
                _bundleChapterData.Add(key, val);
        }

        public Dictionary<string, Dictionary<int, string>> GetChaptersAndLevels()
        {
            return _bundleChapterData;
        }

        public void SetAssetBundleData(Dictionary<string, Dictionary<int, string>> val)
        {
            _bundleChapterData = val;
        }
    }
}