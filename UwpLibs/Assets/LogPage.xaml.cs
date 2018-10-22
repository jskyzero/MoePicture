using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.UI.ViewManagement;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace JskyUwpLibs.Assets
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LogPage : Page
    {

        /// <summary>
        /// 保存更新间隔
        /// </summary>
        private int? seconds = 1;


        public LogPage()
        {
            this.InitializeComponent();
            //this.Loaded += Page_Loaded;
            UpdateValue();  // 先更新次数据

            DispatcherTimerSetupAndStart();


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(900, 600));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            seconds = e.Parameter as int?;
        }

        /// <summary>
        /// 定时器初始化与开始
        /// </summary>
        void DispatcherTimerSetupAndStart()
        {
            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new DispatcherTimer();
            // 设置间隔
            dispatcherTimer.Interval = new TimeSpan(0, 0, seconds ?? 1);
            // 挂载更新函数
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            // 开始定时器
            dispatcherTimer.Start();
        }


        void dispatcherTimer_Tick(object sender, object e)
        {
            UpdateValue();
        }

        /// <summary>
        /// 更新界面数据
        /// </summary>
        void UpdateValue()
        {
            var value = MemoryManager.AppMemoryUsage / 1_000_000;
            Text.Text = value.ToString();
        }
    }

}
