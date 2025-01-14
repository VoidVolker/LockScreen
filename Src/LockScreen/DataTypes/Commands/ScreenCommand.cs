using LockScreen.DataTypes.Events;
using LockScreen.VM;

namespace LockScreen.DataTypes.Commands
{
    /// <summary>
    /// Screen UI control command event
    /// </summary>
    public class ScreenCommand : CommandEvent<ScreenVM, ScreenEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenCommand" /> class.
        /// </summary>
        public ScreenCommand() : base(ScreenVM.ToEventArgs)
        {
        }
    }
}