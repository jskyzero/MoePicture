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
    public enum WebsiteType { Yande, Konachan, Danbooru };

    /// <summary>
    /// 枚举Uri种类
    /// </summary>
    public enum UrlType { PreviewUrl, SampleUrl, SourceUrl };

    /// <summary>
    /// 图片Model类，用于储存图片的各种信息
    /// </summary>
    public class PictureItem : ObservableObject
    {
        #region Properties

        /// <summary> 弱引用对象，用于存储下载好的图片对象 </summary>
        private WeakReference bitmapImage;

        /// <summary> 链接种类 </summary>
        private UrlType urlType;
        /// <summary> 网站类型 </summary>
        private WebsiteType websiteType;
        /// <summary> 图片是否安全 </summary>
        private bool isSafe = false;
        /// <summary> 处理图片过程种是否顺利 </summary>
        private bool isAllRight = true;

        /// <summary> 标识id </summary>
        public string Id { get; set; }
        /// <summary> 标签 </summary>
        public string Tags { get; set; }
        /// <summary> 预览链接 </summary>
        public string PreviewUrl { get; set; }
        /// <summary> 样本链接 </summary>
        public string SampleUrl { get; set; }
        /// <summary> 原图链接 </summary>
        public string SourceUrl { get; set; }
        /// <summary> 文件保存名字 </summary>
        public string FileName { get; set; }
        /// <summary> 显示标题 </summary>
        public string Title { get; set; }
        /// <summary> 图片是否安全 </summary>
        public bool IsSafe { get => isSafe; set => isSafe = value; }
        /// <summary> 处理图片过程种是否顺利 </summary>
        public bool IsAllRight { get => isAllRight; set => isAllRight = value; }
        /// <summary> 网站类型 </summary>
        public WebsiteType Type { get => websiteType; set => websiteType = value; }
        /// <summary> 链接种类 </summary>
        public UrlType UrlType { get { return urlType; } set { urlType = value; bitmapImage = null; } }


        #endregion Properties

        #region Constructer

        /// <summary>
        /// 根据网站类型和节点构造一个实例
        /// </summary>
        /// <param name="type">网站类型</param>
        /// <param name="node">XML节点</param>
        public PictureItem(WebsiteType type, XmlNode node)
        {
            Type = type;
            // 初始化为预览链接
            UrlType = UrlType.PreviewUrl;
            // 用网站特异性的方法来设置具体信息
            WebsiteHelper.SetInfoFromNode(this, node, Type);
        }

        /// <summary>
        /// 静态方法批量构造实例
        /// </summary>
        /// <param name="type">网站类型</param>
        /// <param name="xmlString">XML字符串</param>
        /// <param name="loadAll">加载信息</param>
        /// <returns></returns>
        public static List<PictureItem> GetPictureItems(WebsiteType type, string xmlString, bool loadAll)
        {
            List<PictureItem> Items = new List<PictureItem>();
            XmlDocument xml = new XmlDocument();
            // 加载XML字符数据
            xml.LoadXml(xmlString);

            // 获取xml文件里面包含图片的xml节点
            XmlNodeList nodeList = xml.GetElementsByTagName("post");
            for (int i = 0; i < nodeList.Count; i++)
            {
                var item = new PictureItem(type, nodeList[i]);

                if ((loadAll || item.IsSafe) && item.IsAllRight)
                {
                    Items.Add(item);
                }
            }
            return Items;
        }

        #endregion Constructer

        #region ImageSource Properties
        /// <summary>
        /// 根据链接类型打开不同的文件夹
        /// </summary>
        /// <param name="urlType">链接类型</param>
        /// <returns></returns>
        public async Task<StorageFolder> GetStorageFolder(UrlType urlType)
        {
            string folderToken;
            StorageFolder folder;
            // 根据图片Uri类型，打开到不同文件夹里
            if (urlType == UrlType.PreviewUrl)
            {
                folderToken = ServiceLocator.Current.GetInstance<ViewModels.UserConfigVM>().Config.CacheFolderToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync(MoePictureConfig.CacheFolderName, CreationCollisionOption.OpenIfExists);
            }
            else
            {
                folderToken = ServiceLocator.Current.GetInstance<ViewModels.UserConfigVM>().Config.SaveFolderlToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync(MoePictureConfig.SampleFolderName, CreationCollisionOption.OpenIfExists);
            }
            // save to each sub folder
            folder = await folder.CreateFolderAsync(Type.ToString(), CreationCollisionOption.OpenIfExists);
            return folder;
        }


        /// <summary>
        /// ImageSource属性用于绑定到列表的Image控件上
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                if (bitmapImage != null && bitmapImage.IsAlive)
                {
                    // 如果弱引用没有没回收，则取弱引用的值
                    return (ImageSource)bitmapImage.Target;
                }
                // 弱引用已经被回收那么则进行异步下载
                Uri imageUri = new Uri(UrlType == UrlType.PreviewUrl ? PreviewUrl : SampleUrl);
                // 创建后台线程，下载图片
                Task.Factory.StartNew(async () => { await DownloadImageAsync(imageUri); });
                return null;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="uri">链接</param>
        /// <returns></returns>
        private async Task DownloadImageAsync(object uri)
        {
            var folder = await GetStorageFolder(UrlType);
            var path = Path.Combine(folder.Path, FileName);

            if (!File.Exists(path) || ((new FileInfo(path).Length) == 0))
            {
                if (UrlType == UrlType.PreviewUrl)
                {
                    // 限制同时发起下载次数
                    await Spider.DownloadPictureFromUriToFolderLock(uri as Uri, folder.Path, FileName);
                }
                else
                {
                    await Spider.DownloadPictureFromUriToFolder(uri as Uri, folder.Path, FileName);
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
                    // Let it go
                }
            });
        }

        #endregion ImageSource Properties
    }
}
