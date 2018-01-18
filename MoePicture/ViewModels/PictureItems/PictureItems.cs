using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using MoePicture.Services;
using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MoePicture.ViewModels.PictureItems
{
    public class PictureItems : IncrementalLoadingBase<PictureItem>
    {
        #region Propeities

        private int pageNum;
        private string url;
        private string tag;
        private bool loadAll;
        private bool noMorePicture;

        public DataBase.DataBase DB;

        #endregion Propeities

        #region Constructer

        public PictureItems(string url, string tag = "")
        {
            pageNum = 1;
            this.url = url;
            this.tag = tag == "" ? "" : "&tags=" + tag;
            loadAll = ServiceLocator.Current.GetInstance<UserConfigViewModel>().Config.Raing == UserConfig.rating.all;
            noMorePicture = false;

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
        protected override async Task<IList<Models.PictureItem>> LoadMoreItemsOverrideAsync(CancellationToken c, int count)
        {
            List<Models.PictureItem> Items = new List<Models.PictureItem>();

            XmlDocument xml = new XmlDocument();
            // 组合成要访问的Uri
            string url = this.url + "&page=" + pageNum.ToString() + tag;
            // 先在数据库里查找Uri对应的xml文件，如果没有，从网上获取
            string str = DB.select(url);
            if (str != String.Empty)
            {
                // pass
            }
            else
            {
                str = await Services.Spider.GetString(new Uri(url));
                DB.add(url, str);
            }

            xml.LoadXml(str);

            // 获取xml文件里面包含图片的xml节点
            XmlNodeList nodeList = xml.GetElementsByTagName("post");

            if (nodeList.Count > 0)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var item = new PictureItem(nodeList[i % 100]);

                    if (loadAll || item.IsSafe)
                    {
                        Items.Add(item);
                    }
                }
                // for we use Count to bind
                OnPropertyChanged("Count");
                pageNum++;
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
