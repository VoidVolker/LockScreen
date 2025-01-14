using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LockScreen.DataTypes.Events;
using LockScreen.DataTypes.Properties;
using LockScreen.Tools;

using Microsoft.Win32;

namespace LockScreen.Views.Controls
{
    /// <summary>
    /// File input control with input and button
    /// </summary>
    public class FileInput : Control
    {
        #region Public Constructors

        static FileInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileInput), new FrameworkPropertyMetadata(typeof(FileInput)));
        }

        public FileInput()
        {
            //ExtList = [];
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Browse file command event
        /// </summary>
        public CommandEvent Browse { get; } = new CommandEvent();

        /// <summary>
        /// Browse button text
        /// </summary>
        public string BrowseText
        {
            get => (string)GetValue(BrowseTextProperty);
            set => SetValue(BrowseTextProperty, value);
        }

        /// <summary>
        /// Command
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Command parameter
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Dialog title
        /// </summary>
        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }

        /// <summary>
        /// Extensions list
        /// </summary>
        public IEnumerable<string> ExtList
        {
            get => (IEnumerable<string>)GetValue(ExtListProperty);
            set => SetValue(ExtListProperty, value);
        }

        /// <summary>
        /// Extensions list title
        /// </summary>
        public string ExtListTitle
        {
            get => (string)GetValue(ExtListTitleProperty);
            set => SetValue(ExtListTitleProperty, value);
        }

        /// <summary>
        /// Selected file path
        /// </summary>
        public string File
        {
            get => (string)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        /// <summary>
        /// File multiselect option
        /// </summary>
        public bool Multiselect
        {
            get => (bool)GetValue(MultiselectProperty);
            set => SetValue(MultiselectProperty, value);
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty BrowseTextProperty =
            DP<FileInput>.R(x => x.BrowseText, string.Empty);

        public static readonly DependencyProperty CommandParameterProperty =
            DP<FileInput>.R(x => x.CommandParameter);

        public static readonly DependencyProperty CommandProperty =
            DP<FileInput>.R(x => x.Command);

        public static readonly DependencyProperty DialogTitleProperty =
            DP<FileInput>.R(x => x.DialogTitle, string.Empty);

        public static readonly DependencyProperty ExtListProperty =
            DP<FileInput>.R(x => x.ExtList);

        public static readonly DependencyProperty ExtListTitleProperty =
            DP<FileInput>.R(x => x.ExtListTitle, string.Empty);

        public static readonly DependencyProperty FileProperty =
            DP<FileInput>.R(
                x => x.File,
                string.Empty,
                x => x.OnFileChanged);

        public static readonly DependencyProperty MultiselectProperty =
            DP<FileInput>.R(x => x.Multiselect, false);

        #endregion Public Fields

        #region Private Fields

        private TextBox fileTextBox;

        #endregion Private Fields

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Browse.Event += OpenFileDialog;
            // Manual binding to input for disabling "Changed" event for text and attached to
            // "OnKey" event
            fileTextBox = UIHelper.FindChild<TextBox>(this, "Input");
            fileTextBox.Text = File;
            fileTextBox.LostFocus += Input_LostFocus;
            fileTextBox.KeyUp += Input_KeyUp;
        }

        #endregion Public Methods

        #region Private Methods

        // Very basic format
        private static string ExtFormat(IEnumerable<string> extList = null) =>
            extList is not null && extList.Any()
                ? string.Join(';', extList.Select(ext => $"*{ext}"))
                : "*.*";

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Keyboard.ClearFocus();
            }
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            File = fileTextBox.Text;
        }

        private void OnFileChanged(DependencyPropertyChangedEventArgs<string> e)
        {
            if (fileTextBox is not null)
            {
                fileTextBox.Text = File;
            }
            Command?.Execute(CommandParameter);
        }

        private void OpenFileDialog(object sender, EventArgs e)
        {
            string extList = ExtFormat(ExtList);

            OpenFileDialog dlg = new()
            {
                Filter = $"{ExtListTitle}|{extList}",
                Title = DialogTitle,
                Multiselect = Multiselect
            };

            bool? result = dlg.ShowDialog();
            if (result == true && !string.IsNullOrEmpty(dlg.FileName))
            {
                fileTextBox.Text = File = dlg.FileName;
                Command?.Execute(CommandParameter);
            }
        }

        #endregion Private Methods
    }
}
