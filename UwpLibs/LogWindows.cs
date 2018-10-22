using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JskyUwpLibs
{
    public sealed class LogWindows
    {

        /// <summary>
        /// 创建log窗口
        /// </summary>
        /// <param name="seconds">更新间隔（秒）</param>
        /// <returns>是否创建成功</returns>
        public IAsyncOperation<string> CreateLogWindowsAsync(int seconds)
        {
            return CreateLogWindowsAsyncHelper(seconds).AsAsyncOperation();
        }

        private async Task<string> CreateLogWindowsAsyncHelper(int seconds)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(Assets.LogPage), seconds);
                Window.Current.Content = frame;
                ApplicationView.GetForCurrentView().Title = "Log Windows";
                // why this will change both two windows size ...
                //ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = 600, Height = 600 });
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            return (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId)).ToString();
        }
    }
}
