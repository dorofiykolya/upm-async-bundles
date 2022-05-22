using System;
using Common.AsyncBundles.Presets;

namespace Common.AsyncBundles.Manifests
{
    [Serializable]
    public class Bundle
    {
        public string Name;
        public string Hash;
        public string[] Dependencies;
        public string Uri;
        public long FileSize;
        public UnloadType UnloadType;
        public uint Crc;
        public float DelayToUnload;

        [field: NonSerialized] public Bundle[] DependencyBundles { get; set; }
    }
}