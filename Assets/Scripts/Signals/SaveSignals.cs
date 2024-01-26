using strange.extensions.signal.impl;
using Extentions;
using Data.ValueObjects;
using Enums;

namespace Signals
{
    [Inject]
    public class SaveSignals : MonoSingleton<SaveSignals>
    {
        private Signal<SaveTypes, int> onChangeSaveDataSignal = new Signal<SaveTypes, int>();
        private Signal<IdleLevelListData> onChangeIdleLevelListDataSignal = new Signal<IdleLevelListData>();
        private Signal onApplicationPauseSignal = new Signal();
        private Signal<SaveData> onSendDataToManagersSignal = new Signal();

        public Signal<SaveTypes, int> onChangeSaveData
        {
            get { return onChangeSaveDataSignal; }
        }

        public Signal<IdleLevelListData> onChangeIdleLevelListData
        {
            get { return onChangeIdleLevelListDataSignal; }
        }

        public Signal onApplicationPause
        {
            get { return onApplicationPauseSignal; }
        }

        public Signal<SaveData> onSendDataToManagers
        {
            get { return onSendDataToManagersSignal; }
        }

        public void InvokeOnChangeSaveData(SaveTypes saveType, int data)
        {
            onChangeSaveDataSignal.Dispatch(saveType, data);
        }

        public void InvokeOnChangeIdleLevelListData(IdleLevelListData data)
        {
            onChangeIdleLevelListDataSignal.Dispatch(data);
        }

        public void InvokeOnApplicationPause()
        {
            onApplicationPauseSignal.Dispatch();
        }

        public int InvokeOnGetIntSaveData(SaveTypes saveType)
        {
            return onChangeSaveDataSignal.Dispatch(saveType);
        }

        public IdleLevelListData InvokeOnGetIdleSaveData()
        {
            return onChangeIdleLevelListDataSignal.Dispatch();
        }

        public void InvokeOnSendDataToManagers(SaveData data)
        {
            onSendDataToManagersSignal.Dispatch(data);
        }
    }
}
```