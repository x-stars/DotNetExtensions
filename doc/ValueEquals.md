# .NET 对象的值相等比较

本文叙述了通用的值相等比较方法 `System.Boolean XstarS.ObjectExtensions.ValueEquals(System.Object, System.Object)` 的实现原理。

## 基本原理

### .NET 类型的分类

根据 .NET 框架中特殊类的继承关系，一般将 .NET 中的类型分类以下几类。

* 引用类型 -> `System.Object`
  * 类 -> `System.Object`
  * 数组 -> `System.Array`
  * 委托 -> `System.Delegate`
    * 多播委托 -> `System.MulticastDelegate`
* 值类型 -> `System.ValueType`
  * 结构 -> `System.ValueType`
  * 枚举 -> `System.Enum`
* 无基类
  * 接口
  * 指针
  * 引用传递

> * 所有类型都直接或间接继承 `System.Object`，但 .NET 在分配内存时对引用类型和值类型的处理方法是不同的。
> * .NET 中的所有委托都是多播委托，直接继承自 `System.MulticastDelegate`。
> * 指针类型在分配内存时类似于值类型，但指针没有基类，因此不能像值类型一样自动装箱为 `System.Object` 类型。
> * 引用传递 (C#: `ref`, VB: `ByRef`) 是一种仅能出现在函数参数和返回值处的特殊类型。其能保证对象本身及其各个引用传递有任意一处发生修改时，所有副本都会随之修改。由于引用传递的特殊性，其不能作为类型的字段。

但实际上抛开 .NET 框架而直接分析 IL 汇编代码，可以发现 .NET 在底层实现中其实有一些更加特殊的类型。

按照 IL 中对类型的描述方法，在底层实现中应该将 .NET 中的类型分为以下几类。

* 普通类型 (假设类型名为 `type`)
  * 原生类型 (`type`)
  * 引用类型 (`class type`)
  * 值类型 (`valuetype type`)
* 构造类型 (假设基础类型名为 `base`)
  * 数组 (`base[]`)
  * 指针 (`base*`)
  * 引用传递 (`base&`)

> * 接口在 IL 中也被划归引用类型。
> * 泛型可以视为一种半构造类型（以模板类型和类型参数为基础构造），但因可以分类到引用类型或者值类型中，此处不再单独列出。
> * 数组能以除引用传递以外的类型为基础构造。
> * 指针仅能以以下几种类型为基础构造：
>   1. 除 `object` 和 `string` 以外的原生类型（见后表）；
>   2. 仅含有 1、2 中类型字段的非泛型值类型；
>   3. 指针。
> * 引用传递能以除引用传递以外的类型为基础构造。

相比于按照特殊类继承的分类方法，按照底层实现的分类方法突出了部分类型的特殊地位，也即上称的原生类型。

现将原生类型的名称及其在 .NET 框架中的名称罗列于下。

| IL 名称       | .NET 框架名称    |
| ------------- | ---------------- |
| `bool`        | `System.Boolean` |
| `int8`        | `System.SByte`   |
| `uint8`       | `System.Byte`    |
| `int16`       | `System.Int16`   |
| `uint16`      | `System.UInt16`  |
| `int32`       | `System.Int32`   |
| `uint32`      | `System.UInt32`  |
| `int64`       | `System.Int64`   |
| `uint64`      | `System.UInt64`  |
| `float32`     | `System.Single`  |
| `float64`     | `System.Double`  |
| `native int`  | `System.IntPtr`  |
| `native uint` | `System.UIntPtr` |
| `string`      | `System.String`  |
| `object`      | `System.Object`  |

> 除 `object` 以外，其余原生类型均用于存储值。

### 变量的值与类型的字段

某个类型的实例，在不考虑其内存地址（或称引用）的条件下，能够代表其本身的值的即为其各个实例字段（对于数组则是其中包含的元素）的值（包括在基类中定义的实例字段）。为实现通用的值相等比较，最可行的方法即为先判断类型相同，而后递归比较各个对应的实例字段（元素）是否相等。

但由于递归需要一个结束条件，而由于存在类似于 `System.Int32` 类型中有一个 `System.Int32` 类型的名为 `m_value` 的字段的情况，会导致陷入无限递归的问题，因此需要人为设定一个结束条件。

在上一章中，我们介绍了 .NET 在底层实现中存在被特殊对待的原生类型。所有包含字段的类型，其向下递归到底层，都应由原生类型及其构造类型的字段组成。因此，我们可以以原生类型及其构造类型作为递归的结束点，进行变量的值比较。

我们在 `XstarS.TypeExtension` 中创建了一个 `NativeTypes` 的属性，其中包含了所有原生类型的 `System.Type` 对象，以此作为通用的值相等比较算法的递归结束条件。

### 反射获取字段的值

关于反射获取字段的值的方法，请参看关于 .NET 反射的基础教程。本文仅探讨一种特殊情况：反射获取指针的值。

如前所述，指针类型并没有基类，因此在通过反射获取值时不能直接作为 `System.Object` 类型输出。实际上，在反射获取指针类型的字段的值时，其先会包装为 `System.Reflection.Pointer` 类型，再转换为 `System.Object` 类型输出。

而通过反射获取指针数组的元素的值则会更加复杂。首先，由于指针不能转化为 `System.Object` 类型，因此不能使用动态类型调用索引器访问指针数组的元素（动态类型方法调用的返回值也是动态类型，而动态类型是一种特殊的引用类型），也不能使用 `System.Object System.Array.GetValue(System.Int32[])` 实例方法访问指针数组的元素（此方法的返回值类型为 `System.Object`）。

实际上，所有数组类型都会生成一套无法直接访问的公共方法（假设数组元素的类型为 `T`）：

* `T Get(System.Int32...)`
* `System.Void Set(System.Int32..., T)`
* `T& Address(System.Int32...)`

> `System.Int32...` 表示一系列索引值参数，其数量与数组的维度相等。

通过反射可调用 `T Get(System.Int32...)` 方法，获取数组指定索引处的元素的值。当数组元素为指针类型时，其返回值也为指针类型，因此会被包装为 `System.Reflection.Pointer` 类型。

## 算法实现

> 以下所称的类型均为变量的实际类型而非声明类型。也即使用 `System.Object.GetType()` 实例方法获取的类型。

### 判断流程

1. 空引用
2. 类型不同
3. `System.Object`
4. 原生类型
5. 指针
6. 数组
7. 其它类型

> * 虽然 `System.Object` 类型也是原生类型，但与其它原生类型不同，其并不包含值，且其 `System.Boolean Equals(System.Object)` 方法采用引用比较，而其它原生类型均采用值比较，因此此处将 `System.Object` 类型特殊对待。
> * 如前所述，引用传递不能作为类型的字段和数组的元素，此处不再考虑。

### 原生类型比较

`System.Object` 类型虽然为原生类型，但其不包含任何值，因此直接认为相等。

对于其它的原生类型，则直接使用 `System.Boolean System.Object.Equals(System.Object, System.Object)` 静态方法判断值是否相等。

### 指针比较

参见前述关于反射获取指针的值的部分，反射获取的指针的值会被包装为 `System.Reflection.Pointer` 类型。

`System.Reflection.Pointer` 类型有一个 `System.Void* Unbox(System.Object)` 的静态方法，可将其中包含的指针取出。

指针的值可直接由 `==` 运算符比较是否相等。但考虑到 `System.Reflection.Pointer` 包装的指针的类型可能不同，因此还需要考虑指针本身的类型。

`System.Reflection.Pointer` 本身并未提供取出指针类型的公共方法，但存在一个 `System.Type GetPointerType()` 的程序集内部实例方法，可通过反射调用此方法获取指针的类型进行比较。

### 数组比较

.NET 中的数组存在维度这一属性，在反射获取一维数组和多维数组的元素的值时存在一些区别。

> 多维数组指类似于 `T[,]` 的情形（此例为 `T` 类型的二维数组）。
> 而嵌套数组（类似于 `T[][]` 的情形）实际上可视为数组的数组，处理方法与一维数组相同。

`System.Array` 提供了 `System.Object GetValue(System.Int32)` 实例方法，可直接获取一维数组的指定索引的元素的值。而 `System.Object GetValue(System.Int32[])` 实例方法则可获取多维数组的指定索引的元素的值。关于指针数组反射取值的方法则如前所述。

> 虽然 `System.Object GetValue(System.Int32[])` 实例方法也可用于一维数组，但效率较低。
> 考虑到多维数组使用频率远低于一维数组，此处应将其分开处理。
> `System.Array` 还提供了专用于二维数组和三维数组的 `GetValue` 方法，但考虑到程序设计的简洁性，此处不再单独处理。

在比较多维数组的值时，不能仅仅按照顺序依次比较元素的值，还应比较数组的各个维度的大小，保证多维数组的形状一致。

依次对应取出两数组中的元素后，即以两元素为输入参数，递归调用 `System.Boolean XstarS.ObjectExtensions.ValueEquals(System.Object, System.Object)` 方法获取元素的值相等比较的结果。只要有一组元素不相等，即可认为两数组不相等。

### 其他类型比较

对于其他类型，处理方法则较为简单：

1. 依次反射获取对应字段的值，以两字段为输入参数，递归调用 `System.Boolean XstarS.ObjectExtensions.ValueEquals(System.Object, System.Object)` 方法获取字段的值相等比较的结果。只要有一组字段的值不相等，即可认为两对象不相等。
2. 获取当前类型的基类的类型，依次反射获取基类中对应字段的值，以两字段为输入参数，递归调用 `System.Boolean XstarS.ObjectExtensions.ValueEquals(System.Object, System.Object)` 方法获取字段的值相等比较的结果。只要有一组字段的值不相等，即可认为两对象不相等。
3. 获取基类的基类的类型，重复过程 2，直至没有基类。
4. 若在以上过程都没有对应的字段的值不相等，则认为两对象相等。
