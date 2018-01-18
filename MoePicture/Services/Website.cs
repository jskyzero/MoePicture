using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.Services
{

    internal class Website
    {
        private static string yandeUrl = "https://yande.re/post.xml?limit=100";

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
                default:
                    this.tag = "";
                    break;
            }

        }


        public string Url()
        {
            string url = "";

            switch (Type)
            {
                case WebsiteType.yande:
                    url = yandeUrl + "&page=" + pageNum.ToString() + tag;
                    break;
            }

            pageNum++;

            return url;
        }
    }
}
