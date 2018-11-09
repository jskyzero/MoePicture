using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System;
using Windows.System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JskyUwpLibs
{
    public sealed class LogWindows
    {

        private static bool hasWindows = false;

        /// <summary>
        /// 创建log窗口
        /// </summary>
        /// <param name="seconds">更新间隔（秒）</param>
        /// <returns>是否创建成功</returns>
        public static IAsyncOperation<string> CreateLogWindowsAsync(int seconds)
        {
            return CreateLogWindowsAsyncHelper(seconds).AsAsyncOperation();
        }

        private static async Task<string> CreateLogWindowsAsyncHelper(int seconds)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(LogPage), seconds);
                Window.Current.Content = frame;
                ApplicationView.GetForCurrentView().Title = "Log Windows";
                // why this will change both two windows size ...
                //ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = 600, Height = 600 });
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            hasWindows = true;

            return (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId)).ToString();
        }
    }
}
