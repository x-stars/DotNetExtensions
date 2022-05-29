# XstarS .NET 扩展库

**X**starS .**NET** **Ex**tensions => `XNetEx`

天南十字星 (XstarS) 的自用 .NET 扩展库，程序集名称和命名空间均仿照 .NET 框架。

## 框架支持

| .NET      | 版本         |
| --------- | ------------ |
| Core      | 3.1, 6.0     |
| Framework | 4.6.1, 4.7.2 |
| Standard  | 2.0, 2.1     |

> C# 语言版本 >= 9.0

## 程序集 XNetEx.Core

系统基础相关，目前包含的命名空间：

* `XNetEx`
* `XNetEx.Collections`
* `XNetEx.Collections.Generic`
* `XNetEx.Collections.ObjectModel`
* `XNetEx.Collections.Specialized`
* `XNetEx.Diagnostics`
* `XNetEx.IO`
* `XNetEx.Linq`
* `XNetEx.Reflection`
* `XNetEx.Reflection.Emit`
* `XNetEx.Runtime.CompilerServices`

> `XNetEx.Operators` 类型提供部分常用运算符，建议静态引入后调用。

相关文档：

* [对象的通用值相等比较方法](Documentation/ValueEquals.md)
* [析构元组和其他类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/deconstruct)

## 程序集 XNetEx.CommandLine

提供命令行程序相关类型和扩展方法，包括：

* 控制台方法扩展 `XNetEx.ConsoleEx`
  * 逐个读取按空白符分隔的输入 `ReadToken`
  * 逐个读取按空白符分隔的输入并转换为值 `ReadTokenAs`
  * 以指定的颜色将值写入输出流 `WriteInColor`
  * 以指定的颜色将值写入错误流 `WriteErrorInColor`
* 简易的命令行参数解析器 `XNetEx.CommandLine.ArgumentReader`，以及其他风格的实现
  * 命令提示符 CMD `XNetEx.CommandLine.Specialized.CmdArgumentReader`
  * PowerShell `XNetEx.CommandLine.Specialized.PowerShellArgumentReader`
  * Unix Shell `XNetEx.CommandLine.Specialized.UnixShellArgumentReader`

相关文档：

* [命令行参数解析器](Documentation/ArgumentReaders.md)

## 程序集 XNetEx.DispatchProxy

提供转发代理类型 `System.Reflection.DispatchProxy` 基于委托的简易实现。

## 程序集 XNetEx.DynamicProxy

提供以反射发出 `System.Reflection.Emit` 构造的动态代理类型。

方法调用包装为通用静态委托 `XNetEx.Reflection.MethodDelegate`，保持动态代理灵活性的同时避免了反射调用的低效率问题。

核心 API 类型：

* 直接代理类型 `XNetEx.Reflection.DirectProxyTypeProvider`
* 包装代理类型 `XNetEx.Reflection.WrapProxyTypeProvider`

## 程序集 XNetEx.ObjectModel

提供部分组件模型类型的实现，包括：

* 属性更改通知 `System.ComponentModel.INotifyPropertyChanged`
  * `XNetEx.ComponentModel.ObservableDataObject`
* 数据实体验证 `System.ComponentModel.INotifyDataErrorInfo`
  * `XNetEx.ComponentModel.ObservableValidDataObject`
* 命令 `System.Windows.Input.ICommand`
  * `XNetEx.Windows.Input.DelegateCommand`

此外还为枚举类型提供了特定的的视图类型，包括：

* 枚举列表视图 `XNetEx.ComponentModel.EnumListView<TEnum>`
* 枚举向量视图 `XNetEx.ComponentModel.EnumVectorView<TEnum>`
* 位域枚举向量视图 `XNetEx.ComponentModel.EnumFlagsVectorView<TEnum>`

相关文档：

* [属性更改通知接口实现框架](Documentation/ObservableObject.md)
* [属性更改通知实现方式比较](Documentation/ObservableObjectCompare.md)

## 程序集 XNetEx.ObjectStructure

提供结构化对象（数组、集合等）的结构化相等比较和结构化输出的方法。

核心 API 类型：

* 结构化相等比较 `XNetEx.Collections.Generic.StructuralEqualityComparer<T>`
* 对象结构化输出 `XNetEx.Diagnostics.StructuralRepresenter<T>`

## 程序集 XNetEx.ObjectText

提供对象与文本之间相互转换的方法，即将对象表示为文本和将文本解析为对象的方法。

核心 API 类型：

* 将对象表示为文本 `XNetEx.Diagnostics.Representer<T>`
* 将文本解析为对象 `XNetEx.Text.StringParser<T>`
  * 用户自定义扩展解析方法 `XNetEx.Text.ExtensionParseMethodAttribute`

## 程序集 XNetEx.ObservableProxy

提供以反射发出 `System.Reflection.Emit` 构造的属性更改通知类型 `System.ComponentModel.INotifyPropertyChanged`。

相关文档：

* [属性更改通知接口实现框架](Documentation/ObservableObject.md)
* [属性更改通知实现方式比较](Documentation/ObservableObjectCompare.md)
* [属性更改通知类型提供对象](Documentation/ObservableTypeProvider.md)

## 程序集 XNetEx.Primitives

提供框架原生值类型的补充类型，包括：

* 24 位有符号整数 `XNetEx.Int24`
* 24 位无符号整数 `XNetEx.UInt24`
* 半精度浮点数 `XNetEx.Half`

## 程序集 XNetEx.Unions

提供框架原生值类型的联合 `union`，包括：

* 8 位数据类型联合 `XNetEx.Unions.ByteUnion`: `Byte`, `SByte`, `Boolean`
* 16 位数据类型联合 `XNetEx.Unions.WordUnion`: `Int16`, `UInt16`, `Char`
* 32 位数据类型联合 `XNetEx.Unions.DWordUnion`: `Int32`, `UInt32`, `Single`
* 64 位数据类型联合 `XNetEx.Unions.QWordUnion`: `Int64`, `UInt64`, `Double`
* 指针或句柄类型联合 `XNetEx.Unions.HandleUnion`: `IntPtr`, `UIntPtr`, `Void*`

## 程序集 XNetEx.ValueValidate

提供连续的对象的值的验证和抛出异常的方法。

相关文档：

* [对象的值的验证](Documentation/ValueValidate.md)

## 程序集 XNetEx.Win32

Win32 相关，目前包含的命名空间：

* `XNetEx.Win32`
* `XNetEx.Win32.Profiles`

## 程序集 XNetEx.Windows

WPF 相关，目前包含的命名空间：

* `XNetEx.Windows.Controls`
* `XNetEx.Windows.Data`

XAML 命名空间：

``` XML
<Window xmlns:xnetex="http://dev.x-stars.org/dotnet/extensions"/>
```
