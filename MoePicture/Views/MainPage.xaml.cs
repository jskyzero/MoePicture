using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
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
            InitialSelfDefinedTitleBar();


            // 主界面导航到Views.Shell
            ContentFrame.Navigate(typeof(Shell));
            // 初始化导航项
            InitialNaveMenuItems();
            // 更新到当前选中的导航项
            ContentFrameBackToShell();
            UpdateNavViewSelect();
        }

        private void InitialSelfDefinedTitleBar()
        {

            var view = ApplicationView.GetForCurrentView();

            // 将标题栏的三个键背景设为透明
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            // 失去焦点时，将三个键背景设为透明
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            // 将三个键前景色设为白色
            view.TitleBar.ButtonForegroundColor = Colors.White;
            // 失去焦点时，将三个键前景色设为白色
            view.TitleBar.ButtonInactiveForegroundColor = Colors.White;


            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            // 窗口内容扩展填充到标题栏
            coreTitleBar.ExtendViewIntoTitleBar = !view.IsFullScreenMode;

            UpdateTitleBarLayout(coreTitleBar);

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            // LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            //TitleBarButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);

            // Update title bar control size as needed to account for system size changes.
            AppTitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
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

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
    }
}
