# Events

This package allows abstraction of Events to global asset files to reduce dependencies between objects in your scene. I intruduced generic events that pass values as parameters inlcuding:

- string
- bool
- int
- float
- GameObject
- Vector3

It is easily extendable to new types.

## Usage

#### Create an event asset 
Right click in your Project Window > Create > Events > Event(*T*).

#### Listening to the event

Create a corresponding Listener component
+ *Game Event Listener* for parameterless events.
+ *String Event Listener* for string events.
+ *Float Event Listener* for float events.
+ You get the point ...

Use UnityEvents to respond to the event.

![](https://s18.postimg.org/gj2ckvc21/image.png)

#### Firing the event 

You can reference it directly from your UI components like Buttons

![](https://s18.postimg.org/e30j6t47d/image.png)

and Input Fields (String Events):

![](https://s18.postimg.org/td0edpx21/image.png)

> Be shure to use the dynamic string to send the Input Field content along the event. 

Alternatively, you can reference the event asset in your script:

``` cs
//Assign the event in inspector
public GameEvent event;

void Foo()
{
   event.Raise();
}
```

You can place your asset in a Resources folder to reference it by path:

``` cs
Resources.Load<GameEvent>("Events/OnMyEvent");
```

You can even click the event asset and fire the Event manually (only works for parameterless events).

## Acknowledgement

This is based on the genius work of Ryan Hipple:

- [https://github.com/roboryantron/Unite2017](https://github.com/roboryantron/Unite2017)
- [Youtube - Unite Austin 2017 - Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=3244s)
