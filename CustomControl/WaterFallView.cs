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
    public class WaterFallView : Panel
    {/// <summary>
     /// 设定栈布局个数,最小值为1.
     /// </summary>
        public int StatckCount
        {
            get { return (int)GetValue(StatckCountProperty); }
            set { SetValue(StatckCountProperty, value); }
        }

        public static readonly DependencyProperty StatckCountProperty =
                DependencyProperty.Register("StatckCount", typeof(int), typeof(VirtualizingPanel), new PropertyMetadata(1, RequestArrange));

        /// <summary>
        /// 设定栈布局的间距.
        /// </summary>
        public Double StatckSpacing
        {
            get { return (Double)GetValue(StatckSpacingProperty); }
            set { SetValue(StatckSpacingProperty, value); }
        }

        public static readonly DependencyProperty StatckSpacingProperty =
                DependencyProperty.Register("StatckSpacing", typeof(Double), typeof(VirtualizingPanel), new PropertyMetadata(10, RequestArrange));
        /// <summary>
        /// 设定子元素的间距.
        /// </summary>
        public Double ItemsSpacing
        {
            get { return (Double)GetValue(ItemsSpacingProperty); }
            set { SetValue(ItemsSpacingProperty, value); }
        }

        public static readonly DependencyProperty ItemsSpacingProperty =
                DependencyProperty.Register("ItemsSpacing", typeof(Double), typeof(VirtualizingPanel), new PropertyMetadata(10, RequestArrange));

        /// <summary>
        /// 请求重新测量与布局面板
        /// </summary>
        private static void RequestArrange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vp = d as VirtualizingPanel;
            if (vp != null)
            {
                (d as VirtualizingPanel).InvalidateMeasure();
                (d as VirtualizingPanel).InvalidateArrange();
            }
        }
        /// <summary>
        /// Measure 测量过程
        /// </summary>
        /// <param name="availableSize">由父控件给出的可用空间</param>
        /// <returns>返回自己需要多少控件</returns>
        /// <summary>
        /// 测量面板需要的空间
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            var measure = base.MeasureOverride(availableSize);

            double itemFixed = 0;
            Size requestSize = Size.Empty;

            //创建一个列表记录所有 Stack 的长度
            List<Double> offsetY = new Double[StatckCount].ToList();

            //计算一个 Item 的固定边长度,纵向布局的话是宽固定
            itemFixed = (availableSize.Width - StatckSpacing * (StatckCount - 1)) / StatckCount;

            requestSize = new Size()
            {
                //设定需要的空间的宽,一般是提供多少要多少
                Width = availableSize.Width
            };

            //遍历 Children 来测量长度
            foreach (var item in this.Children)
            {
                //寻找最短的 Stack ,将新的 Item 分配到这个 Stack
                int minIndex = offsetY.IndexOf(offsetY.Min());
                //向 Item 发送测量请求,让 Item 测量自己需要的空间
                item.Measure(new Size(itemFixed, double.PositiveInfinity));
                //测量结果保存在 DesiredSize 属性里面
                var itemRequestSize = item.DesiredSize;
                //将这个 Stack 的长度加上新的 Item 的长度和 Item 的间隙
                var newHeight = itemRequestSize.Height * itemFixed / itemRequestSize.Width + ItemsSpacing;
                offsetY[minIndex] += newHeight;
            }
            //寻找最长的 Stack,这个 Stack 就是面板需要的高度
            requestSize.Height = offsetY.Max();

            //返回我们面板需要的大小
            return requestSize;
        }

        /// <summary>
        /// Arrange布局过程
        /// </summary>
        /// <param name="finalSize">最终分配到的空间大小</param>
        /// <returns>并没有什么特殊含义,一半是原封不动直接返回参数</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            //建立两个列表储存 Item 的X坐标和Y坐标
            List<Double> offsetX = new List<Double>();
            List<Double> offsetY = new List<Double>();

            //最短栈默认为第一个
            int minIndex = 0;
            double itemFixed = 0;
            //纵向布局

            //初始化坐标,由于是纵向布局,纵坐标是从0开始,横坐标则是固定值
            for (int i = 0; i < StatckCount; i++)
            {
                double index = i * (this.DesiredSize.Width + StatckSpacing) / StatckCount;
                offsetX.Add(index);
                offsetY.Add(0);
            }
            itemFixed = (finalSize.Width - StatckSpacing * (StatckCount - 1)) / StatckCount;

            //遍历 Children 进行布局
            foreach (var item in this.Children)
            {
                //取最短的 Stack 加入新的 Item
                double min = offsetY.Min();
                //获取最短的 Stack 的编号
                minIndex = offsetY.IndexOf(min);

                //对 item 进行布局
                var newHeight = item.DesiredSize.Height * itemFixed / item.DesiredSize.Width;
                var rect = new Rect(offsetX[minIndex], offsetY[minIndex], itemFixed, newHeight);
                item.Arrange(rect);
                //递增纵坐标
                offsetY[minIndex] += (item.DesiredSize.Height * itemFixed / item.DesiredSize.Width + ItemsSpacing);
            }

            //直接返回参数
            return finalSize;
        }
    }
}
