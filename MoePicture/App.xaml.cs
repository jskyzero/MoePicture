using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using MoePicture.Services;
using MoePicture.ViewModels;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Threading;
// using Microsoft.AppCenter;
// using Microsoft.AppCenter.Analytics;


namespace MoePicture
{
    /// <summary>
    /// 实现默认的应用程序类，实现必须的接口
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// 初始化页面的类型
        /// </summary>
        private Type initialPageType = typeof(Views.MainPage);

        /// <summary>
        /// 执行的创作代码的第一行，逻辑上等同于 main() 或 WinMain()。
        /// 初始化单一实例应用程序对象，
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            // anyway, do not collect info from user
            // AppCenter.Start("92198d92-999c-4dc1-a02e-d5c0bf73292f", typeof(Analytics));
        }

        /// <summary>
        /// 正常启动时进行调用。（包括打开特定文件等情况。）
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            // 当当前窗口不包含内容，进行初始化，
            if (rootFrame == null)
            {
                // 创建充当导航上下文的Frame
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                // 从之前挂起后被终止的应用程序加载上一次的状态
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // pass
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }
            // 确保不是预启动
            if (e.PrelaunchActivated == false)
            {
                // 检查当前Frame没有内容
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到制定页面
                    // 将所需信息作为导航参数传入来配置参数
                    rootFrame.Navigate(initialPageType, e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
            // 在UI线程初始化调度管理帮助类
            DispatcherHelper.Initialize();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。
        /// 在不知道应用程序会被终止还是会恢复，内存内容无需改变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // 获取一个暂停
            var deferral = e.SuspendingOperation.GetDeferral();
            {
                // 保存设置
                var configService = ServiceLocator.Current.GetInstance<ConfigService>();
                var userConfigVM = ServiceLocator.Current.GetInstance<UserConfigVM>();
                configService.SaveConfig(userConfigVM.Config);
            }
            deferral.Complete();
        }

        /// <summary>
        /// 通过除正常启动之外的某些方式激活应用程序时调用。
        /// <param name="e">一些参数</param>
        protected override void OnActivated(IActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 检查到当前窗口为空，则和正常启动一样先做一些工作
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(initialPageType);
            }

            // 时间线支持，从上次停止的地方继续
            if (e.Kind == ActivationKind.Protocol) // 从url协议启动
            {
                var uriArgs = e as ProtocolActivatedEventArgs;
                if (uriArgs != null && uriArgs.Uri.Host != null)
                {
                    string websiteType = uriArgs.Uri.Host;
                    // Type是首字母大写的
                    websiteType = websiteType.Substring(0, 1).ToUpper() + websiteType.Substring(1);
                    // 跳转到对应的网站
                    ServiceLocator.Current.GetInstance<PictureItemsVM>().ChangeWebsiteCommand.Execute(websiteType);
                }
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
            // 在UI线程初始化调度管理帮助类
            DispatcherHelper.Initialize();
        }
    }
}
