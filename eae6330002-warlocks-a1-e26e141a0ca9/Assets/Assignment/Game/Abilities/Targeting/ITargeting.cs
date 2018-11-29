using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITargeting<TargetEvent,CancelEvent> {

    void StartTargeting();
    void CancelTargeting();

    TargetEvent OnTargeted { get; }
    CancelEvent OnCancelled { get; }

}
