using System;
using UnityEngine;

namespace Common.AsyncBundles.Presets
{
    [Serializable]
    public class AssetGroupBuildProcessor : ScriptableObject
    {
        public virtual void OnPreBuild(AssetGroup group)
        {
        }

        public virtual void OnPostBuild(AssetGroup group)
        {
        }
    }
}