using System;
using Common.AsyncBundles.Presets;

namespace Common.AsyncBundles
{
    [Serializable]
    public class AssetFile
    {
        public string FileName;
        public string BundleName;
        public string Guid;
        public string Name;
        public string[] Tags;

        public AssetFileSource Source;
        public AssetGroup Group;
    }

    [Serializable]
    public enum AssetFileSource
    {
        Bundle,
        Resources
    }
}