using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SixthSense.Buttplug;
using UnityEngine;

namespace SixthSense
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class SixthSense : BaseUnityPlugin
    {
        public static DeviceManager DeviceManager { get; private set; }
        public static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;

            DeviceManager = new DeviceManager(Logger, "SixthSense");
            DeviceManager.ConnectDevices();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has loaded!");
        }
    }

    public class SixthSenseComponent : MonoBehaviour
    {
        void Awake()
        {
            SixthSense.Log.LogInfo("Awoke.");
        }

        void Update()
        {
            SixthSense.Log.LogInfo("called");
        }
    }
}
