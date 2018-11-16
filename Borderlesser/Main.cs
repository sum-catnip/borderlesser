using System;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Borderlesser {
   class Main {
      private readonly Hotkey m_hotkey;

      public Main(Window window) {
         m_hotkey = new Hotkey(window, MakeBorderless, Config.Instance.Hotkey, Config.Instance.HotKeyModifier);
      }

      private void MakeBorderless() {
         HandleRef target = new HandleRef(this, Win32.GetForegroundWindow());

         // set window style
         Win32.SetWindowLongPtr(target, WindowLongFlags.GWL_STYLE, new IntPtr((int)Config.Instance.Styles));
         // set window style ex
         Win32.SetWindowLongPtr(target, WindowLongFlags.GWL_EXSTYLE, new IntPtr((int)Config.Instance.StylesEx));

         // make window full size
         Win32.SetWindowPos(
            target.Handle, 
            new IntPtr((int)InserAfterFlags.HWND_TOPMOST), 
            Config.Instance.FromX,
            Config.Instance.FromY,
            Config.Instance.ScreenWidth,
            Config.Instance.ScreenHeigth, 
            SetWindowPosFlags.FrameChanged
         );
      }

      public void SetAutostart(bool autostart) {
         Config.Instance.Autostart = autostart;
         var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
               "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

         if (autostart) key.SetValue("Borderlesser", System.Windows.Forms.Application.ExecutablePath);
         else           key.DeleteValue("Borderlesser");
      }

      public void SetHotkey(Keys key, HotKeyModifiers mod) {
         Config.Instance.Hotkey = key;
         Config.Instance.HotKeyModifier = mod;

         m_hotkey.ListenForHotKey(key, mod);
      }
   }
}
