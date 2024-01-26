using System.Collections.Generic;
using Data.ValueObjects;
using Signals;
using UnityEditor.Recorder;
using UnityEngine;

namespace Managers
{
    public class IdleManager : MonoBehaviour, ISaveable
    {
        [Inject] private List<BuildingManager> buildingList;

        private IdleLevelListData _idleLevelListData;
        private int _currentIdleLevelId;
        private IdleLevelData _currentIdleLevelData;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onApplicationPause += SendDataToSaveManager;
            SaveSignals.Instance.onSendDataToManagers += OnGetSaveData;
        }

        private void UnSubscribeEvents()
        {
            SaveSignals.Instance.onApplicationPause -= SendDataToSaveManager;
            SaveSignals.Instance.onSendDataToManagers -= OnGetSaveData;
        }

        private void OnGetSaveData(SaveData saveData)
        {
            _currentIdleLevelId = CoreGameSignals.Instance.onGetIdleLevelID.Invoke();
            _idleLevelListData = saveData.IdleLevelListData;
            SyncDataToBuildings();
        }


        public void SendDataToSaveManager()
        {
            PrepareSaveData();
            SaveSignals.Instance.onChangeIdleLevelListData?.Invoke(_idleLevelListData);
        }
    }
}
