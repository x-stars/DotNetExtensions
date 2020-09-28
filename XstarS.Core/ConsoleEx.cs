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
        /// 提供类似于 <see cref="int.Parse(string)"/> 的字符串解析方法的委托。
        /// </summary>
        /// <typeparam name="T">要解析为的数值类型。</typeparam>
        private static class ParseMethod<T>
        {
            /// <summary>
            /// 表示 <typeparamref name="T"/> 类型的字符串解析方法的委托。
            /// </summary>
            internal static readonly Converter<string, T> Delegate = ParseMethod<T>.CreateDelegate();

            /// <summary>
            /// 创建 <typeparamref name="T"/> 类型的字符串解析方法的委托。
            /// </summary>
            /// <returns><typeparamref name="T"/> 类型的字符串解析方法的委托。</returns>
            private static Converter<string, T> CreateDelegate()
            {
                var method = typeof(T).GetMethod(nameof(int.Parse), new[] { typeof(string) });
                return ((method is null) || !method.IsStatic || (method.ReturnType != typeof(T))) ? null :
                    (Converter<string, T>)method.CreateDelegate(typeof(Converter<string, T>));
            }
        }

        /// <summary>
        /// 表示所有空白字符的集合。
        /// </summary>
        private static readonly char[] WhiteSpaces = Enumerable.Range(
            char.MinValue, char.MaxValue).Select(Convert.ToChar).Where(char.IsWhiteSpace).ToArray();

        /// <summary>
        /// 将当前字符串表示形式转换为其等效的数值形式。
        /// 数值形式的类型需包含类似 <see cref="int.Parse(string)"/> 的方法。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="text">包含要转换数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 表示等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException">输入的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        public static T ParseAs<T>(this string text)
        {
            return (typeof(T).BaseType == typeof(Enum)) ? (T)Enum.Parse(typeof(T), text) :
                !(ParseMethod<T>.Delegate is null) ? ParseMethod<T>.Delegate.Invoke(text) :
                throw new InvalidCastException();
        }

        /// <summary>
        /// 从标准输入流读取下一个字符串值。
        /// </summary>
        /// <returns>输入流中的下一个字符串值。</returns>
        public static string ReadToken()
        {
            var iChar = -1;
            while ((iChar = Console.In.Read()) != -1)
            {
                if (!char.IsWhiteSpace((char)iChar)) { break; }
            }
            var token = new mstring();
            token.Append((char)iChar);
            while ((iChar = Console.In.Read()) != -1)
            {
                if (char.IsWhiteSpace((char)iChar)) { break; }
                token.Append((char)iChar);
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
        public static T ReadTokenAs<T>() => ConsoleEx.ParseAs<T>(ConsoleEx.ReadToken());

        /// <summary>
        /// 从标准输入流读取下一行的所有字符串值。
        /// </summary>
        /// <returns>输入流中的下一行包含的所有字符串值。</returns>
        public static string[] ReadLineTokens() =>
            Console.In.ReadLine().Split(ConsoleEx.WhiteSpaces, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// 从标准输入流读取下一行字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一行包含的所有字符串值的数值形式。</returns>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        public static T[] ReadLineTokensAs<T>() =>
            Array.ConvertAll(ConsoleEx.ReadLineTokens(), ConsoleEx.ParseAs<T>);

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
            Console.In.ReadToEnd().Split(ConsoleEx.WhiteSpaces, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流读取到末尾的所有字符串值的数值形式。</returns>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        public static T[] ReadTokensToEndAs<T>() =>
            Array.ConvertAll(ConsoleEx.ReadTokensToEnd(), ConsoleEx.ParseAs<T>);

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
        public static void WriteInColor(string value, ConsoleColor foreground) =>
            ConsoleEx.WriteInColor(value, foreground, Console.BackgroundColor);

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
        public static void WriteInColor(object value, ConsoleColor foreground) =>
            ConsoleEx.WriteInColor(value.ToString(), foreground);

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
        public static void WriteLineInColor(string value, ConsoleColor foreground) =>
            ConsoleEx.WriteLineInColor(value, foreground, Console.BackgroundColor);

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
        public static void WriteLineInColor(object value, ConsoleColor foreground) =>
            ConsoleEx.WriteLineInColor(value.ToString(), foreground);

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
        public static void WriteErrorInColor(string value, ConsoleColor foreground) =>
            ConsoleEx.WriteErrorInColor(value, foreground, Console.BackgroundColor);

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
        public static void WriteErrorInColor(object value, ConsoleColor foreground) =>
            ConsoleEx.WriteErrorInColor(value.ToString(), foreground);

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
        public static void WriteErrorLineInColor(string value, ConsoleColor foreground) =>
            ConsoleEx.WriteErrorLineInColor(value, foreground, Console.BackgroundColor);

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
        public static void WriteErrorLineInColor(object value, ConsoleColor foreground) =>
            ConsoleEx.WriteErrorLineInColor(value.ToString(), foreground);
    }
}
