using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace XstarS.Windows.Media
{
    /// <summary>
    /// 表示将 <see cref="Bitmap"/> 转换为 <see cref="BitmapSource"/> 的转换器。
    /// </summary>
    [ValueConversion(typeof(Bitmap), typeof(BitmapSource))]
    public class BitmapToBitmapSourceConverter : IValueConverter
    {
        /// <summary>
        /// 初始化 <see cref="BitmapToBitmapSourceConverter"/> 类的新实例。
        /// </summary>
        public BitmapToBitmapSourceConverter() { }

        /// <summary>
        /// 将绑定源生成的 <see cref="Bitmap"/> 转换为 <see cref="BitmapSource"/>。
        /// </summary>
        /// <param name="value">绑定源生成的值。应为 <see cref="Bitmap"/>。</param>
        /// <param name="targetType">要转换为的类型。不使用此参数。</param>
        /// <param name="parameter">要使用的转换器参数。不使用此参数。</param>
        /// <param name="culture">要用在转换器中的区域性。不使用此参数。</param>
        /// <returns>转换得到的 <see cref="BitmapSource"/>。</returns>
        public object Convert(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap bitmap)
            {
                using var memory = new MemoryStream();
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将绑定目标生成的 <see cref="BitmapSource"/> 转换为 <see cref="Bitmap"/>。
        /// </summary>
        /// <param name="value">绑定目标生成的值。应为 <see cref="BitmapSource"/>。</param>
        /// <param name="targetType">绑定源属性的类型。不使用此参数。</param>
        /// <param name="parameter">要使用的转换器参数。不使用此参数。</param>
        /// <param name="culture">要用在转换器中的区域性。不使用此参数。</param>
        /// <returns>转换得到的 <see cref="Bitmap"/>。</returns>
        public object ConvertBack(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapSource bitmapSource)
            {
                using var memory = new MemoryStream();
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memory);
                return new Bitmap(memory);
            }
            else
            {
                return null;
            }
        }
    }
}
