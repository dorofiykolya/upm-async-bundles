using System;
using System.Collections.Generic;
using System.IO;
using Common.AsyncBundles.Bundles;
using Common.AsyncBundles.Manifests;
using UnityEngine;

namespace Common.AsyncBundles
{
    public class AsyncAssets
    {
        public static string ResourcesManifestPath => nameof(AssetManifestFile);

        private static AsyncAssetImpl _impl;
        private static AsyncAssetEditorImpl _editorImpl;

        private static AsyncAssetImpl RuntimeImpl => _impl ?? (_impl = new AsyncAssetImpl());
        private static AsyncAssetEditorImpl EditorImpl => _editorImpl ?? (_editorImpl = new AsyncAssetEditorImpl());
        private static bool IsRuntime => Application.isPlaying && !Settings.UseAssetDatabase;

        private static IAsyncAssetImpl Impl
        {
            get
            {
                if (IsRuntime)
                {
                    return RuntimeImpl;
                }

                return EditorImpl;
            }
        }

        public static IEnumerable<AsyncBundle> Bundles => RuntimeImpl.Bundles;

        public static bool IsReady => Impl.IsReady;

        public static void ExecuteOnReady(Lifetime lifetime, Action<IManifestProvider> listener)
        {
            Impl.ExecuteOnReady(lifetime, listener);
        }

        public static void Release(AsyncReference reference)
        {
            Impl.Release(reference);
        }

        public static AsyncReference<List<AsyncReference<T>>> RetainByFilter<T>(AssetFilterFunc filter,
            object context = null) where T : UnityEngine.Object
        {
            return Impl.RetainByFilter<T>(filter, context);
        }

        public static AsyncReference<List<AsyncReference<T>>> RetainByTag<T>(string tag, object context = null)
            where T : UnityEngine.Object
        {
            return Impl.RetainByTag<T>(tag, context);
        }

        public static AsyncReference<T> RetainByName<T>(string name, object context = null) where T : UnityEngine.Object
        {
            return Impl.RetainByName<T>(name, context);
        }

        public static AsyncReference<T> RetainByGuid<T>(string guid, object context = null) where T : UnityEngine.Object
        {
            return Impl.RetainByGuid<T>(guid, context);
        }

        public static class Settings
        {
            private const string UseAssetDatabasePrefName = nameof(AsyncAssets) + ".useAssetDatabase";
            private const string UseAssetDatabaseNetworkEmulationPrefName = nameof(AsyncAssets) + ".useAssetDatabaseNetworkEmulation";
            private const string AssetDatabaseNetworkEmulationSpeedPrefName = nameof(AsyncAssets) + ".assetDatabaseNetworkEmulationSpeed";
            private const string UseLocalBundlesPrefName = nameof(AsyncAssets) + ".useLocalBundles";

            private const string DependencyResolverUseDisableGroupsPrefName =
                nameof(AsyncAssets) + ".dependencyResolverUseDisableGroups";

            private static string _buildPath;

            public static string BuildPath
            {
                get
                {
                    if (_buildPath == null)
                    {
                        var dir = new DirectoryInfo(Application.dataPath).Parent;
                        _buildPath = $"{dir.FullName}/Library/AssetBuilder/Bundles";
                    }

                    return _buildPath;
                }
            }

            public static bool UseLocalBundles
            {
                get
                {
#if UNITY_EDITOR
                    return UnityEditor.EditorPrefs.GetBool(UseLocalBundlesPrefName);
#else
          return false;
#endif
                }
                set
                {
#if UNITY_EDITOR
                    UnityEditor.EditorPrefs.SetBool(UseLocalBundlesPrefName, value);
#endif
                }
            }

            public static bool UseAssetDatabase
            {
                set
                {
#if UNITY_EDITOR
                    UnityEditor.EditorPrefs.SetBool(UseAssetDatabasePrefName, value);
#endif
                }
                get
                {
#if UNITY_EDITOR
                    return UnityEditor.EditorPrefs.GetBool(UseAssetDatabasePrefName);
#else
          return false;
#endif
                }
            }

            public static bool UseAssetDatabaseNetworkEmulation
            {
                set
                {
#if UNITY_EDITOR
                    UnityEditor.EditorPrefs.SetBool(UseAssetDatabaseNetworkEmulationPrefName, value);
#endif
                }
                get
                {
#if UNITY_EDITOR
                    return UnityEditor.EditorPrefs.GetBool(UseAssetDatabaseNetworkEmulationPrefName);
#else
                    return false;
#endif
                }
            }

            public static int AssetDatabaseNetworkEmulationBytesPerSecond
            {
                set
                {
#if UNITY_EDITOR
                    UnityEditor.EditorPrefs.SetInt(AssetDatabaseNetworkEmulationSpeedPrefName, value);
#endif
                }
                get
                {
#if UNITY_EDITOR
                    return UnityEditor.EditorPrefs.GetInt(AssetDatabaseNetworkEmulationSpeedPrefName, 0);
#else
          return 0;
#endif
                }
            }

            public static bool DependencyResolverUseDisableGroups
            {
                set
                {
#if UNITY_EDITOR
                    UnityEditor.EditorPrefs.SetBool(DependencyResolverUseDisableGroupsPrefName, value);
#endif
                }
                get
                {
#if UNITY_EDITOR
                    return UnityEditor.EditorPrefs.GetBool(DependencyResolverUseDisableGroupsPrefName);
#else
          return false;
#endif
                }
            }
        }
    }
}