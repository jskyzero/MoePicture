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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace JskyUwpLibs
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LogPage : Page
    {
        public LogPage()
        {
            this.InitializeComponent();
            DispatcherTimerSetup();
        }

        void DispatcherTimerSetup()
        {
            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        void RandomMemoryUse()
        {
            var size = (new Random((int)DateTimeOffset.Now.ToUnixTimeSeconds())).Next(1_000_000, 10_000_000);
            var array = new List<int>(size) { 0 };
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            //RandomMemoryUse();
            UpdateValue();
        }

        void UpdateValue()
        {
            var value = MemoryManager.AppMemoryUsage / 1_000_000;
            Text.Text = value.ToString();
        }
    }
}
