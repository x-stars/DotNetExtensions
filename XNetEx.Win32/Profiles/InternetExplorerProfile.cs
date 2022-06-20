using System;
using System.Security;
using Microsoft.Win32;

namespace XNetEx.Win32.Profiles;

/// <summary>
/// 表示网络浏览器 Internet Explorer 的相关配置。
/// </summary>
public static class InternetExplorerProfile
{
    /// <summary>
    /// 获取计算机上运行的 Internet Explorer 的版本。
    /// </summary>
    /// <returns>计算机上运行的 Internet Explorer 的版本。</returns>
    /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
    public static Version? Version
    {
        get
        {
            using var regKeyIE = Registry.LocalMachine.CreateSubKey(
                @"SOFTWARE\Microsoft\Internet Explorer");
            var versionSz = (regKeyIE.GetValue("svcVersion") ??
                regKeyIE.GetValue("Version")) as string;
            return (versionSz is null) ? null : new Version(versionSz);
        }
    }

    /// <summary>
    /// 获取计算机上运行的 Internet Explorer 的主要版本。
    /// </summary>
    /// <returns>计算机上运行的 Internet Explorer 的主要版本。</returns>
    /// <exception cref="SecurityException">程序没有足够的权限读取注册表。</exception>
    public static int? MajorVersion => InternetExplorerProfile.Version?.Major;
}
