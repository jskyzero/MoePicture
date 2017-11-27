using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MoePicture.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;                         // 窗口内容扩展填充到标题栏

            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;           // 将标题栏的三个键背景设为透明
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;   // 失去焦点时，将三个键背景设为透明
            view.TitleBar.ButtonInactiveForegroundColor = Colors.White;         // 失去焦点时，将三个键前景色设为白色

            //ArylicMaterial.Win2D.initialBackground(BackGround);
        }
    }
}
