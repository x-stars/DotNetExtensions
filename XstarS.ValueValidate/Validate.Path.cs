using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS
{
    public static partial class Validate
    {
        /// <summary>
        /// 验证路径是否合法。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不为一个合法路径。</exception>
        public static IValidate<string> IsValidPath(this IValidate<string> source,
            string message = null)
        {
            try
            {
                Path.GetFullPath(source.Value);
            }
            catch (Exception e)
            {
                message = message ?? e.Message;
                ThrowHelper.ThrowArgumentException(source.Name, message, e);
            }

            return source;
        }

        /// <summary>
        /// 验证路径对应的文件是否存在。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="FileNotFoundException">
        /// <paramref name="source"/> 的值对应的文件不存在。</exception>
        public static IValidate<string> IsExistingFilePath(this IValidate<string> source,
            string message = null)
        {
            if (!File.Exists(source.Value))
            {
                ThrowHelper.ThrowFileNotFoundException(source.Value, message);
            }

            return source;
        }

        /// <summary>
        /// 验证路径对应的目录是否存在。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="source"/> 的值对应的目录不存在。</exception>
        public static IValidate<string> IsExistingDirectoryPath(this IValidate<string> source,
            string message = null)
        {
            if (!Directory.Exists(source.Value))
            {
                ThrowHelper.ThrowDirectoryNotFoundException(message);
            }

            return source;
        }
    }
}
