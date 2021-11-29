using System;

namespace XstarS
{
    /// <summary>
    /// 提供时间间隔 <see cref="TimeSpan"/> 的扩展方法。
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// 返回当前 <see cref="TimeSpan"/> 对象乘以指定数值得到的对象。
        /// </summary>
        /// <param name="span">要进行乘法运算的 <see cref="TimeSpan"/> 对象。</param>
        /// <param name="factor">当前 <see cref="TimeSpan"/> 对象要乘以的倍数。</param>
        /// <returns><paramref name="span"/> 乘以 <paramref name="factor"/> 的结果。</returns>
        public static TimeSpan Multiply(this TimeSpan span, double factor) =>
            new TimeSpan((long)(span.Ticks * factor));

        /// <summary>
        /// 返回当前 <see cref="TimeSpan"/> 对象除以指定数值得到的对象。
        /// </summary>
        /// <param name="span">要进行除法运算的 <see cref="TimeSpan"/> 对象。</param>
        /// <param name="factor">当前 <see cref="TimeSpan"/> 对象要除以的倍数。</param>
        /// <returns><paramref name="span"/> 除以 <paramref name="factor"/> 的结果。</returns>
        public static TimeSpan Divide(this TimeSpan span, double factor) =>
            new TimeSpan((long)(span.Ticks / factor));

        /// <summary>
        /// 返回当前 <see cref="TimeSpan"/> 对象除以另一 <see cref="TimeSpan"/> 得到的数值。
        /// </summary>
        /// <param name="span">要进行除法运算的 <see cref="TimeSpan"/> 对象。</param>
        /// <param name="other">作为除数的另一个 <see cref="TimeSpan"/> 对象。</param>
        /// <returns><paramref name="span"/> 除以 <paramref name="other"/> 的结果。</returns>
        public static double Divide(this TimeSpan span, TimeSpan other) =>
            (double)span.Ticks / other.Ticks;
    }
}
