using CommonServiceLocator;
using MoePicture.Models;
using MoePicture.Services;
using MoePicture.Services.DataBase;
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

        /// <summary> 是否加载全部图片 </summary>
        private bool loadAll;
        /// <summary> 判断是否还有图片 </summary>
        private bool noMorePicture;
        /// <summary> 网页类型 </summary>
        private WebsiteHelper website;
        /// <summary> 数据库实例 </summary>
        public DataBase DB;

        #endregion Propeities

        #region Constructer

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="websiteType">网站类型</param>
        /// <param name="tag">搜索标签</param>
        public PictureItems(WebsiteType websiteType, string tag = "")
        {
            noMorePicture = false;
            DB = new DataBase(GlobalConfig.DataBaseName);
            website = new Services.WebsiteHelper(websiteType, tag);
            loadAll = ServiceLocator.Current.GetInstance<UserConfigVM>().Config.Rating == RatingType.All;
            //LoadMoreItemsAsync(100);
        }

        #endregion Constructer

        #region Methods

        /// <summary>
        /// 是否还有可获取图片
        /// </summary>
        /// <returns>True:还有可获取图片</returns>
        protected override bool HasMoreItemsOverride()
        {
            return !noMorePicture;
        }


        /// <summary>
        /// 访问网站获取新增图片
        /// </summary>
        /// <param name="c">CancellationToken</param>
        /// <param name="count">数目</param>
        /// <returns></returns>
        protected override async Task<IList<PictureItem>> LoadMoreItemsOverrideAsync(CancellationToken c, int count)
        {
            try
            {
                string url = website.Url();

                // 先在数据库里查找Uri对应的xml文件，如果没有，从网上获取
                string str = DB.select(url);
                if (str == String.Empty)
                {
                    str = await Spider.GetString(new Uri(url));
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
            catch
            {
                ServiceLocator.Current.GetInstance<ShellVM>().ShowError = true;
                return PictureItem.GetPictureItems(website.Type, "", loadAll);
            }
        }

        #endregion Methods
    }
}
