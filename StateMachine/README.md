# StateMachine

This is a simple yet powerful state machine based on ScriptableObjects.

## Getting Started:
- Create a StateManager asset for every universe of states you like.
> You create these assets by right clicking in your Project Window > Create > State Machine > State Manager
- Create a set of states and reference the StateManager asset.
> Right click in your Project Window > Create > State Machine > State
- Add one of the components described below to the GameObject you wish to be controlled by the state.
- Add a reference to the states the component should be active in to the *Active States* list of the component.
- *[Optional]*: Mark one of the states as initial state to define wich state the application starts in.
> You do this by clicking a state asset and click the *Set Initial State* button.

### [StateListener](Core/StateListener.cs)

Base component providing state-based events. Can be inherited for custom behaviour or wired in the inspector.

### [ActiveInState](StateListener/ActiveInState.cs)

This component simple activates or deactivates the parenting GameObject when entering or leaving a state.

### [TransitionAlphaBlend](StateListener/TransitionAlphaBlend.cs)

This component enables easy transitions between UI canvases by scripted fade-in and fade-out animations. Requires a canvas and a canvas group.

### [TransitionAnimator](StateListener/TransitionAnimator.cs)

This component enables easy transitions using animations. Needs an Animator component with Animations named *Open* and *Closed*. A sampe animation can be found in [*/Animation/*](StateMachine/StateListener/Animation/). For performance reasons it is recommended to use TransitionAlphaBlend for Canvases whenever possible.

## Going deeper: Instanced State Machines

By checking the *Instanced* field on a StateManager asset, you enable Instancing for that state machine. 
This allows you to use the state machine as a template and use it for example in a prefab. 
Every StateListener referencing a state of this state machine will now have an instance key field. 
This is used to access an instance of your state machine. You can assign any *UnityEngine.Object* as a key. 
For prefabs you can use the root GameObject as a key. Every prefab now has its own instance of the state machine.

## Going deeper: Networked State Machines

By checking the *PUN Sync* check box you can sync the state machine within a [Photon Unity Networking](https://www.photonengine.com/en/PUN) system. State changes are propagated towards other players automatically. Works with *Instanced State Machines* as well. All state assets need to be placed within the root of a Resources folder with a unique name. You need a *Photon Sync* Component somewhere in your scene.
