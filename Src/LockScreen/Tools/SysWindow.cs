using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Lib.Tools;

namespace LockScreen.Tools
{
#pragma warning disable SYSLIB1054 // Используйте \"LibraryImportAttribute\" вместо \"DllImportAttribute\" для генерирования кода маршализации P/Invoke во время компиляции
    /// <summary>
    /// System level window
    /// </summary>
    /// <param name="hwnd"></param>
    public class SysWindow(IntPtr hwnd)
    {
        public readonly IntPtr Hwnd = hwnd;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        //https://github.com/dotnet/wpf/issues/4127#issuecomment-790194817
        public void Move(Rectangle rect)
        {
            // The first move puts it on the correct monitor, which triggers WM_DPICHANGED
            // The +1/-1 coerces WPF to update Window.Top/Left/Width/Height in the second move
            //MoveWindow(Hwnd, rect.Left + 1, rect.Top + 1, rect.Width - 1, rect.Height - 1, false);
            MoveWindow(Hwnd, rect.Left, rect.Top, rect.Width, rect.Height, true);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern int GetMenuString(IntPtr hMenu, int uIDItem, [Out] StringBuilder lpString, int cchMax, int flags);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern bool ModifyMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        //private const int SC_MAXIMIZE = 0xF030;
        //private const int SC_MINIMIZE = 0xF020;
        //private const int MF_DISABLED = 0x00000002;
        //private const int MF_ENABLED = 0x00000000;
        //private const int MF_BYCOMMAND = 0x00000000;
        //private const int MF_GRAYED = 0x00000001;

        private const uint MF_BYPOSITION = 0x400;
        private const uint MF_REMOVE = 0x1000;

        /// <summary>
        /// Remove Alt + space system menu 
        /// </summary>
        public void RemoveSystemMenu()
        {
            IntPtr hSystemMenu = GetSystemMenu(Hwnd, false);
            int count = GetMenuItemCount(hSystemMenu);
            for (int i = 0; i < count; i++)
            {
                RemoveMenu(hSystemMenu, 0, (MF_BYPOSITION | MF_REMOVE));
            }
            _ = DestroyMenu(hSystemMenu);
        }

        public void ExtToolStyleSet()
        {
            int exStyle = (int)GetWindowLong(Hwnd, (int)GetWindowLongFields.GWL_EXSTYLE);
            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(Hwnd, (int)GetWindowLongFields.GWL_EXSTYLE, exStyle);
        }

        //https://stackoverflow.com/a/551847/2771556
        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion

        public interface ISysWindow
        {
            public SysWindow Win { get; }
        }


        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Native.RECT rectangle);

        public Native.RECT GetPos()
        {
            var r = new Native.RECT();
            GetWindowRect(Hwnd, ref r);
            return r;
        }
    }
#pragma warning restore SYSLIB1054 // Используйте \"LibraryImportAttribute\" вместо \"DllImportAttribute\" для генерирования кода маршализации P/Invoke во время компиляции
}
