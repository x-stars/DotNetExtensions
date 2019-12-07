﻿using System;
using System.Windows.Markup;

// 指示程序元素是否使用公共语言规范进行编译。
[assembly: CLSCompliant(true)]

// 定义 XAML 命名空间映射。
[assembly: XmlnsDefinition("https://github.com/x-stars/xswpf", "XstarS.Windows")]
[assembly: XmlnsDefinition("https://github.com/x-stars/xswpf", "XstarS.Windows.Controls")]
[assembly: XmlnsDefinition("https://github.com/x-stars/xswpf", "XstarS.Windows.Input")]
[assembly: XmlnsDefinition("https://github.com/x-stars/xswpf", "XstarS.Windows.Media")]
[assembly: XmlnsPrefix("https://github.com/x-stars/xswpf", "xswpf")]
