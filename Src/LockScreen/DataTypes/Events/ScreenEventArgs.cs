using System;

using Lib.DataTypes.Structures;
using LockScreen.VM;

namespace LockScreen.DataTypes.Events
{
    /// <summary>
    /// Screen editing event arguments
    /// </summary>
    /// <param name="vm"></param>
    public class ScreenEventArgs(ScreenVM vm) : EventArgs
    {
        /// <summary>
        /// Screen
        /// </summary>
        public Screen Screen { get => VM.Screen; }

        /// <summary>
        /// Screen VM
        /// </summary>
        public ScreenVM VM { get; } = vm;

        /// <summary>
        /// Implicit converter
        /// </summary>
        /// <param name="screen"></param>
        public static implicit operator ScreenEventArgs(ScreenVM vm) => new(vm);
    }
}
