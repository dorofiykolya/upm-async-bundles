using System;
using System.Collections;
using System.IO;
using Common.AsyncBundles.Utils;
using UnityEngine;

namespace Common.AsyncBundles
{
    public class AsyncAssetsCaching
    {
        private const string DirName = "AsyncAssetsBundleCache";

        public static string CacheDir =>
            _cacheDir ?? (_cacheDir = Path.Combine(Application.persistentDataPath, DirName));

        private static string _cacheDir;
        private readonly Signal _onReady;
        private bool _isReady;

        public AsyncAssetsCaching(Lifetime lifetime, ICoroutineProvider provider)
        {
            _onReady = new Signal(lifetime);

            var cache = Caching.GetCacheByPath(CacheDir);
            if (!cache.valid)
            {
                Directory.CreateDirectory(CacheDir);
                cache = Caching.AddCache(CacheDir);
            }

            provider.StartCoroutine(WaitCacheReady(cache), lifetime);
        }

        public void ExecuteOnReady(Lifetime lifetime, Action listener)
        {
            if (_isReady) listener();
            else _onReady.Subscribe(lifetime, listener);
        }

        private IEnumerator WaitCacheReady(Cache cache)
        {
            while (true)
            {
                yield return null;
                if (cache.ready)
                {
                    Caching.MoveCacheBefore(cache, Caching.GetCacheAt(0));

                    _isReady = true;
                    _onReady.Fire();
                    break;
                }
            }
        }
    }
}