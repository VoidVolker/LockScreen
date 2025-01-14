using System;
using System.Diagnostics.CodeAnalysis;

using Prism.Mvvm;

namespace LockScreen.VM
{
    public class AboutVM : BindableBase
    {
        public AboutVM()
        {
            //RaisePropertyChanged(nameof(Year));
        }

        /// <summary>
        /// Current year
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Пометьте члены как статические", Justification = "<Ожидание>")]
        [SuppressMessage("CodeQuality", "IDE0079:Удалить ненужное подавление", Justification = "<Ожидание>")]
        public int Year
        {
            get => DateTime.Now.Year;
        }
    }
}
