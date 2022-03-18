using System;
using System.Security;
using Microsoft.Win32;

namespace XstarS.Win32.Profiles
{
    /// <summary>
    /// 表示当前程序的内置网络浏览器组件的相关配置。
    /// </summary>
    public static class WebBrowserProfile
    {
        /// <summary>
        /// 获取当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本和渲染模式。
        /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
        /// </summary>
        /// <returns>当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本和渲染模式。
        /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。</returns>
        /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
        public static int? Version =>
            WebBrowserProfile.CurrentUser.Version ?? WebBrowserProfile.LocalMachine.Version;

        /// <summary>
        /// 获取当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本。
        /// <see langword="null"/> 表示运行默认主要版本 (7)。
        /// </summary>
        /// <returns>当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本。
        /// <see langword="null"/> 表示运行默认主要版本 (7)。</returns>
        /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
        public static int? MajorVersion => WebBrowserProfile.Version / 1000;

        /// <summary>
        /// 表示当前程序的内置网络浏览器组件在当前用户的相关配置。
        /// </summary>
        public static class CurrentUser
        {
            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
            /// </summary>
            /// <returns>当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 的千位的不在 7 和当前 Internet Explorer 主要版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
            public static int? Version
            {
                get
                {
                    using var regKeyBrowser = Registry.CurrentUser.CreateSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", false);
                    var appName = AppDomain.CurrentDomain.FriendlyName;
                    return regKeyBrowser.GetValue(appName) as int?;
                }

                set
                {
                    if ((value / 1000 < 7) ||
                        (value / 1000 > InternetExplorerProfile.MajorVersion))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    using var regKeyBrowser = Registry.CurrentUser.CreateSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                    var appName = AppDomain.CurrentDomain.FriendlyName;
                    if (value is null)
                    {
                        regKeyBrowser.DeleteValue(appName);
                    }
                    else
                    {
                        regKeyBrowser.SetValue(appName, value, RegistryValueKind.DWord);
                    }
                }
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。
            /// </summary>
            /// <returns>当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 不在 7 和当前 Internet Explorer 版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersion
            {
                get => WebBrowserProfile.CurrentUser.Version / 1000;
                set => WebBrowserProfile.CurrentUser.Version = value * 1000;
            }
        }

        /// <summary>
        /// 表示当前程序的内置网络浏览器组件在所有用户的相关配置。
        /// </summary>
        public static class LocalMachine
        {
            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
            /// </summary>
            /// <returns>当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 的千位的不在 7 和当前 Internet Explorer 主要版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
            public static int? Version
            {
                get
                {
                    using var regKeyBrowser = Registry.LocalMachine.CreateSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", false);
                    var appName = AppDomain.CurrentDomain.FriendlyName;
                    return regKeyBrowser.GetValue(appName) as int?;
                }

                set
                {
                    if ((value / 1000 < 7) ||
                        (value / 1000 > InternetExplorerProfile.MajorVersion))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    using var regKeyBrowser = Registry.LocalMachine.CreateSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                    var appName = AppDomain.CurrentDomain.FriendlyName;
                    if (value is null)
                    {
                        regKeyBrowser.DeleteValue(appName);
                    }
                    else
                    {
                        regKeyBrowser.SetValue(appName, value, RegistryValueKind.DWord);
                    }
                }
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。
            /// </summary>
            /// <returns>当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 不在 7 和当前 Internet Explorer 版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersion
            {
                get => WebBrowserProfile.LocalMachine.Version / 1000;
                set => WebBrowserProfile.LocalMachine.Version = value * 1000;
            }
        }
    }
}
