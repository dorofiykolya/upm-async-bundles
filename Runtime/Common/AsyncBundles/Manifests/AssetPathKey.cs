﻿using System;

namespace Common.AsyncBundles.Manifests
{
    [Serializable]
    public struct AssetPathKey
    {
        public string Key;
        public string Value;
        public AssetPathKeyType Type;
    }

    public enum AssetPathKeyType
    {
        Embed,
        StreamingAssets,
        DataPath,
        PersistentDataPath
    }
}