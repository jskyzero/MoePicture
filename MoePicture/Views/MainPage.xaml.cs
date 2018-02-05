using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
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
            ContentFrame.Navigate(typeof(Shell));
            contentFrameBackToShell();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            ContentFrame.Navigated += (s, a) =>
            {
                if (ContentFrame.CanGoBack)
                {
                    // Setting this visible is ignored on Mobile and when in tablet mode!
                    Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
                }
            };
        }

        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            // Navigate back if possible, and if the event has not already been handled.
            if (ContentFrame.CanGoBack && e.Handled == false)
            {
                ContentFrame.GoBack();
                e.Handled = true;
            }

            updateNavViewSelect();
        }

        private void contentFrameBackToShell()
        {
            // Navigate back if possible, and if the event has not already been handled.
            while (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
            updateNavViewSelect();
        }

        private void updateNavViewSelect()
        {
            //get the Type of the currently displayed page
            var pageName = ContentFrame.Content.GetType().Name;
            if (pageName == typeof(SettingsPage).ToString().Split('.').Last())
            {
                NavView.SelectedItem = NavView.SettingsItem;
            }
            else
            {
                if (pageName == typeof(Shell).ToString().Split('.').Last())
                {
                    pageName = ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().Type.ToString();
                }
                //find menu item that has the matching tag
                var menuItem = NavView.MenuItems
                                         .OfType<NavigationViewItem>()
                                         .Where(item => item.Content.ToString().ToLower() == pageName)
                                         .First();
                //select
                NavView.SelectedItem = menuItem;
            }

        }

        private void NavViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var pageTag = ((args.SelectedItem as NavigationViewItem).Content as string).ToLower();

                switch (pageTag)
                {
                    // now can't be help, but future may have other type.
                    case "help":
                        break;
                    default:
                        contentFrameBackToShell();
                        ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().ChangeWebsiteCommand.Execute(pageTag);
                        break;
                }
            }
        }

        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            contentFrameBackToShell();
            string queryText = args.ChosenSuggestion == null ? sender.Text : args.ChosenSuggestion.ToString();
            ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().RearchCommand.Execute(queryText);
        }

        private async void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // The URI to launch
            var uriBing = new Uri(@"https://jskyzero.github.io/MoePicture/");

            // Set the option to show a warning
            var promptOptions = new Windows.System.LauncherOptions();
            promptOptions.TreatAsUntrusted = true;

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uriBing, promptOptions);
        }
    }
}
