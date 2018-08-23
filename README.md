# 说明

为unity设计的配置表转换工具，可将Excel成json、lua、可供C#反序列化的二进制文件。

# 用法
* 打开工程，直接运行（会报目录不存在的错误）。
* 打开Excutable/Debug/Config/Config.json配置好输入输出路径等。
* 可配置多个Config.json,用命令行传入自定义的Config.json文件路径。


# 配置参数说明

* excelPath ：配置表路径
* outputPath ：输出路径
* exportType ：导出类型（0：Unity  1：Lua）
* rowIndexType ：Excel中表示字段类型的行号
* rowIndexName ：Excel中表示字段名称的行号
* rowIndexContent ：Excel中，字段内容从第几行开始
* 其它配置详见Config.json

# 其它

对于使用unity il2cpp script backend 的朋友，请参见[Managed bytecode stripping with il2cpp](https://docs.unity3d.com/Manual/IL2CPP-BytecodeStripping.html) 。由于unity会默认剥离未使用的代码，所以请在项目中添加link.xml文件，内容如下：
'
<linker>
    <assembly fullname="config" preserve="all" />
</linker>
'
