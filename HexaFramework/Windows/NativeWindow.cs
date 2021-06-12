using HexaFramework.Input;
using HexaFramework.Input.Events;
using HexaFramework.Windows.Native;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static HexaFramework.Windows.Native.Helper;

namespace HexaFramework.Windows
{
    public class NativeWindow
    {
        #region Fields

        private WNDPROC WNDPROC;
        private bool isDisposed;
        private string title;
        private int x;
        private int y;
        private int width;
        private int height;

        #endregion Fields

        #region Properties

        public string Title
        {
            get => title;
            set
            {
                title = value;
                User32.SetWindowTextA(Handle, value);
            }
        }

        public int X { get => x; protected set { x = value; User32.MoveWindow(Handle, x, y, width, height, true); } }

        public int Y { get => y; protected set { y = value; User32.MoveWindow(Handle, x, y, width, height, true); } }

        public int Width { get => width; protected set { width = value; User32.MoveWindow(Handle, x, y, width, height, true); } }

        public int Height { get => height; protected set { height = value; User32.MoveWindow(Handle, x, y, width, height, true); } }

        public WindowStyles Style { get; protected set; }

        public WindowExStyles StyleEx { get; protected set; }

        public IntPtr Handle { get; private set; }

        public bool IsDisposed { get => isDisposed; }

        public bool IsActive { get; protected set; }

        public bool IsShown { get; protected set; }

        public Mouse Mouse { get; protected set; } = new();

        public Keyboard Keyboard { get; protected set; } = new();

        public Cursor Cursor { get; protected set; }

        #endregion Properties

        #region Constructors and Destructors

        public NativeWindow()
        {
        }

        ~NativeWindow()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: false);
        }

        #endregion Constructors and Destructors

        #region Window construction

        private void PlatformConstruct()
        {
            WNDPROC = ProcessWindowMessage;
            var wndClassEx = new WNDCLASSEX
            {
                Size = Unsafe.SizeOf<WNDCLASSEX>(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_OWNDC,
                WindowProc = WNDPROC,
                InstanceHandle = Kernel32.GetModuleHandle(null),
                CursorHandle = User32.LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                BackgroundBrushHandle = IntPtr.Zero,
                IconHandle = IntPtr.Zero,
                ClassName = Title + "Window",
            };

            var atom = User32.RegisterClassEx(ref wndClassEx);
        }

        protected void CreateWindow()
        {
            PlatformConstruct();
            int windowWidth;
            int windowHeight;

            if (Width > 0 && Height > 0)
            {
                var rect = new Rect(0, 0, Width, Height);

                // Adjust according to window styles
                User32.AdjustWindowRectEx(
                    ref rect,
                    Style,
                    false,
                    StyleEx);

                windowWidth = rect.Right - rect.Left;
                windowHeight = rect.Bottom - rect.Top;
            }
            else
            {
                X = Y = windowWidth = windowHeight = unchecked((int)0x80000000);
            }

            var hwnd = User32.CreateWindowEx(
                (int)StyleEx,
                Title + "Window",
                Title,
                (int)Style,
                X,
                Y,
                windowWidth,
                windowHeight,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                return;
            }
            Handle = hwnd;
            Cursor = new Cursor(this);
            OnHandleCreated();
        }

        #endregion Window construction

        #region Window message loop

        protected virtual IntPtr ProcessWindowMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch ((WindowMessage)msg)
            {
                case WindowMessage.ActivateApp:
                    IsActive = IntPtrToInt32(wParam) != 0;
                    if (IsActive)
                        OnActivated();
                    else
                        OnDeactivated();
                    break;

                case WindowMessage.Destroy:
                    OnClose();
                    Close();
                    break;

                case WindowMessage.Size:
                    width = SignedLOWORD(lParam);
                    height = SignedHIWORD(lParam);
                    OnResize();
                    break;

                case WindowMessage.Move:
                    x = SignedLOWORD(lParam);
                    y = SignedHIWORD(lParam);
                    OnMove();
                    break;

                case WindowMessage.MouseMove:
                    if (!Mouse.Hover)
                    {
                        OnMouseEnter(Mouse.Update(true));
                        var tme = new TRACKMOUSEEVENT
                        {
                            cbSize = Marshal.SizeOf(typeof(TRACKMOUSEEVENT)),
                            dwFlags = TMEFlags.TME_LEAVE,
                            hWnd = Handle
                        };
                        _ = User32.TrackMouseEvent(ref tme);
                    }
                    OnMouseMove(Mouse.Update(MakePoint(lParam)));
                    break;

                case WindowMessage.MouseLeave:
                    OnMouseLeave(Mouse.Update(false));
                    break;

                case WindowMessage.MouseWheel:
                    OnMouseWheel(new MouseWheelEventArgs((short)SignedHIWORD(wParam)));
                    break;

                case WindowMessage.LButtonDown:
                    OnMouseDown(Mouse.Update(MouseButton.LButton, MouseButtonState.Pressed));
                    break;

                case WindowMessage.LButtonUp:
                    OnMouseUp(Mouse.Update(MouseButton.LButton, MouseButtonState.Released));
                    break;

                case WindowMessage.MButtonDown:
                    OnMouseDown(Mouse.Update(MouseButton.MButton, MouseButtonState.Pressed));
                    break;

                case WindowMessage.MButtonUp:
                    OnMouseUp(Mouse.Update(MouseButton.MButton, MouseButtonState.Released));
                    break;

                case WindowMessage.RButtonDown:
                    OnMouseDown(Mouse.Update(MouseButton.RButton, MouseButtonState.Pressed));
                    break;

                case WindowMessage.RButtonUp:
                    OnMouseUp(Mouse.Update(MouseButton.RButton, MouseButtonState.Released));
                    break;

                case WindowMessage.LButtonDBLCLK:
                    OnDoubleClick(Mouse.Update(MouseButton.LButton, MouseButtonState.Released));
                    break;

                case WindowMessage.KeyDown:
                    Keyboard.Update((Keys)wParam, KeyStates.Pressed);
                    break;

                case WindowMessage.KeyUp:
                    Keyboard.Update((Keys)wParam, KeyStates.Released);
                    break;

                case WindowMessage.SysKeyDown:
                    Keyboard.Update((Keys)wParam, KeyStates.Pressed);
                    break;

                case WindowMessage.SysKeyUp:
                    Keyboard.Update((Keys)wParam, KeyStates.Released);
                    break;
            }
            return User32.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        #endregion Window message loop

        #region Public Methodes

        public void Show()
        {
            if (Handle == IntPtr.Zero)
                CreateWindow();
            User32.ShowWindow(Handle, ShowWindowCommand.Normal);
            IsShown = true;
        }

        public void Close()
        {
            var hwnd = Handle;
            if (hwnd != IntPtr.Zero)
            {
                var destroyHandle = hwnd;
                Handle = IntPtr.Zero;

                OnHandleDestroy();
                User32.DestroyWindow(destroyHandle);
                User32.UnregisterClass(Title + "Window", destroyHandle);
                IsShown = false;
            }
        }

        #endregion Public Methodes

        #region Events

        public event EventHandler<EventArgs> Activated;

        public event EventHandler<EventArgs> Deactivated;

        protected virtual void OnActivated()
        {
            Activated?.Invoke(this, null);
        }

        protected virtual void OnDeactivated()
        {
            Deactivated?.Invoke(this, null);
        }

        protected virtual void OnHandleCreated()
        {
        }

        protected virtual void OnHandleDestroy()
        {
        }

        protected virtual void OnResize()
        {
        }

        protected virtual void OnMove()
        {
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnMouseEnter(MouseEventArgs mouseEventArgs)
        {
        }

        protected virtual void OnMouseMove(MouseEventArgs mouseEventArgs)
        {
        }

        protected virtual void OnMouseLeave(MouseEventArgs mouseEventArgs)
        {
        }

        protected virtual void OnMouseWheel(MouseWheelEventArgs mouseWheelEventArgs)
        {
        }

        protected virtual void OnMouseDown(MouseEventArgs mouseEventArgs)
        {
        }

        protected virtual void OnMouseUp(MouseEventArgs mouseEventArgs)
        {
        }

        protected virtual void OnDoubleClick(MouseEventArgs mouseEventArgs)
        {
        }

        #endregion Events

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                Close();
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}