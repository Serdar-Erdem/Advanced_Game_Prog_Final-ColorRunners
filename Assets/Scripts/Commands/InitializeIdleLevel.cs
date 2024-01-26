using UnityEngine;
using strange.extensions.command.impl;
using Commands.Signals;

namespace Commands
{
    public class LoadIdleLevelCommand : Command
    {
        [Inject]
        public int IdleLevelID { get; set; }

        [Inject]
        public Transform LevelHolder { get; set; }

        [Inject]
        public CoreGameSignals CoreGameSignals { get; set; }  

        public override void Execute()
        {
            string prefabPath = $"Prefabs/IdleLevelPrefabs/IdleLevel {IdleLevelID}";
            GameObject idleLevelPrefab = Resources.Load<GameObject>(prefabPath);

            if (idleLevelPrefab != null)
            {
                GameObject idleLevelInstance = GameObject.Instantiate(idleLevelPrefab, LevelHolder);
                CoreGameSignals.onGameInit?.Invoke();
            }
            else
            {
                Debug.LogError($"Prefab not found at path: {prefabPath}");
            }
        }
    }
}
