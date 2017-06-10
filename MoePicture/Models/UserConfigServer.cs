using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MoePicture.Models
{
    public class UserConfigServer
    {
        /// <summary>
        /// 获取之前的设置
        /// </summary>
        public Task<UserConfig> GetConfig()
        {
            UserConfig Config;
            // 如果之前有json文件储存记录，读取json文件并反序列化，否则新建一个默认的实例
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Config"))
            {
                var JsonStr = ApplicationData.Current.RoamingSettings.Values["Config"].ToString();
                Config = JsonConvert.DeserializeObject<UserConfig>(JsonStr);
            }
            else
            {
                Config = GetDefaultConfig();
            }

            return Task.FromResult(Config);
        }

        /// <summary>
        /// 序列化成json格式的数据并保存
        /// </summary>
        public void SaveConfig(UserConfig Config)
        {
            ApplicationData.Current.RoamingSettings.Values["Config"] = JsonConvert.SerializeObject(Config);
        }

        /// <summary>
        /// 获取默认的设置
        /// </summary>
        public static UserConfig GetDefaultConfig()
        {
            return new UserConfig()
            {
                // 默认属性为safe
                Raing = UserConfig.rating.safe,
                IsPaneOpen = true
            };
        }
    }
}
