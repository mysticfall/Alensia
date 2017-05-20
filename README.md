# Alensia

A _programmer's framework_ to build RPG style games on Unity engine.

## Motivation

I've been writing business applications in Scala and Java for quite a long time, but I always 
wanted to create my own game someday.

I had thought I'll never be able to do that until I discovered Unity is freely available on 
Linux (which has been my main OS since 2001), and supports C# as its main language. 

So I was like "Why not?" and decided to give it a try.

But even though I could grasp basics of Unity and C# without too much trouble, I soon realized 
that it requires certain changes in my attitude, or mentality to follow the way that is 
recommended by tutorials or any other material I could find on the internet.

As a programmer, I cannot but think that the most important parts of any system I build to be 
the code itself. I believe that class hierarchies are the conceptual models which represent 
the system as a coherent whole, for example, and reading API documentation is the best way to 
figure out any project, and the likes.

However, with Unity, you are supposed to purchase a bunch of items from Asset Store and throw 
them into your game objects and configure them via GUI. Then you can fill in the gaps with 
'scripts' which are often monolithic, heavyweight classes that cannot be instantiated outside 
the Unity runtime, or be easily reused without copy pasting the source.

As such, you are actually _discouraged_ from designing a coherent structure with class 
hierarchies, and it's not easy to follow commonly regarded best practices in programming, like 
'programming to interfaces, rather than implementations', or preferring POCOs to improve 
testability, and etc.

I'm not saying the Unity's way is inherently bad. But at least for those people who have  
strong programming background and intend to create a game on their own, I thought there coud 
be a better way for them to utilize their knowledge.

## Requirements

 * Unity _2017.1 Beta_ with _.NET 4.6_ API compatibility (Change in _Player Settings_).
 * [Zenject](https://github.com/modesttree/Zenject) - Dependency injection framework for Unity.
 * [UniRx](https://github.com/neuecc/UniRx) - Reactive Extensions for Unity
 (Requires `ZEN_SIGNALS_ADD_UNIRX` scripting symbol).

The demo requires [Standard Assets](https://www.assetstore.unity3d.com/en/#!/content/32351) 
package to run, but strafing moves('_A_' and '_D_' key) won't work as intended, because it does not 
provide suitable animations.

## Status

It's in a very very very early stage of development. So don't even think about 
actually using it for anything.

Below list shows the features implemented by the project so far:

### Camera API

 * Simple camera manager API.
 * Third person camera mode with orbiting and wall avoidance features.
 * Head mounted first person camera mode, which shows character's body when looking down.

### Locomotion API

 * Generic locomotion API template.
 * Animation and transform based locomotion for humanoid characters.

### Input API

 * Provides reactive API for input values, based on _UniRx_.
 * Basic input binding managements.

### Control API

 * High level abstraction for action and input mappings (work in progress).

### Physics API

 * Simple collision detector API.
 * Collision and ray cast based ground detector API.

## Plans

For now, I'm working on character customization API (based on _UMA_), and a simple 
wrapper around _IMGUI_, so I could make a basic character creator demo.

## FAQ

> What is 'Alensia' anyway?

Sorry, I have no idea. The name just came up to my mind.

> Isn't there everything you need on Asset Store already? Are you really that 
poor so you can't affford to buy anything?

As I've written my motivation above, I wanted to make a foundation on which I can 
build my game in a way that I feel the most comfortable and intuitive.

In a word, it's a '_programmer's framework_', for those who prefer writing codes 
to tweaking GUIs like me.  

Besides that, there are not many options if you want to make an open source game, 
as only a few items on Asset Store supports such an usage.

So, as a supporter of open source movement, I want to help people creating open 
source games, and also hope that my project would benefit from contributions by 
similar minded people in future. 

> So, it's just a framework then? Where's the real game?

I'm planning to create one based on this framework, but haven't decided what 
exactly would it be yet. So I just decided to create a generic framework first, 
so I can build upon it later.

> When can I expect to see it finished, or at least become remotely usable?

Maybe in next 10 years? (I hope Unity will still be there then.)

## Feedback

Other than using the [issue tracker](https://github.com/mysticfall/Alensia/issues) 
at the project page, You can visit the official forum thread 
[here](https://forum.unity3d.com/threads/alensia-an-open-source-programmer-friendly-rpg-framework-in-a-very-very-early-stage.465618/) 
and leave your comments.

## License

This project is provided under the terms of _Apache License, Version 2.0_.
