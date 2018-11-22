using CommonServiceLocator;
using GalaSoft.MvvmLight.Threading;
using MoePicture.CC;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MoePicture.UC
{
    public sealed partial class PictureGrid : UserControl
    {
        private Compositor compositor;
        private int itemIndex;

        public PictureGrid()
        {
            this.InitializeComponent();
            ServiceLocator.Current.GetInstance<PictureItemsVM>().ScrollRefreshEvent += Refresh;
            this.compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void Refresh()
        {
            if (DispatcherHelper.UIDispatcher != null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    scrollView.ChangeView(null, 0, null, true);
                });
            }
            else
            {
                scrollView.ChangeView(null, 0, null, true);
            }
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var rootGrid = sender as Grid;

            rootGrid.PointerEntered += RootGrid_PointerEntered;
            rootGrid.PointerExited += RootGrid_PointerExited;

            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            var maskVisual = ElementCompositionPreview.GetElementVisual(maskBorder);
            maskVisual.Opacity = 0f;
        }

        private void RootGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                return;
            }
            var rootGrid = sender as Grid;
            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            var img = rootGrid.Children[0] as FrameworkElement;

            ToggleItemPointAnimation(maskBorder, img, false);
        }

        private void RootGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                return;
            }
            var rootGrid = sender as Grid;
            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            var img = rootGrid.Children[0] as FrameworkElement;

            ToggleItemPointAnimation(maskBorder, img, true);



            var context = ((sender as Grid).DataContext as Models.PictureItem);
            itemIndex = ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems.IndexOf(context);
            if (ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems.Count - itemIndex < 20) LoadMore();
        }

        private void ToggleItemPointAnimation(FrameworkElement mask, FrameworkElement img, bool show)
        {
            var maskVisual = ElementCompositionPreview.GetElementVisual(mask);
            var imgVisual = ElementCompositionPreview.GetElementVisual(img);

            var fadeAnimation = CreateFadeAnimation(show);
            var scaleAnimation = CreateScaleAnimation(show);

            if (imgVisual.CenterPoint.X == 0 && imgVisual.CenterPoint.Y == 0)
            {
                imgVisual.CenterPoint = new Vector3((float)mask.ActualWidth / 2, (float)mask.ActualHeight / 2, 0f);
            }

            maskVisual.StartAnimation("Opacity", fadeAnimation);

            imgVisual.StartAnimation("Scale.x", scaleAnimation);
            imgVisual.StartAnimation("Scale.y", scaleAnimation);
        }

        private ScalarKeyFrameAnimation CreateScaleAnimation(bool show)
        {
            var scaleAnimation = compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, show ? 1.1f : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        private ScalarKeyFrameAnimation CreateFadeAnimation(bool show)
        {
            var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, show ? 1f : 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(500);

            return fadeAnimation;
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ServiceLocator.Current.GetInstance<PictureItemsVM>().SelectItemClick(e);
        }

        private double pointerPosition = 0;

        private void LoadMore()
        {
            if (!ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems.Busy)
            {
                ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems.LoadMoreItemsAsync(100);
            }
        }

        private void scrollView_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var verticalOffset = scrollView.VerticalOffset;
            var maxVerticalOffset = scrollView.ScrollableHeight; //sv.ExtentHeight - sv.ViewportHeight;

            if (maxVerticalOffset < 0 ||
                verticalOffset == maxVerticalOffset)
            {
                LoadMore();
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().ChangeWebsiteCommand.Execute("Yande");
            listView.ItemsSource = ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems;
        }

        private void Insert_Button_Click(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().ChangeWebsiteCommand.Execute("Konachan");
            listView.ItemsSource = ServiceLocator.Current.GetInstance<PictureItemsVM>().PictureItems;
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
        }
    }
}
