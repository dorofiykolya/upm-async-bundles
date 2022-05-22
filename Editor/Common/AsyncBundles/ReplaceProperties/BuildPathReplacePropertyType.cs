using System;

namespace Common.AsyncBundles.ReplaceProperties
{
    [Serializable]
    public enum BuildPathReplacePropertyType
    {
        StreamingAssetsPath = 0,
        DataPath = 1,
        PersistentDataPath = 2,
        EmbedToBuild = 3,
        EditorOnly = 4,
    }
}