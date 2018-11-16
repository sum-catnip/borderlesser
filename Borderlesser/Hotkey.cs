using System;
using System.Windows.Interop;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Borderlesser {
   // stole this from here: https://www.pinvoke.net/default.aspx/user32/RegisterHotKey.html
   class Hotkey : IDisposable {
      public const int WM_HOTKEY = 0x312;

      /// <summary>
      /// The unique ID to receive hotkey messages
      /// </summary>
      public short HotkeyID { get; private set; }

      /// <summary>
      /// Handle to the window listening to hotkeys
      /// </summary>
      private readonly IntPtr m_windowHandle;

      /// <summary>
      /// Callback for hot keys
      /// </summary>
      readonly Action m_onHotKeyPressed;

      public Hotkey(Window handlerWindow, Action hotKeyHandler, Keys key, HotKeyModifiers modifiers) {
         m_onHotKeyPressed = hotKeyHandler;

         // Create a unique Id for this class in this instance
         string atomName = Thread.CurrentThread.ManagedThreadId.ToString("X8") + GetType().FullName;
         HotkeyID = Win32.GlobalAddAtom(atomName);

         // Set up the hook to listen for hot keys
         m_windowHandle = new WindowInteropHelper(handlerWindow).EnsureHandle();
         if (m_windowHandle == null) throw new ApplicationException(
            "Cannot find window handle. Try calling this on or after OnSourceInitialized()"
         );

         HwndSource source = HwndSource.FromHwnd(m_windowHandle);
         source.AddHook(HwndHook);

         ListenForHotKey(key, modifiers);
      }

      /// <summary>
      /// Intermediate processing of hotkeys
      /// </summary>
      private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
         if (msg == WM_HOTKEY) {
            if(wParam.ToInt32() == HotkeyID) {
               m_onHotKeyPressed?.Invoke();
               handled = true;
            }
         }
         return IntPtr.Zero;
      }

      /// <summary>
      /// Tell what key you want to listen for.
      /// </summary>
      public void ListenForHotKey(Keys key, HotKeyModifiers modifiers) {
         Win32.UnregisterHotKey(m_windowHandle, HotkeyID);
         Win32.RegisterHotKey(m_windowHandle, HotkeyID, modifiers, (uint)key);
      }

      /// <summary>
      /// Stop listening for hotkeys
      /// </summary>
      private void StopListening() {
         if (HotkeyID != 0) {
            Win32.UnregisterHotKey(m_windowHandle, HotkeyID);
            // clean up the atom list
            Win32.GlobalDeleteAtom(HotkeyID);
            HotkeyID = 0;
         }
      }
      
      public void Dispose() {
         StopListening();
      }
   }
}
