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
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        private const int LIST_SIZE = 5;
        private List<LogLineItem> LogList;
        private float[] ValueList;
        private float[] OldValueList;
        private string[] LableList;


        public LogPage()
        {
            this.InitializeComponent();
            LogList = new List<LogLineItem>();
            ValueList = new float[LIST_SIZE];
            OldValueList = new float[LIST_SIZE];
            LableList = new string[LIST_SIZE] {
                "Memory", "CPU", "Disk", "Other1", "Other2"
            };

            InitialValue();
            UpdateValue();  // 先更新次数据
            DispatcherTimerSetupAndStart();
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

        void InitialValue()
        {
            for (int i = 0; i < LIST_SIZE; i++)
            {
                OldValueList[i] = 0.0f;
                var log = new LogLineItem();
                log.Label = LableList[i];
                LogList.Add(log);
            }
        }

        void UpdateValue()
        {
            LogListView.ItemsSource = null;
            ValueList[0] = MemoryManager.AppMemoryUsage / 1_000_000;
            ValueList[1] = MemoryManager.AppMemoryUsage / 1_000_000;
            ValueList[2] = MemoryManager.AppMemoryUsage / 1_000_000;
            ValueList[3] = MemoryManager.AppMemoryUsage / 1_000_000;
            ValueList[4] = MemoryManager.AppMemoryUsage / 1_000_000;

            for (int i = 0; i < LIST_SIZE; i++)
            {
                var log = LogList[i];
                log.Value = ValueList[i].ToString();
                log.Change = (ValueList[i] - OldValueList[i]).ToString();
                LogList[i] = log;
            }
            LogListView.ItemsSource = LogList;
        }
    }

    //public struct LogLineItem
    //{
    //    public string Label;
    //    public string Value;
    //    public string Change;
    //}
}


namespace JskyUwpLibs.Assets
{

    public sealed class LogLineItem : INotifyPropertyChanged
    {
        internal string Label;
        internal string Value;
        internal string Change;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}