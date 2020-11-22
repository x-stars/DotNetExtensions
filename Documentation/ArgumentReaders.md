# 命令行参数解析器

包含于程序集 XstarS.ArgumentReaders，提供简易的命令行参数解析器，以及以此为基础的多种风格的命令行参数解析器。

## 类 `XstarS.CommandLine.ArgumentReader`

默认的命令行参数解析器，作为命令行参数解析器的基类和默认实现。

* 不支持 Unix / Linux shell 中连字符 "-" 后接多个选项参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
* 不支持多个同名的有名参数的解析。

## 类 `XstarS.CommandLine.Specialized.CmdArgumentReader`

继承 `XstarS.CommandLine.ArgumentReader` 类。

命令提示符 (CMD) 风格的命令行参数解析器，参数名称忽略大小写，有名参数名称与参数值用冒号 ":" 分隔。

* 支持多个同名的有名参数的解析。
* 不支持 Unix / Linux shell 中连字符 "-" 后接多个选项参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。

## 类 `XstarS.CommandLine.Specialized.PowerShellArgumentReader`

继承 `XstarS.CommandLine.ArgumentReader` 类。

PowerShell 风格的命令行参数解析器，参数名称忽略大小写。

* 不支持无名参数的解析，但支持省略名称的有名参数的解析。
* 不支持 Unix / Linux shell 中连字符 "-" 后接多个选项参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
* 不支持多个同名的有名参数的解析。

## 类 `XstarS.CommandLine.Specialized.UnixShellArgumentReader`

继承 `XstarS.CommandLine.ArgumentReader` 类。

Unix / Linux Shell 风格的命令行参数解析器，参数名称区分大小写。

* 支持连字符 "-" 后接多个选项参数的解析。
* 支持连字符 "-" 开头的参数值的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
