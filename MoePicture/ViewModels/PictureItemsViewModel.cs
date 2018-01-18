using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoePicture.ViewModels.PictureItems;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using Windows.UI.Xaml.Controls;
using TileUpdate;

namespace MoePicture.ViewModels
{
    public class PictureItemsViewModel : ViewModelBase
    {
        private string _tag;

        private RelayCommand _refreshCommand;
        private RelayCommand<string> _searchCommand;
        private static TilesUpdater Tiles = new TilesUpdater();

        private string _selectTag;
        private List<string> _selectPictureTags;
        private PictureItem _selectPictureItem;
        private PictureItems.PictureItems _pictureItems;

        public string Tag { get => _tag; set { _tag = value; RaisePropertyChanged(() => Tag); } }
        public string SelectTag { get => _selectTag; set { Set(ref _selectTag, value); } }
        public List<string> SelectPictureTags { get => _selectPictureTags; set { Set(ref _selectPictureTags, value); } }
        public PictureItem SelectPictureItem { get => _selectPictureItem; set { _selectPictureItem = value; SelectPictureItem.UrlType = UrlType.jpeg_url; RaisePropertyChanged(() => SelectPictureItem); } }
        public PictureItems.PictureItems PictureItems { get => _pictureItems; set { Set(ref _pictureItems, value); } }

        public PictureItemsViewModel()
        {
            RefreshPictures();
        }

        private void SearchPictures(string tag)
        {
            Tag = tag;
            RefreshPictures();
        }

        private void RefreshPictures()
        {
            PictureItems = new PictureItems.PictureItems(WebsiteType.yande, Tag);
        }

        public void SelectItemClick(ItemClickEventArgs e)
        {
            SelectPictureItem = (PictureItem)e.ClickedItem;
            SelectPictureTags = new List<string>((SelectPictureItem.Tags).Split(' '));
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
                return _refreshCommand ??
                    (_refreshCommand = new RelayCommand(
                        RefreshPictures));
            }
        }

        public RelayCommand<string> RearchCommand
        {
            get
            {
                return _searchCommand ??
                    (_searchCommand = new RelayCommand<string>(
                        tag => SearchPictures(tag)));
            }
        }
    }
}
