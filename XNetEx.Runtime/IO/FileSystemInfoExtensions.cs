using System;
using System.IO;

namespace XNetEx.IO;

/// <summary>
/// 为文件系统信息 <see cref="FileSystemInfo"/> 类及其派生类提供扩展方法。
/// </summary>
public static class FileSystemInfoExtensions
{
    /// <summary>
    /// 尝试返回指定 <see cref="DirectoryInfo"/> 中文件的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
    /// </summary>
    /// <param name="directory">要获取文件的 <see cref="DirectoryInfo"/> 对象。</param>
    /// <param name="searchPattern">要与文件名匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
    /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
    /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="FileInfo"/> 类型的数组。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directory"/>
    /// 或 <paramref name="searchPattern"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> 不为有效的枚举值。</exception>
    /// <exception cref="ArgumentException"><paramref name="searchPattern"/> 包含无效路径字符。</exception>
    public static FileInfo[] TryGetFiles(this DirectoryInfo directory,
        string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return directory.TryGetFileSystemInfos(
            (dir, pattern) => dir.GetFiles(pattern), searchPattern, searchOption);
    }

    /// <summary>
    /// 尝试返回指定 <see cref="DirectoryInfo"/> 中目录的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
    /// </summary>
    /// <param name="directory">要获取目录的 <see cref="DirectoryInfo"/> 对象。</param>
    /// <param name="searchPattern">要与目录名匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
    /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
    /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="DirectoryInfo"/> 类型的数组。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directory"/>
    /// 或 <paramref name="searchPattern"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> 不为有效的枚举值。</exception>
    /// <exception cref="ArgumentException"><paramref name="searchPattern"/> 包含无效路径字符。</exception>
    public static DirectoryInfo[] TryGetDirectories(this DirectoryInfo directory,
        string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return directory.TryGetFileSystemInfos(
            (dir, pattern) => dir.GetDirectories(pattern), searchPattern, searchOption);
    }

    /// <summary>
    /// 尝试返回指定 <see cref="DirectoryInfo"/> 中文件和目录的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
    /// </summary>
    /// <param name="directory">要获取文件和目录的 <see cref="DirectoryInfo"/> 对象。</param>
    /// <param name="searchPattern">要与文件和目录的名称匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
    /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
    /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="FileSystemInfo"/> 类型的数组。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directory"/>
    /// 或 <paramref name="searchPattern"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> 不为有效的枚举值。</exception>
    /// <exception cref="ArgumentException"><paramref name="searchPattern"/> 包含无效路径字符。</exception>
    public static FileSystemInfo[] TryGetFileSystemInfos(this DirectoryInfo directory,
        string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return directory.TryGetFileSystemInfos(
            (dir, pattern) => dir.GetFileSystemInfos(pattern), searchPattern, searchOption);
    }

    /// <summary>
    /// 尝试返回指定 <see cref="DirectoryInfo"/> 中文件或目录的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
    /// </summary>
    /// <typeparam name="TFileSystemInfo">表示提供文件或目录的信息的类型。</typeparam>
    /// <param name="directory">要获取文件或目录的 <see cref="DirectoryInfo"/> 对象。</param>
    /// <param name="fsInfosFinder">用于获取文件或目录的 <see cref="Func{T1, T2, TResult}"/> 委托。</param>
    /// <param name="searchPattern">要与文件或目录的名称匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
    /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
    /// <returns>与 <paramref name="searchPattern"/> 匹配的 <typeparamref name="TFileSystemInfo"/> 类型的数组。</returns>
    /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> 不为有效的枚举值。</exception>
    /// <exception cref="ArgumentException"><paramref name="searchPattern"/> 包含无效路径字符。</exception>
    private static TFileSystemInfo[] TryGetFileSystemInfos<TFileSystemInfo>(
        this DirectoryInfo directory, Func<DirectoryInfo, string, TFileSystemInfo[]> fsInfosFinder,
        string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        where TFileSystemInfo : FileSystemInfo
    {
        if (directory is null)
        {
            throw new ArgumentNullException(nameof(directory));
        }
        if (fsInfosFinder is null)
        {
            throw new ArgumentNullException(nameof(fsInfosFinder));
        }
        if (searchPattern is null)
        {
            throw new ArgumentNullException(nameof(searchPattern));
        }

        switch (searchOption)
        {
            case SearchOption.TopDirectoryOnly:
                try { return fsInfosFinder.Invoke(directory, searchPattern); }
                catch (ArgumentException) { throw; }
                catch (Exception) { return Array.Empty<TFileSystemInfo>(); }
            case SearchOption.AllDirectories:
                try
                {
                    var result = fsInfosFinder.Invoke(directory, searchPattern);
                    var directoryInfos = directory.GetDirectories();
                    foreach (var directoryInfo in directoryInfos)
                    {
                        try
                        {
                            result = result.Concat(
                                directoryInfo.TryGetFileSystemInfos(
                                    fsInfosFinder, searchPattern, searchOption));
                        }
                        catch (Exception) { }
                    }
                    return result;
                }
                catch (ArgumentException) { throw; }
                catch (Exception) { return Array.Empty<TFileSystemInfo>(); }
            default:
                throw new ArgumentOutOfRangeException(nameof(searchOption));
        }
    }
}
