using Data.ValueObjects;
using Commands;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    [Inject]
    public class SaveManager : MonoBehaviour
    {
        [Header("Data")] public SaveData Data;

        private SaveToDBCommand _saveToDBCommand;
        private InitializationSyncDatasCommand _initializationSyncDatasCommand;
        private OnGetSaveDataCommand _getSaveDataCommand;

        private void Awake()
        {
            Data = GetSaveData();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _initializationSyncDatasCommand = new InitializationSyncDatasCommand();
            _saveToDBCommand = new SaveToDBCommand();
            _getSaveDataCommand = new OnGetSaveDataCommand();
            _initializationSyncDatasCommand.OnInitializeSyncDatas(Data);
        }

        private SaveData GetSaveData() => Resources.Load<CD_Save>("Data/CD_Save").SaveData;

        #region Event Subscriptions

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
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            SaveSignals.Instance.onChangeSaveData += OnChangeSaveData;
            SaveSignals.Instance.onGetIntSaveData += OnGetIntSaveData;
            SaveSignals.Instance.onGetIdleSaveData += OnGetIdleSaveData;
            SaveSignals.Instance.onChangeIdleLevelListData += OnChangeIdleLevelListData;
            CoreGameSignals.Instance.onGameInit += OnLevelIdleInitialize;
            ApplicationSignals.Instance.onApplicationPause += OnApplicationPause;
            ApplicationSignals.Instance.onApplicationQuit += OnApplicationQuit;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            SaveSignals.Instance.onChangeSaveData -= OnChangeSaveData;
            SaveSignals.Instance.onGetIntSaveData -= OnGetIntSaveData;
            SaveSignals.Instance.onGetIdleSaveData -= OnGetIdleSaveData;
            SaveSignals.Instance.onChangeIdleLevelListData -= OnChangeIdleLevelListData;
            CoreGameSignals.Instance.onGameInit -= OnLevelIdleInitialize;
            ApplicationSignals.Instance.onApplicationPause -= OnApplicationPause;
            ApplicationSignals.Instance.onApplicationQuit -= OnApplicationQuit;
        }

        #endregion

        private void OnLevelIdleInitialize()
        {
            SaveSignals.Instance.onSendDataToManagers?.Invoke(Data);
        }

        private void OnChangeIdleLevelListData(IdleLevelListData idleLevelListData)
        {
            Data.IdleLevelListData = idleLevelListData;
            _saveToDBCommand.SaveDataToDatabase(Data);
        }

        private int OnGetIntSaveData(SaveTypes type) => _getSaveDataCommand.GetIntSaveData(type);

        private void OnChangeSaveData(SaveTypes saveType, int saveAmount)
        {
            switch (saveType)
            {
                case SaveTypes.All:
                    Data.Bonus = Data.TotalColorman = Data.Level = Data.IdleLevel = saveAmount;
                    break;
                case SaveTypes.Bonus:
                    Data.Bonus = saveAmount;
                    break;
                case SaveTypes.TotalColorman:
                    Data.TotalColorman = saveAmount;
                    break;
                case SaveTypes.Level:
                    Data.Level = saveAmount;
                    break;
                case SaveTypes.IdleLevel:
                    Data.IdleLevel = saveAmount;
                    break;
            }

            _saveToDBCommand.SaveDataToDatabase(Data);
        }

        private void OnNextLevel() => SaveSignals.Instance.onApplicationPause?.Invoke();

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveSignals.Instance.onApplicationPause?.Invoke();
                Debug.Log("OnApplicationPause");
            }
        }

        private void OnApplicationQuit() => SaveSignals.Instance.onApplicationPause?.Invoke();
    }
}
