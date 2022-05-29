# .NET 对象的值相等比较

本文叙述了通用的值相等比较方法 `XNetEx.Runtime.CompilerServices.ObjectRuntimeValues.RecursiveEquals(System.Object, System.Object)` 的实现原理。

## 基本原理

### .NET 类型的分类

根据 .NET 框架中特殊类的继承关系，一般将 .NET 中的类型分类以下几类（标识符使用 C# 风格）。

* 引用类型 -> `System.Object`
  * 类 `class` -> `System.Object`
  * 委托 `delegate` -> `System.Delegate`
    * 多播委托 -> `System.MulticastDelegate`
  * 数组 `type[]` -> `System.Array`
* 值类型 -> `System.ValueType`
  * 结构 `struct` -> `System.ValueType`
  * 枚举 `enum` -> `System.Enum`
* 无基类
  * 接口 `interface`
  * 指针 `type*`
  * 引用传递 `ref type`

> * 所有有基类的类型都直接或间接继承 `System.Object`，但 .NET 在分配内存时对引用类型和值类型的处理方法是不同的。
> * .NET 中的所有委托都是多播委托，直接继承自 `System.MulticastDelegate`。
> * 指针类型在分配内存时类似于值类型，但指针没有基类，因此不能像值类型一样自动装箱为 `System.Object` 类型。
> * 引用传递 (C#: `ref`, VB: `ByRef`) 是一种仅能出现在函数参数和返回值处的特殊类型。其能保证对象本身及其各个引用传递有任意一处发生修改时，所有副本都会随之修改。由于引用传递的特殊性，其不能作为类型的字段。

但实际上抛开特殊类继承关系而直接分析 IL 汇编代码，可以发现 .NET 在底层实现中其实有一些更加特殊的类型。

假设基础类型名为 `type`，按照 IL 中对类型的描述方法，在底层实现中应该将 .NET 中的类型分为以下几类。

* 普通类型
  * 原生类型 `type`
    * 基元类型
    * 字符串 `string`
    * 引用对象 `object`
    * 类型特定的引用 `typedref`
    * 空返回值 `void`
  * 其他引用类型 `class type`
  * 其他值类型 `valuetype type`
* 构造类型
  * 数组 `type[]`
  * 指针 `type*`
  * 引用传递 `type&`

> * 基元类型可由 `System.Type.IsPrimitive` 属性判断。
> * `typedref` 表示一个类型特定的引用（即同时引用了类型和值），属于值类型，一般用于支持 `vararg` 可变参数列表。此类型由底层运行时实现，不能作为类型的字段，也不能用于创建任何构造类型。
> * 接口在 IL 中也被归为引用类型。
> * 泛型可以视为一种半构造类型（以模板类型和类型参数为基础构造），但因可以分类到引用类型或者值类型中，此处不再单独列出。
> * 数组能以除引用传递以外的类型为基础构造。
> * 指针仅能以非托管类型 (unmanaged type) 为基础构造，非托管类型包括：
>   * 基元类型；
>   * 指针；
>   * 仅包含非托管类型字段的值类型。
> * 存在一种特殊的指针 `void*`，表示指向任何类型的指针。
> * 引用传递能以除引用传递和 `typedref` 以外的类型为基础构造。

相比于按照特殊类继承的分类方法，按照底层实现的分类方法突出了部分类型的特殊地位，也即上称的原生类型。

现将原生类型的名称及其在 .NET 框架中的名称列于下表。

| IL 名称       | .NET 框架名称           |
| ------------- | ----------------------- |
| `bool`        | `System.Boolean`        |
| `char`        | `System.Char`           |
| `float32`     | `System.Single`         |
| `float64`     | `System.Double`         |
| `int8`        | `System.SByte`          |
| `int16`       | `System.Int16`          |
| `int32`       | `System.Int32`          |
| `int64`       | `System.Int64`          |
| `native int`  | `System.IntPtr`         |
| `native uint` | `System.UIntPtr`        |
| `object`      | `System.Object`         |
| `string`      | `System.String`         |
| `typedref`    | `System.TypedReference` |
| `uint8`       | `System.Byte`           |
| `uint16`      | `System.UInt16`         |
| `uint32`      | `System.UInt32`         |
| `uint64`      | `System.UInt64`         |
| `void`        | `System.Void`           |

> * 除 `object`、`string`、`typedref` 和 `void` 外，其余原生类型均为基元类型。
> * `object` 和 `void` 不存储值，且无法创建 `void` 类型的实例。

### 变量的值与类型的字段

某个类型的实例，在不考虑其内存地址（或称引用）的条件下，能够代表其本身的值的即为其各个实例字段（对于数组则是其中包含的元素）的值。为实现通用的值相等比较，最可行的方法即为先判断类型相同，而后递归比较各个对应的实例字段（元素）是否相等。

但由于递归需要一个结束条件，而由于存在类似于 `System.Int32` 类型中有一个 `System.Int32` 类型的名为 `m_value` 的字段的情况，会导致陷入无限递归的问题，因此需要人为设定一个结束条件。

在上一章中，我们介绍了 .NET 在底层实现中存在被特殊对待的原生类型和基元类型。所有包含字段的类型，其向下递归到底层，都应由基元类型及其构造类型的字段组成。因此，我们可以以基元类型作为递归的结束点，进行变量的值比较。

### 反射获取字段的值

关于反射获取字段的值的方法，请参看关于 .NET 反射的基础教程。本文仅探讨一种特殊情况：反射获取指针的值。

如前所述，指针类型并没有基类，因此在通过反射获取值时不能直接作为 `System.Object` 类型输出。实际上，在反射获取指针类型的字段的值时，会被包装为 `System.Reflection.Pointer` 类型输出。

而通过反射获取指针数组的元素的值则会更加复杂。首先，由于指针不能转化为 `System.Object` 类型，因此不能使用动态类型调用索引器访问指针数组的元素，也不能使用 `System.Object System.Array.GetValue(System.Int32[])` 实例方法访问指针数组的元素。

实际上，所有数组类型都会生成一套无法直接访问的公共方法（假设数组元素的类型为 `T`）：

* `T Get(System.Int32...)`
* `System.Void Set(System.Int32..., T)`
* `T& Address(System.Int32...)`

> `System.Int32...` 表示一系列索引值参数，其数量与数组的维度相等。

通过反射可调用 `T Get(System.Int32...)` 方法，获取数组指定索引处的元素的值；当数组元素为指针类型时，其返回值会被包装为 `System.Reflection.Pointer` 类型。

## 算法实现

> 以下所称的类型均为变量的实际类型而非声明类型。也即使用 `System.Object.GetType()` 实例方法获取的类型。

### 判断流程

1. 空引用
2. 类型不同
3. 基元类型
4. 字符串
5. 指针
6. 数组
7. 其它类型

> * 如前所述，引用传递不能作为类型的字段和数组的元素，此处不再考虑。

### 基元类型比较

直接使用 `System.Boolean System.Object.Equals(System.Object, System.Object)` 静态方法判断值是否相等。

### 字符串比较

字符串 `string` 作为唯一的含值又非基元的原生类型，需要被特殊对待。

字符串内部含有一个存储长度的 `System.Int32` 类型的字段，以及一个存储第一个字符的 `System.Char` 类型的字段，显然不足以确定两字符串是否相等。此处应直接调用 `System.Boolean System.String.Equals(System.String, System.String)` 静态方法判断两字符串的值是否相等。

### 指针比较

参见前述关于反射获取指针的值的部分，反射获取的指针的值会被包装为 `System.Reflection.Pointer` 类型。

`System.Reflection.Pointer` 类型有一个 `System.Void* Unbox(System.Object)` 的静态方法，可将其中包含的指针取出，并直接使用 `==` 运算符比较是否相等。

### 数组比较

.NET 中的数组存在维度这一属性，在反射获取一维数组和多维数组的元素的值时存在一些区别。

> 多维数组指类似于 `T[,]` 的情形（此例为 `T` 类型的二维数组），而交错数组（类似于 `T[][]` 的情形）实际上可视为数组的数组，处理方法与一维数组相同。

`System.Array` 提供了 `System.Object GetValue(System.Int32)` 实例方法，可直接获取一维数组的指定索引的元素的值，而 `System.Object GetValue(System.Int32[])` 实例方法则可获取多维数组的指定索引的元素的值。

> 虽然 `System.Object GetValue(System.Int32[])` 实例方法也可用于一维数组，但效率较低。考虑到多维数组使用频率远低于一维数组，此处应将其分开处理。

在比较多维数组的值时，不能仅仅按照顺序依次比较元素的值，还应比较数组的各个维度的大小，保证多维数组的形状一致。

依次对应取出两数组中的元素后，递归比较两对象对应元素的值，只要有一组元素不相等，即可认为两数组不相等。

### 其他类型比较

对于其他类型，处理方法则较为简单：

1. 依次反射获取所有实例字段，递归比较两对象对应字段的值。只要有一组字段的值不相等，即可认为两对象不相等。
2. 获取当前类型的基类，依次反射获取基类中的所有字段，递归比较两对象对应字段的值，只要有一组字段的值不相等，即可认为两对象不相等。
3. 获取基类的基类，重复过程 2，直至没有基类。
4. 若在以上过程都没有对应的字段的值不相等，则认为两对象相等。

### 循环引用问题

为避免循环应用（即对象间的引用形成回环）导致无限递归的问题，需要检测一对对象是否已经比较，并对已经比较的对象不再向下递归。

此处采用泛型哈希集 `System.Collections.Generic.HashSet<T>`、引用相等比较 `System.Boolean System.Object.ReferenceEquals(System.Object, System.Object)` 和引用哈希函数 `System.Int32 System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(System.Object)` 实现重复比较检测。
