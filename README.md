# Unity_BoundsBasedObjectManagementSystem

<img src="https://github.com/XJINE/Unity_BoundsBasedObjectManagementSystem/blob/master/Screenshot.png" width="100%" height="auto" />

BoundsBasedObjectManagementSystem provide a quite simple logic to control object counts and keep references with Bounds.
You can reference some objects and the data in a same bounds, and also in a same manager.

## Import to Your Project

You can import this asset from UnityPackage.

- [BoundsBasedObjectManagementSystem.unitypackage](https://github.com/XJINE/Unity_BoundsBasedObjectManagementSystem/blob/master/BoundsBasedObjectManagementSystem.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_IMonoBehaviour](https://github.com/XJINE/Unity_IMonoBehaviour)
- [Unity_ObjectManagementSystem](https://github.com/XJINE/Unity_ObjectManagementSystem)

NOTE:
BoundsBasedObjectManagementSystem resources are put into ``Assets/Packages/ObjectManagementSystem/BoundsBased``.

## How to Use

Inherit ``BoundsBasedObjectManager``, ``BoundsBasedManagedObject``, ``ObjectGenerator(optional)``.
Please check **[Unity_ObjectManagementSystem](https://github.com/XJINE/Unity_ObjectManagementSystem)** readme too.

Add ``BoundsMonoBehaviour`` to GameObject to make some bounds. And add them to ``BoundsBasedObjectManager.Bounds``.