using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.ViewModels
{
    public class ShellVM : ViewModelBase
    {
        private bool showSingle = false;

        public bool ShowSingle { get => showSingle; set { Set(ref showSingle, value); } }

        private RelayCommand switchSigleCommand;

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
    }
}
