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

                        Vector3 forward = localPlayerController.transform.TransformDirection(Vector3.forward);
                        Vector3 toOther = enemyPrefab.transform.position - localPlayerController.transform.position;

                        float distance = Vector3.Distance(localPlayerController.transform.position, enemyPrefab.transform.position);

                        if (Vector3.Dot(forward, toOther) < 0 && distance <= Config.DetectionRange.Value)
                        {
                            SixthSense.Log.LogInfo("OwO theres a boi behind me >.<");
                        }
                    }
                }
            }
        }
    }
}
