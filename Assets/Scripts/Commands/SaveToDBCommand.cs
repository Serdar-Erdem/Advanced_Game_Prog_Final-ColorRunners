using Data.ValueObjects;
using strange.extensions.command.impl;

namespace Commands
{
    public class SaveToDatabaseCommand : Command
    {
        [Inject]
        public SaveData SaveData { get; set; }

        public override void Execute()
        {
            SaveIntData("Bonus", SaveData.Bonus);
            SaveIntData("Level", SaveData.Level);
            SaveIntData("TotalColorman", SaveData.TotalColorman);
            SaveIdleLevelListData("IdleLevelListData", SaveData.IdleLevelListData);
        }

        private void SaveIntData(string key, int value)
        {
            ES3.Save(key, value);
        }

        private void SaveIdleLevelListData(string key, IdleLevelListData value)
        {
            ES3.Save(key, value);
        }
    }
}
