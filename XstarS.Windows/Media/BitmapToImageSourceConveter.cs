using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XstarS.Windows.Media
{
    /// <summary>
    /// 将 <see cref="Bitmap"/> 转化为 <see cref="ImageSource"/> 的转换器。
    /// </summary>
    [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
    public class BitmapToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// 初始化 <see cref="BitmapToImageSourceConverter"/> 类的新实例。
        /// </summary>
        public BitmapToImageSourceConverter() { }

        /// <summary>
        /// 将 <see cref="Bitmap"/> 类型的对象转化为 <see cref="ImageSource"/>。
        /// </summary>
        /// <param name="value">一个 <see cref="Bitmap"/> 对象。</param>
        /// <param name="targetType"><see cref="ImageSource"/> 类型的 <see cref="Type"/> 对象。</param>
        /// <param name="parameter">转换所用的参数，无关参数。</param>
        /// <param name="culture">转换所用的区域信息，无关参数。</param>
        /// <returns><paramref name="value"/> 对应的 <see cref="ImageSource"/> 对象。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            using (var memory = new MemoryStream())
            {
                (value as Bitmap).Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        /// <summary>
        /// 将 <see cref="ImageSource"/> 类型的对象转化为 <see cref="Bitmap"/>。
        /// </summary>
        /// <param name="value">一个 <see cref="ImageSource"/> 对象。</param>
        /// <param name="targetType"><see cref="Bitmap"/> 类型的 <see cref="Type"/> 对象。</param>
        /// <param name="parameter">转换所用的参数，无关参数。</param>
        /// <param name="culture">转换所用的区域信息，无关参数。</param>
        /// <returns><paramref name="value"/> 对应的 <see cref="Bitmap"/> 对象。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            using (var memory = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(value as BitmapSource));
                encoder.Save(memory);
                return new Bitmap(memory);
            }
        }
    }
}
