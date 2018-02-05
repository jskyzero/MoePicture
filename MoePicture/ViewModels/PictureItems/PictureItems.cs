using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MoePicture.ViewModels
{
    public class PictureItems : IncrementalLoadingBase<PictureItem>
    {
        #region Propeities

        private bool loadAll;
        private bool noMorePicture;
        private Services.Website website;

        public DataBase.DataBase DB;

        #endregion Propeities

        #region Constructer

        public PictureItems(WebsiteType websiteType, string tag = "")
        {
            loadAll = ServiceLocator.Current.GetInstance<UserConfigVM>().Config.Rating == UserConfig.RatingType.all;
            noMorePicture = false;
            website = new Services.Website(websiteType, tag);

            DB = new DataBase.DataBase();
        }

        #endregion Constructer

        #region Methods

        // 父类的虚函数实现，如果下次访问的页面数>0，证明网站还有可获取图片
        protected override bool HasMoreItemsOverride()
        {
            return !noMorePicture;
        }

        // 父类的虚函数实现，根据_uri，_pageNum和_tag属性，组合成要访问的Uri，
        // 并通过该Uri访问网站获取新增图片
        protected override async Task<IList<PictureItem>> LoadMoreItemsOverrideAsync(CancellationToken c, int count)
        {

            string url = website.Url();

            // 先在数据库里查找Uri对应的xml文件，如果没有，从网上获取
            string str = DB.select(url);
            if (str == String.Empty)
            {
                str = await Services.Spider.GetString(new Uri(url));
                DB.add(url, str);
            }

            var Items = PictureItem.GetPictureItems(website.Type, str, loadAll);

            if (Items.Count > 0)
            {
                OnPropertyChanged("Count");
            }
            else
            {
                // 如果xml文件里没有任何图片xml节点，将noMorePicture 设置为 true
                // 表示无法得到更多更多图片
                noMorePicture = true;
            }

            return Items;
        }

        #endregion Methods
    }
}
