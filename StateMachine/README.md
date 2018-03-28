# StateMachine

This is a simple yet powerful state machine based on ScriptableObjects.

## Getting Started:
- Create a StateManager asset for every universe of states you like.
- Create a set of states and reference the StateManager asset.
> You create these assets by right clicking in your Project Window > Create > State Machine > State (Manager)
- Add one of the components described below to the GameObject you wish to be controlled by the state.
- Add a reference to the states the component should be active in to the *Active States* list of the component.
- *[Optional]*: Mark one of the states as initial state to define wich state the application starts in.
> You do this by clicking a state asset and click the *Set Initial State* button.

## [*StateListener*](StateMachine/Core/StateListener.cs)

Base component providing state-based events. Can be inherited for custom behaviour or wired in the inspector.

## [*ActiveInState*](StateMachine/StateListener/ActiveInState.cs)

This component simple activates or deactivates the parenting GameObject when entering or leaving a state.

## [*TransitionAlphaBlend*](StateMachine/StateListener/TransitionAlphaBlend.cs)

This component enables easy transitions between UI canvases by scripted fade-in and fade-out animations. Requires a canvas and a canvas group.

## [*TransitionAnimator*](StateMachine/StateListener/TransitionAnimator.cs)

This component enables easy transitions between UI canvases using animations. Needs an Animator component with Animations named *Open* and *Closed*. A sampe animation can be found in [*/Animation/*](StateMachine/StateListener/Animation/). For performance reasons it is recommended to use TransitionAlphaBlend whenever possible. This may even work with non-UI objects.
