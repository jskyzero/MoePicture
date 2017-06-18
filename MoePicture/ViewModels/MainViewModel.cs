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
        private bool _showPane = false;
        private bool _showSingle = false;
        private bool _showSettings = false;

        public bool ShowSearch { get => _showSearch; set { Set(ref _showSearch, value); } }
        public bool ShowHamburger { get => _showPane; set { Set(ref _showPane, value); } }
        public bool ShowSingle { get => _showSingle; set { Set(ref _showSingle, value); } }
        public bool ShowSettings { get => _showSettings; set { Set(ref _showSettings, value); } }

        private RelayCommand _hideSearchCommand;
        private RelayCommand _showSearchCommand;
        private RelayCommand _switchPaneCommand;
        private RelayCommand _switchSigleCommand;
        private RelayCommand _switchSettingsCommand;

        public RelayCommand SwitchSettingsCommand
        {
            get
            {
                return _switchSettingsCommand ??
               (_switchSettingsCommand = new RelayCommand(
                   () =>
                   {
                       ShowSettings = !ShowSettings;
                       ShowHamburger = false;
                   }));
            }
        }

        public RelayCommand SwitchSigleCommand
        {
            get
            {
                return _switchSigleCommand ??
               (_switchSigleCommand = new RelayCommand(
                   () =>
                   {
                       ShowSingle = !ShowSingle;
                   }));
            }
        }

        public RelayCommand SwitchPaneCommand
        {
            get
            {
                return _switchPaneCommand ??
                    (_switchPaneCommand = new RelayCommand(
                        () => { ShowHamburger = !ShowHamburger; }));
            }
        }

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
