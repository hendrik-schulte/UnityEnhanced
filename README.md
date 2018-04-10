# UnityEnhanced
A set of useful open-sourced tools for Unity3D I wrote or found during development.

Open particular folders for specific description.

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
