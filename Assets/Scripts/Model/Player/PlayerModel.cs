using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model.Player
{
    public class PlayerModel : IPlayerModel
    {
        private CD_Player _playerData;

        private const string PlayerDataPath = "Data/CD_Player";
        
        public byte StageValue { get; set; }

        [PostConstruct]
        private void OnLoadPlayerData()
        {
            _playerData = Resources.Load<CD_Player>(PlayerDataPath);
        }

        public CD_Player PlayerData
        {
            get
            {
                if (_playerData == null)
                {
                    OnLoadPlayerData();
                }

                return _playerData;
            }
            set => _playerData = value;
        }
    }
}