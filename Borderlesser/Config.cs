using Newtonsoft.Json;
using System.Windows.Forms;
using System.Windows;
using System.IO;

namespace Borderlesser {
   class Config {
      private const string CONFIG_PATH = "config.json";
      public static Config Instance;

      public HotKeyModifiers HotKeyModifier { get; set; } = HotKeyModifiers.None;
      public Keys Hotkey                    { get; set; } = Keys.Insert;

      public WindowStyles   Styles          { get; set; } = WindowStyles.WS_VISIBLE;
      public WindowStylesEx StylesEx        { get; set; } = 0;

      public bool           Autostart       { get; set; } = false;

      public int ScreenWidth  { get; set; } = (int)SystemParameters.PrimaryScreenWidth;
      public int ScreenHeigth { get; set; } = (int)SystemParameters.PrimaryScreenHeight;

      public int FromX { get; set; } = 0;
      public int FromY { get; set; } = 0;

      private string Serialize() {
         return JsonConvert.SerializeObject(this, Formatting.Indented);
      }

      private static Config Deserilize(string json) {
         // return the default config
         if(string.IsNullOrEmpty(json)) return new Config();
         return JsonConvert.DeserializeObject<Config>(json);
      }

      static Config() {
         try { Instance = Deserilize(File.ReadAllText(CONFIG_PATH)); }
         catch(IOException) {
            // create default config
            Instance = new Config();
         }
      }

      public void WriteConfig() {
         File.WriteAllText(CONFIG_PATH, Serialize());
      }
   }
}
