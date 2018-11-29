using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorMaterials : MonoBehaviour {

    [SerializeField]
    private List<Renderer> renderers = new List<Renderer>();

    public void SetColor(Color color) {
        foreach(Renderer renderer in renderers) {
            renderer.material.color = color;
        }
    }
	
}
