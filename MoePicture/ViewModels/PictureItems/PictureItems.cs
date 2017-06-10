using Microsoft.Practices.ServiceLocation;
using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MoePicture.ViewModels.PictureItems
{
    public class PictureItems : IncrementalLoadingBase<PictureItem>
    {
        #region Propeities

        private int _pageNum;
        private string _url;
        private string _tag;
        private bool _loadAll = ServiceLocator.Current.GetInstance<UserConfigViewModel>().Config.Raing == UserConfig.rating.all;

        // 爬虫模拟Chrome浏览器的字符串
        public static string User_Agent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit / 537.36(KHTML, like Gecko) Chrome  47.0.2526.106 Safari / 537.36";

        public DataBase.DataBase DB;

        #endregion Propeities

        #region Constructer

        public PictureItems(string url, string tag = "")
        {
            _url = url;
            _pageNum = 1;
            _tag = tag == "" ? "" : "&tags=" + tag;

            DB = new DataBase.DataBase();
        }

        #endregion Constructer

        #region Methods

        // 父类的虚函数实现，如果下次访问的页面数>0，证明网站还有可获取图片
        protected override bool HasMoreItemsOverride()
        {
            return (_pageNum > 0);
        }

        // 父类的虚函数实现，根据_uri，_pageNum和_tag属性，组合成要访问的Uri，
        // 并通过该Uri访问网站获取新增图片
        protected override async Task<IList<Models.PictureItem>> LoadMoreItemsOverrideAsync(CancellationToken c, int count)
        {
            using (var client = new HttpClient())
            {
                // 对爬虫进行伪装，防止网址发现爬虫然后无法访问
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", User_Agent);

                List<Models.PictureItem> Items = new List<Models.PictureItem>();

                XmlDocument xml = new XmlDocument();
                // 组合成要访问的Uri
                string url = _url + "&page=" + _pageNum.ToString() + _tag;
                // 先在数据库里查找Uri对应的xml文件，如果没有，从网上获取
                string str = DB.select(url);
                if (str != String.Empty)
                {
                }
                else
                {
                    str = await client.GetStringAsync(new Uri(url));
                    DB.add(url, str);
                }

                xml.LoadXml(str);

                // 获取xml文件里面包含图片的xml节点
                XmlNodeList nodeList = xml.GetElementsByTagName("post");

                if (nodeList.Count > 0)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        if (_loadAll || nodeList[i % 100].Attributes["rating"].Value == "s")
                        {
                            var item = new PictureItem(nodeList[i % 100]);
                            Items.Add(item);
                        }
                    }

                    _pageNum++;
                }
                else
                {
                    // 如果xml文件里没有任何图片xml节点，将_pageNum设置为0，
                    // 表示无法得到更多更多图片
                    _pageNum = 0;
                }

                return Items;
            }
        }

        #endregion Methods
    }
}
