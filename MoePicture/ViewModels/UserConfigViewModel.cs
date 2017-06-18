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
    public class UserConfigViewModel : ViewModelBase
    {
        private UserConfigServer _configServer;
        private UserConfig _config;

        public UserConfig Config { get => _config; set { Set(ref _config, value); } }

        public UserConfigViewModel(UserConfigServer userConfigServer)
        {
            _configServer = userConfigServer;
            InitialConfigAsync();
        }

        /// <summary>
        /// 异步初始化用户设置
        /// </summary>
        public async Task InitialConfigAsync()
        {
            Config = await _configServer.GetConfig();
        }

        /// <summary>
        /// 新加入设置的tag到Model
        /// </summary>
        /// <param name="tag">tag</param>
        private void AddTag(string tag)
        {
            if (!_config.MyTags.Contains(tag))
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
