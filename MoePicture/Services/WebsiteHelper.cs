using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoePicture.Services
{
    public class WebsiteHelper
    {
        /// <summary> yandeUrl </summary>
        private static string yandeUrl = "https://yande.re/post.xml?limit=100";
        /// <summary> konachanUrl </summary>
        private static string konachanUrl = "http://konachan.com/post.xml?limit=100";
        /// <summary> danbooruUrl </summary>
        private static string danbooruUrl = "https://danbooru.donmai.us/posts.xml?limit=100";

        /// <summary> 网站类型 </summary>
        private WebsiteType websiteType;
        /// <summary> 标签 </summary>
        private string searchTag;
        /// <summary> 页码 </summary>
        private int pageNum;

        /// <summary> 页码 </summary>
        public WebsiteType Type { get => websiteType; set => websiteType = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="websiteType">网站类型</param>
        /// <param name="tag">搜索标签</param>
        public WebsiteHelper(WebsiteType websiteType, string tag)
        {
            pageNum = 1;
            Type = websiteType;

            switch (Type)
            {
                case WebsiteType.Yande:
                case WebsiteType.Konachan:
                case WebsiteType.Danbooru:
                    searchTag = tag == "" ? "": "&tags=" + tag;
                    break;
            }
        }
        /// <summary>
        /// 获取每次的链接
        /// </summary>
        /// <returns></returns>
        public string Url()
        {
            string url = "";

            switch (Type)
            {
                case WebsiteType.Yande:
                    url = yandeUrl + "&page=" + pageNum.ToString() + searchTag;
                    break;
                case WebsiteType.Konachan:
                    url = konachanUrl + "&page=" + pageNum.ToString() + searchTag;
                    break;
                case WebsiteType.Danbooru:
                    url = danbooruUrl + "&page=" + pageNum.ToString() + searchTag;
                    break;
            }

            pageNum++;

            return url;
        }

        /// <summary>
        /// 帮助设置图片信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="node"></param>
        /// <param name="websiteType"></param>
        public static void SetInfoFromNode(PictureItem item, XmlNode node, WebsiteType websiteType)
        {
            switch (websiteType)
            {
                case WebsiteType.Yande:
                case WebsiteType.Konachan:
                    Yande(item, node);
                    break;
                case WebsiteType.Danbooru:
                    Danbooru(item, node);
                    break;
                default:
                    item.IsAllRight = false;
                    break;
            }
        }

        public static void Yande(PictureItem item, XmlNode node)
        {
            try
            {
                // 从节点得到图片信息
                item.Id = node.Attributes["id"].Value;
                item.Tags = node.Attributes["tags"].Value;
                item.PreviewUrl = node.Attributes["preview_url"].Value;
                item.SampleUrl = node.Attributes["sample_url"].Value;
                item.SourceUrl = node.Attributes["jpeg_url"].Value;
                item.IsSafe = node.Attributes["rating"].Value == "s";

                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);

            }
            catch
            {
                item.IsAllRight = false;
            }

        }

        public static void Danbooru(PictureItem item, XmlNode node)
        {
            try
            {
                // 从节点得到图片信息
                item.Id = node["id"].InnerText;
                item.Tags = node["tag-string-general"].InnerText;
                item.PreviewUrl = node["preview-file-url"].InnerText;
                item.SampleUrl = node["file-url"].InnerText;
                item.SourceUrl = node["large-file-url"].InnerText;
                item.IsSafe = node["rating"].InnerText == "s";

                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);

            }
            catch
            {
                item.IsAllRight = false;
            }

        }
    }
}
