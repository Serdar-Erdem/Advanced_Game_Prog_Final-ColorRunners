using Data.ValueObjects;
using Data.UnityObjects;
using strange.extensions.command.impl;
using UnityEngine;

namespace Commands
{
    public class InitializeGameDataCommand : Command
    {
        [Inject]
        public SaveData SaveData { get; set; }

        public override void Execute()
        {
            OnInitializeSyncData();
        }

        private IdleLevelListData GetIdleLevelListData() => Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel")?.IdleLevelListData;

        private void OnInitializeSyncData()
        {
            if (ES3.FileExists())
            {
                SaveData.IdleLevel = ES3.Load<int>("IdleLevel", 0);
                SaveData.TotalColorman = ES3.Load<int>("TotalColorman", 0);
                SaveData.Level = ES3.Load<int>("Level", 0);
                SaveData.Bonus = ES3.Load<int>("Bonus", 0);
                SaveData.IdleLevelListData = ES3.Load<IdleLevelListData>("IdleLevelListData");
            }
            else
            {
                InitializeDefaultValues();
            }
        }

        private void InitializeDefaultValues()
        {
            ES3.Save("IdleLevel", 0);
            ES3.Save("TotalColorman", 0);
            ES3.Save("Level", 0);
            ES3.Save("Bonus", 0);
            ES3.Save("IdleLevelListData", GetIdleLevelListData());
            OnInitializeSyncData();
        }
    }
}
