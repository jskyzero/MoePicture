using CommonServiceLocator;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace MoePicture.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private async void changeSaveFolderPath(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                ServiceLocator.Current.GetInstance<UserConfigVM>().Config.SaveFolderlToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
            }
        }

        private async void changeCacheFolderPath(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                ServiceLocator.Current.GetInstance<UserConfigVM>().Config.CacheFolderToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
            }
        }

        private void resetAllPath(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<UserConfigVM>().Config.SaveFolderlToken = StorageApplicationPermissions.FutureAccessList.Add(KnownFolders.PicturesLibrary);
            ServiceLocator.Current.GetInstance<UserConfigVM>().Config.SaveFolderlToken = StorageApplicationPermissions.FutureAccessList.Add(ApplicationData.Current.LocalCacheFolder);
        }

        private async void openLogWindow(object sender, RoutedEventArgs e)
        {
            await JskyUwpLibs.LogWindows.CreateLogWindowsAsync(1);
        }
    }
}
