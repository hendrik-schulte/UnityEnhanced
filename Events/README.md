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

![](https://s18.postimg.cc/gj2ckvc21/image.png)

#### Firing the event 

You can reference it directly from your UI components like Buttons

![](https://s18.postimg.cc/e30j6t47d/image.png)

and Input Fields (String Events):

![](https://s18.postimg.cc/td0edpx21/image.png)

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

## Going deeper: Instanced Events

By checking the *Instanced* field on an Event asset, you enable Instancing. 
This allows you to use the event as a template and reuse it for example in instances of a prefab. 
Every Listener referencing an instanced event will have an *instance key* field. 
This is used to access an instance of your event via a *key* defined by any kind of *UnityEngine.Object*. You fire an instanced event by passing the instance key to it:

![](https://s7.postimg.cc/wl6nwztvv/image.png)

You can derive from *InstanceObserver* to write your own scripts that send or receive instanced and non-instanced events. The custom editor will automatically detect if the referencing ScriptableObject is instanced and expose the *Instance Key* property.

#### Example

The following script will raise an event OnTriggerEnter. It works for both instanced and non-instanced events.

```cs
public class SendTriggerEnter : InstanceObserver
    {
        public GameEvent Event;

        private void OnTriggerEnter(Collider other)
        {
            Event?.Raise(key);
        }

        public override IInstanciable GetTarget()
        {
            return Event;
        }
    }
```

![](https://s7.postimg.cc/99n5lppl7/image.png)

Now you listen to the event like this:

![](https://s7.postimg.cc/b59xwoyy3/image.png)

## Going deeper: Networked Events

By checking the *PUN Sync* check box you can distribute your event automatically within a [Photon Unity Networking](https://www.photonengine.com/en/PUN) system. You use the system as usual but any networked event will be raised for all players. Works with *Instanced Events* as well. All event assets need to be placed within the root of a Resources folder with a unique name. You need a *Photon Sync Manager* Component somewhere in your scene.

## Acknowledgement

This is based on the genius work of Ryan Hipple:

- [https://github.com/roboryantron/Unite2017](https://github.com/roboryantron/Unite2017)
- [Youtube - Unite Austin 2017 - Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=3244s)
