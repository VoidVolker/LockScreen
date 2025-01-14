using System.Windows;
using System.Windows.Controls;

using LockScreen.DataTypes.Events;

namespace LockScreen.Views.Frames
{
    public class AppHeader : Control
    {
        #region Public Constructors

        static AppHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppHeader), new FrameworkPropertyMetadata(typeof(AppHeader)));
        }

        public AppHeader()
        {
            Close.Event += Close_Event;
            Minimize.Event += Minimize_Event;
        }

        #endregion Public Constructors

        #region Public Properties

        public CommandEvent Close { get; } = new CommandEvent();

        public CommandEvent Minimize { get; } = new CommandEvent();

        #endregion Public Properties

        #region Private Methods

        private void Close_Event(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Event(object sender, System.EventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        #endregion Private Methods
    }
}
