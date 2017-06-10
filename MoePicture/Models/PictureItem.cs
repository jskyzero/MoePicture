using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using MoePicture.Servers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MoePicture.Models
{
    /// <summary>
    /// 图片Model类，用于储存图片的各种信息
    /// </summary>
    public class PictureItem : ObservableObject
    {
        #region Properties

        // 枚举Uri种类
        public enum UrlType { preview_url, sample_url, jpeg_url };

        // 弱引用对象，用于存储下载好的图片对象
        private WeakReference bitmapImage;

        private UrlType _url_type;

        public string id { get; set; }
        public string tags { get; set; }
        public string preview_url { get; set; }
        public string sample_url { get; set; }
        public string jpeg_url { get; set; }
        public string file_name { get; set; }
        public string name { get; set; }
        public UrlType url_type { get { return _url_type; } set { _url_type = value; bitmapImage = null; } }

        #endregion Properties

        #region Constructer

        public PictureItem(XmlNode node)
        {
            url_type = UrlType.preview_url;
            try
            {
                // 从节点得到图片信息
                id = node.Attributes["id"].Value;
                tags = node.Attributes["tags"].Value;
                preview_url = node.Attributes["preview_url"].Value;
                sample_url = node.Attributes["sample_url"].Value;
                jpeg_url = node.Attributes["jpeg_url"].Value;
                // 通过url处理得到两种name
                name = Spider.GetFileNameFromUrl(jpeg_url);
                file_name = Spider.GetFileNameFromUrl(preview_url);
            }
            catch
            {
                // if no tags set as default
            }
        }

        #endregion Constructer

        #region ImageSource Properties

        // ImageSource属性用于绑定到列表的Image控件上
        public ImageSource ImageSource
        {
            get
            {
                if (bitmapImage != null)
                {
                    // 如果弱引用没有没回收，则取弱引用的值
                    if (bitmapImage.IsAlive)
                        return (ImageSource)bitmapImage.Target;
                    else
                        Debug.WriteLine("数据已经被回收");
                }
                // 弱引用已经被回收那么则通过图片网络地址进行异步下载
                Uri imageUri = new Uri(url_type == UrlType.preview_url ? preview_url : sample_url);
                // 创建后台线程，下载图片
                Task.Factory.StartNew(() => { DownloadImageAsync(imageUri); });
                return null;
            }
        }

        // 下载图片的方法
        private async void DownloadImageAsync(object state)
        {
            // 根据图片Uri类型，下载到不同文件夹里
            StorageFolder folder = ApplicationData.Current.LocalCacheFolder;
            folder = await folder.CreateFolderAsync(url_type == UrlType.preview_url ? "perview" : "sample", CreationCollisionOption.OpenIfExists);

            if (!File.Exists(Path.Combine(folder.Path, file_name)))
            {
                if (url_type == UrlType.preview_url)
                {
                    await Spider.DownloadPictureFromUriToFolder(state as Uri, folder.Path, file_name);
                }
                else
                {
                    await Spider.DownloadPictureFromUriToFolderLock(state as Uri, folder.Path, file_name);
                }
            }

            // 在UI线程处理位图和UI更新
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                BitmapImage bm = new BitmapImage(new Uri(Path.Combine(folder.Path, file_name)));

                // 把图片位图对象存放到弱引用对象里面
                if (bitmapImage == null)
                    bitmapImage = new WeakReference(bm);
                else
                    bitmapImage.Target = bm;
                //触发UI绑定属性的改变
                RaisePropertyChanged(() => ImageSource);
            });
        }

        #endregion ImageSource Properties
    }
}
