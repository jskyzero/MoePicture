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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MoePicture.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            //if (args.IsSettingsInvoked)
            //{
            //    ContentFrame.Navigate(typeof(SettingsPage));
            //}
            //else
            //{
            //    switch (args.InvokedItem)
            //    {
            //        case loader.GetString("HomeNavItem.Content"):
            //            ContentFrame.Navigate(typeof(HomePage));
            //            break;

            //        case loader.GetString("AppsNavItem.Content"):
            //            ContentFrame.Navigate(typeof(AppsPage));
            //            break;

            //        case loader.GetString("GamesNavItem.Content"):
            //            ContentFrame.Navigate(typeof(GamesPage));
            //            break;

            //        case loader.GetString("MusicNavItem.Content"):
            //            ContentFrame.Navigate(typeof(MusicPage));
            //            break;

            //        case loader.GetString("ContentNavItem.Content"):
            //            ContentFrame.Navigate(typeof(MyContentPage));
            //            break;
            //    }
            //}
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //if (args.IsSettingsSelected)
            //{
            //    ContentFrame.Navigate(typeof(SettingsPage));
            //}
            //else
            //{

            //    NavigationViewItem item = args.SelectedItem as NavigationViewItem;

            //    switch (item.Tag)
            //    {
            //        case "home":
            //            ContentFrame.Navigate(typeof(HomePage));
            //            break;

            //        case "apps":
            //            ContentFrame.Navigate(typeof(AppsPage));
            //            break;

            //        case "games":
            //            ContentFrame.Navigate(typeof(GamesPage));
            //            break;

            //        case "music":
            //            ContentFrame.Navigate(typeof(MusicPage));
            //            break;

            //        case "content":
            //            ContentFrame.Navigate(typeof(MyContentPage));
            //            break;
            //    }
            //}
        }
    }
}
