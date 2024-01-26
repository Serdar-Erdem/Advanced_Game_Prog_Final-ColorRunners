using Modules.Core.Concrete.Enums;
using UnityEngine;

namespace Modules.Core.Concrete.Data
{
    [CreateAssetMenu(menuName = "Runtime Data/Game Status", order = 10)]
    public class RD_GameStatus : ScriptableObject
    {
        public GameStatus Value;
    }
}