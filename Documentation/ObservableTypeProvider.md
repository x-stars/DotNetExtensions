﻿# 属性更改通知类型提供对象

本文叙述了以原型类型为基础，自动构造属性更改通知的类型的 `XNetEx.ComponentModel.ObservableTypeProvider` 的实现原理和整体思路。

## 属性更改通知与 `INotifyPropertyChanged` 接口

此接口仅包含一个委托类型为 `System.ComponentModel.PropertyChangedEventHandler` 的 `PropertyChanged` 事件。

属性更改通知的实现基于属性值发生更改时同时触发 `PropertyChanged` 事件，以通知客户端属性的值发生更改。

属性更改通知可用于服务端数据绑定到客户端，本文不做详细介绍，可参见数据绑定的相关说明。

### `INotifyPropertyChanged` 接口的实现

由于此接口仅包含一个 `PropertyChanged` 事件，按照标准的事件定义方法，此处应定义 `PropertyChanged` 事件及事件的触发方法 `OnPropertyChanged`。

``` CSharp
using System.ComponentModel;

public class ObservableData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.PropertyChanged?.Invoke(this, e);
    }
}
```

### `PropertyChanged` 事件的触发方法

传统上，直接在属性的 `set` 处调用 `OnPropertyChanged` 方法即可。此处定义一个以属性名称为参数的 `NotifyPropertyChanged` 方法，以更便捷地触发 `PropertyChanged` 事件。

``` CSharp
using System.ComponentModel;

public class ObservableData : INotifyPropertyChanged
{
    private string _Text;
    public string Text
    {
        get => this._Text;
        set
        {
            this._Text = value;
            this.NotifyPropertyChanged(nameof(this.Text));
        }
    }

    protected void NotifyPropertyChanged(string propertyName)
    {
        this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    // Event and On-Event method.
}
```

## 属性更改通知的自动化实现

以下是 C# 中实现属性的两种方法：

``` CSharp
public class Properties
{
    // 传统属性的字段部分。
    private object _LegacyProperty;
    // 传统属性的属性部分。
    public object LegacyProperty
    {
        get => this._LegacyProperty;
        set => this._LegacyProperty = value;
    }

    // 自动属性。
    public object AutoProperty { get; set; }
}
```

以上两种实现属性的方法完全等效，使用自动属性可以大大减少代码量，降低维护难度。但对于提供属性更改通知的属性而言，由于属性的 `set` 处的代码并不仅仅是设定字段的新值，因此无法使用自动属性实现。

针对此种情况，本文提出解决方案为：定义一个原型类型，原型类型的属性均定义为虚自动属性或抽象属性；由代码动态生成基于原型类型的属性更改通知类型，并在属性更改通知类型中实现属性的值发生更改时触发 `PropertyChanged` 事件。

### .NET 动态类型生成技术

.NET 主要有两项用于动态类型生成类型的技术：

1. 基于动态编译技术的 CodeDOM（`Microsoft.(语言).(语言)CodeProvider` 类、`System.CodeDom` 命名空间）
2. 基于 IL 指令生成技术的反射发出（`System.Reflection.Emit` 命名空间）

两项技术各自有着以下的特点：

| 特点     | CodeDOM            | 反射发出               |
| -------- | ------------------ | ---------------------- |
| 使用技术 | 动态编译           | IL 指令生成            |
| 编程语言 | .NET 编程语言      | IL 汇编指令            |
| 组件地位 | 特定语言的组件     | 系统框架组件           |
| 生成速度 | 需要编译，稍慢     | 无需编译，较快         |
| 技术难度 | 基于编程语言，较低 | 基于 IL 汇编指令，较高 |

`ObservableTypeProvider` 最终采用了反射发出技术，原因如下：

* 生成的类型基于原型类型，有大量特性需要反射获取：
  * 将这些特性转换为特定于编程语言的特性的工作量较大；
  * 框架应尽量实现语言无关性，但其他语言的部分特性在 C# 中不一定受支持。
* 需要生成的代码大量重复，且无复杂逻辑，容易使用汇编指令实现。

### 实现思路

1. 定义属性更改通知类型，将原型类型设定为属性更改通知类型的基类（若原型为类）或接口（若原型为接口）。
2. 通过反射获取原型类型的所有成员。
3. 定义构造函数，并指定其仅调用基类中的对应构造函数。
4. 若 `PropertyChanged` 事件未定义或为抽象，则以标准模式实现此事件；否则搜索 `OnPropertyChanged` 方法。
5. 使用 `OnPropertyChanged` 方法，以属性更改通知模式定义各属性，以重写原型类型中定义的属性。
6. 实现原型类型中的其他抽象成员。
7. 生成属性更改通知类型。

对于原型类型中除属性以外的抽象成员，处理方法如下：

* 索引器：实现此索引器，并设定其总是抛出 `System.NotImplementedException` 异常。
* 事件：实现此事件，但并不定义其触发方法。
* 方法：实现此方法，并设定其总是抛出 `System.NotImplementedException` 异常。

### 实现方法

可以作为原型的类型可分为接口和非密封类两类，在定义属性更改通知类型时，针对这两种原型类型的处理方法有一定区别。

| 成员     | 接口                            | 非密封类                                                    |
| -------- | ------------------------------- | ----------------------------------------------------------- |
| 基类     | 无                              | 原型类型                                                    |
| 构造函数 | 直接定义默认构造函数            | 定义与原型类型对应的构造函数                                |
| 属性     | 重写所有，并设定 `newslot` 特性 | 仅重写 `virtual` 且非 `final` 的属性，并去除 `newslot` 特性 |
| 其他成员 | 重写所有，并设定 `newslot` 特性 | 仅重写 `abstract` 的成员，并去除 `newslot` 特性             |

> * 以上特性均指 IL 汇编中的特性而非 C# 中的修饰符。
> * 接口中的所有成员均为 `abstract` 且 `virtual`，尽管在 C# 中并无此修饰符。

#### 类型定义

.NET 中动态定义的类型包含在一个动态程序集 (Assembly) 中，若生成文件，即为一个 *.dll 或 *.exe 文件。从程序集到类型，中间还包含一个称为模块的结构，一个程序集可以包含多个模块（但通常仅包含一个模块）。因此，定义动态类型要经过程序集、模块，再到类型的三个步骤。

使用 `System.Reflection.Emit.AssemblyBuilder` 的静态方法 `DefineDynamicAssembly` 定义一个动态程序集，而后继续使用 `DefineDynamicModule` 和 `DefineType` 方法即可实现动态类型的定义。

> 反射发出 `System.Reflection.Emit` 的基础教程可参考官方文档：[发出动态方法和程序集](https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/emitting-dynamic-methods-and-assemblies)。

#### 具体实现

相关类型：

* `ObservableTypeProvider`: 从原型类型构造属性更改通知类型。
* `ObservableFactory<T>`: 提供创建属性更改通知类型的实例的方法。

具体实现请参见 [XNetEx.ObjectModel.ObservableProxy](../XNetEx.ObjectModel.ObservableProxy) 工程源代码。
