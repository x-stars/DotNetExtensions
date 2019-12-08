using System;
using System.Reflection;
using System.Windows.Controls;

namespace XstarS.Windows.Controls
{
    /// <summary>
    /// 提供 WPF 用户控件 <see cref="Control"/> 及其派生类的扩展方法。
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 尝试设定是否禁止当前 <see cref="WebBrowser"/> 的脚本错误提示。
        /// </summary>
        /// <param name="browser">要设定脚本错误提示的 <see cref="WebBrowser"/> 对象。</param>
        /// <param name="supresses">指示是否要禁止脚本错误提示。</param>
        /// <returns>若设定成功，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="browser"/> 为 <see langword="null"/>。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TrySuppressScriptErrors(this WebBrowser browser, bool supresses = true)
        {
            if (browser is null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            var _axIWebBrowser2 = typeof(WebBrowser).GetField("_axIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(browser);
            if (_axIWebBrowser2 is null) { return false; }

            try
            {
                _axIWebBrowser2.GetType().InvokeMember("Silent",
                    BindingFlags.SetProperty, null, _axIWebBrowser2, new object[] { supresses });
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
