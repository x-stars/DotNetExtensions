# 对象的值的验证

包含于 XNetEx.ValueValidate 程序集，使用扩展方法实现连续对象的值的验证和抛出异常。

## 泛型接口 `XNetEx.IValueInfo<out T>`

提供值的验证所需的数据，包括值 `Value` 和值的名称 `Name`。

## 静态类 `XNetEx.Validate`

提供 `XNetEx.IValueInfo<out T>` 接口实例的工厂方法，以及值的验证和抛出异常的方法。

值的验证过程全部通过 `XNetEx.IValueInfo<out T>` 的扩展方法实现，以便设定各种泛型约束。每个验证方法均包含一个名为 `message` 的可选参数，可自定义异常消息。

## 值验证示例

``` CSharp
using XNetEx;

// 存在一个 string 类型的名为 param 的值。
// 现要求其以数字开头，并且包含在键的集合 keys 中，
// 且不以 "." 结尾。

Validate.Value(param, nameof(param))    // 使用 XNetEx.Validate.Value<T>(T, string) 方法，
                                        // 创建一个 XNetEx.IValueInfo<out T> 接口的实例。

    .IsNotNull()                        // 验证值不为 null，否则抛出 System.ArgumentNullException 异常。

    .IsInRange("0", "9", "OutOfRange")  // 要求实现 System.IComparable<in T> 接口，验证值是否在指定范围内，
                                        // 否则抛出 System.ArgumentException 异常，并设定异常消息为 "OutOfRange"。

    .IsInKeys(keys)                     // 验证值在键的集合中，否则抛出
                                        // System.Collections.Generic.KeyNotFoundException 异常。

    .NotEndsWith(".", "InvalidEnd");    // 验证字符串不以 "." 结尾，否则抛出 System.ArgumentException 异常，
                                        // 并设定异常消息为 "InvalidEnd"。
                                        // 此方法也可使用正则表达式版本的 IsNotMatch(".$", "InvalidEnd") 方法实现。

// ......
```
