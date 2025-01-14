using Lib.DataTypes.Structures;

using LockScreen.DataTypes.Collections.I18n;
using LockScreen.DataTypes.Events;

using Prism.Mvvm;

namespace LockScreen.VM
{
    public class ScreenVM(Screen screen) : BindableBase
    {
        #region Public Properties

        /// <summary>
        /// Is preview window closed
        /// </summary>
        public bool IsPreviewClosed
        {
            get => !isPreviewOpened;
        }

        /// <summary>
        /// Is preview window opened
        /// </summary>
        public bool IsPreviewOpened
        {
            get => isPreviewOpened;
            set
            {
                if (isPreviewOpened == value) { return; }
                isPreviewOpened = value;
                RaisePropertyChanged(nameof(IsPreviewOpened));
                RaisePropertyChanged(nameof(IsPreviewClosed));
                RaisePropertyChanged(nameof(PreviewOperation));
            }
        }

        /// <summary>
        /// Available preview operation
        /// </summary>
        public I18nPreviewOperation PreviewOperation => isPreviewOpened;

        /// <summary>
        /// Screen structure
        /// </summary>
        public Screen Screen { get; } = screen;

        #endregion Public Properties

        #region Private Fields

        private bool isPreviewOpened = false;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Create ScreenVM from Screen instance
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ScreenVM From(Screen s) => new(s);

        /// <summary>
        /// Create event args from VM
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static ScreenEventArgs ToEventArgs(ScreenVM vm) => new(vm);

        #endregion Public Methods
    }
}
