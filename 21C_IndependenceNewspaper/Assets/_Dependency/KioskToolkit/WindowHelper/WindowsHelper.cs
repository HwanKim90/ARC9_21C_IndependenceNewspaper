#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Arc9.Unity.KioskToolkit.Service;


namespace Arc9.Unity.KioskToolkit
{
    public class WindowsHelper : MonoBehaviour
    {
        #region WIN32API

        public static readonly System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
        public static readonly System.IntPtr HWND_NOT_TOPMOST = new System.IntPtr(-2);
        const System.UInt32 SWP_SHOWWINDOW = 0x0040;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int X
            {
                get
                {
                    return Left;
                }
                set
                {
                    Right -= (Left - value);
                    Left = value;
                }
            }

            public int Y
            {
                get
                {
                    return Top;
                }
                set
                {
                    Bottom -= (Top - value);
                    Top = value;
                }
            }

            public int Height
            {
                get
                {
                    return Bottom - Top;
                }
                set
                {
                    Bottom = value + Top;
                }
            }

            public int Width
            {
                get
                {
                    return Right - Left;
                }
                set
                {
                    Right = value + Left;
                }
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern System.IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA")]
        public static extern long GetWindowLong(System.IntPtr hwnd, long nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
        public static extern long SetWindowLong(System.IntPtr hwnd, long nIndex, long dwNewLong);

        #endregion

        const int GWL_STYLE = -16;
        const uint WS_POPUP = 0x80000000;
        const uint WS_VISIBLE = 0x10000000;


        public bool TopMost;
        public bool EnableSetWindowPos;
        public Vector2 WindowStartupPosition;
        public Vector2 WindowSize;

        private void Awake()
        {
            TopMost = AppData.Window.TopMost;
            WindowStartupPosition = new Vector2(AppData.Window.Left, AppData.Window.Top);
            WindowSize = new Vector2(AppData.Window.Width, AppData.Window.Height);
        }

        void Start()
        {
            string[] windowNames = GetWindowTitles();
            foreach (string winName in windowNames)
            {
                UnityEngine.Debug.Log(winName);
            }
#if !UNITY_EDITOR
            Cursor.visible = false;

            if(EnableSetWindowPos)
            {
                StartCoroutine(SetupWindowSize());
            }
#endif
        }

        private IEnumerator SetupWindowSize()
        {
            yield return new WaitForSeconds(3.0f);

            AssignTopmostWindow(Application.productName, TopMost);

            yield return new WaitForSeconds(1.0f);

            AssignTopmostWindow(Application.productName, TopMost);

            yield return null;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                Cursor.visible = !Cursor.visible;
            }

            if (Input.GetKey(KeyCode.H))
            {
             
            }
            else
            {

            }


        }

        public bool AssignTopmostWindow(string windowTitle, bool makeTopmost)
        {
            Rect windowRect = new Rect(WindowStartupPosition.x, WindowStartupPosition.y, WindowSize.x, WindowSize.y);
            return AssignTopmostWindow(windowTitle, makeTopmost, windowRect);
        }

        public bool AssignTopmostWindow(string windowTitle, bool makeTopmost, Rect windowRect)
        {
            UnityEngine.Debug.Log("Assigning top most flag to window of title: " + windowTitle);

            System.IntPtr hWnd = FindWindow((string)null, windowTitle);

            SetWindowLong(hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

            RECT rect = new RECT();

            GetWindowRect(new HandleRef(this, hWnd), out rect);

            return SetWindowPos(hWnd, makeTopmost ? HWND_TOPMOST : HWND_NOT_TOPMOST, (int)windowRect.x, (int)windowRect.y, (int)windowRect.width, (int)windowRect.height, SWP_SHOWWINDOW);
        }

        private string[] GetWindowTitles()
        {
            List<string> WindowList = new List<string>();

            Process[] ProcessArray = Process.GetProcesses();
            foreach (Process p in ProcessArray)
            {
                if (!IsNullOrWhitespace(p.MainWindowTitle))
                {
                    WindowList.Add(p.MainWindowTitle);
                }
            }

            return WindowList.ToArray();
        }

        public bool IsNullOrWhitespace(string Str)
        {
            if (Str.Equals("null"))
            {
                return true;
            }
            foreach (char c in Str)
            {
                if (c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

    }
}

#endif