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
            if (!SixthSense.DeviceManager.IsConnected()) return;

            if (GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null && StartOfRound.Instance != null)
            {
                var localPlayerController = GameNetworkManager.Instance.localPlayerController;

                if (StartOfRound.Instance.shipHasLanded)
                {
                    var level = StartOfRound.Instance.currentLevel;

                    foreach (var enemy in level.Enemies)
                    {
                        var enemyPrefab = enemy.enemyType.enemyPrefab;
                        float distance = Vector3.Distance(localPlayerController.transform.position, enemyPrefab.transform.position);

                        if (distance <= Config.DetectionRange.Value)
                        {
                            SixthSense.Log.LogInfo("OwO theres a boi behind me >.<");
                            SixthSense.DeviceManager.VibrateConnectedDevices(0.1, 0.1f);
                        }
                    }
                }
            }
        }
    }
}
