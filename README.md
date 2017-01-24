# multiFuck
multiFuck is a variant on the programming language brainfuck. brainfuck has one big issue; It is not multithreaded! So that's why I am building this project. It is fully written in C# and consists of a library. The library uses the standard console for in and output.
## usage
Programming is just like normal brainfuck programming. The only difference is that you'll have more processors to use. The number of processors is ajustable. 
when you program, the programs of all processors need to be the same length at the moment. In the future this will be different. if you're using 2 processors, a program could look like this:
,,..++..
In this case both programs are the same. The first command will be excecuted on the first processors, the seccond command on the seccond processor and so on. All the prcessors have their own memory at the moment. But in the future, I will make the Memory used switchable, for making the processors beingn able to interface with eachother.

## Todo:
- Add code comments! (Was lazy implementing this, I was just bored and searched for something to do...)
- Add switching of memory to make processors be able to communicate with eachother.
- Think of a way te make sure that the programs don't need to be the same length (or not, it might be an extra coding challenge...)
