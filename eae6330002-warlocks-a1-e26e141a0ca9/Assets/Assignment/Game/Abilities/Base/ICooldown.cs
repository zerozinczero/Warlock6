using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldown {

    float Duration { get; }     // how long the cooldown is, in seconds
    float Remaining { get; }    // amount of time in seconds until the cooldown is ready
    float RemainingNormalized { get; }  // normalized time remaining, 0 to 1
    bool Ready { get; } // whether or not the cooldown is ready

    void Activate();    // activates the cooldown into a not ready state
    void Reset();   // clears the cooldown into a ready state

}
