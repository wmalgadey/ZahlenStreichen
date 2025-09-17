ZahlenStreichen
===============

A little project our last student apprentice lead us in. Couldn't sleep until I figured it out, how to solve this puzzle

The rules: First you note the numbers from 1 to 19 without the 10. Each digit is written in one column of a row (for 11 you put 1 and 1 in each cell). After 9 Columns you add another row. The starting board looks as follows:

<pre>
123456789
111213141
516171819
</pre>

To solve the puzzle you apply the following actions:

1. You can erase two numbers if a direct neighbor is equal or both added are 10
2. a neighbor could be in every direction (up, down, left, right)
3. if the neighbor is solved, its neighbor, in the same direction, is the new neighbor
4. the last column has the first column of the next row as neighbor and so the first column in the other direction

The solution is developed in C# and makes heavy use of Linq. AsParallel() makes the solution finder on my machine ten times faster (8 cores).
