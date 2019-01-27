using System;
using System.Diagnostics;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供用于程序诊断和测试的帮助方法。
    /// </summary>
    public static class DiagnosticHelper
    {
        /// <summary>
        /// 返回指定过程的运行指定次数的总时间。
        /// </summary>
        /// <param name="process">一个过程的 <see cref="Action"/> 委托。</param>
        /// <param name="time">过程运行的次数，较高时测得的结果更准确。</param>
        /// <param name="repeat">测试重复的次数，用于多次测量取平均值。</param>
        /// <returns><paramref name="process"/> 运行 <paramref name="time"/> 次的总时间。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="process"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="time"/> 小于 0。或 <paramref name="repeat"/> 小于 1。</exception>
        public static TimeSpan RunningTime(Action process, int time, int repeat = 1)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }
            if (time < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }
            if (repeat < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(repeat));
            }

            var allTime = new TimeSpan();
            var sw = new Stopwatch();
            for (int i = 0; i < repeat; i++)
            {
                sw.Reset();
                sw.Start();
                for (int j = 0; j < time; j++)
                {
                    process();
                }
                sw.Stop();
                allTime += sw.Elapsed;
            }
            return new TimeSpan(allTime.Ticks / repeat);
        }

        /// <summary>
        /// 返回指定过程的运行指定次数的总时间。
        /// </summary>
        /// <typeparam name="T">子过程的返回值的类型。</typeparam>
        /// <param name="process">一个过程的 <see cref="Func{TResult}"/> 委托。</param>
        /// <param name="time">过程运行的次数，较高时测得的结果更准确。</param>
        /// <param name="repeat">测试重复的次数，用于多次测量取平均值。</param>
        /// <returns><paramref name="process"/> 运行 <paramref name="time"/> 次的总时间。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="process"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="time"/> 小于 0。或 <paramref name="repeat"/> 小于 1。</exception>
        public static TimeSpan RunningTime<T>(Func<T> process, int time, int repeat = 1)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }
            if (time < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }
            if (repeat < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(repeat));
            }

            var allTime = new TimeSpan();
            var sw = new Stopwatch();
            for (int i = 0; i < repeat; i++)
            {
                sw.Reset();
                sw.Start();
                for (int j = 0; j < time; j++)
                {
                    process();
                }
                sw.Stop();
                allTime += sw.Elapsed;
            }
            return new TimeSpan(allTime.Ticks / repeat);
        }
    }
}
