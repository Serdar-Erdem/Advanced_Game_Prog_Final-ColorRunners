using Runtime.Data.UnityObject;
using strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Model.Level
{
    public class LevelModel : ILevelModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject ContextView { get; set; }

        private CD_Level _levelData { get; set; }
        private Object[] _levelObjects { get; set; }

        private byte _levelID { get; set; }
        public byte TotalLevelCount { get; set; }
        public GameObject LevelHolder { get; set; }

        private const string LevelDataPath = "Data/CD_Level";
        private const string LevelObjectsPath = "Prefabs/LevelPrefabs";
        private const string LevelHolderName = "LevelHolder";

        [PostConstruct]
        public void OnLoadLevelData()
        {
            _levelData = Resources.Load<CD_Level>(LevelDataPath);

            LevelHolder = new GameObject(LevelHolderName)
            {
                transform =
                {
                    parent = ContextView.transform
                }
            };
        }

        [PostConstruct]
        public void OnLoadLevelObjects()
        {
            LevelObjects = Resources.LoadAll(LevelObjectsPath, typeof(Object));
            TotalLevelCount = (byte)LevelObjects.Length;
        }

        public CD_Level LevelData
        {
            get
            {
                if (_levelData == null)
                    OnLoadLevelData();
                return _levelData;
            }
            set => _levelData = value;
        }

        public Object[] LevelObjects
        {
            get
            {
                if (_levelObjects == null)
                    OnLoadLevelObjects();
                return _levelObjects;
            }
            set => _levelObjects = value;
        }


        public byte GetActiveLevel()
        {
            if (ES3.FileExists())
            {
                if (ES3.KeyExists("Level"))
                {
                    return ES3.Load<byte>("Level", 0);
                }
            }

            return 0;
        }

        public byte GetLevelValue()
        {
            return (byte)((byte)GetActiveLevel() % TotalLevelCount);
        }

        public void IncrementLevel()
        {
            _levelID++;
            ES3.Save("Level", _levelID);
        }
    }
}