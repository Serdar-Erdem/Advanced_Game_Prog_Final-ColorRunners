using Runtime.Data.UnityObject;

namespace Runtime.Model.Player
{
    public interface IPlayerModel
    {
        public CD_Player PlayerData { get; set; }
        public byte StageValue { get; set; }
    }
}