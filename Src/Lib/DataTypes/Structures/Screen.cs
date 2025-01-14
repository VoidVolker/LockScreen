using System;
using System.Drawing;

using Newtonsoft.Json;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Lib.DataTypes.Structures
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Screen : IComparable<Screen>, IEquatable<Screen>
    {
        //private EventHandler<ScreenChangedEvent> ChangedHandlers;
        //private string wallpaper = string.Empty;

        // Configuration file fields and device system fileds
        [J] public string Id { get; set; } = string.Empty;
        [J] public string Name { get; set; } = string.Empty;
        [J] public string Wallpaper { get; set; } = string.Empty;

        // UI properties
        public Rectangle Bounds { get; set; }
        public bool IsPrimary { get; set; } = false;
        public bool IsSecondary { get => !IsPrimary; }
        public ushort Index { get; set; } = 0;
        public bool IsConnected { get; set; } = false;
        public bool IsDiconnected { get => !IsConnected; }

        public Screen()
        {

        }


        public Screen(Screen screen, string wallpaper)
        {
            Id = screen.Id;
            Name = screen.Name;
            var b = screen.Bounds;
            Bounds = new Rectangle(b.X, b.Y, b.Width, b.Height);
            IsPrimary = screen.IsPrimary;
            Index = screen.Index;
            IsConnected = screen.IsConnected;
            Wallpaper = wallpaper;
        }

        public Screen(Screen screen) : this(screen, screen.Wallpaper)
        {

        }

        #region IEquatable interface
        public bool Equals(Screen other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || (obj is Screen other && Equals(other));
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Screen left, Screen right)
        {
            return left is null ? right is null : left.Equals(right);
        }

        public static bool operator !=(Screen left, Screen right)
        {
            return !(left == right);
        }
        #endregion

        #region IComparable interface
        public int CompareTo(Screen other) =>
            Index.CompareTo(other.Index) == 0
            ? Name.CompareTo(other.Name)
            : Id.CompareTo(other.Id);

        public static bool operator <(Screen left, Screen right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Screen left, Screen right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Screen left, Screen right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Screen left, Screen right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
        #endregion
    }
}
