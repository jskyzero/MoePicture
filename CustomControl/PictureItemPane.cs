using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CustomControl
{
    public class PictureItemPane : Panel
    {
        /// <summary>
        /// Measure 测量过程
        /// </summary>
        /// <param name="availableSize">由父控件给出的可用空间</param>
        /// <returns>返回自己需要多少控件</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size request = new Size(availableSize.Width, 10);
            foreach (var item in Children)
            {
                item.Measure(availableSize);
                request.Height += 100;
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
                item.Arrange(new Rect(0, offsetY, item.DesiredSize.Width, 100));
                offsetY += item.DesiredSize.Height;
            }
            return finalSize;
        }
    }
}
