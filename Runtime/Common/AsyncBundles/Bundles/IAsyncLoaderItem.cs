﻿using UnityEngine;

namespace Common.AsyncBundles.Bundles
{
    public interface IAsyncLoaderItem : IAsyncLoaderItemProgress
    {
        AssetBundle Bundle { get; }
        void Unload(bool v);
    }

    public interface IAsyncLoaderItemProgress
    {
        string Path { get; }
        float Progress { get; }
    }
}