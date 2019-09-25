# 数据绑定类型提供对象

本文叙述了以原型类型为基础，自动构造可用于数据绑定的类型的 `XstarS.ComponentModel.IBindableTypeProvider<out T>` 的实现原理和整体思路。

## 数据绑定与 `INotifyPropertyChanged` 接口

.NET 框架中，用于实现服务端和客户端的数据绑定的关键即为 `System.ComponentModel.INotifyPropertyChanged` 接口。
此接口仅包含一个委托类型为 `System.ComponentModel.PropertyChangedEventHandler` 的 `PropertyChanged` 事件。

数据绑定的实现基于属性值发生更改时同时触发 `PropertyChanged` 事件，以通知客户端属性的值发生更改。

数据绑定的具体实现原理取决于客户端的实现方法，本文不做详细介绍，可参见 WPF 的相关说明。

### `INotifyPropertyChanged` 接口的实现

由于此接口仅包含一个 `PropertyChanged` 事件，按照标准的事件定义方法，此处应定义 `PropertyChanged` 事件及事件的触发方法 `OnPropertyChanged`。

``` CSharp
using System.ComponentModel;

public class BindingData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### `PropertyChanged` 事件的触发方法

传统上，直接在属性的 `set` 处调用 `OnPropertyChanged` 方法即可。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;

public class BindingData : INotifyPropertyChanged
{
    private string text;

    public string Text
    {
        get => this.text;
        set
        {
            if (!EqualityComparer<string>.Default.Equals(this.text, value))
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));
            }
        }
    }

    // Event and On-Event method.
}
```

> 为避免频繁触发事件造成性能浪费，仅在当前值与新值不相等时触发 `PropertyChanged` 事件。

但容易发现，以上 `set` 处的代码可直接封装成一个泛型方法，我们将其命名为 `SetProperty`。

``` CSharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class BindingData : INotifyPropertyChanged
{
    // Other members.

    protected void SetProperty<T>(ref T item, T value,
        [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(item, value))
        {
            item = value;
            this.OnPropertyChanged(propertyName);
        }
    }
}
```

> `System.Runtime.CompilerServices.CallerMemberNameAttribute` 为 .NET Framework 4.5 中加入的新特性。
> 当此特性用在 `System.String` 类型的可选参数上时，编译器将会自动给此参数输入**调用此方法的成员**的短名称（对于属性则是属性的名称）。
> 详细请参见微软提供的文档：[CallerMemberNameAttribute Class](https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.compilerservices.callermembernameattribute)。

定义此方法之后，属性的定义即可简化为如下所示。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;

public class BindingData : INotifyPropertyChanged
{
    private string text;

    public string Text
    {
        get => this.text;
        set => this.SetProperty(ref this.text, value);
    }

    // Event and On-Event method.
    // SetProperty method.
}
```

> 以上方法已经被封装为 `XstarS.ComponentModel.BindableObject` 抽象类。

## 数据绑定的自动化实现

以下是 C# 中实现属性的两种方法：

``` CSharp
public class Properties
{
    // 传统属性的字段部分。
    private object legacyProperty;

    // 传统属性的属性部分。
    public object LegacyProperty
    {
        get => this.legacyProperty;
        set => this.legacyProperty = value;
    }

    // 自动属性。
    public object AutoProperty { get; set; }
}
```

以上两种实现属性的方法完全等效，使用自动属性可以大大减少代码量，降低维护难度。
但对于用于数据绑定的属性而言，由于属性的 `set` 处的代码并不仅仅是设定字段的新值，因此无法使用自动属性实现。

针对此种情况，本文提出解决方案为：
定义一个原型类型，原型类型的属性均定义为虚自动属性或抽象属性；
由代码动态生成基于原型类型的数据绑定类型，并在数据绑定类型中实现属性的值发生更改时触发 `OnPropertyChanged` 事件。

### .NET 动态类型生成技术

.NET 主要有两项用于动态类型生成类型的技术：

1. 基于动态编译技术的 CodeDOM（`Microsoft.(语言).(语言)CodeProvider` 类、`System.CodeDom` 命名空间）
2. 基于 IL 指令发射技术的 Emit（`System.Reflection.Emit` 命名空间）

关于两者各自的优缺点，已有相当数量的论述，本文不再详细比较。

简单而言，两项技术各自有着以下的特点：

| 特点     | CodeDOM                    | Emit                       |
| -------- | -------------------------- | -------------------------- |
| 使用技术 | 动态编译                   | 汇编指令发射               |
| 编程语言 | .NET 编程语言（C#、VB 等） | IL 汇编指令                |
| 组件地位 | 特定语言的组件             | 系统框架组件               |
| 生成速度 | 需要编译，稍慢             | 无需编译，较快             |
| 技术难度 | 使用编程语言实现，较低     | 需要掌握 IL 汇编指令，较高 |

`IBindableTypeProvider<out T>` 最终采用了 Emit 技术，原因如下：

* 生成的类型基于原型类型，有大量特性需要反射获取：
  * 将这些特性转换为特定于语言（C#）的特性的工作量较大；
  * 框架应尽量实现语言无关性，但其他语言的部分特性在 C# 中不一定受支持。
* 需要生成的代码大量重复，且无复杂逻辑，容易使用汇编指令实现。

### 实现思路

1. 定义数据绑定类型，将原型类型设定为数据绑定类型的基类（若原型为类）或接口（若原型为接口）。
2. 通过反射获取原型类型的所有成员。
3. 定义构造函数，并指定其仅调用基类中的对应构造函数。
4. 若 `PropertyChanged` 事件未定义或为抽象，则以标准模式实现此事件；否则搜索 `OnPropertyChanged` 方法。
5. 使用 `OnPropertyChanged` 方法，以数据绑定模式定义各属性，以重写原型类型中定义的属性。
6. 实现原型类型中的其他抽象成员。
7. 生成数据绑定类型。

对于原型类型中除属性以外的抽象成员，处理方法如下：

* 索引器：实现此索引器，并设定其总是抛出 `System.NotImplementedException` 异常。
* 事件：实现此事件，但并不定义其触发方法。
* 方法：实现此方法，并设定其总是抛出 `System.NotImplementedException` 异常。

### 实现方法

可以作为原型的类型可分为接口和非密封类两类，在定义数据绑定类型时，针对这两种原型类型的处理方法有一定区别。

| 成员     | 接口                            | 非密封类                                                    |
| -------- | ------------------------------- | ----------------------------------------------------------- |
| 基类     | 无                              | 原型类型                                                    |
| 构造函数 | 直接定义默认构造函数            | 定义与原型类型对应的构造函数                                |
| 属性     | 重写所有，并设定 `newslot` 特性 | 仅重写 `virtual` 且非 `final` 的属性，并去除 `newslot` 特性 |
| 其他成员 | 重写所有，并设定 `newslot` 特性 | 仅重写 `abstract` 的成员，并去除 `newslot` 特性             |

> * 以上特性均指 IL 汇编中的特性而非 C# 中的修饰符。
> * 接口中的所有成员均为 `abstract` 且 `virtual`，尽管在 C# 中并无此修饰符。

#### 工程结构设计

为便于与反射框架协作，此处分别定义了泛型和非泛型的实现，并将共通的代码定义为一个抽象类。
整体设计参照 `System.Collection.Generic.EqualityComparer<T>` 模式设计如下（命名空间 `XstarS.ComponentModel`）：

* `IBindableTypeProvider<out T>`
* `BindableTypeProviderBase<T>`
  * `BindableTypeProvider`
  * `BindableTypeProvider<T>`

其中：

* `IBindableTypeProvider<out T>` 作为公共接口。
* `BindableTypeProviderBase<T>` 实现 `IBindableTypeProvider<out T>`接口，提供基类实现。
* `BindableTypeProvider` 继承 `BindableTypeProviderBase<System.Object>` 类，提供非泛型实现。
* `BindableTypeProvider<T>` 继承 `BindableTypeProviderBase<T>` 类，提供泛型实现。

#### 接口设计

``` CSharp
using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    public interface IBindableTypeProvider<out T> where T : class
    {
        Type BindableType { get; }
        T CreateInstance();
        T CreateInstance(params object[] args);
    }
}
```

#### 类型定义

.NET 中动态定义的类型包含在一个动态程序集 (Assembly) 中，若生成文件，即为一个 *.dll 或 *.exe 文件。
从程序集到类型，中间还包含一个称为模块的结构，一个程序集可以包含多个模块（但通常仅包含一个模块）。
因此，定义动态类型要经过程序集、模块，再到类型的三个步骤。

使用 `System.Reflection.Emit.AssemblyBuilder` 的静态方法 `DefineDynamicAssembly` 定义一个动态程序集，
而后继续使用 `DefineDynamicModule` 和 `DefineType` 方法即可实现动态类型的定义。

> 反射发出 Emit 的基础教程可参考微软提供的文档：[发出动态方法和程序集](https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/emitting-dynamic-methods-and-assemblies)。

#### 具体设计

整个数据绑定类型构造器的具体设计请参见 [XstarS.ComponentModel.Bindable](../XstarS.ComponentModel.Bindable) 工程源代码，此处不再详述。
