using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MoePicture.Services
{
    static class Spider
    {
        ///// <summary> client </summary>
        //private static HttpClient client = null;
        /// <summary> 伪装头 </summary>
        private const string User_Agent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit / 537.36(KHTML, like Gecko) Chrome  47.0.2526.106 Safari / 537.36";

        public static HttpClient Client
        {
            get
            {
                //if (client == null)
                //{
                //    client = new HttpClient();
                //    // 对爬虫进行伪装，防止网站发现爬虫然后无法访问
                //    client.DefaultRequestHeaders.Add("User-Agent", User_Agent);
                //    JskyUwpLibs.Tool.LogFile.WriteLog("Create Client");
                //}
                //return client;

                var client = new HttpClient();
                // 对爬虫进行伪装，防止网站发现爬虫然后无法访问
                client.DefaultRequestHeaders.Add("User-Agent", User_Agent);
                JskyUwpLibs.Tool.LogFile.WriteLog("Create Client");
                return client;
            }
        }

        /// <summary>
        /// 下载成字符串
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns></returns>
        public async static Task<string> GetString(Uri url)
        {
            return await Client.GetStringAsync(url);
        }

        /// <summary>
        /// 通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        /// </summary>
        /// <param name="uri">链接</param>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public async static Task DownloadPictureFromUriToFolder(Uri uri, string path, string fileName)
        {
            JskyUwpLibs.Tool.LogFile.WriteLog("Begin " + path + fileName);

            IBuffer buffer = await Client.GetBufferAsync(uri);
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteBufferAsync(file, buffer);

            JskyUwpLibs.Tool.LogFile.WriteLog("Finish " + path + fileName);
        }

        private static SemaphoreSlim DownloadPictureLock = new SemaphoreSlim(5);

        /// <summary>
        /// 限制最大同时下载次数的通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task DownloadPictureFromUriToFolderLock(Uri uri, string path, string fileName)
        {
            await DownloadPictureLock.WaitAsync();

            JskyUwpLibs.Tool.LogFile.WriteLog("Lock Begin " + path + fileName);

            IBuffer buffer = await Client.GetBufferAsync(uri);
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteBufferAsync(file, buffer);

            JskyUwpLibs.Tool.LogFile.WriteLog("Lock Finish " + path + fileName);

            DownloadPictureLock.Release();
        }

        /// <summary>
        /// 获取安全的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSafeFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        /// <summary>
        /// 通过Uri得到对应的本地文件名
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns></returns>
        public static string GetFileNameFromUrl(string url)
        {
            string fileName = Uri.UnescapeDataString(url);
            return Spider.GetSafeFileName(fileName.Substring(fileName.LastIndexOf('/') + 1));
        }
    }
}
