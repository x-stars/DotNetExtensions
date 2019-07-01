using System;
using System.IO;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证路径是否合法。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不为一个合法路径。</exception>
        public static IValueInfo<string> IsValidPath(
            this IValueInfo<string> valueInfo,
            string message = null)
        {
            try
            {
                Path.GetFullPath(valueInfo.Value);
            }
            catch (Exception e)
            {
                message = message ?? e.Message;
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message, e);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证路径对应的文件是否存在。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="FileNotFoundException">
        /// <paramref name="valueInfo"/> 的值对应的文件不存在。</exception>
        public static IValueInfo<string> IsExistingFilePath(
            this IValueInfo<string> valueInfo,
            string message = null)
        {
            if (!File.Exists(valueInfo.Value))
            {
                ThrowHelper.ThrowFileNotFoundException(valueInfo.Value, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证路径对应的目录是否存在。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="valueInfo"/> 的值对应的目录不存在。</exception>
        public static IValueInfo<string> IsExistingDirectoryPath(
            this IValueInfo<string> valueInfo,
            string message = null)
        {
            if (!Directory.Exists(valueInfo.Value))
            {
                ThrowHelper.ThrowDirectoryNotFoundException(message);
            }

            return valueInfo;
        }
    }
}
