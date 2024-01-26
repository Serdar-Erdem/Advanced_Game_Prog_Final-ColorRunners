using Data.ValueObjects;
using Enums;
using Rich.Base.Runtime.Concrete.Promise;
using strange.extensions.command.impl;
using System;
using UnityEngine;

namespace Commands
{
    public class GetSaveDataCommand : Command
    {
        [Inject]
        public ES3Reader ES3Reader { get; set; }

        [Inject]
        public SaveData SaveData { get; set; }

        public void Execute()
        {
            if (ES3Reader.FileExists())
            {
                SaveData.IdleLevel = GetIntSaveData(SaveTypes.IdleLevel);
                SaveData.Bonus = GetIntSaveData(SaveTypes.Bonus);
                SaveData.Level = GetIntSaveData(SaveTypes.Level);
                SaveData.TotalColorman = GetIntSaveData(SaveTypes.TotalColorman);
                SaveData.IdleLevelListData = GetIdleLevelData();
            }
            else
            {
                InitializeDefaultValues();
            }
        }

        private int GetIntSaveData(object ýdleLevel)
        {
            throw new NotImplementedException();
        }

        private int GetIntSaveData(SaveTypes saveType)
        {
            return ES3Reader.Load<int>(saveType.ToString());
        }

        private IdleLevelListData GetIdleLevelData()
        {
            return ES3Reader.Load<IdleLevelListData>("IdleLevelDataList") ?? new IdleLevelListData();
        }

        private void InitializeDefaultValues()
        {
            ES3Reader.Save(SaveTypes.IdleLevel.ToString(), 0);
            ES3Reader.Save(SaveTypes.Bonus.ToString(), 0);
            ES3Reader.Save(SaveTypes.Level.ToString(), 0);
            ES3Reader.Save(SaveTypes.TotalColorman.ToString(), 0);
            ES3Reader.Save("IdleLevelDataList", new IdleLevelListData());
            Execute();  
        }
    }

    internal class IdleLevelListData
    {
    }
}
