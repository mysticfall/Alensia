# Alensia

A high level framework to build RPG style games using Unity3D engine.

## Motivation

I just started to learn C# and Unity3D to create my own game, but couldn't get used 
to the way it's supposed to be done.

So, I decided to create everything from the scratch in my own way.   

## Dependencies

 * [Zenject](https://github.com/modesttree/Zenject) - Dependency injection framework for Unity3D.
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

## FAQ

> What is 'Alensia' anyway?

Sorry, I have no idea. The name just came up to my mind.

> Isn't there everything you need on Asset Store already? Are you really that 
poor so you can't affford to buy anything?

Let's just say I'm so arrogant that I can't stand to look at any code other 
people wrote.

Besides that, there are not many options if you want to make an open source 
game, as only a few items on Asset Store supports such an usage.

> So, it's just a framework then? Where's the real game?

I'm planning to create one based on this framework, but haven't decided what 
exactly would it be yet. So I just decided to create a generic framework first, 
so I can build upon it later.

> When can I expect to see it finished, or at least become remotely usable?

Maybe in next 10 years? (I hope Unity will still be there then.)

## Feedback

Other than using the [issue tracker](https://github.com/mysticfall/Alensia/issues) 
at the project page, You can visit the official forum thread 
[here](https://forum.unity3d.com/threads/alensia-an-open-source-rpg-framework-wanna-be-in-a-very-very-early-stage.465618/) 
and leave your comments.

## License

This project is provided under the terms of _Apache License, Version 2.0_.
