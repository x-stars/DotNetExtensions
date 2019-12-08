# XstarS .NET 扩展库

天南十字星 (XstarS) 的自用 .NET 扩展库，程序集名称和命名空间均仿照 .NET 框架库。

## 程序集 XstarS

系统基础相关，目前包含的命名空间：

* `XstarS`
* `XstarS.Collections.Specialized`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Reflection`

## 程序集 XstarS.ArgumentReaders

提供简易的命令行参数解析器，以及 CMD, PowerShell, Unix Shell 等多种风格的实现。

特征和使用方法简述于[命令行参数解析器说明文档](Documentations/ArgumentReaders.md)中。

## 程序集 XstarS.Collections

集合类型相关，目前包含的命名空间：

* `XstarS.Collections`
* `XstarS.Collections.Generic`

## 程序集 XstarS.Deconstructable

提供可析构类型 `XstarS.IDeconstructable` 及相关的扩展方法。

可析构类型可使用值元组 `System.ValueTuple` 表达式将实例按成员析构为多个变量，
详情可参考微软提供的文档：[析构元组和其他类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/deconstruct)。

## 程序集 XstarS.DispatchProxy

提供转发代理类型 `System.Reflection.DispatchProxy` 基于委托的简易实现。

## 程序集 XstarS.DynamicProxy

提供以反射发出 `System.Reflection.Emit` 构造的动态代理类型。

方法调用包装为通用静态委托 `XstarS.Reflection.MethodDelegate`，保持动态代理灵活性的同时避免了反射调用的低效率问题。

## 程序集 XstarS.ObjectModel

提供部分组件模型的实现类型，包括属性更改通知和命令等。

目前提供的属性更改通知的实现方案：方法提取、值封装、动态生成属性更改通知派生类。

原理和使用方法详述于[属性更改通知接口实现框架说明文档](Documentations/ObservableValue.md)中。
使用此框架的优势叙述于[属性更改通知实现方式比较文档](Documentations/ObservableValueCompare.md)中。

## 程序集 XstarS.Unions

提供框架原生值类型的联合 (`union`)，使用自定义字段布局的结构 (`struct`) 实现。

## 程序集 XstarS.ValueValidate

提供连续的对象的值的验证和抛出异常的方法。

原理和使用方法详述于[对象的值的验证说明文档](Documentations/ValueValidate.md)中。

## 程序集 XstarS.Win32

Win32 相关，目前包含的命名空间：

* `XstarS.Win32`

## 程序集 XstarS.Windows

WPF 相关，目前包含的命名空间：

* `XstarS.Windows.Controls`
* `XstarS.Windows.Media`

XAML 命名空间：

``` XML
xmlns:xswpf="https://github.com/x-stars/xswpf"
```

## 功能设计文档

本工程基础说明文档均置于工程根目录的 [Documentations](Documentations) 文件夹下。

除基础说明文档外，目前还包含部分功能设计文档可供参考：

* [属性更改通知类型提供对象设计文档](Documentations/ObservableTypeProvider.md)
* [对象的通用值相等比较方法设计文档](Documentations/ValueEquals.md)
