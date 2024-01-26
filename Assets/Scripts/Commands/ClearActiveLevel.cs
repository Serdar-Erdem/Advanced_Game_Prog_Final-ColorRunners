using strange.extensions.command.impl;
using UnityEngine;

namespace Commands
{
    public class ClearActiveLevelCommand : Command
    {
        [Inject]
        public Transform LevelHolder { get; set; }

        public override void Execute()
        {
            ClearActiveLevel();
        }

        private void ClearActiveLevel()
        {
            Transform childTransform = LevelHolder.GetChild(0);
            if (childTransform != null)
            {
                GameObject.Destroy(childTransform.gameObject);
            }
        }
    }
}
