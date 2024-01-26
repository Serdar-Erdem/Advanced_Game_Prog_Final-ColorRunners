using System.Collections.Generic;
using Controllers;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    [Inject]
    public class TurretAreaManager : MonoBehaviour
    {
        [SerializeField] private List<TurretAreaController> turretList;

        private List<GameObject> targetList = new List<GameObject>();
        private TurretStates turretState;

        private void Start()
        {
            InvokeRepeating(nameof(KillFromTargetList), 0, 0.5f);
        }

        public void ResetTurretArea()
        {
            CancelInvoke(nameof(KillFromTargetList));
            targetList.Clear();
            turretState = TurretStates.Search;
        }

        public void AddTargetToList(GameObject other)
        {
            targetList.Add(other);
            ChangeTurretState(TurretStates.Warned);
        }

        private void KillFromTargetList()
        {
            if (targetList.Count > 0)
            {
                GameObject currentTarget = targetList[0];
                targetList.RemoveAt(0);
                currentTarget.GetComponent<CollectableManager>().DelayedDeath(true);
                StackSignals.Instance.onDecreaseStack(0);
                turretState = targetList.Count > 0 ? ChangeTurretState(TurretStates.Warned) : ChangeTurretState(TurretStates.Search);

                foreach (var turret in turretList)
                {
                    turret.FireTurretAnimation();
                }
            }
        }

        private TurretStates ChangeTurretState(TurretStates currentState)
        {
            return turretState = currentState;
        }

        private void FixedUpdate()
        {
            foreach (var turret in turretList)
            {
                switch (turretState)
                {
                    case TurretStates.Search:
                        turret.StartSearchRotation();
                        break;
                    case TurretStates.Warned:
                        turret.StartWarnedRotation(targetList.Count > 0 ? targetList[0] : null);
                        break;
                }
            }

            CheckShutDownCondition();
        }

        private void CheckShutDownCondition()
        {
            if (targetList.Count > 0)
            {
                float relativeDistance = transform.position.z - targetList[0].transform.position.z;
                if ((turretList[0].transform.localScale.z / 2) < relativeDistance)
                {
                    Debug.Log("Pass");
                    ResetTurretArea();
                }
            }
        }
    }
}
```