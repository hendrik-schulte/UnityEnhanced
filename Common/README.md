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
