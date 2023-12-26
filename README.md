# UnityPackageTemplate

Requires:
  Odin Inspector

# Editor 
## Prop Placement Tool ⚠️
Currently (8-7-23) non functional
> package editor scripts need to be manually added to the `Assembly-Csharp-Editor` assembly!

# Runtime
## Debug Console ⭐
Enables usage of a debug console at runtime, even in standalone
### Tips!
- **toggle the console with \` (the tilde (~) key)**!
- Run commands by typing their function name in the console. They are currently case sensitive.
- You can add additional commands by adding them to a partial class named DebugCommands in the HammerElf.Tools.Utilities.Commands namespace
- You can display logs from the ConsoleLog utility by using the `ToggleLog` command!

---
## Extensions
### General Extensions ⭐
`GetComponentIfNull()` & `TryGetComponentIfNull()`
- Non-destructive backfillers for GetComponent

`DoFunctionToTree()`
- runs a given function on both a gameobject and all of its descendants in the transform hierarchy

`CopyComponentValues()`
- Should probably be moved to ReflectionExtensions

And More!
### IEnumerableExtensions
The extra things that LINQ forgot.
Currently just a ForEach() that works on IEnumerables, not just Lists
### MaterialExtensions
- SwitchBlendMode, ToPropertyBlock, and more
- Also contains the static ShaderUtilInterface (which is not an interface and should be renamed) class and additional extension methods
### PhysicsExtensions
- Various additional manual shape casts, tools for pulling data from colliders
- SortClosestToFurthest() for raycasts
- Union and intersections for Bounds
- other Bounds extensions, like percent containment for bounds-bounds overlap
- all sorts of things

### ReflectionExtensions
nice utilities for Type and MemberInfo
### StringExtensions ⭐
- `BackFill()` to make null strings easier to spot
- `SplitCamelCase()`
	- splits up a camel case string by adding spaces between words
- `NullIfEmpty()`

### UIExtensions
- `RaycastAllTargetObjects()` on a GraphicRaycaster
- `RaycastOneTargetObject()` on a GraphicRaycaster
### VectorExtensions ⭐
A bunch of nice utilities for working with Vector2, Vector3, and Vector4
- scaling, signed/unsigned component products
	- along with contextually named overloads like area, volume, and hypervolume
- get min/max component
- component-wise inverse of vector

---
## General
### DataStructures/CleanableHashSet
HashSet implementing ICleanable, enabling the ability to easily detect and subscribe to changes in the collection.
### DataStructures/SerializedDictionary
Dictionary that shows up in the inspector
### DataStructures/SerializedHashSet ⚠️
Hashset that shows up in the inspector
When used, can potentially generate editor-only errors if manipulated strangely.

### DataStructures/WeightedChance ⭐
Given a list of `WeightedChanceEntry<T>`, returns a random entry value using each value's weight.
is also enumerable.

### DataStructures/WeightedCurveChance
Wrapper for WeightedChance that supports animation curve weights (as opposed to single numbers).

Given a list of `WeightedCurveChanceEntry<T>` and **an input value to evaluate the curves** at, calculates and returns a random entry value using each value's weight curves. 

> Useful for modeling weights that change in accordance with a parameter, like time of day, depth, or player level.

---

### ConsoleLog ⭐ ⚠️
Improved version of Unity's Debug class. Built-in contextual headings, file logging, AssertWarns, and more. 

> TODO: Enable custom configuration (can't modify package files directly)

### DebugGizmoTool
Debug tool for quickly drawing gizmos into Unity without having to add OnDrawGizmos to a MonoBehavior.

Intended for quick, temporary debug drawing, and not for any long-term or permanent gizmos.
    
### MathFuncs ⭐
Our growing in-house math library.
Prominent functions:
- LerpWithCurve()
- CalcCurveWeight()
- Other animation curve manipulations
- Quaternion.Inverted() shorthand

### Dynamic Boolean
### ICleanable ⭐
Simple but powerful interface that adds an `bool IsDirty` flag, a `ChangedEvent`, and a `Clean()` function intended to reset the IsDirty flag. 

These tools can be used to passive track or react to changes in a collection or whatever other class
Particularly useful for
- Collections
- UI
- cases that need to respond to changes in a modular, unified, or consistent manner

### MoveCameraWithArrowKeys
### MultiKey
Useful for giving a dictionary a key composed of multiple objects without having to make a custom class implementation

Comes in 2 and 3 type generic forms as well as a "variable argument" form using an `object[]`

### PooledObject ⭐
Object pooling that doesn't require a centralized pooling component.
For each prefab utilizing it, PooledObject retains a static container for the instances of that prefab.

Excellent for prototyping.
### ProximityTrigger
Toggles specified scripts on or off depending on whether or not the gameobject is in range of a given transform or bounding box.

Operates using Coroutines.

Excellent for prototyping.

### Singleton
Wrapped up singleton pattern. Can extend this class to automatically make any class behave as a Singleton.

## ToggleDependency
Enables easily specifying Behaviours and GameObjects to enable/activate and disable/deactivate in reaction to the enabled state of the gameobject this script is attached to.

Useful for handling object "death", toggling off components used when its alive and enabling components used when it's dead, for example.

Excellent for prototyping.

---
## UnityEvent Helpers
### GameObjectUtility
### Trove ⚠️ (incomplete)
### UIUtility
