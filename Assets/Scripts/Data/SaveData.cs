using System;

namespace Data.ValueObjects
{
    [Serializable]
    public class SaveData
    {
        public int TotalColorman, Level, IdleLevel, Bonus;

        public IdleLevelListData IdleLevelListData { get; internal set; }
    }

    public class IdleLevelListData
    {
    }
}
