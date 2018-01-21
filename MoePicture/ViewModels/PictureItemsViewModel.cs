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

namespace MoePicture.ViewModels
{
    public class PictureItemsViewModel : ViewModelBase
    {
        private string tag;
        private WebsiteType type;

        private RelayCommand refreshCommand;
        private RelayCommand<string> searchCommand;
        private RelayCommand<string> changeWebsiteCommand;

        private static TilesUpdater Tiles = new TilesUpdater();

        private PictureItem selectPictureItem;
        private PictureItems pictureItems;

        public string Tag { get => tag; set { Set(ref tag, value); } }
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

        public PictureItemsViewModel()
        {
            type = WebsiteType.yande;
            RefreshPictures();
        }

        private void SearchPictures(string tag)
        {
            Tag = tag;
            RefreshPictures();
        }

        private void ChangeWebsite(string websiteStr)
        {
            type = (WebsiteType)Enum.Parse(typeof(WebsiteType), websiteStr);
            RefreshPictures();
        }

        private void RefreshPictures()
        {
            PictureItems = new PictureItems(type, Tag);
        }

        public void SelectItemClick(ItemClickEventArgs e)
        {
            SelectPictureItem = (PictureItem)e.ClickedItem;
            Tiles.UpdataOneItem(SelectPictureItem.FileName);
            ServiceLocator.Current.GetInstance<MainViewModel>().SwitchSigleCommand.Execute(null);
        }

        public void SelectTagClick(ItemClickEventArgs e)
        {
            SearchPictures((string)e.ClickedItem);
            ServiceLocator.Current.GetInstance<MainViewModel>().SwitchSigleCommand.Execute(null);
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
