using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _showSearch = false;

        public bool ShowSearch { get => _showSearch; set { Set(ref _showSearch, value); } }

        private RelayCommand _hideSearchCommand;
        private RelayCommand _showSearchCommand;

        public RelayCommand HideSearchCommand
        {
            get
            {
                return _hideSearchCommand ??
                    (_hideSearchCommand = new RelayCommand(
                        () => { ShowSearch = false; }));
            }
        }

        public RelayCommand ShowSearchCommand
        {
            get
            {
                return _showSearchCommand ??
                    (_showSearchCommand = new RelayCommand(
                        () => { ShowSearch = true; }));
            }
        }
    }
}
