using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace App2
{
    public class Note : INotifyPropertyChanged
    {
        private string _content;
        private Brush _color;
        private DateTime dateTime;

        public string Content { get => _content; set { if (_content != value) { _content = value; this.OnPropertyChanged(); } } }

        public Brush Color { get => _color; set { if (_color != value) { _color = value; this.OnPropertyChanged(); } } }

        public DateTime DateTime { get => dateTime; set => dateTime = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
