using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoePicture.ViewModels.PictureItems;

namespace MoePicture.ViewModels
{
    public class PictureItemsViewModel : ViewModelBase
    {
        private static string url = "https://yande.re/post.xml?limit=100";

        private PictureItems.PictureItems _pictureItems;

        public PictureItems.PictureItems PictureItems { get => _pictureItems; set { Set(ref _pictureItems, value); } }

        public PictureItemsViewModel()
        {
            PictureItems = new PictureItems.PictureItems(url, "");
        }
    }
}
