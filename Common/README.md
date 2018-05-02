# Common

This folder contains helpful utilities used in the entire UnityEnhanced package.

### Readonly Property

``` cs
[Readonly]
public float readonlyFloat;

[Readonly]
public int readonlyInt;

[Readonly]
public bool readonlyBool;

[Readonly]
public string readonlyString = "Can't touch this";
```

![](https://s18.postimg.org/fyrwvmq49/image.png)

### Help Attribute

https://github.com/johnearnshaw/unity-inspector-help

Copyright (c) 2017, John Earnshaw

``` cs
[Help("The cake is a lie.", MessageType.Warning)]
public float myField;

[Help("Something went wrong.", MessageType.Error)]
public float anotherField;

[Help("Some info.", MessageType.Info)]
public float youNeedToKnowThat;
```

![](https://s18.postimg.org/ov2ozgtk9/image.png)

### File Logger

You can easily write your logs to a file using the FileLogger class. Simply write 

``` cs
FileLogger.Write("my.log", "Hello Logging World!");
```

The Event system and State Machine directly support file logging (enable in inspector). 

### Cached Resources

Multiple calls of Resources.Load for the same asset may cause an impact on your performance. This class makes it easy to cache your Resources.Load calls to a dictionary. Instead of calling 
``` cs
Resources.Load<MyAsset>("PathToMyAsset");
```

write

``` cs
CachedResources.Load<MyAsset>("PathToMyAsset");
```

and you are good to go.

Use

``` cs
CachedResources.Flush();
```

To clear the cache.