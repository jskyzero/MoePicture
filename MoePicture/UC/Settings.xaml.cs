using Microsoft.Practices.ServiceLocation;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MoePicture.UC
{
    public sealed partial class Settings : UserControl
    {
        public Settings()
        {
            this.InitializeComponent();
            ArylicMaterial.Win2D.initialGlass(BackGround);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ServiceLocator.Current.GetInstance<UserConfigViewModel>().AddTagtoMyTagsByString(input.Text);
        //}

        private void BackGround_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().SwitchSettingsCommand.Execute(null);
        }
    }
}
