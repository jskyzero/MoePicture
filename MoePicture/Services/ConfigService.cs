using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using MoePicture.Models;
using Windows.Storage.AccessCache;

namespace MoePicture.Services
{
    public class ConfigService
    {
        /// <summary>
        /// 获取之前的设置
        /// </summary>
        public async Task<UserConfig> GetConfig()
        {
            UserConfig Config;
            // 如果之前有json文件储存记录，读取json文件并反序列化，否则新建一个默认的实例
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(GlobalConfig.ConfigKey))
            {
                var jsonStr = ApplicationData.Current.RoamingSettings.Values[GlobalConfig.ConfigKey].ToString();
                try
                {
                    Config = JsonConvert.DeserializeObject<UserConfig>(jsonStr);
                    if (!await TestConfigWorksWell(Config))
                    {
                        Config = GetDefaultConfig();
                    }
                }
                catch(Exception e)
                {
                    Config = GetDefaultConfig();
                }
            }
            else
            {
                Config = GetDefaultConfig();
            }
            return Config;
        }

        /// <summary>
        /// 测试配置文件是否能工作
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<bool> TestConfigWorksWell (UserConfig config)
        {
            try
            {
                string folderToken;
                StorageFolder folder;
                folderToken = config.CacheFolderToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync(GlobalConfig.SampleFolderName, CreationCollisionOption.OpenIfExists);
                folder = await folder.CreateFolderAsync(GlobalConfig.CacheFolderName, CreationCollisionOption.OpenIfExists);
                if (folder == null) return false;
                folderToken = config.SaveFolderlToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync(GlobalConfig.SampleFolderName, CreationCollisionOption.OpenIfExists);
                if (folder == null) return false;
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 序列化成json格式的数据并保存
        /// </summary>
        public void SaveConfig(UserConfig Config)
        {
            ApplicationData.Current.RoamingSettings.Values[GlobalConfig.ConfigKey] = JsonConvert.SerializeObject(Config);
        }

        /// <summary>
        /// 获取默认的设置
        /// </summary>
        public static UserConfig GetDefaultConfig()
        {

            var saveFolderlToken = StorageApplicationPermissions.FutureAccessList.Add(KnownFolders.PicturesLibrary);
            var cacheFolderToken = StorageApplicationPermissions.FutureAccessList.Add(KnownFolders.PicturesLibrary);

            //var cacheFolderToken = StorageApplicationPermissions.FutureAccessList.Add(ApplicationData.Current.LocalCacheFolder);

            return new UserConfig()
            {
                Rating = RatingType.Safe,
                PictureItemSize = 180,
                SaveFolderlToken = saveFolderlToken,
                CacheFolderToken = cacheFolderToken
            };
        }
    }
}
