 # Robot cleaner App

## Introduction

Very simple robot cleaner app which receives map which is matrix m by n and start position coordinates X,Y.    

Can can look like that:
``` json
["D", "D", "D"]
["D", "D", "C"]
["D", "D", "D"]
["D", "null", "D"]
``` 

 * D means dirty so it is cleanable
 * C means cleaned
 * Null represents not valid location e.g wall
 
The robot also recognizes a set of basic commands. Each command drains the battery of the robot by a certain amount. 
* **Turn Left (TL)**. Instructs the robot to turn 90 degrees to the left. Consumes **1** unit of battery. 
* **Turn Right (TR)**. Instructs the robot to turn 90 degrees to the right. Consumes **1** unit of battery. 
* **Advance (A)**. Instructs the robot to advance one cell forward into the next cell. Consumes **2** unit of battery. 
* **Back (B)**. Instructs the robot to move back one cell without changing direction. Consumes **3** units of battery. 
* **Clean (C)**. Instructs the robot to clean the current cell. Consumes **5** units of battery.

and also receives how much batter it has. All can be provided by json that can look like this:

``` json
{ 
 "map":[ 
       ["D", "D", "D"],
       ["D", "D", "C"],
       ["D", "D", "D"],
       ["D", "null", "D"]
       ]   , 
 "start": {"X": 3, "Y": 0, "facing": "N"}, 
 "commands": [ "TL","A","C","A","C","TR","A","C"], 
 "battery": 80 
}
```

If obstacle is hit e.g. next location is null or out of bounds it will do initiate backoff strategy:

1. Perform [TR, A, TL]. If an obstacle is hit, drop the rest of the sequence and
2. perform [TR, A, TR]. If an obstacle is hit, drop the rest of the sequence and
3. perform [TR, A, TR]. If an obstacle is hit, drop the rest of the sequence and
4. perform [TR, B, TR, A]. If an obstacle is hit, drop the rest of the sequence and
5. perform [TL, TL, A]. If an obstacle is hit, the robot is considered stuck. **Skip all the remaining commands and finish the program**.

Program can also finish when it runs out of battery or it runs out of commands. On program finish it will log visited locations, cleaned locations, battery and final position.

## Development setup

To build solution it is required to have [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0).
To build it inside command line write in console:
```sh
dotnet build
```
or build with IDE of your choice.

You can also skip the build step and just run it with:

```sh
dotnet run test1.json result1.json
```

or run it inside IDE of your choice.

If any of the two previous steps were successful you can also go to bin/Debug/net5.0 folder and run it with:

```sh
RobotCleaner.exe test1.json result1.json
```

To run tests:
```sh
dotnet test
```
