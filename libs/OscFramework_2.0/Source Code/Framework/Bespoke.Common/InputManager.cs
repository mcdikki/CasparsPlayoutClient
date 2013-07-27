using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Bespoke.Common
{
    /// <summary>
    /// Set of keyboard modifier keys.
    /// </summary>
    [Flags]
    public enum ModifierKeys
    {
        /// <summary>
        /// No modifier key.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left or right Shift keys.
        /// </summary>
        Shift = 1,

        /// <summary>
        /// The left or right Control keys.
        /// </summary>
        Control = 2,

        /// <summary>
        /// The left or right Alt keys.
        /// </summary>
        Alt = 4,

        /// <summary>
        /// The Caps Lock key.
        /// </summary>
        CapsLock = 8
    }

	/// <summary>
	/// Helper class for keyboard and mouse input.
	/// </summary>
	public static class InputManager
	{
		/// <summary>
		/// Set of mouse buttons.
		/// </summary>
		[Flags]
		public enum MouseButtons
		{
			/// <summary>
			/// No button.
			/// </summary>
			None = 0,

			/// <summary>
			/// The left mouse button.
			/// </summary>
			Left = 1048576,

			/// <summary>
			/// The right mouse button.
			/// </summary>
			Right = 2097152,

			/// <summary>
			/// The middle mouse button.
			/// </summary>
			Middle = 4194304,

			/// <summary>
			/// The first XButton.
			/// </summary>
			XButton1 = 8388608,

			/// <summary>
			/// The second XButton.
			/// </summary>
			XButton2 = 16777216
		}
		
		/// <summary>
		/// Virtual key codes.
		/// </summary>
		public enum VKeys : ushort
		{ 
            /// <summary>
            /// The Shift key.
            /// </summary>
			SHIFT = 0x10,

            /// <summary>
            /// The Control key.
            /// </summary>
			CONTROL = 0x11,

            /// <summary>
            /// The Menu key.
            /// </summary>
			MENU = 0x12,

            /// <summary>
            /// The Escape key.
            /// </summary>
			ESCAPE = 0x1B,

            /// <summary>
            /// The Back key.
            /// </summary>
			BACK = 0x08,

            /// <summary>
            /// The Tab key.
            /// </summary>
			TAB  = 0x09,

            /// <summary>
            /// The Return key.
            /// </summary>
			RETURN = 0x0D,

            /// <summary>
            /// The Prior key.
            /// </summary>
			PRIOR = 0x21,

            /// <summary>
            /// The Next key.
            /// </summary>
			NEXT = 0x22,

            /// <summary>
            /// The End key.
            /// </summary>
			END  = 0x23,

            /// <summary>
            /// The Home key.
            /// </summary>
			HOME = 0x24,

            /// <summary>
            /// The Left Arrow key.
            /// </summary>
			LEFT = 0x25,

            /// <summary>
            /// The Up Arrow key.
            /// </summary>
			UP  = 0x26,

            /// <summary>
            /// The Right Arrow key.
            /// </summary>
			RIGHT = 0x27,

            /// <summary>
            /// The Down Arrow key.
            /// </summary>
			DOWN = 0x28,

            /// <summary>
            /// The Select key.
            /// </summary>
			SELECT = 0x29,

            /// <summary>
            /// The Print key.
            /// </summary>
			PRINT = 0x2A,

            /// <summary>
            /// The Execute key.
            /// </summary>
			EXECUTE = 0x2B,

            /// <summary>
            /// The Snapshot key.
            /// </summary>
			SNAPSHOT = 0x2C,

            /// <summary>
            /// The Insert key.
            /// </summary>
			INSERT = 0x2D,

            /// <summary>
            /// The Delete key.
            /// </summary>
			DELETE = 0x2E,

            /// <summary>
            /// The Help key.
            /// </summary>
			HELP = 0x2F,

            /// <summary>
            /// The Number Pad 0 key.
            /// </summary>
			NUMPAD0 = 0x60,

            /// <summary>
            /// The Number Pad 1 key.
            /// </summary>
			NUMPAD1 = 0x61,

            /// <summary>
            /// The Number Pad 2 key.
            /// </summary>
			NUMPAD2 = 0x62,

            /// <summary>
            /// The Number Pad 3 key.
            /// </summary>
			NUMPAD3 = 0x63,

            /// <summary>
            /// The Number Pad 4 key.
            /// </summary>
			NUMPAD4 = 0x64,

            /// <summary>
            /// The Number Pad 5 key.
            /// </summary>
			NUMPAD5 = 0x65,

            /// <summary>
            /// The Number Pad 6 key.
            /// </summary>
			NUMPAD6 = 0x66,

            /// <summary>
            /// The Number Pad 7 key.
            /// </summary>
			NUMPAD7 = 0x67,

            /// <summary>
            /// The Number Pad 8 key.
            /// </summary>
			NUMPAD8 = 0x68,

            /// <summary>
            /// The Number Pad 9 key.
            /// </summary>
			NUMPAD9 = 0x69,

            /// <summary>
            /// The Multiply key.
            /// </summary>
			MULTIPLY = 0x6A,

            /// <summary>
            /// The Add key.
            /// </summary>
			ADD  = 0x6B,

            /// <summary>
            /// The Separator key.
            /// </summary>
			SEPARATOR = 0x6C,

            /// <summary>
            /// The Subtract key.
            /// </summary>
			SUBTRACT = 0x6D,

            /// <summary>
            /// The Decimal key.
            /// </summary>
			DECIMAL = 0x6E,

            /// <summary>
            /// The Divide key.
            /// </summary>
			DIVIDE = 0x6F,

            /// <summary>
            /// The F1 key.
            /// </summary>
			F1  = 0x70,

            /// <summary>
            /// The F2 key.
            /// </summary>
			F2  = 0x71,

            /// <summary>
            /// The F3 key.
            /// </summary>
			F3  = 0x72,

            /// <summary>
            /// The F4 key.
            /// </summary>
			F4  = 0x73,

            /// <summary>
            /// The F5 key.
            /// </summary>
			F5  = 0x74,

            /// <summary>
            /// The F6 key.
            /// </summary>
			F6  = 0x75,

            /// <summary>
            /// The F7 key.
            /// </summary>
			F7  = 0x76,

            /// <summary>
            /// The F8 key.
            /// </summary>
			F8  = 0x77,

            /// <summary>
            /// The F9 key.
            /// </summary>
			F9  = 0x78,

            /// <summary>
            /// The F10 key.
            /// </summary>
			F10  = 0x79,

            /// <summary>
            /// The F11 key.
            /// </summary>
			F11  = 0x7A,

            /// <summary>
            /// The F12 key.
            /// </summary>
			F12  = 0x7B,

            /// <summary>
            /// ',:' for US.
            /// </summary>
			OEM_1 = 0xBA,  // 

            /// <summary>
            /// '+' any country 
            /// </summary>
			OEM_PLUS = 0xBB,

            /// <summary>
            /// ',' any country 
            /// </summary>
			OEM_COMMA = 0xBC,

            /// <summary>
            /// '-' any country
            /// </summary>
			OEM_MINUS = 0xBD, 

            /// <summary>
            /// '.' any country 
            /// </summary>
			OEM_PERIOD = 0xBE,

            /// <summary>
            /// '/?' for US
            /// </summary>
			OEM_2 = 0xBF, 

            /// <summary>
            /// '`~' for US
            /// </summary>
			OEM_3 = 0xC0, 

            /// <summary>
            /// The Media Next Track key.
            /// </summary>
			MEDIA_NEXT_TRACK = 0xB0,

            /// <summary>
            /// The Media Previous Track key.
            /// </summary>
			MEDIA_PREV_TRACK = 0xB1,

            /// <summary>
            /// The Media Stop Track key.
            /// </summary>
			MEDIA_STOP = 0xB2,

            /// <summary>
            /// The Media Pause Track key.
            /// </summary>
			MEDIA_PLAY_PAUSE = 0xB3,

            /// <summary>
            /// The Left Windows key.
            /// </summary>
			LWIN = 0x5B,

            /// <summary>
            /// The Right Windows key.
            /// </summary>
			RWIN = 0x5C,

            /// <summary>
            /// The A key.
            /// </summary>
            A = 0x41,

            /// <summary>
            /// The B key.
            /// </summary>
            B = 0x42,

            /// <summary>
            /// The C key.
            /// </summary>
            C = 0x43,

            /// <summary>
            /// The D key.
            /// </summary>
            D = 0x44,

            /// <summary>
            /// The E key.
            /// </summary>
            E = 0x45,

            /// <summary>
            /// The F key.
            /// </summary>
            F = 0x46,

            /// <summary>
            /// The G key.
            /// </summary>
            G = 0x47,

            /// <summary>
            /// The H key.
            /// </summary>
            H = 0x48,

            /// <summary>
            /// The I key.
            /// </summary>
            I = 0x49,

            /// <summary>
            /// The J key.
            /// </summary>
            J = 0x4A,

            /// <summary>
            /// The K key.
            /// </summary>
            K = 0x4B,

            /// <summary>
            /// The L key.
            /// </summary>
            L = 0x4C,

            /// <summary>
            /// The M key.
            /// </summary>
            M = 0x4D,

            /// <summary>
            /// The N key.
            /// </summary>
            N = 0x4E,

            /// <summary>
            /// The O key.
            /// </summary>
            O = 0x4F,

            /// <summary>
            /// The P key.
            /// </summary>
            P = 0x50,

            /// <summary>
            /// The Q key.
            /// </summary>
            Q = 0x51,

            /// <summary>
            /// The R key.
            /// </summary>
            R = 0x52,

            /// <summary>
            /// The S key.
            /// </summary>
            S = 0x53,

            /// <summary>
            /// The T key.
            /// </summary>
            T = 0x54,

            /// <summary>
            /// The U key.
            /// </summary>
            U = 0x55,

            /// <summary>
            /// The V key.
            /// </summary>
            V = 0x56,

            /// <summary>
            /// The W key.
            /// </summary>
            W = 0x57,

            /// <summary>
            /// The X key.
            /// </summary>
            X = 0x58,

            /// <summary>
            /// The Y key.
            /// </summary>
            Y = 0x59,

            /// <summary>
            /// The Z key.
            /// </summary>
            Z = 0x5A
		}

        /// <summary>
        /// Set of valid keyboard events.
        /// </summary>
        public enum KeyEventType : uint
        {
            /// <summary>
            /// Key down.
            /// </summary>
            Down = 0,

            /// <summary>
            /// Key up.
            /// </summary>
            Up = KEYEVENTF_KEYUP
        }

		#region User32 Imports

		private const int INPUT_MOUSE = 0;
		private const int INPUT_KEYBOARD = 1;
		private const uint XBUTTON1 = 0x0001;
		private const uint XBUTTON2 = 0x0002;
		private const uint MOUSEEVENTF_MOVE = 0x0001;
		private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const uint MOUSEEVENTF_LEFTUP = 0x0004;
		private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
		private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
		private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
		private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
		private const uint MOUSEEVENTF_XDOWN = 0x0080;
		private const uint MOUSEEVENTF_XUP = 0x0100;
		private const uint MOUSEEVENTF_WHEEL = 0x0800;
		private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
		private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
		private const uint WHEEL_DELTA = 120;
		private const uint KEYEVENTF_KEYUP = 0x0002;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

		[StructLayout(LayoutKind.Sequential)]
		private struct MOUSEINPUT
		{
			public int dx;//4
			public int dy;//4
			public uint mouseData;//4
			public uint dwFlags;//4
			public uint time;//4
			public IntPtr dwExtraInfo;//4
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct KEYBDINPUT
		{
			public ushort wVk;//2
			public ushort wScan;//2
			public uint dwFlags;//4
			public uint time;//4
			public IntPtr dwExtraInfo;//4
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct HARDWAREINPUT
		{
			public uint uMsg;
			public ushort wParamL;
			public ushort wParamH;
		}

		[StructLayout(LayoutKind.Explicit, Size = 28)]
		private struct INPUT
		{
			[FieldOffset(0)]
			public int type;
			[FieldOffset(4)]
			public MOUSEINPUT mi;
			[FieldOffset(4)]
			public KEYBDINPUT ki;
			[FieldOffset(4)]
			public HARDWAREINPUT hi;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetMessageExtraInfo();

		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);
 
		#endregion

		/// <summary>
		/// Mouse the mouse cursor.
		/// </summary>
		/// <param name="x">The x position.</param>
		/// <param name="y">The y position.</param>
		public static void MouseMove(int x, int y)
		{
			SetCursorPos(x, y);
		}

		/// <summary>
		/// Send a mouse "down" event.
		/// </summary>
		/// <param name="button">The associated mouse button.</param>
		public static void MouseDown(MouseButtons button)
		{
			uint mouseData;
			uint downFlag = MouseButtonToFlag(button, true, out mouseData);

			INPUT[] buffer = new INPUT[1];
			buffer[0].type = INPUT_MOUSE;
			buffer[0].mi.dx = 0;
			buffer[0].mi.dy = 0;
			buffer[0].mi.mouseData = mouseData;
			buffer[0].mi.dwFlags = downFlag;
			buffer[0].mi.time = 0;
			buffer[0].mi.dwExtraInfo = (IntPtr)0;

			SendInput(1, buffer, Marshal.SizeOf(buffer[0]));
		}

        /// <summary>
        /// Send a mouse "up" event.
        /// </summary>
        /// <param name="button">The associated mouse button.</param>
		public static void MouseUp(MouseButtons button)
		{
			uint mouseData;
			uint downFlag = MouseButtonToFlag(button, false, out mouseData);

			INPUT[] buffer = new INPUT[1];
			buffer[0].type = INPUT_MOUSE;
			buffer[0].mi.dx = 0;
			buffer[0].mi.dy = 0;
			buffer[0].mi.mouseData = mouseData;
			buffer[0].mi.dwFlags = downFlag;
			buffer[0].mi.time = 0;
			buffer[0].mi.dwExtraInfo = (IntPtr)0;

			SendInput(1, buffer, Marshal.SizeOf(buffer[0]));
		}

        /// <summary>
        /// Send a mouse "click" event.
        /// </summary>
        /// <param name="button">The associated mouse button.</param>
		public static void MouseClick(MouseButtons button)
		{
			uint mouseData;
			uint downFlag = MouseButtonToFlag(button, true, out mouseData);

			INPUT[] buffer = new INPUT[2];
			buffer[0].type = INPUT_MOUSE;
			buffer[0].mi.dx = 0;
			buffer[0].mi.dy = 0;
			buffer[0].mi.mouseData = mouseData;
			buffer[0].mi.dwFlags = downFlag;
			buffer[0].mi.time = 0;
			buffer[0].mi.dwExtraInfo = (IntPtr)0;

			uint upFlag = MouseButtonToFlag(button, false, out mouseData);

			buffer[1].type = INPUT_MOUSE;
			buffer[1].mi.dx = 0;
			buffer[1].mi.dy = 0;
			buffer[1].mi.mouseData = mouseData;
			buffer[1].mi.dwFlags = upFlag;
			buffer[1].mi.time = 0;
			buffer[1].mi.dwExtraInfo = (IntPtr)0;

			SendInput(2, buffer, Marshal.SizeOf(buffer[0]));
		}

        /// <summary>
        /// Send a mouse "double cick" event.
        /// </summary>
		public static void MouseDoubleClick()
		{
			MouseClick(MouseButtons.Left);
			MouseClick(MouseButtons.Left);
		}

        /// <summary>
        /// Send a mouse "wheel" event.
        /// </summary>
		/// <param name="value">The value of the mouse wheel.</param>
		public static void MouseWheel(uint value)
		{
			uint mouseData = value * WHEEL_DELTA;
			
			INPUT[] buffer = new INPUT[1];
			buffer[0].type = INPUT_MOUSE;
			buffer[0].mi.dx = 0;
			buffer[0].mi.dy = 0;
			buffer[0].mi.mouseData = mouseData;
			buffer[0].mi.dwFlags = MOUSEEVENTF_WHEEL;
			buffer[0].mi.time = 0;
			buffer[0].mi.dwExtraInfo = (IntPtr)0;

			SendInput(1, buffer, Marshal.SizeOf(buffer[0]));
		}

        /// <summary>
        /// Send a "down" then "up" event for a set of keys.
        /// </summary>
        /// <param name="keys">The associated keys.</param>
        public static void SendKeys(VKeys[] keys)
        {
            SendKeys(ModifierKeys.None, keys, KeyEventType.Down);
            SendKeys(ModifierKeys.None, keys, KeyEventType.Up);
        }

        /// <summary>
        /// Send a "down" then "up" event for a set of keys.
        /// </summary>
        /// <param name="modifierKeys">Modifier keys associated with each key event.</param>
        /// <param name="keys">The associated keys.</param>
        public static void SendKeys(ModifierKeys modifierKeys, VKeys[] keys)
        {
            SendKeys(modifierKeys, keys, KeyEventType.Down);
            SendKeys(modifierKeys, keys, KeyEventType.Up);
        }

        /// <summary>
        /// Send an event for a set of keys.
        /// </summary>
        /// <param name="modifierKeys">Modifier keys associated with each key event.</param>
        /// <param name="keys">The associated keys.</param>
        /// <param name="eventType">The event type.</param>
        public static void SendKeys(ModifierKeys modifierKeys, VKeys[] keys, KeyEventType eventType)
        {
            Assert.ParamIsNotNull(keys);

            List<INPUT> inputBuffer = new List<INPUT>();

            if (eventType == KeyEventType.Down)
            {
                // Key down events
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Shift))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.SHIFT, KeyEventType.Down));
                }
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Control))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.CONTROL, KeyEventType.Down));
                }
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Alt))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.MENU, KeyEventType.Down));
                }
                foreach (VKeys key in keys)
                {
                    inputBuffer.Add(CreateKeyEvent(key, KeyEventType.Down));
                }
            }
            else
            {
                // Add key up events
                foreach (VKeys key in keys)
                {
                    inputBuffer.Add(CreateKeyEvent(key, KeyEventType.Up));
                }
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Alt))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.MENU, KeyEventType.Up));
                }
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Control))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.CONTROL, KeyEventType.Up));
                }
                if (Utility.IsFlagSet(modifierKeys, Bespoke.Common.ModifierKeys.Shift))
                {
                    inputBuffer.Add(CreateKeyEvent(VKeys.SHIFT, KeyEventType.Up));
                }
            }

            if (inputBuffer.Count > 0)
            {
                INPUT[] buffer = inputBuffer.ToArray();
                SendInput((uint)buffer.Length, buffer, Marshal.SizeOf(buffer[0]));
            }
        }

		/// <summary>
		/// Send Tab down/up events.
		/// </summary>
		public static void Tab()
		{
            VKeys[] keys = { VKeys.TAB };
            SendKeys(ModifierKeys.None, keys);
		}

		/// <summary>
        /// Send Shift-Tab down/up events.
		/// </summary>
		public static void ShiftTab()
		{
            VKeys[] keys = { VKeys.TAB };
            SendKeys(ModifierKeys.Shift, keys);
		}

		/// <summary>
        /// Send Alt-Tab down/up events.
		/// </summary>
		public static void AltTab()
		{
            VKeys[] keys = { VKeys.TAB };
            SendKeys(ModifierKeys.Alt, keys);
		}

		/// <summary>
        /// Send Alt-Shift=Tab down/up events.
		/// </summary>
		public static void AltShiftTab()
		{
            VKeys[] keys = { VKeys.TAB };
            SendKeys(ModifierKeys.Alt | ModifierKeys.Shift, keys);
		}

		#region Private Methods

		private static uint MouseButtonToFlag(MouseButtons button, bool down, out uint mouseData)
		{
			uint mouseEventFlag;

			switch (button)
			{
				case MouseButtons.Left:
					mouseEventFlag = (down ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP);
					mouseData = 0;
					break;

				case MouseButtons.Middle:
					mouseEventFlag = (down ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP);
					mouseData = 0;
					break;

				case MouseButtons.Right:
					mouseEventFlag = (down ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP);
					mouseData = 0;
					break;

				case MouseButtons.XButton1:
					mouseEventFlag = (down ? MOUSEEVENTF_XDOWN : MOUSEEVENTF_XUP);
					mouseData = XBUTTON1;
					break;

				case MouseButtons.XButton2:
					mouseEventFlag = (down ? MOUSEEVENTF_XDOWN : MOUSEEVENTF_XUP);
					mouseData = XBUTTON2;
					break;

				default:
					throw new ArgumentException();
			}

			return mouseEventFlag;
		}

        private static INPUT CreateKeyEvent(VKeys key, KeyEventType eventType)
        {
            INPUT keyEvent = new INPUT();
            keyEvent.type = INPUT_KEYBOARD;
            keyEvent.ki.wVk = (ushort)key;
            keyEvent.ki.wScan = 0;
            keyEvent.ki.dwFlags = (uint)eventType;
            keyEvent.ki.time = 0;
            keyEvent.ki.dwExtraInfo = (IntPtr)0;

            return keyEvent;
        }

		#endregion
	}
}
