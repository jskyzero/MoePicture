using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.Services
{
    /// <summary>
    /// 一些方便修改的全局配置文件
    /// </summary>
    public static class GlobalConfig
    {
        #region Properties

        /// <summary> 缓存文件夹 </summary>
        public const string CacheFolderName = "Cache";
        /// <summary> 样例文件夹 </summary>
        public const string SampleFolderName = "MoePicture";
        /// <summary> 配置文件KEY </summary>
        public const string ConfigKey = "Config";
        /// <summary> 数据库文件名字 </summary>
        public const string DataBaseName = "MoePicture.db";
        /// <summary> 帮助链接 </summary>
        public const string HelpWebSiteUrl = @"https://jskyzero.github.io/MoePicture/";
        
        #endregion
    }
}
