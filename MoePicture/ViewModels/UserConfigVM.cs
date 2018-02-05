using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.ViewModels
{
    public class UserConfigVM : ViewModelBase
    {
        private Services.ConfigSaver configService;
        private UserConfig config;

        public UserConfig Config { get => config; set { Set(ref config, value); } }

        public UserConfigVM(Services.ConfigSaver userConfigServer)
        {
            configService = userConfigServer;
            InitialConfigAsync();
        }

        /// <summary>
        /// 异步初始化用户设置
        /// </summary>
        public async Task InitialConfigAsync()
        {
            Config = await configService.GetConfig();
        }

        /// <summary>
        /// 新加入设置的tag到Model
        /// </summary>
        /// <param name="tag">tag</param>
        private void AddTag(string tag)
        {
            if (!config.MyTags.Contains(tag))
                Config.MyTags.Add(tag);
        }

        /// <summary>
        /// 清除设置的所有tags
        /// </summary>
        public void CleanAllTag()
        {
            Config.MyTags.Clear();
        }

        public RelayCommand CleanTagCommand { get { return new RelayCommand(() => { CleanAllTag(); }); } }

        /// <summary>
        /// 通过String来添加新的Tag
        /// </summary>
        /// <param name="str"></param>
        public void AddTagtoMyTagsByString(string str)
        {
            AddTag(str);
        }

        public RelayCommand<string> AddTagCommand { get { return new RelayCommand<string>((str) => { AddTagtoMyTagsByString(str); }); } }
    }
}
