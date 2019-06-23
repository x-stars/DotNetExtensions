# 参数验证

包含于 XstarS.ValueValidate 程序集，使用扩展方法实现连续参数验证和抛出异常。

## 泛型接口 `XstarS.IValidate<out T>`

提供参数验证所需的数据，包括参数值 `Value` 和参数名 `Name`。

## 静态类 `XstarS.Validate`

提供 `XstarS.IValidate<out T>` 接口实例的工厂方法，以及参数验证和抛出异常的方法。

参数验证过程全部通过 `XstarS.IValidate<out T>` 的扩展方法实现，以便设定各种泛型约束。
每个验证方法均包含一个名为 `message` 的可选参数，可自定义异常消息。

## 参数验证示例

``` CSharp
using XstarS;

// 存在一个 string 型的名为 param 的参数。
// 现要求其以数字开头，并且包含在键的集合 keys 中，
// 且不以 "." 结尾。

Validate.Value(param, nameof(param))    // 使用 XstarS.Validate.Value<T>(T, string) 方法，
                                        // 创建一个 XstarS.IValidate<out T> 接口的实例。

    .IsNotNull()                        // 验证参数不为 null，否则抛出 System.ArgumentNullException 异常。

    .IsInRange("0", "9", "OutOfRange")  // 要求实现 System.IComparable<in T> 接口，验证参数是否在指定范围内，
                                        // 否则抛出 System.ArgumentException 异常，并设定异常消息为 "OutOfRange"。

    .IsInKeys(keys)                     // 验证参数在键的集合中，否则抛出
                                        // System.Collections.Generic.KeyNotFoundException 异常。

    .NotEndsWith(".", "InvalidEnd");    // 验证字符串不以 "." 结尾，否则抛出 System.ArgumentException 异常，
                                        // 并设定异常消息为 "InvalidEnd"。
                                        // 此方法也可使用正则表达式版本的 IsNotMatch(".$", "InvalidEnd") 方法实现。

// ......
```
