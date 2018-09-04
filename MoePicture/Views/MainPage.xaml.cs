using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
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
            // 主界面导航到Views.Shell
            ContentFrame.Navigate(typeof(Shell));
            // 初始化导航项
            InitialNaveMenuItems();
            // 更新到当前选中的导航项
            ContentFrameBackToShell();
            UpdateNavViewSelect();
        }

        private void InitialNaveMenuItems()
        {
            foreach (var type in Enum.GetValues(typeof(Services.WebsiteType)))
            {
                NavView.MenuItems.Add(new NavigationViewItem()
                {
                    Content = type.ToString(),
                    Icon = new FontIcon()
                    {
                        Glyph = "\uEB9F"
                    }
                });
            }
        }


        private void ContentFrameBackToShell()
        {
            // 退到最上层
            while (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void UpdateNavViewSelect()
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
                // find menu item that has the matching tag
                var menuItem = NavView.MenuItems
                                      .OfType<NavigationViewItem>()
                                      .First(item => item.Content.ToString() == pageName);
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
                var pageTag = ((args.SelectedItem as NavigationViewItem).Content as string);

                switch (pageTag)
                {
                    // now can't be help, but future may have other type.
                    case "help":
                        break;
                    default:
                        ContentFrameBackToShell();
                        ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().ChangeWebsiteCommand.Execute(pageTag);
                        break;
                }
            }
        }

        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ContentFrameBackToShell();
            string queryText = args.ChosenSuggestion == null ? sender.Text : args.ChosenSuggestion.ToString();
            ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().SearchCommand.Execute(queryText);
        }

        private async void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // The URI to launch
            var uriBing = new Uri(Services.GlobalConfig.HelpWebSiteUrl);

            // Set the option to show a warning
            var promptOptions = new Windows.System.LauncherOptions();
            promptOptions.TreatAsUntrusted = true;

            // Launch the URI
            await Windows.System.Launcher.LaunchUriAsync(uriBing, promptOptions);
        }
    }
}
