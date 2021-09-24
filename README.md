# Entity Component System
Eventually, I aim for this to be a C# implementation of ECS that is easy to read and use.

## Disclaimer:

This project is currently a work in progress, and is incomplete, and lacking many features that a useful ECS should have. There are also a lot of sub-projects that exist for my own experimentation that will be removed in the future.

## Why am I making this

A friend of mine told me about ECS and after looking at the Wikipedia entry, I just assumed it was an esoteric way of organizing code that didn't really have any other benefits outside of that, so I didn't really give it a second thought.

But it came up again a game engine development tutorial by TheBennyBox, and upon more digging - it seems that ECS representations are now being preferred over a traditional Scene hierarchy tree representation (like that used by the Unity game engine for a long time) for because of the way that they enforce cache locality, which can have enormous performance benefits for applications like video games.

So why is it, that I can't seem to find anything on the internet that explores the performance of ECS in comparison to the other representation? 
Is ECS just so much faster that it is plainly noticeable and so these questions are just never asked by the people that use it? 
If so, are these performance benefits accessible to naive implementations of ECS? 

These are the questions that I intend for this project to answer.

I also intend to implement a hierarchy-tree model similar to that found in Unity, and then benchmark a physics simulation in both an ECS environment and a SceneGraph environment if I ever end up finishing the ECS.