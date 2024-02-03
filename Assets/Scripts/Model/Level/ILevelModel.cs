using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model.Level
{
    public interface ILevelModel
    {
        public CD_Level LevelData { get; set; }
        public Object[] LevelObjects { get; set; }
        public byte TotalLevelCount { get; set; }
        public GameObject LevelHolder { get; set; }
        byte GetActiveLevel();
        byte GetLevelValue();
        void IncrementLevel();
    }
}