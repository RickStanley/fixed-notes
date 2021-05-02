using System.ComponentModel;
using System.Runtime.CompilerServices;
//using App2.Models;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace App2
{
    public class Customer : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get => _name; set { if (_name != value) { _name = value; this.OnPropertyChanged(); } } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
