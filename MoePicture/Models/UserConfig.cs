using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MoePicture.Models
{

    /// <summary> 评级设置 </summary>
    public enum RatingType { Safe, All };

    /// <summary>
    /// 用户配置
    /// </summary>
    public class UserConfig : ObservableObject
    {
        #region Properties
        /// <summary> 评级设置 </summary>
        private RatingType rating;
        /// <summary> 单张图片大小 </summary>
        private double pictureItemSize;


        /// <summary> 缓存设置文件夹 </summary>
        public string CacheFolderToken { get; set; }
        /// <summary> 保存设置文件夹 </summary>
        public string SaveFolderlToken { get; set; }
        /// <summary> 单张图片大小 </summary>
        public Double PictureItemSize { get => pictureItemSize; set { Set(ref pictureItemSize, value); } }
        /// <summary> 评级 </summary>
        public RatingType Rating { get => rating; set { Set(ref rating, value); } }

        // no use right now
        // private ObservableCollection<string> myTags = new ObservableCollection<string>();
        // public ObservableCollection<string> MyTags { get => myTags; set { Set(ref myTags, value); } }

        #endregion Properties
    }
}
