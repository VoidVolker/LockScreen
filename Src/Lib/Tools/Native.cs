using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Lib.Tools
{
    /// <summary>
    /// WinAPI native methods
    /// </summary>
    [SuppressMessage("Style", "IDE0059:Ненужное присваивание значения", Justification = "<Ожидание>")]
    public static class Native
    {
        [DllImport("user32.dll")]
        private static extern nint MonitorFromWindow(nint handle, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(nint hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// Check if system is shutting down
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        /// </summary>
        /// <returns></returns>
        public static bool IsShuttingDown() =>
            GetSystemMetrics(Const.SM_SHUTTINGDOWN) != 0;

        /// <summary>
        /// Check if system is not shutting down
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        /// </summary>
        /// <returns></returns>
        public static bool IsNotShuttingDown() =>
            GetSystemMetrics(Const.SM_SHUTTINGDOWN) == 0;

        /// <summary>
        /// A utility class to determine a process parent.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            // These members must match PROCESS_BASIC_INFORMATION
            internal nint Reserved1;

            internal nint PebBaseAddress;
            internal nint Reserved2_0;
            internal nint Reserved2_1;
            internal nint UniqueProcessId;
            internal nint InheritedFromUniqueProcessId;

            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(
#pragma warning restore SYSLIB1054 // Используйте \"LibraryImportAttribute\" вместо \"DllImportAttribute\" для генерирования кода маршализации P/Invoke во время компиляции
                nint processHandle,
                int processInformationClass,
                ref ParentProcessUtilities processInformation,
                int processInformationLength, out int returnLength
            );

            /// <summary>
            /// Gets the parent process of the current process.
            /// </summary>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess()
            {
                return GetParentProcess(Process.GetCurrentProcess().Handle);
            }

            /// <summary>
            /// Gets the parent process of specified process.
            /// </summary>
            /// <param name="id">The process id.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(int id)
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }

            /// <summary>
            /// Gets the parent process of a specified process.
            /// </summary>
            /// <param name="handle">The process handle.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(nint handle)
            {
                ParentProcessUtilities pbi = new();
                int status = NtQueryInformationProcess(
                    handle,
                    0,
                    ref pbi,
                    Marshal.SizeOf(pbi),
                    out int returnLength
                );
                if (status != 0)
                {
                    throw new Win32Exception(status);
                }

                try
                {
                    return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException)
                {
                    // not found
                    return null;
                }
            }
        }

        public static nint MinMaxInfoHookProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            if (msg == Const.WM_GETMINMAXINFO)
            {
                // We need to tell the system what our size should be when maximized. Otherwise it will
                // cover the whole screen, including the task bar.
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

                // Adjust the maximized size and position to fit the work area of the correct monitor
                nint monitor = MonitorFromWindow(hwnd, Const.MONITOR_DEFAULTTONEAREST);

                if (monitor != nint.Zero)
                {
                    MONITORINFO monitorInfo = new()
                    {
                        cbSize = Marshal.SizeOf(typeof(MONITORINFO))
                    };
                    GetMonitorInfo(monitor, ref monitorInfo);
                    RECT rcWorkArea = monitorInfo.rcWork;
                    RECT rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }

                Marshal.StructureToPtr(mmi, lParam, true);
            }

            return nint.Zero;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT(int left, int top, int right, int bottom)
        {
            public int Left = left;
            public int Top = top;
            public int Right = right;
            public int Bottom = bottom;
            public readonly int Width => Right - Left;
            public readonly int Height => Bottom - Top;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT(int x, int y)
        {
            public int X = x;
            public int Y = y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        public static class Const
        {
            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;
            public const int SM_SHUTTINGDOWN = 0x2000;
            public const int WM_GETMINMAXINFO = 0x0024;
            public const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
            public const uint WS_EX_TOOLWINDOW = 0x00000080;
            public const uint WS_EX_COMPOSITED = 0x02000000;
        }
    }
}
