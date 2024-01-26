using System.Collections.Generic;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Concrete.Promise;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Model
{
    public interface IBundleModel
    {
        void AddBundleData(string key, BundleDataVo vo);

        Dictionary<string, BundleDataVo> GetBundleDatas();

        IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false);

        IPromise<BundleLoadData> LoadBundle(BundleLoadData loadData);

        void Clear(string name, bool clearAll = true);

        void ClearLayers(string[] names);

        BundleLoadData GetBundleByName(string name);

        GameObject GetPrefabByAssetName(string bundleKey, string assetKey);

        void SetBundleData(Dictionary<string, BundleDataVo> dir);

 	    void AddAssetBundleData(string key, Dictionary<int, string> val);

        Dictionary<string, Dictionary<int, string>> GetChaptersAndLevels();

        void SetAssetBundleData(Dictionary<string, Dictionary<int, string>> val);

    }
}