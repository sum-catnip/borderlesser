using System;
using System.Runtime.InteropServices;

namespace Borderlesser {
   #region flags
   [Flags]
   public enum WindowLongFlags : int {
      GWL_EXSTYLE = -20,
      GWL_STYLE = -16
   }

   [Flags]
   public enum WindowStyles : uint {
      WS_BORDER = 0x00800000,
      WS_CAPTION = 0x00C00000,
      WS_CHILD = 0x40000000,
      WS_CHILDWINDOW = 0x40000000,
      WS_CLIPCHILDREN = 0x02000000,
      WS_CLIPSIBLINGS = 0x04000000,
      WS_DISABLED = 0x08000000,
      WS_DLGFRAME = 0x00400000,
      WS_GROUP = 0x00020000,
      WS_HSCROLL = 0x00100000,
      WS_ICONIC = 0x20000000,
      WS_MAXIMIZE = 0x01000000,
      WS_MAXIMIZEBOX = 0x00010000,
      WS_MINIMIZE = 0x20000000,
      WS_MINIMIZEBOX = 0x00020000,
      WS_POPUP = 0x80000000,
      WS_SIZEBOX = 0x00040000,
      WS_SYSMENU = 0x00080000,
      WS_TABSTOP = 0x00010000,
      WS_THICKFRAME = 0x00040000,
      WS_VISIBLE = 0x10000000,
      WS_VSCROLL = 0x00200000
   }

   [Flags]
   public enum WindowStylesEx : uint {
      WS_EX_ACCEPTFILES = 0x00000010,
      WS_EX_APPWINDOW = 0x00040000,
      WS_EX_CLIENTEDGE = 0x00000200,
      WS_EX_COMPOSITED = 0x02000000,
      WS_EX_CONTEXTHELP = 0x00000400,
      WS_EX_CONTROLPARENT = 0x00010000,
      WS_EX_DLGMODALFRAME = 0x00000001,
      WS_EX_LAYERED = 0x00080000,
      WS_EX_LAYOUTRTL = 0x00400000,
      WS_EX_LEFTSCROLLBAR = 0x00004000,
      WS_EX_MDICHILD = 0x00000040,
      WS_EX_NOACTIVATE = 0x08000000,
      WS_EX_NOINHERITLAYOUT = 0x00100000,
      WS_EX_NOPARENTNOTIFY = 0x00000004,
      WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
      WS_EX_RIGHT = 0x00001000,
      WS_EX_RTLREADING = 0x00002000,
      WS_EX_STATICEDGE = 0x00020000,
      WS_EX_TOOLWINDOW = 0x00000080,
      WS_EX_TOPMOST = 0x00000008,
      WS_EX_TRANSPARENT = 0x00000020,
      WS_EX_WINDOWEDGE = 0x00000100
   }

   [Flags]
   public enum HotKeyModifiers : uint {
      Alt   = 1,        // MOD_ALT
      Ctrl  = 2,        // MOD_CONTROL
      Shift = 4,        // MOD_SHIFT
      Win   = 8,        // MOD_WIN
      None  = 0
   }

   [Flags]
   public enum SetWindowPosFlags : uint {
      /// <summary>If the calling thread and the thread that owns the window are attached to different input queues,
      /// the system posts the request to the thread that owns the window. This prevents the calling thread from
      /// blocking its execution while other threads process the request.</summary>
      /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
      AsynchronousWindowPosition = 0x4000,
      /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
      /// <remarks>SWP_DEFERERASE</remarks>
      DeferErase = 0x2000,
      /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
      /// <remarks>SWP_DRAWFRAME</remarks>
      DrawFrame = 0x0020,
      /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
      /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
      /// is sent only when the window's size is being changed.</summary>
      /// <remarks>SWP_FRAMECHANGED</remarks>
      FrameChanged = 0x0020,
      /// <summary>Hides the window.</summary>
      /// <remarks>SWP_HIDEWINDOW</remarks>
      HideWindow = 0x0080,
      /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
      /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
      /// parameter).</summary>
      /// <remarks>SWP_NOACTIVATE</remarks>
      DoNotActivate = 0x0010,
      /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
      /// contents of the client area are saved and copied back into the client area after the window is sized or
      /// repositioned.</summary>
      /// <remarks>SWP_NOCOPYBITS</remarks>
      DoNotCopyBits = 0x0100,
      /// <summary>Retains the current position (ignores X and Y parameters).</summary>
      /// <remarks>SWP_NOMOVE</remarks>
      IgnoreMove = 0x0002,
      /// <summary>Does not change the owner window's position in the Z order.</summary>
      /// <remarks>SWP_NOOWNERZORDER</remarks>
      DoNotChangeOwnerZOrder = 0x0200,
      /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
      /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
      /// window uncovered as a result of the window being moved. When this flag is set, the application must
      /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
      /// <remarks>SWP_NOREDRAW</remarks>
      DoNotRedraw = 0x0008,
      /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
      /// <remarks>SWP_NOREPOSITION</remarks>
      DoNotReposition = 0x0200,
      /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
      /// <remarks>SWP_NOSENDCHANGING</remarks>
      DoNotSendChangingEvent = 0x0400,
      /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
      /// <remarks>SWP_NOSIZE</remarks>
      IgnoreResize = 0x0001,
      /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
      /// <remarks>SWP_NOZORDER</remarks>
      IgnoreZOrder = 0x0004,
      /// <summary>Displays the window.</summary>
      /// <remarks>SWP_SHOWWINDOW</remarks>
      ShowWindow = 0x0040,
   }

   [Flags]
   public enum InserAfterFlags : int {
      HWND_BOTTOM = 1,
      HWND_NOTOPMOST = -2,
      HWND_TOP = 0,
      HWND_TOPMOST = -1
   }
   #endregion

   static class Win32 {
      public static IntPtr SetWindowLongPtr(HandleRef hWnd, WindowLongFlags nIndex, IntPtr dwNewLong) {
         if (IntPtr.Size == 8) return _SetWindowLongPtr64(hWnd, nIndex, dwNewLong); // if x64
         else return new IntPtr(_SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
      }

      public static IntPtr GetForegroundWindow()                                          { return _GetForegroundWindow();                     }
      public static bool   RegisterHotKey(IntPtr hWnd, int id, HotKeyModifiers fsModifiers, uint vk) { return _RegisterHotKey(hWnd, id, fsModifiers, vk); }
      public static int    UnregisterHotKey(IntPtr hWnd, int id)                          { return _UnregisterHotKey(hWnd, id);                }
      public static short  GlobalAddAtom(string lpString)                                 { return _GlobalAddAtom(lpString);                   }
      public static short  GlobalDeleteAtom(short nAtom)                                  { return _GlobalDeleteAtom(nAtom);                   }

      public static bool   SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags) 
         { return _SetWindowPos(hWnd, hWndInsertAfter, X, Y, cx, cy, uFlags); }

      #region dll imports
      [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
      private static extern int _SetWindowLong32(HandleRef hWnd, WindowLongFlags nIndex, int dwNewLong);

      [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
      private static extern IntPtr _SetWindowLongPtr64(HandleRef hWnd, WindowLongFlags nIndex, IntPtr dwNewLong);

      [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
      private static extern IntPtr _GetForegroundWindow();

      [DllImport("user32", SetLastError = true, EntryPoint = "RegisterHotKey")]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool  _RegisterHotKey(IntPtr hwnd, int id, HotKeyModifiers fsModifiers, uint vk);

      [DllImport("user32", SetLastError = true, EntryPoint = "UnregisterHotKey")]
      private static extern int   _UnregisterHotKey(IntPtr hwnd, int id);

      [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalAddAtom")]
      private static extern short _GlobalAddAtom(string lpString);

      [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalDeleteAtom")]
      private static extern short _GlobalDeleteAtom(short nAtom);

      [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowPos")]
      private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
      #endregion
   }
}
