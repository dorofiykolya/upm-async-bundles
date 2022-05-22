using System;

namespace Common.AsyncBundles.ReplaceProperties
{
    [Serializable]
    public struct BuildPathReplaceProperty
    {
        public string Key;
        public Func<string> Value;
        public BuildPathReplacePropertyType Type;
    }
}