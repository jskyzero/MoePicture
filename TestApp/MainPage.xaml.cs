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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using System.Drawing;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace TestApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ItemCollection Children = null;

        public MainPage()
        {
            this.InitializeComponent();
            JskyUwpLibs.LogWindows.CreateLogWindowsAsync(2);
            Children = Holder.Items;
        }

        private Windows.UI.Color GetRandomColor()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[3];
            rnd.NextBytes(b);
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, b[0], b[1], b[2]);
            return color;
        }

        private UIElement CreateNewUIElement()
        {
            var rect = new Windows.UI.Xaml.Shapes.Rectangle();
            Random rnd = new Random();
            rect.Width = 50 + rnd.Next() % 100;
            rect.Height = 50 + rnd.Next() % 100;
            rect.Margin = new Thickness(1);
            //rect.Fill = new SolidColorBrush(GetRandomColor());
            var grid = new GridView();
            grid.Items.Add(rect);
            grid.Background = new SolidColorBrush(GetRandomColor());
            return grid;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Children.Add(CreateNewUIElement());
        }

        private void Insert_Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
