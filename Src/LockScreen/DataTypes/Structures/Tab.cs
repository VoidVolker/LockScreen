using System.Windows.Controls;

namespace LockScreen.DataTypes.Structures
{
    public class Tab
    {
        public Control Button { get; set; }
        public Control Content { get; set; }
        public ushort Index { get; set; }
        public bool IsActive { get; set; }
    }
}
