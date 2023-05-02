# Binary-Search-Tree

This is an implementation of an oct tree in C# Unity ECS DOTS - Feel free to use it for your own educational will - Any utilisation of this code is at your own risk!
In any situation please open this Unity project in unity version 2020-3-30f1 as it is a version that supports the ECS, the Jobs System...

I wrote an oct tree that encapsulates a huge amount of multithreaded birds. The oct tree subdivides and unsubdivides to a given threshhold automatically and updates periotically

The bird simulation was programmed from https://www.youtube.com/watch?v=RQ_peeoXTLo&list=WL&index=13 I just rewrote parts of it as I couldnt get it to run. The only reason I have it in my project is to test the oct tree. In reality a fast moving bird cloud is one of the worst scenarios for any optimisation. My oct tree is rather suited for a huge pile of things where not every object changes each frame in a that drastic manner. 

The goal with this project was to at least partially understand the entity component system and combine the cpu multithreading with spacial partitioning
I don't think I achieved it as my oct tree is not multi-threaded. I tried to make it work with blobs as they get rid of the problem that my oct tree is a complex data type which is not supported with the jobs multithreading system but I was unable to get it running.

The oct tree can be found in the QuadTree class ...\Assets\Scripts\OctTree\OctTreeDateStructureSystem - I didn't know that such a thing as an oct tree existed I just thought Id try get a cube tree working in 3d.

In reality one would directly reference all birds positions in the Oct tree and not an old version of them - so that the tree can update more accurately.

This code as been added to github late so here is its creation date:
[Third semestere Animation and Game - age 19]
