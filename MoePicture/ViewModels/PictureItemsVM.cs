using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using Windows.UI.Xaml.Controls;
using TileUpdate;
using MoePicture.Converters;
using Windows.UI.Xaml.Data;

namespace MoePicture.ViewModels
{
    public class PictureItemsVM : ViewModelBase
    {
        private string tag;
        private WebsiteType type;

        private RelayCommand refreshCommand;
        private RelayCommand<string> searchCommand;
        private RelayCommand<string> changeWebsiteCommand;

        private static TilesUpdater Tiles = new TilesUpdater();

        private PictureItem selectPictureItem;
        private PictureItems pictureItems;

        public string Tag { get => tag; set { Set(ref tag, value); RefreshPictures(); } }
        public WebsiteType Type { get => type; set { type = value; RaisePropertyChanged(() => Tag); RefreshPictures(); } }

        public PictureItems PictureItems { get => pictureItems; set { Set(ref pictureItems, value); } }
        public List<string> SelectPictureTags
        {
            get => SelectPictureItem == null ? null : new List<string>((SelectPictureItem.Tags).Split(' '));
        }

        public PictureItem SelectPictureItem
        {
            get => selectPictureItem;
            set
            {
                value.UrlType = UrlType.jpeg_url;
                Set(ref selectPictureItem, value);
                RaisePropertyChanged(() => SelectPictureTags);
            }
        }



        public PictureItemsVM()
        {
            Type = WebsiteType.yande;
            Tag = string.Empty;
        }

        private void SearchPictures(string tag)
        {
            Tag = tag;
        }

        private void ChangeWebsite(string websiteStr)
        {
            Type = (WebsiteType)Enum.Parse(typeof(WebsiteType), websiteStr);
        }

        private void RefreshPictures()
        {
            PictureItems = new PictureItems(Type, Tag);
        }

        public void SelectItemClick(ItemClickEventArgs e)
        {
            SelectPictureItem = (PictureItem)e.ClickedItem;
            Tiles.UpdataOneItem(SelectPictureItem.FileName);
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }

        public void SelectTagClick(ItemClickEventArgs e)
        {
            SearchPictures((string)e.ClickedItem);
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                    (refreshCommand = new RelayCommand(
                        RefreshPictures));
            }
        }

        public RelayCommand<string> RearchCommand
        {
            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand<string>(
                        tag => SearchPictures(tag)));
            }
        }

        public RelayCommand<string> ChangeWebsiteCommand
        {
            get
            {
                return changeWebsiteCommand ??
                    (changeWebsiteCommand = new RelayCommand<string>(
                        websiteStr => ChangeWebsite(websiteStr)));
            }
        }

    }
}
