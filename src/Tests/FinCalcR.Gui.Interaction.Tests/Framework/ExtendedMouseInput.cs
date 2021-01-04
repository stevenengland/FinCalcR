using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FinCalcR.Gui.Interaction.Tests.Framework
{
    public static class ExtendedMouseInput
    {
        [Flags]
        public enum MouseEvent
        {
            None = 0,
            Move = 0x00000001,
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            RightDown = 0x00000008,
            RightUp = 0x00000010,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Absolute = 0x00008000,
        }

        public static async Task LongLeftMouseClickAsync(TimeSpan waitTime)
        {
            SendMouseEvent(MouseEvent.LeftDown);
            await Task.Delay(waitTime).ConfigureAwait(false);
            SendMouseEvent(MouseEvent.LeftUp);
        }

        public static void SendMouseEvent(MouseEvent value)
        {
            var position = GetCursorPosition();

            mouse_event(
                (int)value,
                position.X,
                position.Y,
                0,
                0);
        }

        public static MousePoint GetCursorPosition()
        {
            var gotPoint = GetCursorPos(out var currentMousePoint);
            if (!gotPoint)
            {
                currentMousePoint = new MousePoint(0, 0);
            }

            return currentMousePoint;
        }

        [DllImport("user32.dll")]
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable IDE1006 // Naming Styles
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore SA1300 // Element should begin with upper-case letter

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public readonly int X;
            public readonly int Y;

            public MousePoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
