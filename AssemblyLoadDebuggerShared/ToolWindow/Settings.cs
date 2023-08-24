using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ToolWindow
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty("autoCapture")]
        public bool AutoCapture { get; set; }

        [JsonProperty("breakPoints")]
        public List<string> BreakPoints{ get; set; }

        public static Settings FromFile(string settingsFilePath)
        {
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(settingsFilePath);
                    return JObject.Parse(jsonString).ToObject<Settings>();
                }
                catch
                {
                }
            }

            return new Settings()
            {
                BreakPoints = new List<string>()
            };
        }

        public void SaveSettings(string settingsFilePath)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                File.WriteAllText(settingsFilePath, jsonString);
            }
            catch
            {
            }
        }
    }
}