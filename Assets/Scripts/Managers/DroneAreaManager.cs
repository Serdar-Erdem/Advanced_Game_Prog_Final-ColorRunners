using System.Threading.Tasks;
using DG.Tweening;
using Enums;
using Controllers;
using Signals;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

namespace YourNamespace
{
    public class DroneAreaManager : MonoBehaviour
    {
        [Inject] private GameObject droneColliderObject { get; set; }
        [Inject] private List<Collider> droneColliderForDetect { get; set; }
        [Inject] private List<DroneColorAreaManager> droneColorAreaManagers { get; set; }
        [Inject] private GameObject droneObject { get; set; }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            DroneAreaSignals.Instance.onDroneCheckCompleted += OnDroneCheckCompleted;
            DroneAreaSignals.Instance.onDroneCheckStarted += OnDroneCheckStarted;
        }

        private void UnsubscribeEvents()
        {
            DroneAreaSignals.Instance.onDroneCheckCompleted -= OnDroneCheckCompleted;
            DroneAreaSignals.Instance.onDroneCheckStarted -= OnDroneCheckStarted;
        }

        private async void OnDroneCheckStarted()
        {
            droneObject.SetActive(true);
            await Task.Delay(200);

            foreach (var droneColorAreaManager in droneColorAreaManagers)
            {
                if (droneColorAreaManager.matchType == MatchType.UnMatched)
                {
                    droneColorAreaManager.gameObject.transform.DOScaleZ(0, 0.5f).OnComplete(() =>
                    {
                        droneColorAreaManager.gameObject.transform.DOScaleX(0, 0.5f);
                    });
                }
            }
        }

        private void OnDroneCheckCompleted()
        {
            ChangeColliders();
        }

        private async void ChangeColliders()
        {
            foreach (var collider in droneColliderForDetect)
            {
                collider.enabled = false;
            }

            droneColliderObject.SetActive(true);
            await Task.Delay(200);
            droneColliderObject.SetActive(false);
        }

        internal void OnAreaColorChanged(ColorTypes colorType)
        {
            throw new NotImplementedException();
        }
    }
}
