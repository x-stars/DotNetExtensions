using System;
using System.Reflection;
using System.Windows.Controls;

namespace XNetEx.Windows.Controls
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
        /// <param name="suppresses">指示是否要禁止脚本错误提示。</param>
        /// <returns>若设定成功，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="browser"/> 为 <see langword="null"/>。</exception>
        public static bool TrySuppressScriptErrors(this WebBrowser browser, bool suppresses = true)
        {
            if (browser is null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            var axIWebBrowser2 = typeof(WebBrowser).GetProperty("AxIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(browser);
            if (axIWebBrowser2 is null) { return false; }

            try
            {
                axIWebBrowser2.GetType().InvokeMember("Silent",
                    BindingFlags.SetProperty, null, axIWebBrowser2, new object[] { suppresses });
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
