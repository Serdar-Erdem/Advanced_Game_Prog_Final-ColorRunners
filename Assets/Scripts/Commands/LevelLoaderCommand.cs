using Commands.Signals;
using Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Commands
{
    public class LoadLevelCommand : Command
    {
        [Inject]
        public int LevelID { get; set; }

        [Inject]
        public Transform LevelHolder { get; set; }

        [Inject]
        public CoreGameSignals CoreGameSignals { get; set; }

        public override void Execute()
        {
            string prefabPath = $"Prefabs/LevelPrefabs/level {LevelID}";
            GameObject levelPrefab = Resources.Load<GameObject>(prefabPath);

            if (levelPrefab != null)
            {
                GameObject levelInstance = GameObject.Instantiate(levelPrefab, LevelHolder);
                CoreGameSignals.onGameInitLevel?.Invoke();
            }
            else
            {
                Debug.LogError($"Prefab not found at path: {prefabPath}");
            }
        }
    }
}
