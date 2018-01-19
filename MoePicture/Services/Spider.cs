﻿using System;
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
        private static HttpClient client = null;
        private static string User_Agent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit / 537.36(KHTML, like Gecko) Chrome  47.0.2526.106 Safari / 537.36";

        public static HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                    // 对爬虫进行伪装，防止网站发现爬虫然后无法访问
                    client.DefaultRequestHeaders.Add("User-Agent", User_Agent);
                }
                return client;
            }
        }


        public async static Task<string> GetString(Uri url)
        {
            // var client = new System.Net.Http.HttpClient();
            // client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", User_Agent);
            return await Client.GetStringAsync(url);
        }

        // 通过Uri下载图片到本地文件
        public async static Task DownloadPictureFromUriToFile(Uri uri, StorageFile file)
        {
            // 获取图片流下载到文件中
            IBuffer buffer = await Client.GetBufferAsync(uri);
            await FileIO.WriteBufferAsync(file, buffer);
        }

        // 通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        public async static Task DownloadPictureFromUriToFolder(Uri uri, string path, string fileName)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await DownloadPictureFromUriToFile(uri, file);
        }

        private static SemaphoreSlim DownloadPictureLock = new SemaphoreSlim(5);


        // 通过Uri下载图片到本地文件(通过path和fileName路径得到本地文件)
        // 最多同时发送五个请求
        public async static Task DownloadPictureFromUriToFolderLock(Uri uri, string path, string fileName)
        {
            await DownloadPictureLock.WaitAsync();

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await DownloadPictureFromUriToFile(uri, file);

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
