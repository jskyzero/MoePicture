using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoePicture.ViewModels.PictureItems;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace MoePicture.ViewModels
{
    public class PictureItemsViewModel : ViewModelBase
    {
        private static string url = "https://yande.re/post.xml?limit=100";
        private string _tag;

        private RelayCommand _refreshCommand;
        private RelayCommand<string> _searchCommand;

        private PictureItems.PictureItems _pictureItems;

        public PictureItems.PictureItems PictureItems { get => _pictureItems; set { Set(ref _pictureItems, value); } }
        public string Tag { get => _tag; set { _tag = value; RaisePropertyChanged(() => Tag); } }

        public PictureItemsViewModel()
        {
            RefreshPictures();
        }

        private void SetTag(string newTag)
        {
            Tag = newTag;
            RefreshPictures();
        }

        private void SearchPictures(string tag)
        {
            SetTag(tag);
            ServiceLocator.Current.GetInstance<MainViewModel>().HideSearchCommand.Execute(null);
        }

        private void RefreshPictures()
        {
            PictureItems = new PictureItems.PictureItems(url, Tag);
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
