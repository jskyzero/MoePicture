using Microsoft.Practices.ServiceLocation;
using MoePicture.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public PictureGrid()
        {
            this.InitializeComponent();
            this.compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
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
            var btn = rootGrid.FindName("DownloadBtn") as Button;

            //var unsplashImage = rootGrid.DataContext as UnsplashImageBase;
            //if (unsplashImage.DownloadStatus != DownloadStatus.Pending)
            //{
            //    btn.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    btn.Visibility = Visibility.Visible;
            //}
            //if (!checkListImageDownloaded(unsplashImage))
            //{
            //    btn.Visibility = Visibility.Collapsed;
            //}

            ToggleItemPointAnimation(maskBorder, img, true);
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
            ServiceLocator.Current.GetInstance<PictureItemsViewModel>().SelectItemClick(e);
        }
    }
}
