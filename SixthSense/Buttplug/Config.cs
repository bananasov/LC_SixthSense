using BepInEx;
using BepInEx.Configuration;

namespace SixthSense.Buttplug
{
    internal class Config
    {
        private static ConfigFile ConfigFile { get; set; }

        internal static ConfigEntry<string> ServerUri { get; set; }

        internal static ConfigEntry<float> DetectionRange { get; set; }

        static Config()
        {
            ConfigFile = new ConfigFile(Paths.ConfigPath + "\\SixthSense.cfg", true);

            ServerUri = ConfigFile.Bind(
                "Devices",
                "Server Uri",
                "ws://localhost:12345",
                "URI of the Intiface server."
            );

            DetectionRange = ConfigFile.Bind("Vibrations", "DetectionRange", 50.0f, "How close the enemy has to be to vibrate");
        }
    }
}
