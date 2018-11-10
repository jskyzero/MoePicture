using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;

namespace MoePicture.Services
{
    /// <summary>
    /// 枚举网站种类
    /// </summary>
    public enum WebsiteType { Yande, Konachan, Danbooru, Gelbooru, Safebooru };

    public class WebsiteHelper
    {

        /// <summary> url table </summary>
        static Dictionary<WebsiteType, string> UrlDict = new Dictionary<WebsiteType, string>
        {
            {WebsiteType.Yande, "https://yande.re/post.xml?limit=100"} ,
            {WebsiteType.Konachan, "http://konachan.com/post.xml?limit=100"},
            {WebsiteType.Danbooru, "https://danbooru.donmai.us/posts.xml?limit=100"},
            {WebsiteType.Gelbooru, "https://gelbooru.com/index.php?page=dapi&s=post&q=index&limit=100"},
            {WebsiteType.Safebooru, "http://safebooru.org/index.php?page=dapi&s=post&q=index&limit=100"},
        };

        // Safebooru can't find website now
        // https://chan.sankakucomplex.com/post/index.xml 403 Forbidden
        // https://idol.sankakucomplex.com/post/index.xml 403 Forbidden
        // Behoimi I don't like add cosplay site


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
                default:
                    searchTag = tag == "" ? "" : "&tags=" + tag;
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
                case WebsiteType.Konachan:
                case WebsiteType.Danbooru:
                    url = UrlDict[Type] + "&page=" + pageNum.ToString() + searchTag;
                    break;
                case WebsiteType.Gelbooru:
                case WebsiteType.Safebooru:
                    url = UrlDict[Type] + "&pid=" + pageNum.ToString() + searchTag;
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
                case WebsiteType.Gelbooru:
                    Gelbooru(item, node);
                    break;
                case WebsiteType.Safebooru:
                    Safebooru(item, node);
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
                item.PreviewSize = new Size(int.Parse(node.Attributes["preview_width"].Value), 
                                            int.Parse(node.Attributes["preview_height"].Value));
                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);

            }
            catch(Exception e)
            {
                item.IsAllRight = false;
            }
        }

        public static void Gelbooru(PictureItem item, XmlNode node)
        {
            try
            {
                // 从节点得到图片信息
                item.Id = node.Attributes["id"].Value;
                item.Tags = node.Attributes["tags"].Value;
                item.PreviewUrl = node.Attributes["preview_url"].Value;
                item.SampleUrl = node.Attributes["sample_url"].Value;
                item.SourceUrl = node.Attributes["file_url"].Value;
                item.IsSafe = node.Attributes["rating"].Value == "s";
                item.PreviewSize = new Size(int.Parse(node.Attributes["preview_width"].Value),
                                            int.Parse(node.Attributes["preview_height"].Value));
                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);
            }
            catch(Exception e)
            {
                item.IsAllRight = false;
            }
        }

        public static void Safebooru(PictureItem item, XmlNode node)
        {
            try
            {
                // 从节点得到图片信息
                item.Id = node.Attributes["tags"].Value;
                item.Tags = node.Attributes["tags"].Value;
                item.PreviewUrl = "http:" + node.Attributes["preview_url"].Value;
                item.SampleUrl = "http:" + node.Attributes["sample_url"].Value;
                item.SourceUrl = "http:" + node.Attributes["file_url"].Value;
                item.IsSafe = node.Attributes["rating"].Value == "s";
                item.PreviewSize = new Size(int.Parse(node.Attributes["preview_width"].Value),
                                            int.Parse(node.Attributes["preview_height"].Value));
                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);
            }
            catch(Exception e)
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
                item.PreviewSize = new Size(int.Parse(node["image-width"].InnerText),
                                            int.Parse(node["image-height"].InnerText));
                // 通过url处理得到两种name
                item.Title = Spider.GetFileNameFromUrl(item.SourceUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);

            }
            catch(Exception e)
            {
                item.IsAllRight = false;
            }

        }
    }
}
