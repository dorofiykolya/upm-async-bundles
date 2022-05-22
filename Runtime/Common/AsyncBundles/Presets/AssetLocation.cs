using System;
using UnityEngine;

namespace Common.AsyncBundles.Presets
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(AssetLocation), menuName = "Assets/AssetLocation")]
    public class AssetLocation : ScriptableObject
    {
        public string BuildPath;
        public string LoadPath;

#if UNITY_EDITOR
        public static AssetLocation[] EditorCache;

        void OnEnable()
        {
            EditorCache = null;
        }

        void OnDestroy()
        {
            EditorCache = null;
        }
#endif
    }
}