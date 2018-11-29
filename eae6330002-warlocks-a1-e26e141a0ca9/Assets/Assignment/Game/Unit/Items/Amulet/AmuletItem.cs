using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletItem : Item, IModifier<ClampedFloat> {

	[SerializeField] private float multiplier = 1f;
	public float Multiplier { get { return multiplier; } set { multiplier = value; } }

	public override void Equip() {
		BindToAbilities();
	}

    public override void Unequip() {
		UnbindFromAbilities();
    }

	public ClampedFloat Modify(ClampedFloat cooldown, object context = null) {
        cooldown.max *= multiplier;
		cooldown.value *= multiplier;
		return cooldown;
	}

	public int Priority { get { return 0; } }

	// TODO: tie this in to events
	public void OnAbilityAdded(Ability ability) {
		BindToAbilities();
	}
    
	private void BindToAbilities() {
		foreach(Ability ability in owner.Abilities) {
			Cooldown cooldown = ability.GetComponent<Cooldown>();
			if(cooldown == null)
				continue;
			cooldown.AddModifier(this);
		}
	}

	private void UnbindFromAbilities() {
        foreach(Ability ability in owner.Abilities) {
            Cooldown cooldown = ability.GetComponent<Cooldown>();
            if(cooldown == null)
                continue;
            cooldown.RemoveModifier(this);
        }
	}
}
