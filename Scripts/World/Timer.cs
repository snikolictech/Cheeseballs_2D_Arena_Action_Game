using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script is a custom Timer class that is used in various scripts wherever a framerate independent Timer is needed.
*/
#endregion

public class Timer
{
    //Simply returns delaTime * 60 (at 60FPS will be close to 1)
    //which makes it useful as a counter on every frame or deltaTime multiplier
    public static float DeltaTimeMod
    {
        get { return Time.deltaTime * 60; }
    }

    //The main float used to store the actual counter wherever a Timer is used
    //Note that counters are usually integers, however a float is preferable when
    //using Time.deltaTime and keeping the game framerate independent
    public float Counter;

    public Timer(float startingPoint)
    {
        Counter = startingPoint;
    }

    //Timer method that simply runs the Counter in reverse
    public void RunReverse()
    {
        Counter = (Counter > 0) ? Counter -= DeltaTimeMod : 0;
    }

    //Timer method that runs the Counter foward until a set limit is reached
    public void RunForwardTo(float limit)
    {
        Counter = (Counter < limit) ? Counter += DeltaTimeMod : 0;
    }

    //Timer method that runs reverse from a set point until 0 is reached
    public void RunReverseFrom(float resetTo)
    {
        Counter = (Counter > 0) ? Counter -= DeltaTimeMod : resetTo;
    }
}