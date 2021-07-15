# EntityComponentSystem
A C# implementation of ECS that is easy to understand and use.
If it's actually any good performance-wise, I'll make this repo public and use it in my other stuff.

A friend of mine told me about ECS and I didn't really give it a second thought.
But it came up again a game engine development tutorial by TheBennyBox, and upon more digging - it seems that ECS representations are now being preferred over a traditional Scene-Tree representation (like that found in game engines like Unity) for video game worlds because of the way that they enforce cache locality, and in doing so vastly improve performance.

So why is it, that I can't seem to find anything on the internet that explores the performance of ECS in comparison to Tree representations?
Are the performance benefits overhyped, or inaccessible to naive implementations of ECS?
Can an ordinary developer create a good ECS system, or do you need expert knowledge that very few have?

These are the questions that this project intend to answer.

I intend to also implement a SceneGraph model similar to that found in Unity, and then benchmark a physics simulation in both an ECS environment and a SceneGraph environment.