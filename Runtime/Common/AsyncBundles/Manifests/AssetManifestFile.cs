using System;
using UnityEngine;

namespace Common.AsyncBundles.Manifests
{
    [Serializable]
    public class AssetManifestFile : ScriptableObject
    {
        public AssetsManifest Manifest;
    }
}