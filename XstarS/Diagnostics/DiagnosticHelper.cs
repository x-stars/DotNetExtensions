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
        /// 返回指定子过程的运行指定次数的总时间。
        /// </summary>
        /// <param name="sub">一个子过程的 <see cref="Action"/> 委托。</param>
        /// <param name="repeat">子过程重复运行的次数，较高时测得的结果更准确。</param>
        /// <param name="times">运行时间测试运行的次数，用于多次测量取平均值。</param>
        /// <returns><paramref name="sub"/> 运行 <paramref name="repeat"/> 次的总时间。</returns>
        public static TimeSpan RunningTime(Action sub, int repeat, int times = 1)
        {
            if (sub is null)
            {
                throw new ArgumentNullException(nameof(sub));
            }
            if (repeat < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repeat));
            }
            if (times < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(times));
            }

            var allTime = new TimeSpan();
            var sw = new Stopwatch();
            for (int i = 0; i < times; i++)
            {
                sw.Reset();
                sw.Start();
                for (int j = 0; j < repeat; j++)
                {
                    sub();
                }
                sw.Stop();
                allTime += sw.Elapsed;
            }
            return new TimeSpan(allTime.Ticks / times);
        }

        /// <summary>
        /// 返回指定子过程的运行指定次数的总时间。
        /// </summary>
        /// <typeparam name="T">子过程的返回值的类型。</typeparam>
        /// <param name="sub">一个子过程的 <see cref="Func{TResult}"/> 委托。</param>
        /// <param name="repeat">子过程重复运行的次数，较高时测得的结果更准确。</param>
        /// <param name="times">运行时间测试运行的次数，用于多次测量取平均值。</param>
        /// <returns><paramref name="sub"/> 运行 <paramref name="repeat"/> 次的总时间。</returns>
        public static TimeSpan RunningTime<T>(Func<T> sub, int repeat, int times = 1)
        {
            if (sub is null)
            {
                throw new ArgumentNullException(nameof(sub));
            }
            if (repeat < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repeat));
            }
            if (times < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(times));
            }

            var allTime = new TimeSpan();
            var sw = new Stopwatch();
            for (int i = 0; i < times; i++)
            {
                sw.Reset();
                sw.Start();
                for (int j = 0; j < repeat; j++)
                {
                    sub();
                }
                sw.Stop();
                allTime += sw.Elapsed;
            }
            return new TimeSpan(allTime.Ticks / times);
        }
    }
}
