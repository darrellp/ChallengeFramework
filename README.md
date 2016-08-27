# ChallengeFramework
There's a CodeProject article on this software here:
  http://www.codeproject.com/Articles/1008583/Coding-Challenges-Framework

This is a framework for writing code for the many coding challenges out there.
I did a lot of these and got tired of creating a new project each time, putting
in boilerplate timing code, having a million project spread hither and thither.
Having to use something other than STDIN while testing and then using STDIN
when entering into the contest.  This is my attempt to gather all the test code
in one place and support all of this without any trouble.

It allows for coding in either C++, C# or F# and puts all the challenges you solve
into a nice listbox.  The challenge interface is simple - a Solve method, an Input
method and an Output method.  The Input and Output methods return the input for the
challenge and the expected output.  The framework turns the input into STDIN and
gathers its output from STDOUT as do most of these challenges.  If they return
nothing then there is no input and the output is shown in black.  This is for things
like the Euler challenges where there is no input and the answer is to be determined.
Each challenge has a simple annotation which gives the information about the challenge
which is how the listbox is created.  It also allows for a URL pointing to the original
website for the challenge.  Each challenge can be placed in a separate file in either the
standard (C#) challenges directory or the C++ or F# challenge.

When run, the challenge framework shows the input (which is editable but initially
comes from the Input method of the challenge interface) and when run displays the
output in red if it doesn't match the input and in green if it matches properly.
The time it took to run the challenge is also calculated and displayed.
Challenges can be aborted if they are taking too long and there's a button to take
you to the web page for the challenge.

In the end this has made doing these challenges much more fun for me.  It keeps all
of them in one place so I can go back and review/steal from any of them I've ever
done and makes it really simple to do a new one.  This is my personal version of this
so has lots of completed challenges.  If someone wanted to, I suppose they could "solve"
the challenges by using this code, but answers to these things are already all over the
internet so this doesn't really change all that much.  I really don't see the point of
how you could feel good about copying code anyway.  Generally, I would suggest cloning
the project and clearing out all of my challenges except a few examples and then solving
your own challenges.  In other words - have fun!

Oh - one other thing - it has an "External Dependencies" directory where I put my own
Number Theory library and Priority Queue library dlls.  Hopefully, I'll put these up
on GitHub soon.  If you can figure out how to use them from the examples, feel free.
There's a NumberTheoryBig which uses C# BigIntegers and NumberTheoryLong which uses
longs.  Obviously, NumberTheoryBig is somewhat slower but can handle arbitrarily
large values.
