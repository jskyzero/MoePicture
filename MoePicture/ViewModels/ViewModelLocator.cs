using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MoePicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoePicture.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public UserConfigVM ConfigVM => ServiceLocator.Current.GetInstance<UserConfigVM>();
        public ShellVM ShellVM => ServiceLocator.Current.GetInstance<ShellVM>();
        public PictureItemsVM PicturesVM => ServiceLocator.Current.GetInstance<PictureItemsVM>();

        /// <summary>
        /// 初始化ServiceLocator，注册ViewModels
        /// </summary>
        /// 为什么是 static
        /// 静态构造函数用于初始化任何静态数据，或执行仅需要执行一次的特定操作。
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<Services.ConfigService>();
            SimpleIoc.Default.Register<UserConfigVM>();
            SimpleIoc.Default.Register<PictureItemsVM>();
            SimpleIoc.Default.Register<ShellVM>();

            JskyUwpLibs.Tool.LogFile.InitialFilePath("LOG.TXT");
            JskyUwpLibs.Tool.LogFile.WriteLog("ViewModelLocator Initial");

            // 旧时代导航相关服务
            // SimpleIoc.Default.Register<INavigationService>(() => nav);
            // var nav = new NavigationService();
            // nav.Configure(ShellKey, typeof(Views.Shell));
        }
    }
}
