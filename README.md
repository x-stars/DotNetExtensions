# XstarS .NET 扩展库

天南十字星 (XstarS) 的自用 .NET 扩展库，程序集名称和命名空间均仿照 .NET 框架。

## 程序集 XstarS

系统基础相关，目前包含的命名空间：

* `XstarS`
* `XstarS.Collections.Specialized`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Reflection`

相关文档：

* [对象的通用值相等比较方法](Documentations/ValueEquals.md)

## 程序集 XstarS.ArgumentReaders

提供简易的命令行参数解析器，以及 CMD, PowerShell, Unix Shell 等多种风格的实现。

相关文档：

* [命令行参数解析器](Documentations/ArgumentReaders.md)

## 程序集 XstarS.Collections

集合类型相关，目前包含的命名空间：

* `XstarS.Collections`
* `XstarS.Collections.Generic`

## 程序集 XstarS.Deconstructable

提供可析构类型 `XstarS.IDeconstructable` 及相关的扩展方法。

可析构类型可使用值元组 `System.ValueTuple` 表达式将实例按成员析构为多个变量，详情可参看官方文档：[析构元组和其他类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/deconstruct)。

## 程序集 XstarS.DispatchProxy

提供转发代理类型 `System.Reflection.DispatchProxy` 基于委托的简易实现。

## 程序集 XstarS.DynamicProxy

提供以反射发出 `System.Reflection.Emit` 构造的动态代理类型。

方法调用包装为通用静态委托 `XstarS.Reflection.MethodDelegate`，保持动态代理灵活性的同时避免了反射调用的低效率问题。

## 程序集 XstarS.ObjectModel

提供部分组件模型的实现类型，包括属性更改通知 `System.ComponentModel.INotifyPropertyChanged`
和命令 `System.Windows.Input.ICommand` 等。

相关文档：

* [属性更改通知接口实现框架](Documentations/ObservableObject.md)
* [属性更改通知实现方式比较](Documentations/ObservableObjectCompare.md)

## 程序集 XstarS.ObservableProxy

提供以反射发出 `System.Reflection.Emit` 构造的属性更改通知类型 `System.ComponentModel.INotifyPropertyChanged`。

相关文档：

* [属性更改通知接口实现框架](Documentations/ObservableObject.md)
* [属性更改通知实现方式比较](Documentations/ObservableObjectCompare.md)
* [属性更改通知类型提供对象](Documentations/ObservableTypeProvider.md)

## 程序集 XstarS.Unions

提供框架原生值类型的联合 (`union`)，使用自定义字段布局的结构 (`struct`) 实现。

## 程序集 XstarS.ValueValidate

提供连续的对象的值的验证和抛出异常的方法。

相关文档：

* [对象的值的验证](Documentations/ValueValidate.md)

## 程序集 XstarS.Win32

Win32 相关，目前包含的命名空间：

* `XstarS.Win32`

## 程序集 XstarS.Windows

WPF 相关，目前包含的命名空间：

* `XstarS.Windows.Controls`
* `XstarS.Windows.Media`

XAML 命名空间：

``` XML
<Window xmlns:xsext="https://x-stars.github.io/dotnet/extensions"/>
```
