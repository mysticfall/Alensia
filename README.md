# Alensia

Programmer friendly framework to build RPG style games on _Unity_ engine.

**Development is on hold indefinitely since Jan., 2018. Please read the _Status_ section below.**

## Motivation

I've been writing business applications in _Scala_ and _Java_ for quite a long time, but I always wanted to create my 
own game someday.

I had thought I'll never be able to do that until I discovered _Unity_ is freely available on _Linux_ (which has been my 
main OS since 2001), and supports C# as its main language. So I was like "Why not?" and decided to give it a try.

But even though I could grasp basics of _Unity_ and _C#_ without too much trouble, I soon realized that it requires 
certain changes in my attitude, or mentality to follow the way that is recommended by tutorials or any other material I 
could find on the internet.

As a programmer, I cannot but think that the most important parts of any system I build to be the code itself. I believe 
that class hierarchies are the conceptual models which represent the system as a coherent whole, for example, and 
reading API documentation is the best way to figure out any project, and the likes.

However, with _Unity_, you are supposed to purchase a bunch of items from Asset Store and throw them into your game 
objects and configure them via GUI. Then you can fill in the gaps with 'scripts' which are often monolithic, heavyweight 
classes that cannot be instantiated outside the Unity runtime, or be easily reused without copy pasting the source.

As such, you are actually _discouraged_ from designing a coherent structure with class hierarchies, and it's not easy to 
follow commonly regarded best practices in programming, like 'programming to interfaces, rather than implementations', 
or preferring POCOs to improve testability, and etc.

I'm not saying the _Unity_'s way is inherently bad. But at least for those people who have strong programming background 
and intend to create a game on their own, I thought there coud be a better way for them to utilize their knowledge.

## Requirements

 * _Unity_ `2017.1` with _.NET_ `4.6` API compatibility (Change in _Player Settings_).
 * [_Zenject_](https://github.com/modesttree/Zenject) `5.3.0` or later.
 * [_UniRx_](https://www.assetstore.unity3d.com/en/#!/content/17276) (Requires 
 `ENABLE_MONO_BLEEDING_EDGE_EDITOR`, and `ENABLE_MONO_BLEEDING_EDGE_STANDALONE` scripting symbol).
 * [_UMA_](https://www.assetstore.unity3d.com/kr/#!/content/35611) (Optional) Used for _UMA_ integration API and demo.
 * [_Reorderable List_](https://github.com/cfoulston/Unity-Reorderable-List)

The demo requires [_Standard Assets_](https://www.assetstore.unity3d.com/en/#!/content/32351) 
package to run, but strafing moves(`A` and `D` key) won't work as intended, because it does not provide any suitable animations.

## Status

Development is on hold indefinitely, since I decided to move to [Godot](https://github.com/godotengine) 
engine and start over. I already started a new project named _[Alley Cat](https://github.com/mysticfall/AlleyCat)_ to 
continue pursuing my goal.

You can read about my motivation of the decision [here](https://forum.unity.com/threads/thinking-about-moving-to-godot.510826/), if you like.

For historic purposes, you can see how the development had been going from the official 
[forum thread](https://forum.unity3d.com/threads/alensia-an-open-source-programmer-friendly-rpg-framework-in-a-very-very-early-stage.465618/) 
or the project's [_YouTube_ channel](https://www.youtube.com/playlist?list=PLN4J41q17fIKgwcwiwERReerLBgC9GFDf).

## FAQ

> What is 'Alensia' anyway?

Sorry, I have no idea. The name just came up to my mind.

> Isn't there everything you need on Asset Store already? Are you really that poor so you can't affford to buy anything?

As I've written my motivation above, I wanted to make a foundation on which I can build my game in a way that I feel the 
most comfortable and intuitive.

In a word, it's a '_programmer's framework_', for those who prefer writing codes to tweaking GUIs like me.  

Besides that, there are not many options if you want to make an open source game, as only a few items on Asset Store 
supports such an usage.

So, as a supporter of open source movement, I want to help people creating open source games, and also hope that my 
project would benefit from contributions by similar minded people in future. 

> So, it's just a framework then? Where's the real game?

I'm planning to create one based on this framework, but haven't decided what exactly would it be yet. So I just decided 
to create a generic framework first, so I can build upon it later.

> When can I expect to see it finished, or at least become remotely usable?

Maybe in next 10 years? (I hope Unity will still be there then.)

## Feedback

Other than using the [issue tracker](https://github.com/mysticfall/Alensia/issues) at the project page, You can visit 
the official forum thread [here](https://forum.unity3d.com/threads/alensia-an-open-source-programmer-friendly-rpg-framework-in-a-very-very-early-stage.465618/) 
and leave your comments.

## License

This project is provided under the terms of _Apache License, Version 2.0_.
