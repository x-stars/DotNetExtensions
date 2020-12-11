# XstarS .NET 扩展库

天南十字星 (XstarS) 的自用 .NET 扩展库，程序集名称和命名空间均仿照 .NET 框架。

## 框架支持

| .NET      | 版本         |
| --------- | ------------ |
| Core      | 2.1, 3.1     |
| Framework | 4.6.1, 4.7.2 |
| Standard  | 2.0, 2.1     |

> C# 版本 >= 8.0

## 程序集 XstarS.Core

系统基础相关，目前包含的命名空间：

* `XstarS`
* `XstarS.Collections`
* `XstarS.Collections.Generic`
* `XstarS.Collections.Specialized`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Reflection`
* `XstarS.Runtime`

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

## 程序集 XstarS.ObjectModel

提供部分组件模型类型的实现，包括：

* 数据实体验证 `System.ComponentModel.INotifyDataErrorInfo`
* 属性更改通知 `System.ComponentModel.INotifyPropertyChanged`
* 命令 `System.Windows.Input.ICommand`

相关文档：

* [属性更改通知接口实现框架](Documentation/ObservableObject.md)
* [属性更改通知实现方式比较](Documentation/ObservableObjectCompare.md)

## 程序集 XstarS.ObjectStructure

提供结构化对象（数组、集合等）的结构化相等比较和结构化输出的方法。

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

## 程序集 XstarS.Windows

WPF 相关，目前包含的命名空间：

* `XstarS.Windows.Controls`
* `XstarS.Windows.Data`
* `XstarS.Windows.Media`

XAML 命名空间：

``` XML
<Window xmlns:xsext="http://dev.x-stars.org/dotnet/extensions"/>
```
