using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.ValueObject
{
    [Serializable]
    public class BundleDataVo
    {
        public string Key;

        public float Version;

        public string Path;

        [HideInEditorMode]
        public long Size;

        public int Priority;
    }

    [Serializable]
    public class BundleInfoResponseVo
    {
        public List<BundleDataVo> list;

        public string root;
    }

    [Serializable]
    public class BundleInfoRequestVo
    {
        public const string TypeDevelopment = "dev";

        public const string TypeProduction = "prod";

        public string platform;

        public string type;
    }

    public class BundleLoadData : IDisposable
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public uint Vers { get; set; }

        public AssetBundle Bundle { get; set; }

        public bool Load { get; set; }

        public void Dispose()
        {
            Bundle = null;
        }
    }
}