using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ScarletMaiden
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]    
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.scarletmaiden.uwfixes";
        private const string PluginName = "Scarlet Maiden UltraWide Fixes";
        private const string PluginVersion = "0.0.1";
        public static ManualLogSource LOG { get; private set; }
        internal static ConfigEntry<bool> EnableCustomOrthographicSize { get; set; }
        internal static ConfigEntry<float> CustomOrthographicSize { get; set; }
        internal static ConfigEntry<KeyboardShortcut> IncreaseZoom { get; set; }
        internal static ConfigEntry<KeyboardShortcut> DecreaseZoom { get; set; }
        public static float OriginalZoom { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);
            EnableCustomOrthographicSize = Config.Bind("Camera", "Enable Custom Camera Zoom", false, new ConfigDescription("Enable this if you are using an ultra-wide monitor and the camera is too close/far away for your liking.", null, new ConfigurationManagerAttributes {Order = 4}));
            CustomOrthographicSize = Config.Bind("Camera", "Custom Custom Camera Zoom", 5f, new ConfigDescription("Adjust this number to your personal preference. Use Num+, Num- to control in game.", null, new ConfigurationManagerAttributes {Order = 3}));
            IncreaseZoom = Config.Bind("Camera", "Increase Zoom", new KeyboardShortcut(KeyCode.KeypadPlus), new ConfigDescription("Increase the camera zoom.", null, new ConfigurationManagerAttributes {Order = 2}));
            DecreaseZoom = Config.Bind("Camera", "Decrease Zoom", new KeyboardShortcut(KeyCode.KeypadMinus), new ConfigDescription("Decrease the camera zoom.", null, new ConfigurationManagerAttributes {Order = 1}));
            Config.Bind("Miscellaneous", "Reset to Recommended", true, new ConfigDescription("Set the mod to p1xel8ted's recommended settings.", null, new ConfigurationManagerAttributes {CustomDrawer = ResetToDefault, Order = 0}));
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogWarning($"Plugin {PluginName} is loaded!");
        }

        private static void ResetToDefault(ConfigEntryBase entry)
        {
            var button = GUILayout.Button("Original Zoom", GUILayout.ExpandWidth(true));
            if (!button) return;
            
            EnableCustomOrthographicSize.Value = false;
            CustomOrthographicSize.Value = OriginalZoom;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFix>().UpdateOrthographicSize();
        }


        private void OnDestroy()
        {
            LOG.LogError("I've been destroyed!");
        }
        
        private void OnDisable()
        {
            LOG.LogError("I've been disabled!");
        }
    }
}