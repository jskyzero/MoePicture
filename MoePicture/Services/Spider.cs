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
    internal class Spider
    {

        // 爬虫模拟Chrome浏览器的字符串
        public static string User_Agent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit / 537.36(KHTML, like Gecko) Chrome  47.0.2526.106 Safari / 537.36";

        public async static Task<string> GetString(Uri url)
        {
            // var client = new System.Net.Http.HttpClient();
            // client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", User_Agent);
            using (var client = new HttpClient())
            {
                // 对爬虫进行伪装，防止网站发现爬虫然后无法访问
                client.DefaultRequestHeaders.Add("User-Agent", User_Agent);
                return await client.GetStringAsync(url);
            }
        }

        // 通过Uri下载图片到本地文件
        public async static Task DownloadPictureFromUriToFile(Uri uri, StorageFile file)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", User_Agent);
                try
                {
                    // 获取图片流下载到文件中
                    IBuffer buffer = await client.GetBufferAsync(uri);
                    try
                    {
                        await FileIO.WriteBufferAsync(file, buffer);
                    }
                    catch
                    {
                        // Fail If WriteBuffer Error
                        // throw new Exception("??????");
                    }
                }
                catch
                {
                    // Fail If GetBuffer Error
                }
            }
        }

        // 通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        public async static Task DownloadPictureFromUriToFolder(Uri uri, string path, string fileName)
        {
            try
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);
                await DownloadPictureFromUriToFile(uri, file);
            }
            catch
            {
                // Fail If Exists
            }
        }

        private static SemaphoreSlim DownloadPictureLock = new SemaphoreSlim(5);

        // 通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        // 最多同时发送五个请求
        public async static Task DownloadPictureFromUriToFolderLock(Uri uri, string path, string fileName)
        {
            await DownloadPictureLock.WaitAsync();

            try
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);
                await DownloadPictureFromUriToFile(uri, file);
            }
            catch
            {
                // Fail If Exists
            }

            DownloadPictureLock.Release();
        }

        // 将fileName中不符合在文件名中使用的字符清除
        public static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        // 通过Uri得到对应死亡本地缓存的文件名
        public static string GetFileNameFromUrl(string url)
        {
            string fileName = Uri.UnescapeDataString(url);
            return Spider.CleanFileName(fileName.Substring(fileName.LastIndexOf('/') + 1));
        }
    }
}
