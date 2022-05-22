using System;

namespace Common.AsyncBundles.Presets
{
    [Serializable]
    public enum UnloadType
    {
        HoldWhileUsed = 0,
        UnloadAfterResolve = 1,
        NeverUnload = 2
    }
}