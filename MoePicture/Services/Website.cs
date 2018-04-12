using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MoePicture.Services
{

    internal class Website
    {
        private static string yandeUrl = "https://yande.re/post.xml?limit=100";
        private static string konachanUrl = "http://konachan.com/post.xml?limit=100";
        private static string danbooruUrl = "https://danbooru.donmai.us/posts.xml?limit=100";

        private WebsiteType websiteType;
        private string tag;
        private int pageNum;

        public WebsiteType Type { get => websiteType; set => websiteType = value; }


        public Website(WebsiteType websiteType, string tag)
        {
            pageNum = 1;
            Type = websiteType;

            switch (Type)
            {
                case WebsiteType.yande:
                    this.tag = tag == "" ? "" : "&tags=" + tag;
                    break;
                case WebsiteType.konachan:
                    this.tag = tag == "" ? "" : "&tags=" + tag;
                    break;
                case WebsiteType.danbooru:
                    this.tag = tag == "" ? "" : "&tags=" + tag;
                    break;
                default:
                    this.tag = "";
                    break;
            }
        }

        //public static List<string> ParseTags(WebsiteType websiteType)
        //{
        //    switch (websiteType)
        //    {
        //        case WebsiteType.yande:
        //            return new List<string> { "id", "tags", "preview_url", "sample_url", "jpeg_url", "rating", "s" };
        //        case WebsiteType.konachan:
        //            return new List<string> { "id", "tags", "preview_url", "sample_url", "jpeg_url", "rating", "s" };
        //        case WebsiteType.danbooru:
        //            return new List<string> { "id", "tag-string-general", "preview-file-url", "file-url", "large-file-url", "rating", "s" };
        //        default:
        //            return new List<string> { "id", "tags", "preview_url", "sample_url", "jpeg_url", "rating", "s" };
        //    }
        //}


        public string Url()
        {
            string url = "";

            switch (Type)
            {
                case WebsiteType.yande:
                    url = yandeUrl + "&page=" + pageNum.ToString() + tag;
                    break;
                case WebsiteType.konachan:
                    url = konachanUrl + "&page=" + pageNum.ToString() + tag;
                    break;
                case WebsiteType.danbooru:
                    url = danbooruUrl + "&page=" + pageNum.ToString() + tag;
                    break;
            }

            pageNum++;

            return url;
        }

        public static void SetInfoFromNode(PictureItem item, XmlNode node, WebsiteType websiteType)
        {
            switch (websiteType)
            {
                case WebsiteType.yande:
                case WebsiteType.konachan:
                    Yande(item, node);
                    break;
                case WebsiteType.danbooru:
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
                item.JpegUrl = node.Attributes["jpeg_url"].Value;
                item.IsSafe = node.Attributes["rating"].Value == "s";

                // 通过url处理得到两种name
                item.Name = Spider.GetFileNameFromUrl(item.JpegUrl);
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
                //string site = "https://danbooru.donmai.us";
                // 从节点得到图片信息
                item.Id = node["id"].InnerText;
                item.Tags = node["tag-string-general"].InnerText;
                item.PreviewUrl = node["preview-file-url"].InnerText;
                item.SampleUrl = node["file-url"].InnerText;
                item.JpegUrl = node["large-file-url"].InnerText;
                item.IsSafe = node["rating"].InnerText == "s";

                // 通过url处理得到两种name
                item.Name = Spider.GetFileNameFromUrl(item.JpegUrl);
                item.FileName = Spider.GetFileNameFromUrl(item.PreviewUrl);

            }
            catch
            {
                item.IsAllRight = false;
            }

        }
    }
}
