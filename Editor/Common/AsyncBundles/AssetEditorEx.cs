﻿using Common.AsyncBundles.Manifests;
using Common.AsyncBundles.ReplaceProperties;

namespace Common.AsyncBundles
{
    public static class AssetEditorEx
    {
        public static AssetPathKeyType ToKeyType(this BuildPathReplacePropertyType type)
        {
            AssetPathKeyType keyType = AssetPathKeyType.Embed;
            switch (type)
            {
                case BuildPathReplacePropertyType.StreamingAssetsPath:
                    keyType = AssetPathKeyType.StreamingAssets;
                    break;
                case BuildPathReplacePropertyType.DataPath:
                    keyType = AssetPathKeyType.DataPath;
                    break;
                case BuildPathReplacePropertyType.PersistentDataPath:
                    keyType = AssetPathKeyType.PersistentDataPath;
                    break;
            }

            return keyType;
        }
    }
}