# XstarS .NET 扩展库

天南十字星 (XstarS) 的自用 .NET 扩展库，程序集名称和命名空间均仿照 .NET 框架。

## 框架支持

| .NET      | 版本         |
| --------- | ------------ |
| Core      | 3.1, 6.0     |
| Framework | 4.6.1, 4.7.2 |
| Standard  | 2.0, 2.1     |

> C# 语言版本 >= 9.0

## 程序集 XstarS.Core

系统基础相关，目前包含的命名空间：

* `XstarS`
* `XstarS.Collections`
* `XstarS.Collections.Generic`
* `XstarS.Collections.Specialized`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Linq`
* `XstarS.Reflection`
* `XstarS.Reflection.Emit`
* `XstarS.Runtime.CompilerServices`
* `XstarS.Text.RegularExpressions`

> `XstarS.Operators` 类型提供部分常用运算符，建议静态引入后调用。

相关文档：

* [对象的通用值相等比较方法](Documentation/ValueEquals.md)
* [析构元组和其他类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/deconstruct)

## 程序集 XstarS.CommandLine

提供命令行程序相关类型和扩展方法，包括：

* 控制台方法扩展 `XstarS.ConsoleEx`
  * 逐个读取按空白符分隔的输入 `ReadToken`
  * 逐个读取按空白符分隔的输入并转换为值 `ReadTokenAs`
  * 以指定的颜色将值写入输出流 `WriteInColor`
  * 以指定的颜色将值写入错误流 `WriteErrorInColor`
* 简易的命令行参数解析器 `XstarS.CommandLine.ArgumentReader`，以及其他风格的实现
  * 命令提示符 CMD `XstarS.CommandLine.Specialized.CmdArgumentReader`
  * PowerShell `XstarS.CommandLine.Specialized.PowerShellArgumentReader`
  * Unix Shell `XstarS.CommandLine.Specialized.UnixShellArgumentReader`

相关文档：

* [命令行参数解析器](Documentation/ArgumentReaders.md)

## 程序集 XstarS.DispatchProxy

提供转发代理类型 `System.Reflection.DispatchProxy` 基于委托的简易实现。

## 程序集 XstarS.DynamicProxy

提供以反射发出 `System.Reflection.Emit` 构造的动态代理类型。

方法调用包装为通用静态委托 `XstarS.Reflection.MethodDelegate`，保持动态代理灵活性的同时避免了反射调用的低效率问题。

核心 API 类型：

* 直接代理类型 `XstarS.Reflection.DirectProxyTypeProvider`
* 包装代理类型 `XstarS.Reflection.WrapProxyTypeProvider`

## 程序集 XstarS.ObjectModel

提供部分组件模型类型的实现，包括：

* 属性更改通知 `System.ComponentModel.INotifyPropertyChanged`
  * `XstarS.ComponentModel.ObservableDataObject`
* 数据实体验证 `System.ComponentModel.INotifyDataErrorInfo`
  * `XstarS.ComponentModel.ObservableValidDataObject`
* 命令 `System.Windows.Input.ICommand`
  * `XstarS.Windows.Input.DelegateCommand`

此外还为枚举类型提供了特定的的视图类型，包括：

* 枚举列表视图 `XstarS.ComponentModel.EnumListView<TEnum>`
* 枚举向量视图 `XstarS.ComponentModel.EnumVectorView<TEnum>`
* 位域枚举向量视图 `XstarS.ComponentModel.EnumFlagsVectorView<TEnum>`

相关文档：

* [属性更改通知接口实现框架](Documentation/ObservableObject.md)
* [属性更改通知实现方式比较](Documentation/ObservableObjectCompare.md)

## 程序集 XstarS.ObjectStructure

提供结构化对象（数组、集合等）的结构化相等比较和结构化输出的方法。

核心 API 类型：

* 结构化相等比较 `XstarS.Collections.Generic.StructuralEqualityComparer<T>`
* 对象结构化输出 `XstarS.Diagnostics.StructuralRepresenter<T>`

## 程序集 XstarS.ObjectText

提供对象与文本之间相互转换的方法，即将对象表示为文本和将文本解析为对象的方法。

核心 API 类型：

* 将对象表示为文本 `XstarS.Diagnostics.Representer<T>`
* 将文本解析为对象 `XstarS.Text.StringParser<T>`
  * 用户自定义扩展解析方法 `XstarS.Text.ExtensionParseMethodAttribute`

## 程序集 XstarS.ObservableProxy

提供以反射发出 `System.Reflection.Emit` 构造的属性更改通知类型 `System.ComponentModel.INotifyPropertyChanged`。

相关文档：

* [属性更改通知接口实现框架](Documentation/ObservableObject.md)
* [属性更改通知实现方式比较](Documentation/ObservableObjectCompare.md)
* [属性更改通知类型提供对象](Documentation/ObservableTypeProvider.md)

## 程序集 XstarS.Primitives

提供框架原生值类型的补充类型，包括：

* 24 位有符号整数 `XstarS.Int24`
* 24 位无符号整数 `XstarS.UInt24`
* 半精度浮点数 `XstarS.Half`

## 程序集 XstarS.Unions

提供框架原生值类型的联合 `union`，包括：

* 8 位数据类型联合 `XstarS.Unions.ByteUnion`: `Byte`, `SByte`, `Boolean`
* 16 位数据类型联合 `XstarS.Unions.WordUnion`: `Int16`, `UInt16`, `Char`
* 32 位数据类型联合 `XstarS.Unions.DWordUnion`: `Int32`, `UInt32`, `Single`
* 64 位数据类型联合 `XstarS.Unions.QWordUnion`: `Int64`, `UInt64`, `Double`
* 指针或句柄类型联合 `XstarS.Unions.HandleUnion`: `IntPtr`, `UIntPtr`, `Void*`

## 程序集 XstarS.ValueValidate

提供连续的对象的值的验证和抛出异常的方法。

相关文档：

* [对象的值的验证](Documentation/ValueValidate.md)

## 程序集 XstarS.Win32

Win32 相关，目前包含的命名空间：

* `XstarS.Win32`
* `XstarS.Win32.Profiles`

## 程序集 XstarS.Windows

WPF 相关，目前包含的命名空间：

* `XstarS.Windows.Controls`
* `XstarS.Windows.Data`

XAML 命名空间：

``` XML
<Window xmlns:xsext="http://dev.x-stars.org/dotnet/extensions"/>
```
