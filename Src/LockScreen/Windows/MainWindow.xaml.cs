using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Navigation;

using LockScreen.Tools;

using static LockScreen.Tools.SysWindow;

namespace LockScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISysWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += Window_SourceInitialized;
            // User input behavior
            MouseDown += MainWindow_DragMove;
            // Hyperlonks processing
            AddHandler(Hyperlink.RequestNavigateEvent, handler: new RequestNavigateEventHandler(RequestNavigateHandler));
        }

        public SysWindow Win { get; private set; }

        /// <summary>
        /// Window drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_DragMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }

        private void RequestNavigateHandler(object sender, RequestNavigateEventArgs e)
        {
            string url = e.Uri.ToString();
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch (Exception err)
            {
                string message = $"{I18n("Error Url open")}: {url}\n{err.Message}";
                MessageBox.Show(
                    message,
                    I18n("Error caption"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // Fix for window Maximized state (screen overflow)
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Win = new SysWindow(new WindowInteropHelper(this).Handle);
        }
    }
}
