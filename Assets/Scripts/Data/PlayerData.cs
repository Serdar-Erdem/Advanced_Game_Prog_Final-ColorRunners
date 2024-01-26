using System;

namespace Data.ValueObjects
{
    [Serializable]
    public class PlayerData
    {
        public PlayerMovementData PlayerMovementData;
        public float ScaleCounter = 3.5f;
    }
}
