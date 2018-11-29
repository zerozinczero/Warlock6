using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    [SerializeField]
    protected Player player = null;
    public IPlayer Player { get { return player; } }

    [SerializeField]
    protected Game game = null;

    [SerializeField]
    protected List<Unit> units = new List<Unit>();
    public List<IUnit> Units { get { return units.ConvertAll<IUnit>((u) => u as IUnit); } }

    protected virtual void Start() {
        AssignPlayerColorToUnits();
    }

    private void AssignPlayerColorToUnits() {
        foreach (Unit unit in units) {
            PlayerColorMaterials[] playerColorMaterials = unit.GetComponentsInChildren<PlayerColorMaterials>();
            foreach (PlayerColorMaterials pcm in playerColorMaterials) {
                pcm.SetColor(player.Color);
            }
        }
    }

}
