﻿using System;

namespace Common.AsyncBundles.Manifests
{
    [Serializable]
    public class Asset
    {
        public string Name;
        public string Bundle;
        public string Guid;
        public string[] Tags;
    }
}