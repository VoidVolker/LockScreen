using System;
using System.Drawing;

using LockScreen.DataTypes.Enums;

using static Lib.Tools.Native;

using LSScreen = Lib.DataTypes.Structures.Screen;

using System.Windows.Forms;

namespace Wallpaper
{
    public class WallpaperForm : Form
    {
        private readonly AppMode Mode = AppMode.User;
        private static Size onePxSize = new(1, 1);
        private readonly Label FindScreenLabel = null;

        public Rectangle ScreenBounds;
        public readonly string WallpaperFile;
        public readonly string ScreenId;

        public WallpaperForm(AppMode mode, string screenId, Rectangle bounds)
        {
            Mode = mode;
            ScreenId = screenId;
            ScreenBounds = bounds;

            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            FormBorderStyle = FormBorderStyle.None;
            SizeGripStyle = SizeGripStyle.Hide;
            Size = onePxSize;
            MinimumSize = onePxSize;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = Text = "Wallpaper";
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Zoom;
            ShowInTaskbar = false;
            Visible = false;
            ResumeLayout(false);
        }

        public WallpaperForm(AppMode mode, string screenId, string file, Rectangle bounds) : this(mode, screenId, bounds)
        {
            WallpaperFile = file;
        }

        public WallpaperForm(AppMode mode, LSScreen screen) : this(mode, screen.Id, screen.Bounds)
        {
            Size s = screen.Bounds.Size;
            FindScreenLabel = new()
            {
                Text = $"#{screen.Index}\n{screen.Name}\n{s.Width}x{s.Height}\n{screen.Id}",
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.White,
                BackColor = Color.Black,
                AutoSize = true,
                Font = new Font("Consolas", 40),
            };
            FindScreenLabel.Click += WinClicked;
            FindScreenLabel.KeyPress += KeyPressed;

            Controls.Add(FindScreenLabel);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            if (Mode == AppMode.User)
            {
                Click += WinClicked;
                KeyPress += KeyPressed;
            }
            else
            {
                FormClosing += CancelWinClosing;
            }
            ApplyLayout();
            Visible = true;
            base.OnLoad(e);
        }

        /// <summary>
        /// Load wallpaper file and render it
        /// </summary>
        public void ApplyLayout()
        {
            SuspendLayout();
            Location = ScreenBounds.Location;
            Size = ScreenBounds.Size;
            ClientSize = ScreenBounds.Size;

            if (!string.IsNullOrEmpty(WallpaperFile))
            {
                BackgroundImage = Image.FromFile(WallpaperFile);
            }
            else if (FindScreenLabel != null)
            {
                FindScreenLabelReposition();
            }

            ResumeLayout(true);
        }


        private void FindScreenLabelReposition()
        {
            FindScreenLabel.Top = Size.Height / 2 - FindScreenLabel.Height / 2;
            FindScreenLabel.Left = Size.Width / 2 - FindScreenLabel.Width / 2;
        }

        /// <summary>
        /// Move and resize window to a new bounds and rerender image
        /// </summary>
        /// <param name="bounds"></param>
        public void RepostionTo(Rectangle bounds)
        {
            ScreenBounds = bounds;
            // DPI to lower changing surprise: GDI+ refreshing only part of the window
            Visible = false;
            SuspendLayout();
            Location = ScreenBounds.Location;
            Size = ScreenBounds.Size;
            FindScreenLabelReposition();
            ResumeLayout(true);
            Visible = true;
        }

        /// <summary>
        /// Manual window close
        /// </summary>
        public new void Close()
        {
            if (Mode == AppMode.Logon)
            {
                FormClosing -= CancelWinClosing;
            }
            base.Close();
        }

        private void CancelWinClosing(object sender, FormClosingEventArgs e)
        {
            // Allow app exit on shutdown and exit in user mode
            if (IsShuttingDown() || Mode != AppMode.Logon) { return; }
            e.Cancel = true;
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            Close();
        }

        private void WinClicked(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Fix for slow UI rendering
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = (int)(Const.WS_EX_COMPOSITED | Const.WS_EX_TOOLWINDOW);
                return cp;
            }
        }
    }
}
