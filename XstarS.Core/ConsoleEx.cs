using System;
using System.Linq;
using mstring = System.Text.StringBuilder;

namespace XstarS
{
    /// <summary>
    /// 提供控制台输入输出的扩展方法。
    /// </summary>
    public static class ConsoleEx
    {
        /// <summary>
        /// 表示所有空白字符的集合。
        /// </summary>
        private static readonly char[] WhiteSpaces =
            Enumerable.Range(0, char.MaxValue).Select(Convert.ToChar).Where(char.IsWhiteSpace).ToArray();

        /// <summary>
        /// 将当前字符串表示形式转换为其等效的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="text">包含要转换数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 表示等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException">输入的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        [CLSCompliant(false)]
        public static T ConvertTo<T>(this string text) where T : IConvertible =>
            (T)Convert.ChangeType(text, typeof(T));

        /// <summary>
        /// 从标准输入流读取下一个字符串值。
        /// </summary>
        /// <returns>输入流中的下一个字符串值。</returns>
        public static string ReadToken()
        {
            var iChar = -1;
            while (true)
            {
                iChar = Console.In.Read();
                if (iChar == -1) { break; }
                if (!char.IsWhiteSpace((char)iChar)) { break; }
            }
            var token = new mstring();
            token.Append((char)iChar);
            while (true)
            {
                iChar = Console.In.Read();
                if (iChar == -1) { break; }
                token.Append((char)iChar);
                if (char.IsWhiteSpace((char)iChar)) { break; }
            }
            return token.ToString();
        }

        /// <summary>
        /// 从标准输入流读取下一个字符串值，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一个字符串值的数值形式。</returns>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        [CLSCompliant(false)]
        public static T ReadTokenAs<T>() where T : IConvertible =>
            ConsoleEx.ConvertTo<T>(ConsoleEx.ReadToken());

        /// <summary>
        /// 从标准输入流读取下一行的所有字符串值。
        /// </summary>
        /// <returns>输入流中的下一行包含的所有字符串值。</returns>
        public static string[] ReadLineTokens() =>
            Console.In.ReadLine().Split(ConsoleEx.WhiteSpaces);

        /// <summary>
        /// 从标准输入流读取下一行字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一行包含的所有字符串值的数值形式。</returns>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        [CLSCompliant(false)]
        public static T[] ReadLineTokensAs<T>() where T : IConvertible =>
            Array.ConvertAll(ConsoleEx.ReadLineTokens(), ConsoleEx.ConvertTo<T>);

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符。
        /// </summary>
        /// <returns>输入流到末尾的所有字符。</returns>
        public static string ReadToEnd() => Console.In.ReadToEnd();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符串值。
        /// </summary>
        /// <returns>输入流到末尾的所有字符串值。</returns>
        public static string[] ReadTokensToEnd() =>
            Console.In.ReadToEnd().Split(ConsoleEx.WhiteSpaces);

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流读取到末尾的所有字符串值的数值形式。</returns>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        [CLSCompliant(false)]
        public static T[] ReadTokensToEndAs<T>() where T : IConvertible =>
            Array.ConvertAll(ConsoleEx.ReadTokensToEnd(), ConsoleEx.ConvertTo<T>);

        /// <summary>
        /// 将指定的字符串值以指定的前景色和背景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Out.Write(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定的字符串值以指定的前景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteInColor(string value, ConsoleColor foreground)
        {
            ConsoleEx.WriteInColor(value, foreground, Console.BackgroundColor);
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色和背景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Out.Write(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteInColor(object value, ConsoleColor foreground)
        {
            ConsoleEx.WriteInColor(value.ToString(), foreground);
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色和背景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteLineInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Out.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteLineInColor(string value, ConsoleColor foreground)
        {
            ConsoleEx.WriteLineInColor(value, foreground, Console.BackgroundColor);
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色和背景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteLineInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Out.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色写入到标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteLineInColor(object value, ConsoleColor foreground)
        {
            ConsoleEx.WriteLineInColor(value.ToString(), foreground);
        }

        /// <summary>
        /// 将指定的字符串值写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        public static void WriteError(string value) => Console.Error.Write(value);

        /// <summary>
        /// 将指定对象的文本表示形式写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        public static void WriteError(object value) => Console.Error.Write(value);

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        public static void WriteErrorLine(string value) => Console.Error.WriteLine(value);

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        public static void WriteErrorLine(object value) => Console.Error.WriteLine(value);

        /// <summary>
        /// 将指定的字符串值以指定的前景色和背景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteErrorInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Error.Write(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定的字符串值以指定的前景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteErrorInColor(string value, ConsoleColor foreground)
        {
            ConsoleEx.WriteErrorInColor(value, foreground, Console.BackgroundColor);
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色和背景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteErrorInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Error.Write(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteErrorInColor(object value, ConsoleColor foreground)
        {
            ConsoleEx.WriteErrorInColor(value.ToString(), foreground);
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色和背景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteErrorLineInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Error.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteErrorLineInColor(string value, ConsoleColor foreground)
        {
            ConsoleEx.WriteErrorLineInColor(value, foreground, Console.BackgroundColor);
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色和背景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        public static void WriteErrorLineInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Error.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色写入到标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        public static void WriteErrorLineInColor(object value, ConsoleColor foreground)
        {
            ConsoleEx.WriteErrorLineInColor(value.ToString(), foreground);
        }
    }
}
