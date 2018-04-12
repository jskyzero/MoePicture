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
        /// <summary> 显示单个页面 </summary>
        private bool showSingle = false;

        /// <summary> 显示单个页面 </summary>
        public bool ShowSingle { get => showSingle; set { Set(ref showSingle, value); } }


        /// <summary> 切换单个页面 </summary>
        private RelayCommand switchSigleCommand;

        /// <summary> 切换单个页面 </summary>
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
