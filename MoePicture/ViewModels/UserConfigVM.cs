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
    /// <summary>
    /// 用户设置VM
    /// </summary>
    public class UserConfigVM : ViewModelBase
    {
        /// <summary> ConfigHelper </summary>
        private Services.ConfigService configHelper;
        /// <summary> 用户设置 </summary>
        private UserConfig config;

        /// <summary> 用户设置 </summary>
        public UserConfig Config { get => config; set { Set(ref config, value); } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configHelper">Services.ConfigHelper</param>
        public UserConfigVM(Services.ConfigService configHelper)
        {
            this.configHelper = configHelper;
            InitialConfigAsync();
        }

        /// <summary>
        /// 异步初始化用户设置
        /// </summary>
        public void InitialConfigAsync()
        {
            Config = configHelper.GetConfig();
        }

        /// <summary>
        /// 新加入设置的tag到Model
        /// </summary>
        /// <param name="tag">tag</param>
        //private void AddTag(string tag)
        //{
        //    if (!config.MyTags.Contains(tag))
        //        Config.MyTags.Add(tag);
        //}

        /// <summary>
        /// 清除设置的所有tags
        /// </summary>
        //public void CleanAllTag()
        //{
        //    Config.MyTags.Clear();
        //}

        //public RelayCommand CleanTagCommand { get { return new RelayCommand(() => { CleanAllTag(); }); } }

        /// <summary>
        /// 通过String来添加新的Tag
        /// </summary>
        /// <param name="str"></param>
        //public void AddTagtoMyTagsByString(string str)
        //{
        //    AddTag(str);
        //}

        //public RelayCommand<string> AddTagCommand { get { return new RelayCommand<string>((str) => { AddTagtoMyTagsByString(str); }); } }
    }
}
