# UnityEnhanced
A set of useful tools aimed at easy and powerful architecture in Unity3D based on ScriptableObjects! In short, you use references to asset files rather than wiring up all objects in your scene directly. This leads to loosely coupled modules in your scene and code that is much easier to debug and maintain. See [this great talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) of Ryan Hipple at the Unite 2017 to get introduced to the concept. This library includes an extended implementation of the tools presented in the talk and much more. 

[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.png?v=103)](https://opensource.org/licenses/mit-license.php)
[![Open Source Love](https://badges.frapsoft.com/os/v1/open-source.png?v=103)](https://github.com/ellerbrock/open-source-badges/)
[![Gitter Chat](https://badges.gitter.im/frapsoft/frapsoft.svg)](https://gitter.im/unity-enhanced/Lobby)

### Most notable features
- A [State Machine](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/StateMachine)
  + Utilize the power of ScriptableObjects for State Machines
  + Very flexible usage
  + Includes handy Listener components
  + Custom Editors for fool-proof usage
  + Works with prefabs
  + Photon Networking Integration
- An [Event System](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Events)
  + Embrace the power of ScriptableObjects for Events
  + Keep your systems perfectly separated
  + Wire things up in the editor
  + Write less code
  + Photon Networking Integration
- A [Variables System](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Variables)
  + Rise with the power of ScriptableObjects for shared Variables
  + Save your data to an asset file
  + Dynamically reference your data from scripts
  + Share data of multiple objects
  + Override data when needed
- [Cached Resources](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Common)
- [Readonly Attributes](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Common)
- Many useful improvements
  + Instantiate prefabs by alt-clicking them
  + Editor Extensions for ReorderableLists, CustomDrawers, Buttons, ...

Open particular folders for detailed description.

## Requirements

- Unity 2017.1 or later
- .NET 4.6 or above

## Setup

### Installation as Git submodule

It is recommended to install the package as a git submodule in your project.

In Sourcetree:
- Click Repository > Add Submodule ...
- As Source Path / URL you specify: 

``` 
git@github.com:hendrik-schulte/UnityEnhanced.git
```

- Set Local Relative Path to:

``` 
Assets/UnityEnhanced
``` 

- Pull submodule

### Manual Installation

Copy content of this repository to a folder named *UnityEnhanced* inside of your asset folder.

### Assembly Definition

The package includes [Assembly Definition Files](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html). Since the Photon Networking package is optional, you may need to add or remove Photon as reference in the assembly definition. Or you can simply delete all *.asmdef* files.

## More to come soon

Copyright (c) 2018, Hendrik Schulte
