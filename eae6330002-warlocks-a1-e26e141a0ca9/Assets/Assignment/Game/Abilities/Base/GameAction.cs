using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction : MonoBehaviour {

    public GameActionEvent OnPerformed = null;
    public GameActionEvent OnCancelled = null;

    public abstract bool IsInProgress { get; }

    public abstract void Perform();
    public abstract void Stop();

}
