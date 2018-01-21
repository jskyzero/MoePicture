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
        private bool showSearch = false;
        private bool showPane = false;
        private bool showSingle = false;
        private bool showSettings = false;

        public bool ShowSearch { get => showSearch; set { Set(ref showSearch, value); } }
        public bool ShowHamburger { get => showPane; set { Set(ref showPane, value); } }
        public bool ShowSingle { get => showSingle; set { Set(ref showSingle, value); } }
        public bool ShowSettings { get => showSettings; set { Set(ref showSettings, value); } }

        private RelayCommand hideSearchCommand;
        private RelayCommand showSearchCommand;
        private RelayCommand switchPaneCommand;
        private RelayCommand switchSigleCommand;
        private RelayCommand switchSettingsCommand;

        public RelayCommand SwitchSettingsCommand
        {
            get
            {
                return switchSettingsCommand ??
               (switchSettingsCommand = new RelayCommand(
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
                return switchSigleCommand ??
               (switchSigleCommand = new RelayCommand(
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
                return switchPaneCommand ??
                    (switchPaneCommand = new RelayCommand(
                        () => { ShowHamburger = !ShowHamburger; }));
            }
        }

        public RelayCommand HideSearchCommand
        {
            get
            {
                return hideSearchCommand ??
                    (hideSearchCommand = new RelayCommand(
                        () => { ShowSearch = false; }));
            }
        }

        public RelayCommand ShowSearchCommand
        {
            get
            {
                return showSearchCommand ??
                    (showSearchCommand = new RelayCommand(
                        () => { ShowSearch = true; }));
            }
        }
    }
}
