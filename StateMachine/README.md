# StateMachine

This is a simple yet powerful state machine based on ScriptableObjects.

## Getting Started:
- Create a StateManager asset for every universe of states you like
- Create a set of states and reference the StateManager asset.
- Add a [*ActiveInState*](StateMachine/ActiveInState.cs) or [*Transition*](StateMachine/Transition.cs) component to the GameObject you wish to be controlled by the state.
- Add a reference to the states the component should be active in to the *Active States* list of the component.

## ActiveInState

This component simple activates or deactivates the parenting GameObject when entering or leaving a state.

## Transition

This component enables easy transitions between UI windows using animations. Needs an Animator component with Animations named *Open* and *Closed*. A sampe animation can be found in [*/Animation/*](StateMachine/Animation/).
