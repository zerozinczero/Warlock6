using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This simple input manager allows input controllers to coordinate
/// so they're not all processing the same input.
/// </summary>
public class InputManager : MonoBehaviour {

    private List<IInputController> controllers = new List<IInputController>();

    private List<IInputController> controllersToAdd = new List<IInputController>();

    public void Add(IInputController controller) {
        controllersToAdd.Add(controller);
    }

    public void Remove(IInputController controller) {
        controllers.Remove(controller);
        controllersToAdd.Remove(controller);
    }

    private void Update() {
        for (int i = controllers.Count - 1; i >= 0; i--) {
            IInputController controller = controllers[i];
            if(controller != null && controller.ProcessInput()) {
                break;
            }
        }

        controllers.AddRange(controllersToAdd);
        controllersToAdd.Clear();
    }

}
