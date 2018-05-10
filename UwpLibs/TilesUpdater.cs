using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace JskyUwpLibs
{
    /// <summary>
    /// 用于管理磁贴，进行磁贴的更新
    /// </summary>
    public sealed class TilesUpdater
    {
        // xml的内容转成string并存储
        private string XmlData;

        public TilesUpdater()
        {
            ReadFile();
        }

        // 读取项目中Tiles.xml文件
        private async void ReadFile()
        {
            var folder = Package.Current.InstalledLocation;
            var file = await (await folder.GetFolderAsync("JskyUwpLibs")).GetFileAsync("Tiles.xml");
            XmlData = await FileIO.ReadTextAsync(file);
        }

        // 更新磁贴，向磁贴中加入一个新图片
        public async void UpdataOneItem(StorageFolder folder,  string fileName)
        {
            try
            {
                // 得到该item在本地缓存的本地Uri
                var file = await folder.GetFileAsync(fileName);

                // 进行xml内容的替换
                var doc = new Windows.Data.Xml.Dom.XmlDocument();
                var xml = string.Format(XmlData, "", "", new Uri(file.Path));
                // 磁贴更新
                var update = TileUpdateManager.CreateTileUpdaterForApplication();
                update.EnableNotificationQueue(true);

                doc.LoadXml(WebUtility.HtmlDecode(xml), new XmlLoadSettings
                {
                    ProhibitDtd = false,
                    ValidateOnParse = false,
                    ElementContentWhiteSpace = false,
                    ResolveExternals = false
                });

                // 磁贴添加新图片
                update.Update(new TileNotification(doc));
            }
            catch
            {
                // file not download then do not update
            }
        }
    }
}
