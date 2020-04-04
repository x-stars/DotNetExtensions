using System;
using System.Windows.Markup;

// 指示程序元素是否使用公共语言规范进行编译。
[assembly: CLSCompliant(true)]

// 定义 XAML 命名空间映射。
[assembly: XmlnsPrefix("https://x-stars.github.io/dotnet/extensions", "xsext")]
[assembly: XmlnsDefinition("https://x-stars.github.io/dotnet/extensions", "XstarS.Windows")]
[assembly: XmlnsDefinition("https://x-stars.github.io/dotnet/extensions", "XstarS.Windows.Controls")]
[assembly: XmlnsDefinition("https://x-stars.github.io/dotnet/extensions", "XstarS.Windows.Input")]
[assembly: XmlnsDefinition("https://x-stars.github.io/dotnet/extensions", "XstarS.Windows.Media")]
