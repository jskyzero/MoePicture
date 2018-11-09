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
using System.Collections.ObjectModel;
using Windows.System.Diagnostics;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace JskyUwpLibs
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
        internal LogLineList logLineList = new LogLineList();

        public LogPage()
        {
            this.InitializeComponent();
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
            dispatcherTimer.Tick += (s, e) => { UpdateValue(); };
            // 开始定时器
            dispatcherTimer.Start();
        }

        void UpdateValue()
        {
            Enumerable.Range(0, logLineList.Count).ToList().ForEach(new Action<int>(i => logLineList[i].Update()));
        }
    }

    internal class LogLineList : ObservableCollection<LogLineItem>
    {
        public LogLineList() : base()
        {
            Add(new LogLineItem("Memory(MB)",
                new Func<float>(() => { return MemoryManager.AppMemoryUsage / 1_000_000; })));
            Add(new LogLineItem("CPU User Time (Second)",
                new Func<float>(() => { return (float)ProcessDiagnosticInfo.GetForCurrentProcess().CpuUsage.GetReport().UserTime.TotalSeconds; })));
            Add(new LogLineItem("CPU Kernel Time (Second)",
                new Func<float>(() => { return (float)ProcessDiagnosticInfo.GetForCurrentProcess().CpuUsage.GetReport().KernelTime.TotalSeconds; })));
            Add(new LogLineItem("Disk Read", new Func<float>(() => { return ProcessDiagnosticInfo.GetForCurrentProcess().DiskUsage.GetReport().ReadOperationCount; })));
            Add(new LogLineItem("Disk Write",
                new Func<float>(() => { return ProcessDiagnosticInfo.GetForCurrentProcess().DiskUsage.GetReport().WriteOperationCount; })));
        }
    }

    internal class LogLineItem : INotifyPropertyChanged
    {
        private readonly string label;
        private float value;
        private float oldValue;
        private Func<float> caluateFucntion;

        internal string Label { get => label; }
        internal string CurrentValue { get => "Current = " + value.ToString(); }
        internal string ChangeValue { get => "Change = " + (value - oldValue).ToString(); }

        internal LogLineItem(string label, Func<float> caluateFucntion)
        {
            this.label = label;
            this.caluateFucntion = caluateFucntion;
        }

        internal void Update()
        {
            oldValue = value;
            value = caluateFucntion();
            RaisePropertyChanged("CurrentValue");
            RaisePropertyChanged("ChangeValue");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

