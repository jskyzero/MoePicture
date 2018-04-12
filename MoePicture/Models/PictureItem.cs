using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using MoePicture.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MoePicture.Models
{

    /// <summary>
    /// 枚举网站种类
    /// </summary>
    public enum WebsiteType { yande, konachan, danbooru };

    /// <summary>
    /// 枚举Uri种类
    /// </summary>
    public enum UrlType { preview_url, sample_url, jpeg_url };

    /// <summary>
    /// 图片Model类，用于储存图片的各种信息
    /// </summary>
    public class PictureItem : ObservableObject
    {
        #region Properties

        // 弱引用对象，用于存储下载好的图片对象
        private WeakReference bitmapImage;

        private UrlType urlType;
        private WebsiteType websiteType;

        private bool isSafe = false;
        private bool isAllRight = true;

        public string Id { get; set; }
        public string Tags { get; set; }
        public string PreviewUrl { get; set; }
        public string SampleUrl { get; set; }
        public string JpegUrl { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public bool IsAllRight { get => isAllRight; set => isAllRight = value; }
        public bool IsSafe { get => isSafe; set => isSafe = value; }

        public WebsiteType Type { get => websiteType; set => websiteType = value; }
        public UrlType UrlType { get { return urlType; } set { urlType = value; bitmapImage = null; } }


        #endregion Properties

        #region Constructer

        public PictureItem(WebsiteType type, XmlNode node)
        {
            UrlType = UrlType.preview_url;
            Type = type;
            Website.SetInfoFromNode(this, node, Type);
        }


        public static List<PictureItem> GetPictureItems(WebsiteType type, string str, bool loadAll)
        {
            List<PictureItem> Items = new List<PictureItem>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(str);

            // 获取xml文件里面包含图片的xml节点
            XmlNodeList nodeList = xml.GetElementsByTagName("post");
            if (nodeList.Count > 0)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var item = new PictureItem(type, nodeList[i]);

                    if ((loadAll || item.IsSafe) && item.IsAllRight)
                    {
                        Items.Add(item);
                    }
                }
            }
            return Items;
        }

        #endregion Constructer

        #region ImageSource Properties

        public async Task<StorageFolder> GetStorageFolder(UrlType urlType)
        {
            string folderToken;
            StorageFolder folder;
            // 根据图片Uri类型，打开到不同文件夹里
            if (urlType == UrlType.preview_url)
            {
                folderToken = ServiceLocator.Current.GetInstance<ViewModels.UserConfigVM>().Config.CacheFolderToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync("cache", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                folderToken = ServiceLocator.Current.GetInstance<ViewModels.UserConfigVM>().Config.SaveFolderlToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync("MoePicture", CreationCollisionOption.OpenIfExists);
            }
            folder = await folder.CreateFolderAsync(Type.ToString(), CreationCollisionOption.OpenIfExists);
            return folder;
        }


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
                    //else
                    //    Debug.WriteLine("数据已经被回收");
                }
                // 弱引用已经被回收那么则进行异步下载
                Uri imageUri = new Uri(UrlType == UrlType.preview_url ? PreviewUrl : SampleUrl);
                // 创建后台线程，下载图片
                Task.Factory.StartNew(() => { DownloadImageAsync(imageUri); });
                return null;
            }
        }

        // 下载图片的方法
        private async void DownloadImageAsync(object state)
        {

            var folder = await GetStorageFolder(UrlType);
            var path = Path.Combine(folder.Path, FileName);


            if (!File.Exists(path) || ((new FileInfo(path).Length) == 0))
            {
                if (UrlType == UrlType.preview_url)
                {
                    await Spider.DownloadPictureFromUriToFolderLock(state as Uri, folder.Path, FileName);
                }
                else
                {
                    await Spider.DownloadPictureFromUriToFolder(state as Uri, folder.Path, FileName);
                }
            }

            // 在UI线程处理位图和UI更新
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                try
                {
                    var file = await StorageFile.GetFileFromPathAsync(path);
                    var stream = await file.OpenReadAsync();
                    var bm = new BitmapImage();
                    await bm.SetSourceAsync(stream);
                    // 把图片位图对象存放到弱引用对象里面
                    if (bitmapImage == null)
                        bitmapImage = new WeakReference(bm);
                    else
                        bitmapImage.Target = bm;
                    //触发UI绑定属性的改变
                    RaisePropertyChanged(() => ImageSource);
                }
                catch
                {
                    // pass
                }
            });
        }

        #endregion ImageSource Properties
    }
}
