# DotNetExtensionLibrary

天南十字星 (XstarS) 的 .NET 扩展库。

C# 底层面向对象练习作品，同时也可用作自己开发时的实用库。

工程结构和命名空间均仿照 .NET 框架库。

## 程序集 XstarS

系统基础相关，目前包含的命名空间：

* `XstarS`
* `XstarS.Collections`
* `XstarS.Collections.Generic`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Reflection`

## 程序集 XstarS.ComponentModel.Binding

提供便捷的数据绑定接口实现方法，目前提供三种实现方案：方法提取、绑定值封装、动态生成数据绑定派生类。

原理和使用方法详述于[数据绑定接口实现框架说明文档](Documentations/BindingFramework.md)中。

## 程序集 XstarS.ParamReaders

提供简易的命令行参数解析器，以及 CMD, PowerShell, Unix Shell 等多种风格的实现。

原理和使用方法详述于[命令行参数解析器说明文档](Documentations/ParamReaders.md)中。

## 程序集 XstarS.Reflection.Proxy

提供以反射发出 (Emit) 实现的代理类型构造方法，保持动态代理灵活性的同时避免了反射带来的低效率问题。

目前实现了两种注入的代理代码的方法，分别以特性 (Attribute) 标注和委托 (Delegate) 调用实现，均为运行时代码注入。
其中，特性标注方法的动态性稍弱，但可读性较强，便于维护；而委托调用方法的动态性极强，自由度极高，但不便于维护。

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

## 功能设计文档

本工程基础说明文档均置于工程根目录的 [Documentations](Documentations) 文件夹下。

除基础说明文档外，目前还包含部分功能设计文档可供参考：

* [数据绑定类型构造器设计文档](Documentations/BindableBuilders.md)
* [对象的通用值相等比较方法设计文档](Documentations/ValueEquals.md)
