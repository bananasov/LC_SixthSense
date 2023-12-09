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

            // Wow, this is stupid but i guess it works
            GameObject gameObject = new("SixthSense");
            gameObject.AddComponent<SixthSenseComponent>();
            gameObject.hideFlags = (HideFlags)61;
            DontDestroyOnLoad(gameObject);

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
            if (GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null && StartOfRound.Instance != null)
            {
                if (StartOfRound.Instance.shipHasLanded)
                {
                    var level = StartOfRound.Instance.currentLevel;

                    foreach (var enemy in level.Enemies)
                    {
                        SixthSense.Log.LogInfo($"{enemy.enemyType.enemyName}");
                    }
                }
            }
        }
    }
}
