using CommonServiceLocator;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class PictureSingle : UserControl
    {
        public PictureSingle()
        {
            this.InitializeComponent();
            // Win2D.initialGlass(TagsGlass);
        }

        private void TagsButton_Click(object sender, RoutedEventArgs e)
        {
            TagsGrid.Visibility = TagsGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ServiceLocator.Current.GetInstance<PictureItemsVM>().SelectTagClick(e);
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            var SelectItem = (Models.PictureItem)((Button)sender).DataContext;
            FileSavePicker savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var titleStrs = SelectItem.Title.Split('.').ToList();
            if (titleStrs.Count > 1) titleStrs.RemoveAt(titleStrs.Count - 1);
            savePicker.SuggestedFileName = String.Join('.', titleStrs.ToArray());
            savePicker.FileTypeChoices.Add(".png", new List<string>() { ".png" });
            savePicker.FileTypeChoices.Add(".jpg", new List<string>() { ".jpg", ".jpeg" });

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await Services.Spider.DownloadPictureFromUriToFolder(new Uri(SelectItem.SourceUrl), (await file.GetParentAsync()).Path, file.Name);
            }
        }

        // 图片分享的绑定函数
        private async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var SelectItem = (Models.PictureItem)TagsGrid.DataContext;
            // 添加文字描述
            request.Data.Properties.Title = SelectItem.Title;
            request.Data.Properties.Description = "Moe picture share";
            request.Data.SetText(SelectItem.Tags);

            // 添加当前图片
            if (SelectItem.ImageSource != null)
            {
                StorageFolder folder = await SelectItem.GetStorageFolder(Models.UrlType.SampleUrl);
                var photoFile = await folder.GetFileAsync(SelectItem.FileName);
                request.Data.SetStorageItems(new List<StorageFile> { photoFile });
            }
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }
    }
}
