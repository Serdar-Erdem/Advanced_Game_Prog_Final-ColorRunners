using Data.ValueObjects;
using Enums;
using strange.extensions.command.impl;
using UnityEngine;

namespace Commands
{
    public class LoadSaveDataCommand : Command
    {
        [Inject]
        public GetSaveData GetSaveData { get; set; }

        [Inject]
        public SaveData SaveData { get; set; }

        public override void Execute()
        {
            LoadIntSaveData(SaveTypes.IdleLevel);
            LoadIntSaveData(SaveTypes.Bonus);
            LoadIntSaveData(SaveTypes.Level);
            LoadIntSaveData(SaveTypes.TotalColorman);
            LoadIdleLevelData();
        }

        private void LoadIntSaveData(SaveTypes saveType)
        {
            int value = GetSaveData.GetIntSaveData(saveType);
            SetSaveData(saveType, value);
        }

        private void SetSaveData(SaveTypes saveType, int value)
        {
            switch (saveType)
            {
                case SaveTypes.IdleLevel:
                    SaveData.IdleLevel = value;
                    break;

                case SaveTypes.Bonus:
                    SaveData.Bonus = value;
                    break;

                case SaveTypes.Level:
                    SaveData.Level = value;
                    break;

                case SaveTypes.TotalColorman:
                    SaveData.TotalColorman = value;
                    break;
            }
        }

        private void LoadIdleLevelData()
        {
            SaveData.IdleLevelListData = GetSaveData.GetIdleLevelData();
        }
    }
}
