using System;
using UnityEngine;

namespace Common.AsyncBundles.Presets
{
    [Serializable]
    public abstract class AssetGroupBuildCondition : ScriptableObject
    {
        public abstract bool Success { get; }
    }
}