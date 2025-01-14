using System.Windows;
using System.Windows.Controls;

namespace LockScreen.DataTypes.Interfaces
{
    public interface ITab
    {
        public Control Button { get; set; }
        public Control Content { get; set; }
        public ushort Index { get; set; }
        public bool IsActive { get; set; }
    }
}
