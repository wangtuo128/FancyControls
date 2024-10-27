using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyControl.App
{
    public sealed partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isAnimating;

        [RelayCommand]
        private void ControlAnimating()
        {
            IsAnimating = !IsAnimating;
        }
    }
}
