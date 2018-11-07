using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoePicture.CC
{
    public class PictureItem : Panel
    {


        public PictureItem()
        {
            DataContextChanged += PictureItem_DataContextChanged;
        }

        private void PictureItem_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var context = DataContext as Models.PictureItem;
            if (DataContext != null)
            {
                InvalidateMeasure();
                InvalidateArrange();
            }
        }



        /// <summary>
        /// Measure 测量过程
        /// </summary>
        /// <param name="availableSize">由父控件给出的可用空间</param>
        /// <returns>返回自己需要多少控件</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            //Size request = new Size(availableSize.Width, availableSize.Height);
            //request.Height = Height * availableSize.Width / Width;
            //return request;
            Size request = new Size(availableSize.Width, availableSize.Height);
            var context = DataContext as Models.PictureItem;
            if (context != null)
            {
                var size = context.PreviewSize;
                request.Height = size.Height * availableSize.Width / size.Width;
            }
            else
            {
                request.Height = 0;
            }

            return request;
        }

        /// <summary>
        /// Arrange布局过程
        /// </summary>
        /// <param name="finalSize">最终分配到的空间大小</param>
        /// <returns>并没有什么特殊含义,一半是原封不动直接返回参数</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double offsetY = 0;
            foreach (var item in Children)
            {
                item.Arrange(new Rect(0, offsetY, item.DesiredSize.Width, item.DesiredSize.Height));
                offsetY += item.DesiredSize.Height;
            }
            return finalSize;
        }
    }
}
