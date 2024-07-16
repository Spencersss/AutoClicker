using System;
using System.Windows.Input;
using SharpHook.Native;
using SpencerAutoClicker.Source.Model.Exceptions;

namespace SpencerAutoClicker.Source.Model
{
    public class Hotkey
    {
        // Properties
        public InputType Type { get; set; }
        public ushort KeyCode { get; set; }

        // Constructor(s)
        public Hotkey(KeyCode keyVal)
        {
            Type = InputType.Keyboard;
            KeyCode = (ushort)keyVal;
        }

        public Hotkey(SharpHook.Native.MouseButton keyVal)
        {
            Type = InputType.Mouse;
            KeyCode = (ushort)keyVal;
        }

        public Hotkey(string inputCode)
        {
            PopulateFromRawInputCode(inputCode);
        }

        // Methods
        public override string ToString()
        {
            if (Type == InputType.Keyboard) return Enum.Parse(typeof(KeyCode), KeyCode.ToString()).ToString()[2..];
            else return Enum.Parse(typeof(SharpHook.Native.MouseButton), KeyCode.ToString()).ToString();
        }

        public bool IsMouseHotkey()
        {
            return Type == InputType.Mouse;
        }

        public bool IsKeyboardHotkey()
        {
            return Type == InputType.Keyboard;
        }

        public KeyCode GetKeyCode()
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), KeyCode.ToString());
        }

        public SharpHook.Native.MouseButton GetMouseButton()
        {
            return (SharpHook.Native.MouseButton)Enum.Parse(typeof(SharpHook.Native.MouseButton), KeyCode.ToString());
        }

        private void PopulateFromRawInputCode(string inputCode)
        {
            bool isKeyBoardInput = Enum.IsDefined(typeof(KeyCode), inputCode);
            bool isMouseInput = Enum.IsDefined(typeof(SharpHook.Native.MouseButton), inputCode);

            if (isKeyBoardInput)
            {
                Type = InputType.Keyboard;
                KeyCode = (ushort)Enum.Parse(typeof(KeyCode), inputCode);
            }
            else if (isMouseInput)
            {
                Type = InputType.Mouse;
                KeyCode = (ushort)Enum.Parse(typeof(SharpHook.Native.MouseButton), inputCode);
            }
            else
            {
                throw new InputNotFoundException(inputCode);
            }
        }

    }
}
