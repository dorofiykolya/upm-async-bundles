using System;
using Common.AsyncBundles.Manifests;
using Common.AsyncBundles.Presets;
using UnityEditor;
using UnityEngine;

namespace Common.AsyncBundles
{
    public class AssetsSettingsEditorWindow : EditorWindow
    {
        [MenuItem(AssetsPresetUtils.MenuItem + "/Settings", priority = -2010)]
        public static void Open()
        {
            GetWindow<AssetsSettingsEditorWindow>("AsyncAssets Settings").Show(true);
        }

        [MenuItem(AssetsPresetUtils.MenuItem + "/Use AssetDatabase", priority = -3010)]
        public static void ToggleUseAssetDatabase()
        {
            AsyncAssets.Settings.UseAssetDatabase = !AsyncAssets.Settings.UseAssetDatabase;
            Menu.SetChecked(AssetsPresetUtils.MenuItem + "/Use AssetDatabase", AsyncAssets.Settings.UseAssetDatabase);
        }

        [MenuItem(AssetsPresetUtils.MenuItem + "/Use AssetDatabase", true)]
        private static bool ToggleUseAssetDatabaseValidator()
        {
            Menu.SetChecked(AssetsPresetUtils.MenuItem + "/Use AssetDatabase", AsyncAssets.Settings.UseAssetDatabase);
            return true;
        }

        //
        [MenuItem(AssetsPresetUtils.MenuItem + "/Use LocalBundles", priority = -3010)]
        public static void ToggleUseLocalBundle()
        {
            AsyncAssets.Settings.UseLocalBundles = !AsyncAssets.Settings.UseLocalBundles;
            Menu.SetChecked(AssetsPresetUtils.MenuItem + "/Use LocalBundles", AsyncAssets.Settings.UseLocalBundles);
        }

        [MenuItem(AssetsPresetUtils.MenuItem + "/Use LocalBundles", true)]
        private static bool ToggleUseLocalBundleValidator()
        {
            Menu.SetChecked(AssetsPresetUtils.MenuItem + "/Use LocalBundles", AsyncAssets.Settings.UseLocalBundles);
            return true;
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        string GetFileSize(long byteCount, bool bits = true)
        {
            int div = bits ? 8 : 1;
            string size = "0 Bytes";
            if (byteCount >= (1073741824 / div))
                size = $"{byteCount / (1073741824.0 / div):##.##}" + (bits ? " GBit" : " GByte");
            else if (byteCount >= (1048576.0 / div))
                size = $"{byteCount / (1048576.0 / div):##.##}" + (bits ? " MBit" : " MByte");
            else if (byteCount >= (1024.0 / div))
                size = $"{byteCount / (1024.0 / div):##.##}" + (bits ? " KBit" : " KByte");
            else if (byteCount > 0 && byteCount < (1024.0 / div))
                size = byteCount + (bits ? " Bits" : " Byte");

            return size;
        }

        void OnGUI()
        {
            OnGUISettings();

            EditorGUILayout.Separator();

            OnGUIAssetDatabaseTools();

            EditorGUILayout.Separator();

            OnGUIDebug();

            EditorGUILayout.Separator();

            OnGUITools();
        }

        void OnGUIAssetDatabaseTools()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("AssetDatabase", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            AsyncAssets.Settings.UseAssetDatabaseNetworkEmulation = EditorGUILayout.ToggleLeft("Network Emulation",
                AsyncAssets.Settings.UseAssetDatabaseNetworkEmulation);

            if (AsyncAssets.Settings.UseAssetDatabaseNetworkEmulation)
            {
                EditorGUILayout.Separator();
                GUILayout.Label(GetFileSize(AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond, true));
                GUILayout.Label(GetFileSize(AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond, false));
                AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = EditorGUILayout.IntSlider(AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond, 1024, 100 * 1024 * 1024);
                EditorGUILayout.Separator();
                var index = GUILayout.Toolbar(-1, new[] { "dial-up", "GPRS", "EDGE", "3G", "HSPA", "4G", "4G+" }, EditorStyles.miniButton);
                switch (index)
                {
                    //DialUp
                    case 0: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = 56 * 1024; break;
                    //GPRS
                    case 1: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(0.1 * 1024 * 1024) / 8; break;
                    //EDGE
                    case 2: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(0.3 * 1024 * 1024) / 8; break;
                    //3G
                    case 3: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(1.5 * 1024 * 1024) / 8; break;
                    //HSPA
                    case 4: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(4 * 1024 * 1024) / 8; break;
                    //4G
                    case 5: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(15 * 1024 * 1024) / 8; break;
                    //4G+
                    case 6: AsyncAssets.Settings.AssetDatabaseNetworkEmulationBytesPerSecond = (int)(30 * 1024 * 1024) / 8; break;
                }
            }


            EditorGUILayout.EndVertical();
        }

        void OnGUITools()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Tools", EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            if (GUILayout.Button("Fix Assets Name", EditorStyles.miniButton))
            {
                var preset = AssetsPresetUtils.Get();
                AssetsPresetUtils.FixAssetsName(preset);
            }

            if (GUILayout.Button("Check Recursion", EditorStyles.miniButton))
            {
                var manifest = Resources.Load<AssetManifestFile>(AsyncAssets.ResourcesManifestPath);
                if (manifest == null)
                {
                    EditorUtility.DisplayDialog("Error",
                        $"Manifest not found on path:{AsyncAssets.ResourcesManifestPath}",
                        "close");
                }
                else
                {
                    AssetManifestUtils.HasRecursion(manifest.Manifest, new Progress<AssetManifestUtils.Progress>(
                        (progress =>
                        {
                            EditorUtility.DisplayProgressBar("Check recursion", progress.Message, progress.Percent);
                            if (progress.IsRecursion)
                            {
                                EditorUtility.ClearProgressBar();
                                Debug.LogError(
                                    $"Recursion Found! {progress.Message}\n -> {string.Join(" \n -> ", progress.Bundles)}");
                                EditorUtility.DisplayDialog("Recursion Found!", progress.Message, "Close");
                            }
                        })));
                    EditorUtility.ClearProgressBar();
                }
            }

            EditorGUILayout.EndVertical();
        }

        void OnGUISettings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            AsyncAssets.Settings.UseAssetDatabase =
                EditorGUILayout.ToggleLeft("Use AssetDatabase", AsyncAssets.Settings.UseAssetDatabase);
            AsyncAssets.Settings.UseLocalBundles =
                EditorGUILayout.ToggleLeft("Use Local Bundles", AsyncAssets.Settings.UseLocalBundles);
            EditorGUILayout.HelpBox("\"Use AssetDatabase\" and \"Use Local Bundles\" take effect on start play",
                MessageType.Info);
            GUILayout.Space(16f);
            AsyncAssets.Settings.DependencyResolverUseDisableGroups = EditorGUILayout.ToggleLeft(
                $"Dependency Resolver Use Do Not \"{nameof(AssetGroup.PackToBundle)}\" Groups",
                AsyncAssets.Settings.DependencyResolverUseDisableGroups);
            EditorGUILayout.EndVertical();
        }

        void OnGUIDebug()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Debug", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            var preset = AssetsPresetUtils.Get();
            if (GUILayout.Button("apply to bundle", EditorStyles.miniButtonLeft))
            {
                AssetsPresetUtils.UnCheckToBundles(preset);
                AssetsPresetUtils.CheckToBundles(preset);
            }

            if (GUILayout.Button("clear from bundles", EditorStyles.miniButtonRight))
            {
                AssetsPresetUtils.UnCheckToBundles(preset);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        class Progress<T> : IProgress<T>
        {
            private readonly Action<T> _action;

            public Progress(Action<T> action)
            {
                _action = action;
            }

            public void Report(T value)
            {
                _action(value);
            }
        }
    }
}