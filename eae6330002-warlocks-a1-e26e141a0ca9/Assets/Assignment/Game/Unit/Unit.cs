using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IUnit {
    
    [SerializeField]
    private Health health = null;
    public IHealth Health { get { return health; } }

    [SerializeField] private UnitMovement movement = null;
    public UnitMovement Movement { get { return movement; } }

    public string Name { get { return name; } }

    [SerializeField]
    private Player owner = null;
    public Player Owner { get { return owner; } }

    [SerializeField]
    private UnitController controller = null;
    public IUnitController Controller { get { return controller; } }

    [SerializeField]
    private List<Ability> abilities = new List<Ability>();
    public List<Ability> Abilities { get { return abilities; } }

    [SerializeField] private Transform abilitiesParent = null;
    public Transform AbilitiesParent { get { return abilitiesParent; } }

    [SerializeField] private FloatStat abilityRangeMultiplier = null;
    public FloatStat AbilityRangeMultiplier { get { return abilityRangeMultiplier; } }

    [SerializeField] private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    [SerializeField] private Transform itemsParent = null;
    public Transform ItemsParent { get { return itemsParent; } }

    [SerializeField]
    private Ragdoll ragdoll = null;
    public Ragdoll Ragdoll { get { return ragdoll; } }

    [SerializeField]
    private Transform faceTransform = null;
    public Transform FaceTransform { get { return faceTransform; } }

    [SerializeField]
    private Transform chestTransform = null;
    public Transform ChestTransform { get { return chestTransform; } }
	
}
