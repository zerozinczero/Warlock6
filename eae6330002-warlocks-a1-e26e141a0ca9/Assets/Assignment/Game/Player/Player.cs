using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer {

    [SerializeField]
    private Color color = Color.white;
    public Color Color { get { return color; } }

    [SerializeField]
    private bool isLocal = false;
    public bool IsLocal { get { return isLocal; } }

    public string Name { get { return name; } }
	
}
