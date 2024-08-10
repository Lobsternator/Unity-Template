# What is... Unity-Template?
At first glance, you may think this a bog-standard, normal-ass unity project... and, well, that's because it is. Sorta, it's a little more than that though, at least so I'd like to think. "What makes it special then?" I hear you ask, and I'm glad you did because I'm actually quite excited to tell you :)
###
The relative "uniquness" of this project arrises from the fact that it comes out of the box with some prebuilt "architecture" and "logic" beyond what unity provides to you by default, and here come the features! :O

## Features (not exhaustive)
### Very minimal clutter
I've structured all the files related to the template such that they are out of the way for any new project files. The project folder structure looks something like this:
```
Assets
├── _External
|   └── Template
|       └── ... (where all the files related to the template are stored)
└── _Project
    └── ... (where all your files would go)
```
### Audio
- [AudioManager.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Audio/AudioManager.cs) - If you like using FMOD, this is for you! AudioManager uses exclusively FMOD to play sounds (just do it the regular way otherwise). Provides an easy way to play FMOD events, at a specified location, or attached to an object.

### Core
- [Singleton.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Core/Singleton.cs) - Highly contentious! Personally, I like using singletons, in moderation. Simple enough to explain, provides my personal implementation of singletons in Unity for MonoBehaviour scripts via the SingletonBehaviour class.
  ###
  As an additional feature there is also the SingletonAsset class which allows you to create singleton ScriptableObjects. The way this works is say you have a ScriptableObject and want to make it into a singleton. You make it inherit from SingletonAsset, and by the default behaviour, you will get an error logged in the console if that asset is missing from your Resources folder. You will also get a warning logged if there is more than one of those assets.
  ###
  Once the asset has been created in the Resources folder you can then access that asset from (pretty much) anywhere in your code using the AssetUtility.GetSingletonAsset function (caching included!) There is also a seperate (not recommended, but possible) way you can do this, which is by using the SingletonAssetAttribute to specify you want to AutoCreate the asset in the Resources folder. That way, it will automatically create the asset, if it is missing.
  ###
  As stated, this is not the recommended way as it can sometimes create unintuitive behaviours when messing around with the asset, but for those who want the quality of life feature, it's there. Just be aware of the potential unintuitiveness auto-creation can bring.
  ###
- [PersistentRuntimeObjects](https://github.com/Lobsternator/Unity-Template/tree/main/Assets/_External/Template/Scripts/Core/PersistentRuntimeObjects) - Oh boy, this one is gonna need a fair bit of explaining...
  ###
  Do you hate forgetting to add manager scripts to your scenes? Having to use Script Execution Orders to make sure they always load first? Otherwise breaking everyhting, leaving you confused and dazed? Wondering why you even try? Don't let such lowly quarrels bother you any longer! With PersistentRuntimeObjects you will never have such a problem again!
  ###
  ... Ahem, anyways. Now, I hear you asking "okay... that sounds nice... but what exactly are PersistentRuntimeObjects?" and I'm glad you are asking, because it's actually quite exciting! PersistentRuntimeObjects are objects that are are automatically created at the start of the application, are meant to live througout the entire lifetime of an application and have the ability to always load before any other scripts in any scene. Sounds pretty good, ey? 😏
  ###
  "But how does it actually work?" I hear you asking. Well, let me tell you! The whole crux of how this works relies heavily on a relatively unknown Unity feature, namely "RuntimeInitializeOnLoadMethodAttribute," which is a quite strange, but also quite useful attribute which you can apply to a static function that makes Unity automatically call it at a certain step in the initialization stage of the engine.
  ###
  One of these stages is called "BeforeSceneLoad." It calls the function, as you can figure, before the first scene ever gets loaded. Now, strangely enough, you can still add objects to the scene in this stage! Additionally, since this is before the first scene is ever loaded, no other scripts are going to be loaded before this point. This is exactly what we want!
  ###
  Now, the next step is to create the PersistentRuntimeObject and shove it into the DontDestroyOnLoad scene, that way it will stay alive for the entire lifetime of the application (unless you destroy it manually... which you shouldn't do anyways :p) And there we go! Now, all you have to do is know how to create one of these "PersistentRuntimeObjects" yourself!
  ###
  The way you go about this is say you have some sort of manager script or something the rather, and want it to become a "PersistentRuntimeObject." All you do is simply make it inherit from the PersistentRuntimeObject class... and... that's it :)
  ###
  Yes, that's right, one single inheritence and all the logic I just described to you will happen automatically behind the scenes, ain't that just neato. Now, understandably, you may want to have a little more fine control in this process, and this is when I direct you to look at the "PersistentRuntimeObjectAttribute," which allows you to more precicely define in what order other PersistentRuntimeObjects are to be loaded relative to eachother.
  ###
  Now, if you are studious, or frankly, simply a fucking genious, you may realize that you will not be able to edit any inspector fields for scripts that are created this way (as they literally don't exist in your scene until you've already started the game.) Now, I suppose you could get around this by hardcoding the values, but, in my opinion, that is kinda fucking cringe, So, instead, I've deviced a different way for you to do it :)
  ###
  Introducing: "PersistentRuntimeObjectData!" It is simply a ScriptableObject which should reside in the Resources folder, where you can then edit your fields till your heart's content. Once you have created the data asset you'll want to pass type of the PersistentRuntimeObjectData class you made as a second generic parameter for the desired PersistentRuntimeObject, and it will automatically grab the correct data asset from the Resources folder when created.
  ###
  "Okay... but how does it do that?" you ask? Using SingletonAssets of course! Now, this does also mean that all PersistentRuntimeObjects of the same type will have to share the same data asset. However, if you don't like this, I have a solution for you! (Sort of, depends on how you look at it :p)
  ###
  Introducing: "PersistentRuntimeSingleton!" (This is simply a singleton version of a PersistentRuntimeObject, really nothing more to it.) (This is also the one you should be using 99% of the time when messing around with PersistentRuntimeObjects.)
  ###
  And with that, ladies and gentlemen, this novel of a fucking bullet point, is concluded xD
  ###
- [SerializableInterface.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Core/Editor/SerializableInterface.cs) - Exposes a field in the inspector to allow you to drag any object into that field which inherits from a specified interface (and inherits from Unity's Object class). You can constrict the type of the objects you can drag to the field by specifying a different covering type. For example, if you choose MonoBehaviour as the covering type, only MonoBehaviours (who also inherit from the interface) can be dragged into the field.
  ###
- [State.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Core/StateMachine/State.cs), and [StateMachine.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Core/StateMachine/StateMachine.cs) - Simple enough. Provides an implementation of a generic finite state machine. Quite proud of how far I was able to push C#'s generic type system to prevent you from setting a state on a state machine that it is not meant for.
  ###
- [TimeManager.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Core/Time/TimeManager.cs) - Simply for modifying Unity's timeScale in a safe and reversable way. If you use TimeManager to change the timeScale it'll send out events every time, as well as specific events for when time has frozen, and unfrozen. As an additional feature, you can also use it to create hitstops. If you don't know what a hitstop is, it's basically when time momentarily slows down during something like an attack impact to add some extra "oomph."

### Gameplay
- [ValuePool.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Gameplay/ValuePool.cs), and [DamagePool.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Gameplay/Damage/DamagePool.cs) - Provides a simple implementation of a damage system heavily inspired by the one implemented in Unreal (albeit I don't have too much experience with it :p) This is achieved by having "DamagePools" as fields on objects which store the current, minimum and maximum damage, as well as methods for changing it and events for when it does change. With damage there also comes damage types, which you can create and specify as you like. There is also the more lightweight version, "ValuePool," which simply omits the concept of damage types, but otherwise keeps all the same functionality as DamagePools.

### Physics
- [PhysicsChecker.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Physics/PhysicsChecker.cs) - Provides useful physics-related information such as if you are currently moving, or grounded to a surface, and the normal of the surface you are currently standing on.
  ###
- [ExtendedPhysics.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Physics/ExtendedPhysics.cs), and [ExtendedPhysicsMaterial.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Physics/ExtendedPhysicsMaterial.cs) - Attach the ExtendedPhysics script to an object and assign an ExtendedPhysicsMaterial (created via context menu) to use this feature. ExtendedPhysicsMaterials store information for all vanilla physics material properties (such as friction and bounciness), as well as support for bounciness values that go above one. They also store values for linear and angular drag which allow you to apply linear and angular drag to any objects that are currently in contact with a collider that's attached to an ExtendedPhysics script.

### Saving
- [SaveManager.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Saving/SaveManager.cs) - A saving system! Very proud of this one :) Provides a simple framework for saving/loading different states of your game to/from a file on disk. It automatically handles things like serialization/deserialization for a short list of basic unity types (currently) (might add more supported types later... or you could do it it yourself... or ask me to do it :p) Multiple save "slots" are supported by default.

### Scenes
- [ExtendedSceneManager.cs](https://github.com/Lobsternator/Unity-Template/blob/main/Assets/_External/Template/Scripts/Scenes/ExtendedSceneManager.cs) - I've always thought the vanilla SceneManager is a little bit lacking in terms of utility. ExtendedSceneManager simply seeks to remedy this lack of utility.
  ###
    1. Provides the ability to at any point check if any given scene is either loaded, unloaded, or in the process of loading or unloading, even for scenes that you have not directly called to unload (eg. having multiple scenes open and then loading a scene with LoadSceneMode.Single).
    2. Provides a way to at any point get the AsyncSceneOperation associated with a certain scene loading or unloading, even if you never saved the original operation (this also works for scenes you have not directly called to unload as mentioned before.)
    3. Provides a way of loading/unloading multiple scenes at once that returns an AsyncSceneOperationCollection that can be yielded in a coroutine, which (if yielded) will then wait until all operations in that collection have been completed.

### UI
- [Forwardings](https://github.com/Lobsternator/Unity-Template/tree/main/Assets/_External/Template/Scripts/UI/Forwardings) - Provides a simple way of forwarding various functions (for previously mentioned and unmentioned scripts, as well as some vanilla ones) for use with user interface (like... for buttons or something, idfk).

## Examples/Documentation
Looking for examples? You can find a handful [here!](https://github.com/Lobsternator/Unity-Template/tree/main/Assets/_External/Template/Examples) It is by no means an exhaustive list of examples, but you will find some of the most useful ones (in my opinion). I've also lightly documented (mostly) all classes in the project, so feel free to read and learn that way too :)

## Why?
Often when I make new unity projects, I find myself implementing a lot of the same things in the same kind of structure, so I decided to make a sort of "template" where all of those same things have already been implemented (as well as various features I could see myself getting some good use out of). I also figured, why not give others the ability to use it as well :)

## Do you promise to keep the project updated and to respond to any bug reports and/or feature requests expeditiously?
Maybe... I'd be more motivated if you say please :)
###
In all seriousness though, this is currently just a hobby project I do on the side for fun, but if it would happen by some miracle to get some popularity... uhhhm... shit OwO Guess I might have to... that's what I'm signing myself up for then I suppose :p

## Get it all, now! For the low-low price of [a mention in the credits]!
For real! Feel free to test, experiment, use, and market in accordance with the license. Just remember to give credit where credit is due, please, and thanks 😎

Ciao 🙋‍♂️
