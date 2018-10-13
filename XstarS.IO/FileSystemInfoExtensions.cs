using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XstarS.IO
{
    /// <summary>
    /// 为文件系统信息 <see cref="FileSystemInfo"/> 类及其派生类提供扩展方法的静态类。
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        /// <summary>
        /// 尝试返回指定 <see cref="DirectoryInfo"/> 中目录和文件的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
        /// </summary>
        /// <param name="source">要获取目录和文件的 <see cref="DirectoryInfo"/> 实例。</param>
        /// <param name="searchPattern">要与文件和目录的名称匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
        /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="FileSystemInfo"/> 类型的数组。</returns>
        public static FileSystemInfo[] TryGetFileSystemInfos(this DirectoryInfo source,
            string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            switch (searchOption)
            {
                case SearchOption.TopDirectoryOnly:
                    try { return source.GetFileSystemInfos(searchPattern); }
                    catch (Exception) { return new FileSystemInfo[0]; }
                case SearchOption.AllDirectories:
                    try
                    {
                        var result = source.GetFileSystemInfos(searchPattern);
                        var directoryInfos = source.GetDirectories();
                        foreach (var directoryInfo in directoryInfos)
                        {
                            try
                            {
                                result = result.Concat(
                                    directoryInfo.TryGetFileSystemInfos(
                                        searchPattern, searchOption)).ToArray();
                            }
                            catch (Exception) { }
                        }
                        return result;
                    }
                    catch (Exception) { return new FileSystemInfo[0]; }
                default:
                    return new FileSystemInfo[0];
            }
        }

        /// <summary>
        /// 尝试返回指定 <see cref="DirectoryInfo"/> 中目录的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
        /// </summary>
        /// <param name="source">要获取目录的 <see cref="DirectoryInfo"/> 实例。</param>
        /// <param name="searchPattern">要与目录名匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
        /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="DirectoryInfo"/> 类型的数组。</returns>
        public static DirectoryInfo[] TryGetDirectories(this DirectoryInfo source,
            string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            switch (searchOption)
            {
                case SearchOption.TopDirectoryOnly:
                    try { return source.GetDirectories(searchPattern); }
                    catch (Exception) { return new DirectoryInfo[0]; }
                case SearchOption.AllDirectories:
                    try
                    {
                        var result = source.GetDirectories(searchPattern);
                        var directoryInfos = source.GetDirectories();
                        foreach (var directoryInfo in directoryInfos)
                        {
                            try
                            {
                                result = result.Concat(
                                    directoryInfo.TryGetDirectories(
                                        searchPattern, searchOption)).ToArray();
                            }
                            catch (Exception) { }
                        }
                        return result;
                    }
                    catch (Exception) { return new DirectoryInfo[0]; }
                default:
                    return new DirectoryInfo[0];
            }
        }

        /// <summary>
        /// 尝试返回指定 <see cref="DirectoryInfo"/> 中文件的数组，该数组与给定的搜索条件匹配并使用某个值确定是否搜索子目录。
        /// </summary>
        /// <param name="source">要获取文件的 <see cref="DirectoryInfo"/> 实例。</param>
        /// <param name="searchPattern">要与文件名匹配的搜索字符串。 此参数可包含有效路径和通配符。</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一。</param>
        /// <returns>与 <paramref name="searchPattern"/> 匹配的 <see cref="FileInfo"/> 类型的数组。</returns>
        public static FileInfo[] TryGetFiles(this DirectoryInfo source,
            string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            switch (searchOption)
            {
                case SearchOption.TopDirectoryOnly:
                    try { return source.GetFiles(searchPattern); }
                    catch (Exception) { return new FileInfo[0]; }
                case SearchOption.AllDirectories:
                    try
                    {
                        var result = source.GetFiles(searchPattern);
                        var directoryInfos = source.GetDirectories();
                        foreach (var directoryInfo in directoryInfos)
                        {
                            try
                            {
                                result = result.Concat(
                                    directoryInfo.TryGetFiles(
                                        searchPattern, searchOption)).ToArray();
                            }
                            catch (Exception) { }
                        }
                        return result;
                    }
                    catch (Exception) { return new FileInfo[0]; }
                default:
                    return new FileInfo[0];
            }
        }
    }
}
