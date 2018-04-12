﻿using Newtonsoft.Json;
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
    public class ConfigSaver
    {
        private const string configKey = "Config";
        /// <summary>
        /// 获取之前的设置
        /// </summary>
        public UserConfig GetConfig()
        {
            UserConfig Config;
            // 如果之前有json文件储存记录，读取json文件并反序列化，否则新建一个默认的实例
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(configKey))
            {
                var jsonStr = ApplicationData.Current.RoamingSettings.Values[configKey].ToString();
                try
                {
                    Config = JsonConvert.DeserializeObject<UserConfig>(jsonStr);
                    if (!TestConfigWorksWell(Config).Result)
                    {
                        throw new Exception("Test Error");
                    }
                }
                catch
                {
                    Config = GetDefaultConfig();
                }
            }
            else
            {
                Config = GetDefaultConfig();
            }
            //return Task.FromResult(Config);
            return Config;
        }

        public async Task<bool> TestConfigWorksWell (UserConfig config)
        {
            try
            {
                string folderToken;
                StorageFolder folder;
                folderToken = config.CacheFolderToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync("cache", CreationCollisionOption.OpenIfExists);
                folderToken = config.SaveFolderlToken;
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                folder = await folder.CreateFolderAsync("MoePicture", CreationCollisionOption.OpenIfExists);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 序列化成json格式的数据并保存
        /// </summary>
        public void SaveConfig(UserConfig Config)
        {
            ApplicationData.Current.RoamingSettings.Values[configKey] = JsonConvert.SerializeObject(Config);
        }

        /// <summary>
        /// 获取默认的设置
        /// </summary>
        public static UserConfig GetDefaultConfig()
        {

            var saveFolderlToken = StorageApplicationPermissions.FutureAccessList.Add(KnownFolders.PicturesLibrary);
            var cacheFolderToken = StorageApplicationPermissions.FutureAccessList.Add(ApplicationData.Current.LocalCacheFolder);

            return new UserConfig()
            {
                Rating = UserConfig.RatingType.safe,
                SaveFolderlToken = saveFolderlToken,
                CacheFolderToken = cacheFolderToken
            };
        }
    }
}
