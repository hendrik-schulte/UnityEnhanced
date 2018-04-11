# UnityEnhanced
A set of useful open-sourced tools for Unity3D I wrote or found over the time. They aim for easy and powerful architecture of your project without writing much code.

Most notably:
- A [State Machine](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/StateMachine)
  + Utilize the power of ScriptableObjects
  + Very flexible
  + Works with prefabs
  + Includes handy Listener components
  + Custom Editors for fool-proof usage
  + Photon Networking Support
- An [Event System](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Events)
  + Embrace the power of ScriptableObjects
  + Keep your systems perfectly separated
  + Wire things up in the editor
  + Write less code
  + Photon Networking Support
- A [Variables System](https://github.com/hendrik-schulte/UnityEnhanced/tree/master/Variables)
  + Rise with the power of ScriptableObjects
  + Save your data in a file
  + Dynamically reference your data from scripts
  + Share data of multiple objects

Open particular folders for detailed description.

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

## To be included soon

- Networked prameter events
- Demo Scenes
