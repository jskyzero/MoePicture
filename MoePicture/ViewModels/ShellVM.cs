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
        /// <summary> 显示错误页面 </summary>
        private bool showError = false;

        /// <summary> 显示单个页面 </summary>
        public bool ShowSingle { get => showSingle; set { Set(ref showSingle, value); } }
        /// <summary> 显示错误页面 </summary>
        public bool ShowError { get => showError; set { Set(ref showError, value); } }

        public ShellVM()
        {
            //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;

            //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            //ContentFrame.Navigated += (s, a) =>
            //{
            //    if (ContentFrame.CanGoBack)
            //    {
            //        // Setting this visible is ignored on Mobile and when in tablet mode!
            //        Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            //    }
            //    else
            //    {
            //        Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            //    }
            //};
        }


        //private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        //{
        //    // Navigate back if possible, and if the event has not already been handled.
        //    if (ContentFrame.CanGoBack && e.Handled == false)
        //    {
        //        ContentFrame.GoBack();
        //        e.Handled = true;
        //    }

        //    UpdateNavViewSelect();
        //}

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
