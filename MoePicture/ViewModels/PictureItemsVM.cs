using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoePicture.Models;
using MoePicture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JskyUwpLibs;
using Windows.UI.Xaml.Controls;

namespace MoePicture.ViewModels
{
    /// <summary>
    /// PictureItemsVM
    /// </summary>
    public class PictureItemsVM : ViewModelBase
    {
        #region private Properties

        /// <summary> 搜索标签 </summary>
        private string tag;
        /// <summary> 网站类型 </summary>
        private WebsiteType type;

        /// <summary> 刷新 </summary>
        private RelayCommand refreshCommand;
        /// <summary> 搜索 </summary>
        private RelayCommand<string> searchCommand;
        /// <summary> 切换网站 </summary>
        private RelayCommand<string> changeWebsiteCommand;

        public delegate void ScrollRefresh();
        public ScrollRefresh ScrollRefreshEvent;

        /// <summary> TilesUpdater实例 </summary>
        private static TilesUpdater Tiles = new TilesUpdater();

        /// <summary> 当前选中对象 </summary>
        private PictureItem selectPictureItem;
        /// <summary> PictureItems实例 </summary>
        private PictureItems pictureItems;

        #endregion

        #region Public Properties

        /// <summary> 搜索标签 </summary>
        public string Tag { get => tag; set { Set(ref tag, value); RefreshPictures(); } }
        /// <summary> 网站类型 </summary>
        public WebsiteType Type { get => type; set { type = value; RaisePropertyChanged(() => Tag); RefreshPictures(); } }
        /// <summary> PictureItems实例 </summary>
        public PictureItems PictureItems { get => pictureItems; set { Set(ref pictureItems, value); } }
        /// <summary> 当前选中对象标签 </summary>
        public List<string> SelectPictureTags
        {
            get => SelectPictureItem == null ? null : (SelectPictureItem.Tags).Split(' ').OrderByDescending(s => s.Length).ToList<string>();
        }
        /// <summary> 当前选中对象 </summary>
        public PictureItem SelectPictureItem
        {
            get => selectPictureItem;
            set
            {
                value.UrlType = value.UrlType == UrlType.PreviewUrl ? UrlType.SampleUrl : UrlType.SourceUrl;
                Set(ref selectPictureItem, value);
                RaisePropertyChanged(() => SelectPictureTags);
            }
        }

        #endregion

        #region Functions
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PictureItemsVM()
        {
            Type = WebsiteType.Yande;
            Tag = string.Empty;
        }
        /// <summary> 搜索 </summary>
        private void SearchPictures(string tag)
        {
            Tag = tag;
        }
        /// <summary> 切换网站 </summary>
        private void ChangeWebsite(string websiteStr)
        {
            Type = (WebsiteType)Enum.Parse(typeof(WebsiteType), websiteStr);
        }
        /// <summary> 刷新 </summary>
        private void RefreshPictures()
        {
            PictureItems = new PictureItems(Type, Tag);
            ScrollRefreshEvent?.Invoke();
            ServiceLocator.Current.GetInstance<ShellVM>().ShowError = false;
        }
        /// <summary> 点击单个对象 </summary>
        public async Task SelectItemClick(ItemClickEventArgs e)
        {
            UserActivitiesHelper helper = new UserActivitiesHelper();
            SelectPictureItem = (PictureItem)e.ClickedItem;
            Tiles.UpdataOneItem(await SelectPictureItem.GetStorageFolder(UrlType.PreviewUrl), SelectPictureItem.FileName);
            helper.GenerateActivityAsync(type.ToString(), type.ToString(), SelectPictureItem.Title, SelectPictureItem.SampleUrl);
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }
        /// <summary> 点击标签 </summary>
        public void SelectTagClick(ItemClickEventArgs e)
        {
            SearchPictures((string)e.ClickedItem);
            ServiceLocator.Current.GetInstance<ShellVM>().SwitchSigleCommand.Execute(null);
        }

        #endregion

        #region Commands
        /// <summary> 刷新 </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                    (refreshCommand = new RelayCommand(
                        RefreshPictures));
            }
        }
        /// <summary> 搜索 </summary>
        public RelayCommand<string> SearchCommand
        {
            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand<string>(
                        tag => SearchPictures(tag)));
            }
        }
        /// <summary> 切换网站 </summary>
        public RelayCommand<string> ChangeWebsiteCommand
        {
            get
            {
                return changeWebsiteCommand ??
                    (changeWebsiteCommand = new RelayCommand<string>(
                        websiteStr => ChangeWebsite(websiteStr)));
            }
        }
        #endregion
    }
}
