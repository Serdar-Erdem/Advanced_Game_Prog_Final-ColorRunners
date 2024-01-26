using System;

namespace Modules.Core.Concrete.Enums
{
    [Flags]
    public enum GameStatus
    {
        None = 0,
        Main = 1 << 0,
        Blocking = 1 << 1,
        All = 1 << 2
    }
}